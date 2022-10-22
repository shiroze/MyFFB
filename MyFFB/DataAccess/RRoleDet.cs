using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class RRoleDet : IRRoleDet
    {
        public RRoleDet(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public bool Add(T_RoleDet obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                int result = db.Execute("sp_CRUDRoleDet", new { action = "ADD", FCROLEID = obj.FCRoleID, FCMENUID = obj.FCMenuID, FCADD = obj.FCAdd, FCEDIT = obj.FCEdit, FCDELETE = obj.FCDelete, FCUNDELETE = obj.FCUndelete, FCApprove = obj.FCApprove, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool DefunctByRoleID(int? fcroleid, int accessid)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("UPDATE t_RoleDet SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID", new { FCROLEID = fcroleid, ACCESSID = accessid }, commandType: CommandType.Text);
                return result != 0;
            }
        }

        public bool Delete(T_RoleDet obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                int result = db.Execute("sp_CRUDRoleDet", new { action = "DELETE", FCROLEID = obj.FCRoleID, FCMENUID = obj.FCMenuID, FCADD = obj.FCAdd, FCEDIT = obj.FCEdit, FCDELETE = obj.FCDelete, FCUNDELETE = obj.FCUndelete, FCApprove = obj.FCApprove, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_RoleDet obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                int result = db.Execute("sp_CRUDRoleDet", new { action = "EDIT", FCROLEID = obj.FCRoleID, FCMENUID = obj.FCMenuID, FCADD = obj.FCAdd, FCEDIT = obj.FCEdit, FCDELETE = obj.FCDelete, FCUNDELETE = obj.FCUndelete, FCApprove = obj.FCApprove, ACCESSID = obj.AccessID }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public T_RoleDet FindRoleDet(int? roleid, int? menuid, string menucd = "")
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT rd.*,m.*
                    FROM t_RoleDet rd LEFT JOIN t_Menu m ON rd.FCMenuID=m.FCMenuID WHERE
                    ISNULL(rd.FCDefunctInd,0)=0 AND ISNULL(m.FCDefunctInd,0)=0 AND rd.FCRoleID=@FCROLEID ";

                if (menuid != null)
                    que += " AND rd.FCMenuID=@FCMENUID ";

                if (!string.IsNullOrEmpty(menucd))
                    que += " AND m.FCMenuCode=@FCMENUCD ";

                var result = db.Query<T_RoleDet, T_Menu, T_RoleDet>(que, (rd, m) =>
                {
                    rd.Menu = m;
                    return rd;
                }, new { FCROLEID = roleid, FCMENUID = menuid, FCMENUCD = menucd }, splitOn: "FCMenuID").SingleOrDefault();

                return result;
            }
        }

        public List<T_RoleDet> GetListRoleDet(int? roleid, bool? defunct)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @"SELECT * FROM 
                    t_RoleDet a INNER JOIN t_Menu b ON a.FCMenuID=b.FCMenuID 
                    INNER JOIN t_Menu c ON b.FCMenuPID=c.FCMenuID
                    WHERE a.FCRoleID=@FCROLEID ";

                if (defunct != null)
                    que += "AND ISNULL(a.FCDefunctInd,0)=@FCDEFUNCTIND ";

                que += "ORDER BY c.FCOrderNo,b.FCMenuPID,b.FCOrderNo,b.FCMenuDesc";
                var result = db.Query<T_RoleDet, T_Menu, T_Menu, T_RoleDet>(que,
                    (a, b, c) =>
                    {
                        a.Menu = b;
                        b.RootMenu = c;
                        return a;
                    }, new { FCROLEID = roleid, FCDEFUNCTIND = defunct }, splitOn: "FCMenuID,FCMenuID", commandType: CommandType.Text).ToList();

                return result;
            }
        }
    }
}
