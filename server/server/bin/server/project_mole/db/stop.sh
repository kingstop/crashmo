#!/bin/bash 

echo  "## stop ... ##"
cat ./pid_temp.pid | while read line
do
  kill $line
done 
   

