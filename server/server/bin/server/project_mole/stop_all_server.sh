#/bim/bash/
echo "stop login server"
cd ./lg
./stop.sh

echo "stop gate server"
cd ./../gt
./stop.sh

echo "stop game server"
cd ./../gs
./stop.sh 

echo "stop db server"

cd ./../db
./stop.sh

