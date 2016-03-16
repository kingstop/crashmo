#PROJECT(project_gate)  
#¹¤³ÌÃû  

set(CMAKE_EXE_LINK_FLAGS,"-lexpat -lapr-1 -lpthread")
#add_definitions(-lexpat -lapr-1 -lpthread)
FILE(GLOB SOURCE_1 "./*.cpp") 
ADD_EXECUTABLE(dbserver  ${SOURCE_1})
TARGET_LINK_LIBRARIES(dbserver libprotobuf.so common.a libmysqlpp.so libboost_system.a libboost_thread.a liblog4cplus.so libjsoncpp.a)

