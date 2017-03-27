#pragma once

struct RankMapTg
{
	message::CrashMapData info_;
	u64 publish_time_;
	s32 challenge_times_;
	s32 failed_of_challenge_times_;
	s32 map_rank_;
	std::vector< message::RankMapBlogEntry> blogs_;
	void Copy(message::CrashPlayerPublishMap* entry)
	{
		entry->mutable_crashmap()->CopyFrom(info_);
		entry->set_challenge_times(challenge_times_);
		entry->set_failed_of_challenge_times(failed_of_challenge_times_);
		entry->set_publish_time(publish_time_);
		std::vector< message::RankMapBlogEntry>::iterator it = blogs_.begin();
		for (; it != blogs_.end(); ++ it)
		{
			entry->add_map_blog()->CopyFrom(*it);
		}
		
	}
};

class RankMapManager : public EventableObject
{
	typedef std::map<s32, std::vector<message::CrashPlayerPublishMap* >> NEW_PUBLISH_MAP;
public:	
	RankMapManager();
	virtual ~RankMapManager();
public:	
	void Init(DBQuery* p);
	void SortMap();
	s64 getSortTimeStamp();
	const std::map<s32, RankMapTg*>* GetRankMaps();
	void PublishMap(const message::MsgC2SReqPlayerPublishMap* msg, CrashPlayer* p);
	void ReqPublishMapList(const message::MsgC2SReqPublishMapList* msg, Session* p);
protected:
	int getNextDaySec();
	void PrepareNextSort();
protected:
	std::map<s32, RankMapTg*> _rank_maps;
	NEW_PUBLISH_MAP _new_maps;
	s64 _sort_time_stamp;

};

