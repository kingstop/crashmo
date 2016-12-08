using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class RankMapManager
{
	protected long _time_stamp;
	protected Dictionary<int, message.CrashPlayerPublishMap> _RankMaps = new Dictionary<int, message.CrashPlayerPublishMap>();
	public RankMapManager ()
	{

	}

	public void LoadMap()
	{
		_time_stamp = 0;
		_RankMaps.Clear ();
		message.MsgC2SRankMapReq msg = new message.MsgC2SRankMapReq();
		msg.rank_begin = -1;
		msg.map_count = 5;
		msg.time_stamp = 0;
		global_instance.Instance._net_client.send (msg);
	}

	public void parseRankMapACK(message.MsgS2CRankMapACK msg)
	{
		_time_stamp = msg.time_stamp;
		int rank_index = -1;
		int count_rank = msg.rank_map_count;
		foreach (KeyValuePair<int,message.CrashPlayerPublishMap> entry_pair in _RankMaps) 
		{
			message.CrashPlayerPublishMap entry = entry_pair.Value;
			rank_index = entry.map_rank;
			_RankMaps [rank_index] = entry;
		}
		if (_RankMaps.Count == count_rank) 
		{
			EndLoadMap ();
		} 
		else 
		{
			message.MsgC2SRankMapReq MsgReq = new message.MsgC2SRankMapReq();
			MsgReq.rank_begin = rank_index;
			MsgReq.map_count = 5;
			MsgReq.time_stamp = _time_stamp;
			global_instance.Instance._net_client.send (msg);
		}
	}

	protected void EndLoadMap()
	{

	}
}

