#pragma once
struct RankMapBlogEntry
{
	account_type acc_;
	std::string name_;
	std::string suggest_;
	u32 time_;
};
struct RankMapTg
{
	message::CrashMapData info_;
	u64 publish_time_;
	s32 challenge_times_;
	s32 failed_of_challenge_times_;
	s32 map_rank_;
	std::map<u32, RankMapBlogEntry> blog_;
};

class RankMapManager : public EventableObject
{

public:	
	RankMapManager();
	virtual ~RankMapManager();
public:
	
	void Init(DBQuery* p);
	void SortMap();
protected:
	int getNextDaySec();
	void PrepareNextSort();
protected:
	std::map<s32, RankMapTg*> _rank_maps;

};

