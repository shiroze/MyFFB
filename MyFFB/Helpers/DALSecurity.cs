using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyAttd.Helpers
{
    public class DALSecurity
    {
        public static IConfiguration configuration;

        public static string ConStr { get; set; }
        //public static string ConStr1 { get; set; }
        public static string OraConStr { get; set; }

        //public static string OraConStr { get; set; }
        public static string PhaseKey { get; set; }
        public static string OraPhaseKey { get; set; }

        public static IDbConnection GetSqlConnection
        {
            get
            {
                var dbconstr = "Data Source=172.21.8.12;Initial Catalog=FFB_Dbase;User ID=sa;Password=mentimoen";

                //var dbconstr = "Server=172.24.128.189; Database=FFB_Dbase; Trusted_Connection=True; MultipleActiveResultSets=true";
                var encryption = Encrypter(dbconstr, "ATTDMDN");

                //var test = Decrypter("vruvsDt7Eax+LWyCRPlgXCTcnMcNduTuUwpGAMF6cw6w5bX2+Xtmc41trDATJ3Xj0dqZuyb69ULHcvP9/Mbd3+7GQGIcfhn8Lem9PH5MzuK3p9zCQ1jbmmq9cl+rBQpP", PhaseKey + "MDN");

                var encryptConStr = ConStr;
                //var encryptConStr1 = ConStr1;
                ////var connStr = String_Cipher.Decrypt(encryptConStr, PhaseKey + "MDN");
                var connStr = Decrypter(encryptConStr, PhaseKey + "MDN");
                //var connStr = "Data Source=172.21.8.64;Initial Catalog=db_Attd;User ID=sa;Password=mentimoen";
                var conn = new SqlConnection(connStr);
                return conn;
            }
        }

        //public static IDbConnection GetOraConnection
        //{
        //    get
        //    {
        //        var encryptConStr = OraConStr;
        //        //var connStr = String_Cipher.Decrypt(encryptConStr, PhaseKey + "MDN");
        //        var connStr = Decrypter(encryptConStr, OraPhaseKey + "MDN");
        //        var conn = new OracleConnection(connStr);
        //        return conn;
        //    }
        //}

        public static string Encrypter(string plaintext, string myphasekey)
        {
            return String_Cipher.Encrypt(plaintext, myphasekey);
        }

        public static string Decrypter(string encrypttext, string myphasekey)
        {
            return String_Cipher.Decrypt(encrypttext, myphasekey);
        }
    }
}
