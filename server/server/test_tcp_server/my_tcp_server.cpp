#include "my_tcp_server.h"
#include "my_tcp_session.h"

my_tcp_server::my_tcp_server():tcp_server(1)
{

}
my_tcp_server::~my_tcp_server()
{

}

base_session* my_tcp_server::create_session()
{
	return new my_tcp_session();
}
void my_tcp_server::run()
{
	tcp_server::run();
}