using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MyAttd.DataAccess
{
    public class REmailPrice : IREmailPrice
    {
        public REmailPrice(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }
        public string Add(T_EmailPrice obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();
                //p.Add("@FCSAPID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new
                {
                    action = "ADD",
                    EmailID = obj.EmailID,
                    AddressEmail = obj.AddressEmail,
                    ListSend = obj.ListSend,
                    AccessID = obj.AccessID,
                });
                db.Execute("sp_CRUDEmailPrice", p, commandType: CommandType.StoredProcedure);
                return p.Get<string>("@AddressEmail");
            }
        }

        public bool Delete(T_EmailPrice obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDEmailPrice", new
                {
                    action = "DELETE",
                    EmailID = obj.EmailID,
                    AddressEmail = obj.AddressEmail,
                    ListSend = obj.ListSend,
                    AccessID = obj.AccessID,
                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Edit(T_EmailPrice obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDEmailPrice",
                                new
                                {
                                    action = "EDIT",
                                    EmailID = obj.EmailID,
                                    AddressEmail = obj.AddressEmail,
                                    ListSend = obj.ListSend,
                                    AccessID = obj.AccessID,
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }


        public T_EmailPrice FindEmailPrice(string AddressEmail)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" SELECT * FROM t_Email
                                WHERE 1=1 ";


                if (AddressEmail != null)
                    que += "  AND AddressEmail=@AddressEmail ";

                return db.Query<T_EmailPrice>(que, new { AddressEmail = AddressEmail }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_EmailPrice> GetListEmailPrice()
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = 
                    @"
                    SELECT * FROM t_Email a
                    LEFT join t_User b ON a.FCCreatedBy=b.FCUserID
                    LEFT join t_User b1 ON a.FCUpdatedBy=b1.FCUserID
                    --WHERE (a.EmailPriceID in (select * from dbo.splitstring(@setEmailPriceid)) or isnull(@setEmailPriceid,'')='')
                    ";


                que += " ORDER BY a.EmailID, a.ListSend,a.AddressEmail";


                var result = db.Query<T_EmailPrice,T_User, T_User, T_EmailPrice>(que, (a,b,c) =>
               {
                   //a.RootMenu = b;
                   a.CreatedBy = b;
                   a.UpdatedBy = c;
                   return a;
               }, splitOn: "FCUserID,FCUserID", param: new { }).ToList();
                return result;
            }
        }
    }
}
