#include "stdafx.h"
#include "RankMapManager.h"
#include "base64_encode.h"
#include "CrashPlayer.h"
static inline bool my_greater(RankMapTg* a, RankMapTg* b)
{
	bool ret = false;
	if (a != NULL && b != NULL)
	{
		if (a->failed_of_challenge_times_ > b->failed_of_challenge_times_)
		{
			ret = true;
		}
		else if(a->failed_of_challenge_times_ < b->failed_of_challenge_times_)
		{
			ret = false;
		}
		else
		{
			if (a->challenge_times_ > b->challenge_times_)
			{
				ret = true;
			}
			else if(a->challenge_times_ < b->challenge_times_)
			{
				ret = false;
			}
			else
			{
				if (a->publish_time_ > b->publish_time_)
				{
					ret = true;
				}
				
			}
		}
		
	}
	return ret;
}

RankMapManager::RankMapManager()
{
}


RankMapManager::~RankMapManager()
{
}

void RankMapManager::Init(DBQuery* p)
{
	DBQuery& query = *p;
	query.reset();
	query << "select *,UNIX_TIMESTAMP(`publish_time`), UNIX_TIMESTAMP(`create_time`) from `player_rank_map`;";
	query.parse();
	SDBResult sResult = query.store();
	int count = sResult.size();
	for (int i = 0; i < count; i++)
	{
		RankMapTg* rank_map_entry = new RankMapTg();				
		DBRow row = sResult[i];
		rank_map_entry->info_.set_mapname(row["map_name"].c_str());
		rank_map_entry->info_.set_creatername(row["creater_name"].c_str());
		message::CrashmoMapBaseData* data_base = rank_map_entry->info_.mutable_data();
		std::string str_data = row["map_data"].c_str();
		data_base->ParseFromString(base64_decode(str_data));
		rank_map_entry->info_.set_create_time(row["UNIX_TIMESTAMP(`create_time`)"]);
		rank_map_entry->info_.set_chapter(row["chapter"]);
		rank_map_entry->info_.set_section(row["section"]);
		rank_map_entry->publish_time_ = row["UNIX_TIMESTAMP(`publish_time`)"];
		rank_map_entry->challenge_times_ = row["challenge_times"];
		rank_map_entry->failed_of_challenge_times_ = row["failed_of_challenge_times"];
		rank_map_entry->map_rank_ = row["map_rank"];
		_rank_maps[rank_map_entry->map_rank_] = rank_map_entry;
	}
	
	PrepareNextSort();
}

void RankMapManager::PrepareNextSort()
{
	int next_sort = (getNextDaySec() - g_server_time) * _TIME_SECOND_MSEL_;

	if (gEventMgr.hasEvent(this, EVENT_DAILY_RANK_) == false)
	{
		gEventMgr.addEvent(this, &RankMapManager::SortMap, EVENT_DAILY_RANK_, next_sort, 1, 0);
	}
	else
	{
		gEventMgr.modifyEventTime(this, EVENT_DAILY_RANK_, next_sort);
	}
}

int RankMapManager::getNextDaySec()
{
	struct tm *ts = localtime(NULL);
	ts->tm_hour = 3;
	ts->tm_min = 0;
	ts->tm_sec = 0;
	int sec = mktime(ts);
	sec = sec + 24 * 60 * 60;
	return sec;
}


s64 RankMapManager::getSortTimeStamp()
{
	return _sort_time_stamp;
}

const std::map<s32, RankMapTg*>* RankMapManager::GetRankMaps()
{
	return &_rank_maps;
}

void RankMapManager::SortMap()
{
	std::vector<RankMapTg*> vcTempRankMap;
	std::map<s32, RankMapTg*>::iterator it = _rank_maps.begin();
	for (; it != _rank_maps.end(); ++ it)
	{
		vcTempRankMap.push_back(it->second);
	}
	_rank_maps.clear();
	std::sort(vcTempRankMap.begin(), vcTempRankMap.end(), &my_greater);
	std::vector<RankMapTg*>::iterator vc_it = vcTempRankMap.begin();
	for (int i = 1; vc_it != vcTempRankMap.end(); ++ vc_it, i ++)
	{
		RankMapTg* entry_map = (*vc_it);
		entry_map->map_rank_ = i;
		_rank_maps[i] = entry_map;
	}
	_sort_time_stamp = g_server_time;
	PrepareNextSort();
}


void RankMapManager::ReqPublishMapList(const message::MsgC2SReqPublishMapList* msg, Session* p)
{
	int req_count = msg->req_count();
	message::MsgS2CPublishMapListACK msgACK;
	msgACK.set_end_map_index(0);
	msgACK.set_req_count(req_count);

	if (req_count != 0)
	{
		int current_count = 0;
		u64 map_begin_index = msg->begin_map_index();
		bool begin = false;
		if (map_begin_index == 0)
		{
			begin = true;
		}
		bool end = false;
		NEW_PUBLISH_MAP::reverse_iterator it = _new_maps.rbegin();
		for (; it != _new_maps.rend(); ++ it)
		{
			const std::vector<message::CrashPlayerPublishMap* >& day_publish_map = it->second;

			std::vector<message::CrashPlayerPublishMap* >::const_reverse_iterator  it_map = day_publish_map.rbegin();
			for (; it_map != day_publish_map.rend(); ++ it_map)
			{
				const message::CrashPlayerPublishMap* publish_map = (*it_map);
				u64 current_map_index = publish_map->crashmap().data().map_index();
				
				if (begin)
				{
					if (current_count < req_count)
					{
						message::CrashPlayerPublishMap* new_map = msgACK.add_maps();
						new_map->CopyFrom(*publish_map);
						//add_map->CopyFrom()
						
						msgACK.set_end_map_index(current_map_index);
					}
					else
					{
						end = true;
						break;
					}
				}

				if (begin == false)
				{
					if (map_begin_index == current_map_index)
					{
						begin = true;
					}
				}
			}
			if (end)
			{
				break;
			}
		}
	}
}

void RankMapManager::PublishMap(const message::MsgC2SReqPlayerPublishMap* msg, CrashPlayer* p)
{
	int passed_time = gGameConfig.GetServerOpenPassedTime(g_server_time);
	std::map<s32, std::vector<message::CrashPlayerPublishMap*> >::iterator it = _new_maps.find(passed_time);

	if (it == _new_maps.end())
	{
		std::vector<message::CrashPlayerPublishMap*> temp;
		_new_maps[passed_time] = temp;
	}

	message::CrashPlayerPublishMap* PublishMapData = new message::CrashPlayerPublishMap();
	PublishMapData->mutable_crashmap()->CopyFrom(msg->map());
	PublishMapData->set_challenge_times(0);
	PublishMapData->set_failed_of_challenge_times(0);
	PublishMapData->set_map_rank(0);
	PublishMapData->set_publish_time(g_server_time);
	_new_maps[passed_time].push_back(PublishMapData);
	message::MsgS2CPlayerPublishMapACK msgACK;
	msgACK.mutable_map()->CopyFrom(*PublishMapData);
	p->sendPBMessage(&msgACK);
}