#include "mapstorage.h"

#include "database.h"

/* 

mapstorage::mapstorage(void)

{

	//open("MapData.pb",O_CREAT|O_TRUNC|O_RDWR,0644); 

	//boost::system::error_code err;

	//int fd = boost::asio::detail::descriptor_ops::open("MapData.pb", O_CREAT|O_TRUNC|O_RDWR, err);

	

	FILE * fp = fopen("MapData.pb","rb");

	if (fp != NULL)

	{

		fseek(fp, 0, SEEK_END);

		int length = ftell(fp);

		fseek(fp, 0, SEEK_SET);

		char* buff = new char[length];



		fread(buff, length, 1, fp);

		//_TileMap.ParseFromFileDescriptor()

		bool suc = false;

		suc = _TileMap.ParseFromArray(buff, length);



		fclose(fp);

		//delete[] buff;

	}

	



	

	_TileMap.groupdatalist().size();



	int t_s = _TileMap.groupdatalist().size();

	int tt_s =  _TileMap.tiledatalist().size();

	for (int i = 0; i < t_s; i ++)

	{



		const message::TileGroupProto* proto = &_TileMap.groupdatalist(i);	

		tile_group temp_group;

		temp_group.min_x_ = proto->startindex().x();

		temp_group.min_y_ = proto->startindex().y();



		temp_group.max_x_ = temp_group.min_x_ + proto->size().x();

		temp_group.max_y_ = temp_group.min_y_ + proto->size().y();



		if (proto->islocked() == false)

		{

			_unlock_group.insert(MapTileGroup::value_type(proto->id(), temp_group));

		}

		else

		{

			_lock_group.insert(MapTileGroup::value_type(proto->id(), temp_group));



		}

		_source_group.insert(MapSourceTileGroup::value_type(proto->id(), proto));

	}





	for (int i = 0; i < tt_s; i ++)

	{

		const message::TileProto* proto = &_TileMap.tiledatalist(i);

		proto->index().x();

		proto->index().y();

		_mole_farm_config[proto->index().x()][proto->index().y()] = proto->tiletype();

	}

 

}





mapstorage::~mapstorage(void)

{

}





const mapstorage::MapTileGroup& mapstorage::getUnLockGroup()

{

	return _unlock_group;

}





u32 mapstorage::get_area_flag(int pos_x, int pos_y)

{

	return _mole_farm_config[pos_x][pos_y];

}





void mapstorage::init_from_wolrd_db(DBQuery* p)

{

	p->reset();

	DBQuery& query = *p;

	query << "SELECT * FROM construction_map_config";

	query.parse();

	SDBResult result = query.store();

	for (u16 i = 0 ; i < result.num_rows(); ++i)

	{

		DBRow row = result[i];

		mole_construction_begin entry;

		entry.congfig_id = row["config_id"];

		entry.pos_x = row["pos_x"];

		entry.pos_y = row["pos_y"];

		entry.level = row["level"];

		_begin_constructions.push_back(entry);

	}



	query.reset();

	result.clear();

	query << "SELECT * FROM resource_config";

	query.parse();

	result = query.store();

	for (u16 i = 0 ; i < result.num_rows(); ++i)

	{

		DBRow row = result[i];

		

		int resource_config_id = row["resource_config_id"];

		int resource_count = row["resource_count"];

		_begin_resource[resource_config_id] = resource_count;

	}

}



const std::vector<mole_construction_begin>& mapstorage::get_begin_constructions()

{

	return _begin_constructions;

}



const std::map<int, int>& mapstorage::get_begin_resource()

{

	return _begin_resource;

}

*/

