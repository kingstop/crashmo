#include "stdafx.h"
#include "session.h"
#include "CrashPlayer.h"
//#include "NoneCharacter.h"
//#include "NoneCharacterManager.h"

/************************************************************************/

/*                          注册消息实例                                */  

/*  message::MsgDB2GSQueryCharResult 注册回调    prasePBTest             */

/************************************************************************/
typedef void(Session::*pn_msg_cb)(google::protobuf::Message*);
static std::map<std::string, pn_msg_cb > static_session_cb_funs;
static void registerCBFun(std::string str, pn_msg_cb fun)
{
    static_session_cb_funs.insert(std::make_pair(str, fun));
}

void Session::prasePBDefault(google::protobuf::Message* p)
{
     Mylog::log_server(LOG_INFO, "Parse message name [%s]", p->GetTypeName().c_str());
}


//这里负责注册消息
void Session::registerPBCall()
{
	registerCBFun(PROTOCO_NAME(message::MsgSaveMapReq), &Session::parseSaveMap);
	registerCBFun(PROTOCO_NAME(message::MsgDelMapReq), &Session::parseDelMap);
	registerCBFun(PROTOCO_NAME(message::MsgModifySectionNameReq), &Session::parseModifySectionName);
	registerCBFun(PROTOCO_NAME(message::MsgSectionNameReq), &Session::parseGetSectionNameReq);
	registerCBFun(PROTOCO_NAME(message::MsgC2SRankMapReq), &Session::parseReqRankMap);
	registerCBFun(PROTOCO_NAME(message::MsgC2SOfficeMapReq), &Session::parseReqOfficilMap);
	registerCBFun(PROTOCO_NAME(message::MsgC2SOfficeStatusReq), &Session::parseReqOfficilStatus);
	registerCBFun(PROTOCO_NAME(message::MsgC2SReqLoadTaskConfigs), &Session::parseReqLoadTaskConfigs);
	registerCBFun(PROTOCO_NAME(message::MsgC2SReqModifyTaskInfo), &Session::parseReqModifyTaskInfo);
	registerCBFun(PROTOCO_NAME(message::MsgC2SReqPassOfficilMap), &Session::parsePassOfficilGame);

}

void Session::parsePBMessage(google::protobuf::Message* p)
{
    std::map<std::string, pn_msg_cb >::iterator it = static_session_cb_funs.find(p->GetTypeName());
    if (it != static_session_cb_funs.end())
    {
        pn_msg_cb fun = boost::ref( it->second);
        if ( NULL != fun )
        {
            (this->*fun)(p);
            return ;
        }
    }
    prasePBDefault(p);
}

//////////////////////////////////////////////////////////////////////////

Session::Session(tran_id_type t, account_type a, u16 gate)
    :m_tranid(t), m_account(a), m_gate(gate),m_state(_session_online_)/*,_character(NULL)*/
{
	_player = gPlayerManager.GetPlayer(m_account);
	if (_player == NULL)
	{
		message::ApplyCharacterDataReq msg;
		msg.set_account(m_account);
		gGSDBClient.sendPBMessage(&msg, m_tranid);
	}
	else
	{
		_player->SetSession(this);
		_player->StartSave();
		CreateCrashMoClientInit();
	}
}

Session::~Session()
{

}


void Session::setPlayer(CrashPlayer* p)
{
	_player = p;
}

void Session::queryDBPlayerInfo()
{

}

void Session::close()
{
	if (_player)
	{
		_player->StartDeleteClock();
	}
}

void Session::setReconnet()
{
    m_state = _session_online_;
}

void Session::setWaitReconnet()
{
    m_state = _session_offline_;
}

void Session::sendPBMessage(google::protobuf::Message* p)
{
    if (m_state == _session_online_)
    {
	    gGSGateManager.sendMessage(p, m_tranid, m_gate);
    }
	else
    {

    }
}


