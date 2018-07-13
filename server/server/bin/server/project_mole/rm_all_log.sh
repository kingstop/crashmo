#/bim/bash/
echo "remove login log"
cd ./lg
./remove_log.sh

echo "remove gate log"
cd ./../gt
./remove_log.sh

echo "remove game log"
cd ./../gs
./remove_log.sh 

echo "remove db log"

cd ./../db
./remove_log.sh

