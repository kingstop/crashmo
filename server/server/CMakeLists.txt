PROJECT(project_mole)

CMAKE_MINIMUM_REQUIRED(VERSION 2.8) 
INCLUDE(FindBoost)
IF(LINUX)
    MESSAGE("LINUX")
ELSEIF(WIN32)
    MESSAGE("WIN32")
ELSE()
 MESSAGE("not found")
ENDIF()

MESSAGE(${PROJECT_SOURCE_DIR})
MESSAGE(${PROJECT_SOURCE_DIR}/../../../../../external/enet-1.3.13/include/)

INCLUDE_DIRECTORIES( 
    ${PROJECT_SOURCE_DIR}/include 
    ${PROJECT_SOURCE_DIR}/include/linklist
    ${PROJECT_SOURCE_DIR}/../../../../../external/boost/boost_1_59_0/
    ${PROJECT_SOURCE_DIR}/../../../../../external/mysql++-3.2.2/lib/
    ${PROJECT_SOURCE_DIR}/../../../../../external/google/protobuf/src/
    ${PROJECT_SOURCE_DIR}/../../../../../external/jsoncpp/include/
    ${PROJECT_SOURCE_DIR}/../../../../../external/log4cplus/include/
    ${PROJECT_SOURCE_DIR}/../../../../../external/mysql-5.7.10/build/include/
    ${PROJECT_SOURCE_DIR}/../../../../../external/mysql-5.7.10/include/ 
    ${PROJECT_SOURCE_DIR}/../../../../../external/enet/include/
    /root/work/external/enet/include/
    ./server/common/
    ./common/
    ./server/msg/
    ./msg/
    /home/mysql/include/
    /usr/include/mysql/
   /usr/include/ 
   /usr/local/include/
) 

LINK_DIRECTORIES( 
    ${PROJECT_SOURCE_DIR}/lib 
    ${PROJECT_SOURCE_DIR}/../../../../../external_lib/boost/
    ${PROJECT_SOURCE_DIR}/../../../../../external_lib/protobuf/  
    ${PROJECT_SOURCE_DIR}/../../../../../external_lib/jsoncpp/  
    ${PROJECT_SOURCE_DIR}/../../../../../external_lib/log4cplus/  
    ${PROJECT_SOURCE_DIR}/../../../../../external_lib/mysql++/ 
    ${PROJECT_SOURCE_DIR}/../../../../../external_lib/enet/ 
    /home/mysql/lib/ 
    /usr/local/lib 
    /usr/lib 
) 
 
SET(EXECUTABLE_OUTPUT_PATH ${PROJECT_BINARY_DIR}/../bin/server/${PROJECT_NAME}/ ) 
set(CMAKE_EXE_LINKER_FLAGS "-lpthread -lrt")
SET(LIBRARY_OUTPUT_PATH ${PROJECT_BINARY_DIR}/../lib) 

add_definitions(-DDEBUG -pthread)
add_definitions("-g")
add_definitions("-std=c++11")
add_definitions(-DLOG4CXX_STATIC)
ADD_SUBDIRECTORY(server)

