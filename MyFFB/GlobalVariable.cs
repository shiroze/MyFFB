using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Web;

//namespace MyAttd
//{
public class GlobalVariable
{
    static string _gv_AreaOperational;
    static string _gv_Regional;
    static string _tanggal;
    static string _aliasDomain;

    public static string GV_AreaOperational
    {
        get { return _gv_AreaOperational; }
        set { _gv_AreaOperational = value; }
    }
    public static string GV_Regional
    {
        get { return _gv_Regional; }
        set { _gv_Regional = value; }
    }
    public static string Tanggal
    {
        get { return _tanggal; }
        set { _tanggal = value; }
    }
    public static string GV_AliasDomain
    {
        get { return _aliasDomain; }
        set { _aliasDomain = value; }
    }
}

//}
