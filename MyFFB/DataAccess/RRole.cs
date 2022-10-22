using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RRole : IRRole
    {
        public RRole(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public int Add(T_Role obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@FCROLEID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD", FCROLEDESC = obj.FCRoleDesc, ACCESSID = obj.AccessID });
                db.Execute("sp_CRUDRole", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@FCROLEID");
            }
        }

        public bool Defunct(T_Role obj, bool stsDefunct)
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

                int result = db.Execute("sp_CRUDRole", new { action = act, FCROLEID = obj.FCRoleID, FCROLEDESC = obj.FCRoleDesc, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_Role obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDRole", new { action = "DELETE", FCROLEID = obj.FCRoleID, FCROLEDESC = obj.FCRoleDesc, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Role obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDRole", new { action = "EDIT", FCROLEID = obj.FCRoleID, FCROLEDESC = obj.FCRoleDesc, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_Role FindByRoleID(int roleid, bool defunct=false)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que = "SELECT * FROM t_Role WHERE ISNULL(FCDefunctInd,0)=@FCDEFUNCTIND AND FCRoleID=@FCROLEID";
                return db.Query<T_Role>(que, new { FCROLEID = roleid, FCDEFUNCTIND=defunct }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_Role> GetListRole(bool? defunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT a.*,c.FCUserID,c.FCName,u.FCUserID,u.FCName  
                    FROM t_Role a 
                    LEFT JOIN t_User c ON a.FCCreatedBy=c.FCUserID
                    LEFT JOIN t_User u ON a.FCUpdatedBy=u.FCUserID
                    WHERE 1=1 ";

                if (defunct != null)
                    que += " AND ISNULL(a.FCDefunctInd,0)=@FCDEFUNCTIND ";

                var result = db.Query<T_Role, T_User, T_User, T_Role>(que, (a, b, c) =>
                {
                    a.CreatedBy = b;
                    a.UpdatedBy = c;
                    return a;
                }, splitOn: "FCUserID, FCUserID", param: new { FCDEFUNCTIND = defunct }).ToList();
                return result;
            }
        }

        public T_Role GetRoleInfo(int roleid)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var dictionary = new Dictionary<string, T_Role>();

                string que = @"SELECT * 
                    FROM t_Role a  
                    INNER JOIN t_RoleDet b ON a.FCRoleID=b.FCRoleID  
                    LEFT JOIN t_Menu c ON c.FCMenuID=b.FCMenuID 
                    LEFT JOIN t_Menu d ON c.FCMenuPID=d.FCMenuID
                    WHERE ISNULL(a.FCDefunctInd,0)=0 AND a.FCRoleID=@FCROLEID 
                    ";

                var result = db.Query<T_Role, T_RoleDet, T_Menu, T_Menu, T_Role>(que, (a, b, c, d) =>
                {
                    T_Role entry;

                    if (!dictionary.TryGetValue(a.FCRoleID.ToString(), out entry))
                    {
                        entry = a;
                        entry.RoleDetail = new List<T_RoleDet>();
                        dictionary.Add(entry.FCRoleID.ToString(), entry);
                    }

                    b.Menu = c;
                    c.RootMenu = d;
                    entry.RoleDetail.Add(b);
                    return entry;

                }, param: new { FCROLEID = roleid }, splitOn: "FCRoleID,FCMenuID,FCMenuID").Distinct().SingleOrDefault();

                return result;

            }
        }
    }
}
