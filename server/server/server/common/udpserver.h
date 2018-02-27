#pragma once
#include <enet/enet.h>
#include <list>
#include <map>
#include "common_type.h"


extern int InitEnet();

extern void DeinitEnet();

class udp_server
{
public:
	udp_server();
	virtual ~udp_server();
public:
	typedef std::map<u32, ENetPeer*> MAPNETPEERS;
public:
	bool Init(u32 host, s32 port);
	void Run();
protected:
	ENetAddress _address;
	ENetHost * _server;
	MAPNETPEERS _netPeers;
	unsigned long _connect_index;

};

