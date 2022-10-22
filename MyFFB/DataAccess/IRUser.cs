using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRUser
    {
        List<T_User> GetListUser(int? roleid, bool? blocked, bool? defunct, string name = "");
        T_User FindUser(int? userid, string usrname = "");
        T_User LoginUser(string usrname, string usrpass);

        T_User KeycloakUserLogin(string usremail);

        int Add(T_User obj);
        bool Edit(T_User obj);
        bool Defunct(T_User obj, bool stsDefunct);
        bool Delete(T_User obj);
        bool Reset(T_User obj);
        bool EditPassword(T_User obj);
        //bool FirstLogin(T_User obj);
        bool Block(T_User obj, bool stsBlock);
    }
}
