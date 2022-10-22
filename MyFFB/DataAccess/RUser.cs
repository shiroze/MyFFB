using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RUser : IRUser
    {
        public RUser(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public int Add(T_User obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@FCUSERID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(
                    new { 
                        action = "ADD", 
                        FCUSERNAME = obj.FCUserName, 
                        FCUSERPASS = obj.Password, 
                        FCNAME = obj.FCName,
                        SetPMKSID = obj.SetPMKSID,
                        FCROLEID = obj.FCRoleID,
                        ACCESSID = obj.AccessID });
                db.Execute("sp_CRUDUser", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@FCUSERID");
            }
        }

        public bool Block(T_User obj, bool stsBlock)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string act;
                if (stsBlock)
                    act = "BLOCK";
                else
                    act = "UNBLOCK";

                int result = 
                    db.Execute("sp_CRUDUser", 
                    new { 
                        action = act, 
                        FCUSERID = obj.FCUserID, 
                        FCUSERNAME = obj.FCUserName,
                        FCUSERPASS = obj.Password,
                        FCNAME = obj.FCName,
                        SetPMKSID = obj.SetPMKSID,
                        FCROLEID = obj.FCRoleID, 
                        ACCESSID = obj.AccessID 
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Defunct(T_User obj, bool stsDefunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string act;
                if (stsDefunct)
                    act = "DEFUNCT";
                else
                    act = "UNDEFUNCT";

                int result = 
                    db.Execute("sp_CRUDUser", 
                    new { 
                        action = act, 
                        FCUSERID = obj.FCUserID, 
                        FCUSERNAME = obj.FCUserName, 
                        FCUSERPASS = obj.Password, 
                        FCNAME = obj.FCName,
                        SetPMKSID = obj.SetPMKSID,
                        FCROLEID = obj.FCRoleID, 
                        ACCESSID = obj.AccessID 
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_User obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = 
                    db.Execute("sp_CRUDUser", 
                    new { 
                        action = "DELETE", 
                        FCUSERID = obj.FCUserID, 
                        FCUSERNAME = obj.FCUserName, 
                        FCUSERPASS = obj.Password, 
                        FCNAME = obj.FCName,
                        SetPMKSID = obj.SetPMKSID,
                        FCROLEID = obj.FCRoleID, 
                        ACCESSID = obj.AccessID 
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_User obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = 
                    db.Execute("sp_CRUDUser", 
                    new { 
                        action = "EDIT", 
                        FCUSERID = obj.FCUserID, 
                        FCUSERNAME = obj.FCUserName, 
                        FCUSERPASS = obj.Password, 
                        FCNAME = obj.FCName, 
                        SetPMKSID=obj.SetPMKSID,
                        FCROLEID = obj.FCRoleID, 
                        ACCESSID = obj.AccessID 
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_User FindUser(int? userid, string usrname = "")
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM t_User u INNER JOIN t_Role r ON u.FCRoleID=r.FCRoleID 
                    WHERE  1=1 ";

                if (userid != null)
                    que += " AND u.FCUserID=@FCUSERID ";

                if (!string.IsNullOrEmpty(usrname))
                    que += " AND u.FCUserName=@FCUSERNAME ";

                var tuser = db.Query<T_User, T_Role, T_User>(que,
                    map: (u, r) =>
                    {
                        u.Role = r;
                        return u;
                    }, param: new { FCUSERID = userid, FCUSERNAME = usrname }, splitOn: "FCRoleID").SingleOrDefault();

                return tuser;
            }
        }

        public List<T_User> GetListUser(int? roleid, bool? blocked, bool? defunct, string name = "")
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @"SELECT u.*,r.*,x.FCUserID,x.FCName,y.FCUserID,y.FCName 
                    FROM t_User u INNER JOIN t_Role r ON u.FCRoleID=r.FCRoleID 
                    LEFT JOIN t_User x ON u.FCCreatedBy=x.FCUserID
                    LEFT JOIN t_User y ON u.FCUpdatedBy=y.FCUserID 
                    WHERE  1=1 ";

                if (roleid != null)
                    que += " AND u.FCRoleID=@FCROLEID ";

                if (blocked != null)
                    que += " AND u.FCBlocked=@FCBLOCKED ";

                if (defunct != null)
                    que += " AND u.FCDefunctInd=@FCDEFUNCTIND ";

                if (!string.IsNullOrEmpty(name))
                    que += " AND (u.FCUserName LIKE '%'+@KEYWORDS+'%' OR u.FCName LIKE '%'+@KEYWORDS+'%' )";

                var result = db.Query<T_User, T_Role, T_User, T_User, T_User>(que,
                    map: (u, r, x, y) =>
                    {
                        u.Role = r;
                        u.CreatedBy = x;
                        u.UpdatedBy = y;
                        return u;
                    }, splitOn: "FCRoleID, FCUserID, FCUserID", param: new { FCROLEID = roleid, FCBLOCKED = blocked, FCDEFUNCTIND = defunct, KEYWORDS = name }).ToList();

                return result;
            }
        }

        public T_User LoginUser(string usrname, string usrpass)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                DynamicParameters p = new DynamicParameters();
                p.Add("@FCUSERID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@FCROLEID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@FCNAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                p.Add("@FCROLEDESC", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                //p.Add("FCFirstLogin", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                //p.Add("@AreaOperational", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                //p.Add("@Regional", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                p.Add("@MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                p.AddDynamicParams(new { FCUSERNAME = usrname, FCUSERPASS = usrpass });
                db.Execute("sp_Login", p, commandType: CommandType.StoredProcedure);

                T_User resultobj = new T_User();
                resultobj.MsgResult = p.Get<string>("@MESSAGE");
                if (resultobj.MsgResult == "OK" || resultobj.MsgResult == "PASSWORD EXPIRED" )/*|| resultobj.MsgResult == "FIRST LOGIN"*/
                {
                    resultobj.FCUserID = p.Get<int>("@FCUSERID");
                    resultobj.FCRoleID = p.Get<int>("@FCROLEID");
                    resultobj.FCName = p.Get<string>("@FCNAME");
                    //resultobj.FCFirstLogin= p.Get<bool>("@FCFirstLogin");
                    //resultobj.AreaOperational = p.Get<string>("@AreaOperational");
                    //resultobj.Regional = p.Get<string>("@Regional");

                    T_Role role = new T_Role();
                    role.FCRoleID = p.Get<int>("@FCROLEID");
                    role.FCRoleDesc = p.Get<string>("@FCROLEDESC");
                    resultobj.Role = role;
                }
                //GlobalVariable.GV_AreaOperational = resultobj.AreaOperational;
                //GlobalVariable.GV_Regional = resultobj.Regional;
                return resultobj;
            }
        }

        public T_User KeycloakUserLogin(string usremail)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                DynamicParameters p = new DynamicParameters();
                p.Add("@FCUSERID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@FCUSERNAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                p.Add("@FCROLEID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@FCNAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                p.Add("@FCROLEDESC", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                p.Add("@MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                p.AddDynamicParams(new { FCUSEREMAIL = usremail });
                db.Execute("sp_Login_Keycloak", p, commandType: CommandType.StoredProcedure);

                T_User resultobj = new T_User();
                resultobj.MsgResult = p.Get<string>("@MESSAGE");
                if (resultobj.MsgResult == "OK")
                {
                    resultobj.FCUserID = p.Get<int>("@FCUSERID");
                    resultobj.FCUserName = p.Get<string>("@FCUSERNAME");
                    resultobj.FCRoleID = p.Get<int>("@FCROLEID");
                    resultobj.FCName = p.Get<string>("@FCNAME");

                    T_Role role = new T_Role();
                    role.FCRoleID = p.Get<int>("@FCROLEID");
                    role.FCRoleDesc = p.Get<string>("@FCROLEDESC");
                    resultobj.Role = role;
                }
                //GlobalVariable.GV_AreaOperational = resultobj.AreaOperational;
                //GlobalVariable.GV_Regional = resultobj.Regional;
                return resultobj;
            }
        }

        public bool Reset(T_User obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = 
                    db.Execute("sp_CRUDUser", 
                    new { 
                        action = "RESET", 
                        FCUSERID = obj.FCUserID, 
                        FCUSERNAME = obj.FCUserName, 
                        FCUSERPASS = obj.Password,
                        FCNAME = obj.FCName,
                        SetPMKSID = obj.SetPMKSID,
                        FCROLEID = obj.FCRoleID,
                        ACCESSID = obj.AccessID 
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        //public bool FirstLogin(T_User obj)
        //{
        //    using (IDbConnection db = DALSecurity.GetSqlConnection)
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();
        //        int result =
        //            db.Execute("sp_CRUDUser",
        //            new
        //            {
        //                action = "FIRSTLOGIN",
        //                FCUSERID = obj.FCUserID,
        //                FCUSERNAME = obj.FCUserName,
        //                FCUSERPASS = obj.Password,
        //                FCNAME = obj.FCName,
        //                SetPMKSID = obj.SetPMKSID,
        //                FCROLEID = obj.FCRoleID,
        //                ACCESSID = obj.AccessID
        //            }, commandType: CommandType.StoredProcedure);
        //        return result != 0;
        //    }
        //}

        public bool EditPassword(T_User obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDUser",
                    new
                    {
                        action = "EDITPASS",
                        FCUSERID = obj.FCUserID,
                        FCUSERNAME = obj.FCUserName,
                        FCUSERPASS = obj.Password,
                        FCNAME = obj.FCName,
                        SetPMKSID = obj.SetPMKSID,
                        FCROLEID = obj.FCRoleID,
                        ACCESSID = obj.AccessID
                    }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }
    }
}
