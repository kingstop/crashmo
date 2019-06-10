#include "test_udp_server.h"
#include "test_udp_session.h"
test_udp_server::test_udp_server(int id):udp_server(id)
{

}
test_udp_server::~test_udp_server()
{

}
base_session* test_udp_server::create_session()
{
	base_session*  session = new test_udp_session();
	return session;
}

