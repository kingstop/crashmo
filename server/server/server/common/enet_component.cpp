#include "enet_component.h"
#include "enet/enet.h"

enet_component::enet_component():_exit(false),_cpu_wait(false)
{

}
enet_component::~enet_component()
{

}

bool enet_component::is_exit()
{
	return _exit;
}

void enet_component::extra_process(bool is_wait)
{

}

bool enet_component::is_cpu_wait()
{
	return _cpu_wait;
}

void enet_component::set_exit(bool exit)
{
	_exit = exit;
}