void Session::parseGetSectionNameReq(google::protobuf::Message* p)
{
	if (_player != NULL)
	{
		message::MsgSectionNameACK msg;
		CHAPTERSNAMES sections = gOfficilMapManager.getSectionNames();
		CHAPTERSNAMES::iterator it = sections.begin();
		for (; it != sections.end(); ++ it)
		{
			message::MsgIntStringProto* proto = msg.add_sections();
			proto->set_intger_temp(it->first);
			proto->set_string_temp(it->second.c_str());
		}
		sendPBMessage(&msg);
	}
}

void Session::parseReqLoadTaskConfigs(google::protobuf::Message* p)
{
	message::MsgC2SReqLoadTaskConfigs* msg = (message::MsgC2SReqLoadTaskConfigs*)p;
	const TaskManager::TASKCONFIGS* task_configs = gTaskManager.GetTaskConfigs();
	message::MsgS2CLoadTaskConfigsACK msgACK;
	msgACK.set_total_task_count(task_configs->size());
	int begin_id = msg->begin_id();
	int count = msg->load_count();
	TaskManager::TASKCONFIGS::const_iterator it = task_configs->begin();
	for (; it != task_configs->end(); ++ it)
	{
		const message::TaskInfoConfig& info = it->second;
		if (info.task_id() <= begin_id)
		{
			continue;
		}		
		message::TaskInfoConfig* entry = msgACK.add_task_configs();
		entry->CopyFrom(info);
	}
	sendPBMessage(&msgACK);
}

void Session::parseEnterOfficilGame(google::protobuf::Message* p)
{
	message::MsgC2SReqEnterOfficilMap* msg = (message::MsgC2SReqEnterOfficilMap*)p;

}

void Session::parsePassOfficilGame(google::protobuf::Message* p)
{
	message::MsgC2SReqPassOfficilMap* msg = (message::MsgC2SReqPassOfficilMap*)p;
	if (_player)
	{
		_player->PassOfficilMap(msg->chapter_id(), msg->section_id(), msg->use_step(), msg->use_time());
	}

}

void Session::parseReqModifyTaskInfo(google::protobuf::Message* p)
{
	message::MsgC2SReqModifyTaskInfo* msg = (message::MsgC2SReqModifyTaskInfo*)p;
	gTaskManager.AddTask(msg->info());
	message::MsgS2CModifyTaskInfoACK msgACK;
	msgACK.mutable_info()->CopyFrom(msg->info());
	sendPBMessage(&msgACK);
}

void Session::parseModifySectionName(google::protobuf::Message* p)
{
	if (_player != NULL)
	{
		message::MsgModifySectionNameReq* msg = (message::MsgModifySectionNameReq*)p;
		gOfficilMapManager.modifySectionName(msg->section(), msg->section_name().c_str(), _player);
	}
	
}

void Session::parseSaveMap(google::protobuf::Message* p)
{
	if (_player != NULL)
	{
		message::MsgSaveMapReq* msg = (message::MsgSaveMapReq*)p;
		if (msg->save_type() == message::OfficeMap)
		{
			gOfficilMapManager.saveMapOfficilMap(msg->mutable_map(), _player);
		}
		else
		{
			_player->SaveMap(msg);
		}
	}
}

void Session::parseDelMap(google::protobuf::Message* p)
{
	if (_player != NULL)
	{
		message::MsgDelMapReq* msg = (message::MsgDelMapReq*)p;
		_player->DelMap(msg);
	}
}


void Session::CreateCrashMoClientInit()
{
	message::CrashmoClientInit msg;
	const CHAPTERSNAMES sessionNames = gOfficilMapManager.getSectionNames();
	CHAPTERSNAMES::const_iterator it = sessionNames.begin();
	for (; it != sessionNames.end(); ++it)
	{
		message::MsgIntStringProto* pair_temp = msg.add_chapter_names();
		pair_temp->set_intger_temp(it->first);
		pair_temp->set_string_temp(it->second.c_str());
	}
	const MapConfig* config_entry = gGameConfig.getMapConfig();
	msg.set_map_height_config_max(config_entry->config_heigth_max_);
	msg.set_map_width_config_max(config_entry->config_width_max_);
	msg.set_map_count_max(config_entry->config_count_max_);
	std::map<int,int>::const_iterator it_group_config = config_entry->group_config_max_count_.begin();
	for (; it_group_config != config_entry->group_config_max_count_.end(); it_group_config++)
	{
		message::intPair* pairEntry =msg.add_resources_config_max();
		pairEntry->set_number_1(it_group_config->first);
		pairEntry->set_number_2(it_group_config->second);
	}
	message::CrashPlayerInfo* info = msg.mutable_info();
	info->CopyFrom(_player->GetInfo());
	sendPBMessage(&msg);
}

