#!/bin/bash
cd ../../../build/
cmake ../
make
cd ../bin/server/project_mole/
mv dbserver  ./db/cb_db
mv gameserver ./gs/cb_gs
mv gate ./gt/cb_gt
mv login ./lg/cb_lg


