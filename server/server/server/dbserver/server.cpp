#include "stdafx.h"

bool DBServer::init()
{
	ServerFrame::init();
	DataBaseConfig dbconfig;
	if (!ServerFrame::loadDBConfig(dbconfig, CENTER_DATABASE_CONFIG))
	{
		Mylog::log_server( LOG_ERROR, "load database config failed !");
		return false;
	}

	if (!Database::addBlockTask(dbconfig, this, &DBServer::initDataFromDatabase, NULL))
	{
		Mylog::log_server( LOG_ERROR, "Init Server from DataBase Error!");
		return false;
	}

	Mylog::log_server( LOG_INFO, "Start DBServer v:[%u] Success!", gDBVersion.toString().c_str());
	return true;
}

bool DBServer::onKey()
{
	return true;
}

void DBServer::runOnce(u32 nDiff)
{
	net_global::update_net_service();
	gDBServerTime = time(NULL);
	gDBCharDatabase.run(nDiff);
	gDBGameServer.run_no_wait();

	if (nDiff > 800)
	{
		Mylog::log_server(LOG_WARNING, "server delay [%u]", nDiff);
	}
}

void DBServer::shutDown()
{
	gDBCharDatabase.shutDown();
	gDBGameServer.stop();
}

void DBServer::memoryLog() 
{

}

void DBServer::setStop()
{

}

void DBServer::checkStop()
{

}

u16  DBServer::getCityGS(u32 city)
{
	u16 gs = INVALID_GAME_ID; 
	m_cityServers.getData(city, gs);
	return gs;
}

bool DBServer::initDataFromCharDB(DBQuery* p, const void* data)
{
	return true;
};



bool DBServer::initDataFromWorldDB(DBQuery* p, const void* data)
{
	DBQuery& query = *p;
	query.reset();
	return true;

}



bool DBServer::initDataFromDatabase(DBQuery* p, const void* data)
{
	if (!p)
	{ 
		return false;
	}

	net_info user_net;
	DataBaseConfig db_config;
	DataBaseConfig db_world_config;
	DBQuery& query = *p;
	DBQParms parms;
	parms << _DB_LISNEN_GAME_;
	query << "SELECT * FROM `net_config` WHERE `Category` = %0";
	query.parse();
	if (!ServerFrame::getNetConfig(user_net, query.use(parms)))
	{ 
		Mylog::log_server( LOG_ERROR, "Init login server net config failed !");
		return false;
	}

	if(!gDBGameServer.create( user_net._port, user_net._max_connect, user_net._thread_count))
	{
		Mylog::log_server( LOG_ERROR, "dbserver listen game Init NetConfig failed!");
		return false;
	}

	Mylog::log_server( LOG_INFO, "dbserver begin lister port[%d], MaxConnect[%d], ThreadCount[%d]!", user_net._port, user_net._max_connect, user_net._thread_count);

	query.reset();
	parms.clear();
	parms << _FU_CHARACTER_DATABASE_;
	query << "SELECT * FROM `database_config` WHERE `Category` = %0";
	query.parse();
	if (!ServerFrame::getDatabaseConfig(db_config, query.store(parms)))
	{ 
		Mylog::log_server( LOG_ERROR, "load character db config failed !");
		return false;
	}

	if (!Database::addBlockTask(db_config, this, &DBServer::initDataFromCharDB, NULL))
	{
		Mylog::log_server( LOG_ERROR, "Init Server from character DataBase Error!");
		return false;
	}

	if (!gDBCharDatabase.initDatabase(db_config))
	{
		Mylog::log_server(LOG_ERROR, "start character db service failed!");
	}    
	
	parms.clear();
	query.reset();
	parms << _FU_WORLD_DATABASE_;
	query << "SELECT * FROM `database_config` WHERE `Category` = %0";
	query.parse();
	if (!ServerFrame::getDatabaseConfig(db_world_config, query.store(parms)))
	{ 
		Mylog::log_server( LOG_ERROR, "load world db config failed !");
		return false;
	}


	if (gWorldDatabase.initDatabase(m_world_db_conf) == false)
	{
		Mylog::log_server(LOG_ERROR, "init world db failed !");
		return false;
	}


	if (!Database::addBlockTask(db_world_config, this, &DBServer::initDataFromWorldDB, NULL))
	{
		Mylog::log_server( LOG_ERROR, "Init Server from character DataBase Error!");
		return false;
	}
	return true;
}