void Session::createInfo()
{
	_player = gPlayerManager.CreatePlayer(m_account, this);
	CreateCrashMoClientInit();
	_player->StartSave();
}

void Session::createInfo(message::CharacterDataACK* msg)
{
	_player = gPlayerManager.CreatePlayer(m_account, this, msg);
	CreateCrashMoClientInit();
	_player->StartSave();
}

void Session::parseReqOfficilStatus(google::protobuf::Message* p)
{
	message::MsgC2SOfficeStatusReq* msg = (message::MsgC2SOfficeStatusReq*)p;
	message::MsgS2COfficeStatusACK msgACK;
	const OFFICILMAPLIST* officilmap = gOfficilMapManager.getOfficilMap();
	OFFICILMAPLIST::const_iterator it_const = officilmap->begin();
	for (; it_const != officilmap->end(); ++ it_const)
	{
		msgACK.add_chapter_id(it_const->first);		
	}
	sendPBMessage(&msgACK);
}

void Session::parseReqOfficilMap(google::protobuf::Message* p)
{
	message::MsgC2SOfficeMapReq* msg = (message::MsgC2SOfficeMapReq*)p;
	int chapter_id = msg->chapter_id();
	int section_id = msg->section_id();
	int map_count = msg->map_count();
	int current_count = 0;

	message::MsgS2COfficeMapACK msgACK;
	msgACK.set_section_count(0);
	msgACK.set_chapter_id(chapter_id);
	msgACK.set_section_id(0);
	const OFFICILMAPLIST* officilmap = gOfficilMapManager.getOfficilMap();
	OFFICILMAPLIST::const_iterator it_const = (*officilmap).find(chapter_id);
	if (it_const != officilmap->end())
	{
		msgACK.set_section_count(it_const->second.size());
		const std::map<int, message::CrashMapData>& map_entry = it_const->second;
		msgACK.set_section_count(map_entry.size());

		std::map<int, message::CrashMapData>::const_iterator it_section = map_entry.begin();
		for (; it_section != map_entry.end(); ++ it_section)
		{
			if (it_section->first <= section_id)
			{
				continue;
			}
			else
			{
				current_count++;
				message::CrashMapData* mapEntry = msgACK.add_maps();
				mapEntry->CopyFrom(it_section->second);
				msgACK.set_section_id(it_section->first);
				if (current_count >= map_count)
				{
					break;
				}
			}
		}
	}
	sendPBMessage(&msgACK);
}
void Session::parseReqRankMap(google::protobuf::Message* p)
{
	message::MsgC2SRankMapReq* msg = (message::MsgC2SRankMapReq*)p;
	message::MsgS2CRankMapACK msgACK;
	const std::map<s32, RankMapTg*>* rankMaps = gRankMapManager.GetRankMaps();
	msgACK.set_time_stamp(gRankMapManager.getSortTimeStamp());
	msgACK.set_rank_map_count(rankMaps->size());
	int map_count = msg->map_count();
	int current_count = 0;
	std::map<s32, RankMapTg*>::const_iterator it_rank_map = rankMaps->begin();
	for (; it_rank_map != rankMaps->end(); ++ it_rank_map)
	{
		if (it_rank_map->first <= msg->rank_begin())
		{
			continue;
		}
		else
		{
			RankMapTg* RankEntry = it_rank_map->second;
			current_count++;
			message::CrashPlayerPublishMap entryRankMap;
			RankEntry->Copy(&entryRankMap);
			msgACK.set_end_rank(it_rank_map->first);
		}
	}
	sendPBMessage(&msgACK);
}
