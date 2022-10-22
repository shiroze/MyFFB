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
    public class RHoliday : IRHoliday
    {
        public RHoliday(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }


        public List<T_Holiday> GetListHoliday(string Periode)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que =
                    @"
                    SELECT [Date], [Remarks]
                    FROM [TblHolidayBank]
                    WHERE YEAR(Date)=@Periode
                    ORDER BY [Date]
                    ";

                var result = db.Query<T_Holiday>(que,param: new { Periode=Periode}).ToList();
                return result;
            }
        }

        public T_Holiday FindHoliday(string p_date, string p_remarks)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                string que =
                    @"
                    SELECT [Date], [Remarks]
                    FROM [TblHolidayBank]
                    WHERE Date=@date and Remarks=@remarks
                    ";
                return db.Query<T_Holiday>(que, new { Date = p_date, Remarks = p_remarks }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public bool Add(T_Holiday obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = 
                    db.Execute("sp_CRUDHolidayBank",
                        new
                        {
                            action = "ADD",
                            Date = obj.Date,
                            Remarks = obj.Remarks,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Delete(T_Holiday obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result =
                    db.Execute("sp_CRUDHolidayBank",
                        new
                        {
                            action = "DELETE",
                            Date = obj.Date,
                            Remarks = obj.Remarks,
                            AccessID = obj.AccessID
                        }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

    }
}
