#PROJECT(project_db)  

#¹¤³ÌÃû  

FILE(GLOB SOURCE_1 "./*.cpp") 

ADD_EXECUTABLE(dbserver  ${SOURCE_1})
set(CMAKE_EXE_LINKER_FLAGS "-lpthread -lrt")
TARGET_LINK_LIBRARIES(dbserver common.a libmsg.a libprotobuf.a libmysqlpp.so libmysqlclient.so libboost_system.so libboost_thread.so liblog4cplus.so libjsoncpp.a)


