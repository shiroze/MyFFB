using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace MyAttd.DataAccess
{
    public class RSetGeneral : IRSetGeneral
    {
        public RSetGeneral(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }


        public List<T_SetGeneral> GetListSetGeneral()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT * FROM t_setGeneral
                    ";

                var result = db.Query<T_SetGeneral>(que,param: new {}).ToList();
                return result;
            }
        }

        public T_SetGeneral FindSetGeneral(int? SetID)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que =
                    @"
                    SELECT * FROM t_setGeneral WHERE SetID=@SetID
                    ";
                var result = db.Query<T_SetGeneral>(que, param: new { SetID = SetID }).SingleOrDefault();
                return result;
            }
        }

        public bool Add(T_SetGeneral obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDSetGeneral",
                        new
                        {
                            action = "ADD",
                            SetID = obj.SetId,
                            SetName = obj.SetName,
                            value = obj.value,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_SetGeneral obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDSetGeneral",
                        new
                        {
                            action = "DELETE",
                            SetID = obj.SetId,
                            SetName = obj.SetName,
                            value = obj.value,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_SetGeneral obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDSetGeneral",
                        new
                        {
                            action = "EDIT",
                            SetID = obj.SetId,
                            SetName = obj.SetName,
                            value = obj.value,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

    }
}
