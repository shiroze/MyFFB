using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RMenu : IRMenu
    {
        public RMenu(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public int Add(T_Menu obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                p.Add("@FCMENUID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { action = "ADD", FCMENUCODE = obj.FCMenuCode, FCMENUDESC = obj.FCMenuDesc, FCMENUPID = obj.FCMenuPID, FCMENULINK = obj.FCMenuLink, FCORDERNO = obj.FCOrderNo, FCICON = obj.FCIcon, FCHIDDEN = obj.FCHidden, ACCESSID = obj.AccessID });
                db.Execute("sp_CRUDMenu", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@FCMENUID");
            }
        }

        public bool Defunct(T_Menu obj, bool stsDefunct)
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

                int result = db.Execute("sp_CRUDMenu", new { action = act, FCMENUID = obj.FCMenuID, FCMENUCODE = obj.FCMenuCode, FCMENUDESC = obj.FCMenuDesc, FCMENUPID = obj.FCMenuPID, FCMENULINK = obj.FCMenuLink, FCORDERNO = obj.FCOrderNo, FCICON = obj.FCIcon, FCHIDDEN = obj.FCHidden, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_Menu obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDMenu", new { action = "DELETE", FCMENUID = obj.FCMenuID, FCMENUCODE = obj.FCMenuCode, FCMENUDESC = obj.FCMenuDesc, FCMENUPID = obj.FCMenuPID, FCMENULINK = obj.FCMenuLink, FCORDERNO = obj.FCOrderNo, FCICON = obj.FCIcon, FCHIDDEN = obj.FCHidden, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_Menu obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDMenu", new { action = "EDIT", FCMENUID = obj.FCMenuID, FCMENUCODE = obj.FCMenuCode, FCMENUDESC = obj.FCMenuDesc, FCMENUPID = obj.FCMenuPID, FCMENULINK = obj.FCMenuLink, FCORDERNO = obj.FCOrderNo, FCICON = obj.FCIcon, FCHIDDEN = obj.FCHidden, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_Menu FindMenu(int? menuid, string menucd = "")
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = "SELECT * FROM t_Menu WHERE 1=1 ";

                if (menuid != null)
                    que += " AND FCMenuID=@FCMENUID ";

                if (!string.IsNullOrEmpty(menucd))
                    que += " AND FCMenuCode=@FCMENUCODE ";

                return db.Query<T_Menu>(que, new { FCMENUID = menuid, FCMENUCODE = menucd }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_Menu> GetListMenu(bool? defunct, int? parentid)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                //string que = @" SELECT a.*,b.*,c.FCUserID,c.FCName  
                //    FROM t_Menu a 
                //    LEFT JOIN t_Menu b ON a.FCMenuPID=b.FCMenuID 
                //    LEFT JOIN t_User c ON a.FCCreatedBy=c.FCUserID 
                //    WHERE 1=1 ";

                //if (parentid == 0)
                //{
                //    que = @" SELECT a.*,b.*,c.FCUserID,c.FCName  
                //    FROM t_Menu a 
                //    LEFT JOIN t_Menu b ON a.FCMenuPID=b.FCMenuID 
                //    LEFT JOIN t_User c ON a.FCCreatedBy=c.FCUserID 
                //    WHERE 1=1 ";
                //}                   

                //if (defunct != null)
                //    que += " AND ISNULL(a.FCDefunctInd,0)=@FCDEFUNCTIND ";

                //if (parentid != null)
                //    que += " AND a.FCMenuPID=@FCMENUPID ";

                //que += " ORDER BY a.FCMenuPID,a.FCOrderNo,a.FCMenuDesc ";

                //var result = db.Query<T_Menu, T_Menu, T_User, T_Menu>(que , (a, b, c) =>
                //{
                //    a.RootMenu = b;
                //    a.CreatedBy = c;
                //    return a;
                //}, splitOn: "FCMenuID,FCUserID", param: new { FCDEFUNCTIND = defunct, FCMENUPID = parentid }).ToList();

                string que = @" SELECT *,c.FCUserID,c.FCName FROM t_Menu a LEFT JOIN t_User c ON a.FCCreatedBy=c.FCUserID";
                if (parentid == null)
                {
                    que += " WHERE FCMenuLink <> 0";
                }
                else {
                    que += " WHERE FCMenuPID = @FCMENUPID";
                }
                var result = db.Query<T_Menu, T_User, T_Menu>(que, (a, c) =>
                {
                    a.CreatedBy = c;
                    return a;
                }, splitOn: "FCMenuID,FCUserID", param: new { FCMENUPID = parentid }).ToList();

                return result;
            }
        }

        public List<T_Menu> GetListMenuNotInRole(int roleid)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                return db.Query<T_Menu>(@"SELECT B.* 
                    FROM (SELECT * FROM T_RoleDet WHERE FCRoleID=@FCROLEID) A FULL JOIN 
                    (SELECT x.*,y.FCMenuDesc Root FROM t_Menu x INNER JOIN t_Menu y ON x.FCMenuPID=y.FCMenuID WHERE x.FCMenuPID <> 0 AND ISNULL(x.FCDefunctInd,0)=0) B 
                    ON A.FCMenuID=B.FCMenuID 
                    WHERE A.FCRoleID is NULL
                    ORDER BY B.FCMenuPID, B.FCOrderNo", new { FCROLEID = roleid }, commandType: CommandType.Text).ToList();
            }
        }

        public minmaxpass SizePass(int? SetID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = "SELECT * FROM t_setGeneral WHERE 1=1 ";

                if (SetID > 0)
                    que += " AND SetID=@SetID ";

                return db.Query<minmaxpass>(que, new { SetID = SetID }, commandType: CommandType.Text).SingleOrDefault();
            }
        }
    }
}
