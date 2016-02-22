#PROJECT(project_gate)  
#¹¤³ÌÃû  
add_definitions(-lexpat)
FILE(GLOB SOURCE_1 "./*.cpp") 
ADD_EXECUTABLE(gate  ${SOURCE_1})
TARGET_LINK_LIBRARIES(gate common.a libprotobuf.a libmysqlpp.so libboost_system-gcc47-1_58.a libboost_thread-gcc47-mt-1_58.a liblog4cplus.so libjsoncpp.a)

