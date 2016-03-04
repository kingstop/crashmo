#ifndef __db_quest_h__
#define __db_quest_h__

#include "message/msg_game_db.pb.h"
class DBQuestManager
{
public:
	void queryInfo(account_type a, tran_id_type t, u16 gs);
	void saveCharacterInfo(message::ReqSaveCharacterData* msg);
	void saveOfficilMap(message::gs2dbSaveOfficileMapReq* msg);
	//void saveOfficilMap(message::MsgSaveOfficilMapReq* msg, tran_id_type t);
	//void officilMapReq(message::MsgOfficilMapReq* msg, tran_id_type t);
	void delOfficilMap(int section, int number);
protected:
	void dbDoQueryCharacter(DBQuery* p, const void* d);
	void dbCallQueryCharacter(const void* d, bool s);
    void dbCallNothing(const SDBResult* , const void* , bool){;}
protected:


private:

};

#endif

