using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using message;

public class CrashPlayer
{
    public CrashPlayer()
    {

    }
    ~CrashPlayer()
    {

    }

    public void SetInfo(CrashPlayerInfo info)
    {
        _info = info;
    }

    public CrashPlayerInfo GetInfo()
    {
        return _info;
    }

    protected  CrashPlayerInfo _info;
}
