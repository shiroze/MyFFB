using Dapper;
using MyAttd.Helpers;
using MyAttd.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Data.SqlClient;

namespace MyAttd.DataAccess
{
    public class RPPN : IRPPN
    {
        public RPPN(string constr, string phasekey)
        {
            DALSecurity.ConStr = constr;
            DALSecurity.PhaseKey = phasekey;
        }

        public T_PPN FindPPN(string p_faktur)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @"SELECT round(cast(a.amount_cash_advance*(round(cast(a.total_amount as float),2)/iif(ISNULL(a.ppn,0)=0,1,a.ppn))as float),2)/100 as ppn1,
                                    b.Company as sap_company_name,c.vendor_name as sap_vendor_name,*
	                            FROM .[dbo].[tblPPN] a
		                            LEFT JOIN (SELECT DISTINCT Company,CompanyCode FROM tblPMKS) b
			                            ON a.sap_company_code=b.CompanyCode
		                            LEFT JOIN tblSAPVendor c
			                            ON a.sap_vendor_code=c.vendor_code
                                WHERE a.no_faktur_pajak=@no_faktur_pajak
                            ";

                return db.Query<T_PPN>(que, new { no_faktur_pajak =p_faktur}, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        public List<T_PPN> GetListPPN(bool? defunct, string p_periode)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" --DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
                                --SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@FCUserID

                                SELECT round(cast(a.amount_cash_advance*(round(cast(a.total_amount as float),2)/iif(ISNULL(a.ppn,0)=0,1,a.ppn))as float),2)/100 as ppn1,
                                    b.Company as sap_company_name,c.vendor_name as sap_vendor_name,*
	                            FROM [dbo].[tblPPN] a
		                            LEFT JOIN (SELECT DISTINCT Company,CompanyCode FROM tblPMKS) b
			                            ON a.sap_company_code=b.CompanyCode
		                            LEFT JOIN tblSAPVendor c
			                            ON a.sap_vendor_code=c.vendor_code
                                WHERE a.periode=@periode";

                que += " ORDER BY a.no_faktur_pajak";


                var result = db.Query<T_PPN>(que, param: new { periode= p_periode }).ToList();
                return result;
            }
        }

        public T_PPN FindPPNProsesHitung(string CC, string VC, string periode, string awal, string akhir)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                string que = @" --Declare @VendorCode as Varchar(50)='106356', 
                                --        @CompanyCode as Varchar(50)='3100',
                                --        @Periode as Varchar(50)='2021-11',
                                --        @Periode_Awal as date ='2021-11-01',
                                --        @Periode_AKhir as date='2021-11-30'

                                SELECT pmks.CompanyCode,sup.code,sum(a.Netto*a.Incentive) as Incentive into #Temp1
                                  FROM [t_Incentive] a
	                                LEFT JOIN tblPMKS pmks ON a.PMKSID=pmks.PMKSID
	                                LEFT JOIN tblSupplier sup ON a.SupplierID=sup.SupplierID and a.PMKSID=sup.PMKSID
                                where ISNULL(a.Approval,0)=1 and sup.code=@VendorCode and pmks.CompanyCode=@CompanyCode
	                                and Periode=@Periode
                                GROUP BY pmks.CompanyCode,sup.code

                                select a.companycode,a.code, sum(a.TotalPembayaran) as TotalPembayaran,a.VAT into #Temp2
                                from 
	                                (select distinct 
		                                a.PMKSID,b.SupplierID,a.CompanyCode,b.Code,c.TanggalTimbang,c.TotalPembayaran,b.VAT
	                                from tblPMKS a
		                                inner join tblSupplier b
			                                ON a.PMKSID=b.PMKSID
		                                inner join tblffb c on c.PMKSID=a.PMKSID and c.Supplier=b.SupplierID
	                                where a.CompanyCode =@CompanyCode and b.code=@VendorCode 
		                                and cast(c.TanggalTimbang as date)>=cast(@Periode_Awal as date)
		                                and cast(c.TanggalTimbang as date)<=cast(@Periode_Akhir as date)) a
                                group by a.companycode,a.code,a.VAT

                                select 
	                                --a.companycode,a.code,
                                a.TotalPembayaran as total_amount, b.incentive as incentive, 
	                                round(cast(a.TotalPembayaran+ISNULL(b.Incentive,0) as float)/100*a.VAT,0) as ppn
	                                FROM #Temp2 a
		                                LEFT JOIN #Temp1 b
			                                ON a.companycode=b.companycode and a.code=b.code
                                DROP TABLE #Temp1
                                DROP TABLE #Temp2
                                ";
                return db.Query<T_PPN>(que, new { VendorCode=VC, CompanyCode =CC, Periode =periode, Periode_Awal=awal, Periode_Akhir=akhir }, commandType: CommandType.Text).SingleOrDefault();
            }
        }

        //public List<T_FFB> PrintFFB(string CC, string VC, string awal, string akhir)
        //{
        //    using (IDbConnection db = DALSecurity.GetSqlConnection)
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        string que = @" --Declare @VendorCode as Varchar(50)='106356', 
        //                        --        @CompanyCode as Varchar(50)='3100',
        //                        --        @Periode_Awal as date ='2021-11-01',
        //                        --        @Periode_AKhir as date='2021-11-30'

        //                        select dbo.GeneratePeriode(@Periode) as Periode,a.PMKSName,a.Company,a.CompanyCode,b.SupplierName,b.Code,c.*
        //                        from tblPMKS a
	       //                         inner join tblSupplier b
		      //                          ON a.PMKSID=b.PMKSID
	       //                         inner join tblffb c on c.PMKSID=a.PMKSID and c.Supplier=b.SupplierID
        //                        where a.CompanyCode =@CompanyCode and b.code=@VendorCode 
	       //                         and cast(c.TanggalTimbang as date)>=cast(@Periode_Awal as date)
	       //                         and cast(c.TanggalTimbang as date)<=cast(@Periode_Akhir as date)
        //                    ";
        //        que += " ORDER BY c.ticket";


        //        var result = db.Query<T_FFB>(que, param: new { VendorCode = VC, CompanyCode = CC,Periode="2021-11", Periode_Awal = awal, Periode_Akhir = akhir }).ToList();
        //        return result;
        //    }
        //}
        public DataTable PrintFFB(string noFaktur)
        {
            var connStr = DALSecurity.GetSqlConnection.ConnectionString;
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string query =
                    @"
                    Declare @VendorCode Varchar(50),
                            @CompanyCode Varchar(50),
		                    @Periode varchar(20),
                            @Periode_Awal date,
                            @Periode_AKhir date
                    SELECT 
	                    @VendorCode=sap_vendor_code, 
	                    @CompanyCode=sap_company_code,
	                    @Periode=periode,
	                    @Periode_Awal=periode_awal,
	                    @Periode_AKhir=periode_akhir
                    FROM tblppn
                    WHERE no_faktur_pajak=@NoFaktur

                    SELECT 
                        ppn.no_faktur_pajak,convert(varchar,ppn.periode_awal) as periode_awal,
                        convert(varchar,ppn.periode_akhir) as periode_akhir,
                        dbo.GeneratePeriode(@Periode) as Periode,a.PMKSName, a.Company,a.CompanyCode,b.Code,
                        c.SupplierName, c.Kendaraan, convert(varchar,c.TanggalTimbang,120) as TanggalTimbang,
	                    c.Ticket, c.BeratNetto, c.Potongan, c.Netto, c.Harga, c.PPH22, c.TotalPembayaran
                    FROM tblPMKS a
	                    INNER JOIN tblSupplier b
		                    ON a.PMKSID=b.PMKSID
	                    INNER JOIN tblffb c 
		                    ON c.PMKSID=a.PMKSID and c.Supplier=b.SupplierID
                        INNER JOIN tblPPN ppn
		                    ON ppn.no_faktur_pajak=@NoFaktur
                    WHERE a.CompanyCode =@CompanyCode and b.code=@VendorCode 
	                    and CAST(c.TanggalTimbang as date)>=CAST(@Periode_Awal as date)
	                    and CAST(c.TanggalTimbang as date)<=CAST(@Periode_Akhir as date)

                    ";
                query += " ORDER BY c.ticket";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@NoFaktur", SqlDbType.VarChar);
                cmd.Parameters["@NoFaktur"].Value = noFaktur;
                conn.Open();

                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                //string json = JsonConvert.SerializeObject(dataTable);

                return dataTable;
            }
        }

        public bool Delete(T_PPN obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPPN",
                                new
                                {
                                    action = "DELETE"
                                    , periode=""
                                    , no_faktur_pajak=obj.no_faktur_pajak
                                    , ppn_type=""
                                    , tgl_faktur_pajak=""
                                    , sap_company_code=""
                                    , sap_vendor_code=""
                                    , periode_awal=""
                                    , periode_akhir=""
                                    , disetorKe=obj.disetorke
                                    , ppn_penyelesaian=""
                                    , no_faktur_pajak_advance = ""
                                    , CashNo=""
                                    , amount_cash_advance=""
                                    , total_amount=""
                                    , incentive = ""
                                    , ppn=""
                                    , AccessID = obj.AccessID,
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool Add(T_PPN obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                string coba = obj.disetorke.ToString();
                
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPPN",
                                new
                                {
                                    action = "ADD"
                                    , periode=obj.Periode.Substring(0,4)+obj.Periode.Substring(5,2)
                                    , no_faktur_pajak=obj.no_faktur_pajak
                                    , ppn_type=obj.ppn_type
                                    , tgl_faktur_pajak=obj.tgl_faktur_pajak
                                    , sap_company_code=obj.sap_company_code
                                    , sap_vendor_code=obj.sap_vendor_code
                                    , periode_awal=obj.periode_awal
                                    , periode_akhir=obj.periode_akhir
                                    , disetorKe=obj.disetorke.ToString()
                                    , ppn_penyelesaian=obj.ppn_penyelesaian
                                    , no_faktur_pajak_advance = obj.no_faktur_pajak_advance
                                    , CashNo=obj.CashNo
                                    , amount_cash_advance=obj.amount_cash_advance
                                    , total_amount=obj.total_amount
                                    , incentive = obj.incentive
                                    , ppn=obj.ppn
                                    , AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }

        public bool AddAdvance(T_PPN obj)
        {
            using (IDbConnection db = DALSecurity.GetSqlConnection)
            {
                string coba = obj.disetorke.ToString();
                
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("sp_CRUDPPN",
                                new
                                {
                                    action = "ADDADVANCE"
                                    , periode=obj.Periode.Substring(0,4)+obj.Periode.Substring(5,2)
                                    , no_faktur_pajak=obj.no_faktur_pajak
                                    , ppn_type="A"
                                    , tgl_faktur_pajak=obj.tgl_faktur_pajak
                                    , sap_company_code=obj.sap_company_code
                                    , sap_vendor_code=obj.sap_vendor_code
                                    , periode_awal=""
                                    , periode_akhir=""
                                    , disetorKe=obj.disetorke.ToString()
                                    , ppn_penyelesaian=obj.ppn_penyelesaian
                                    , no_faktur_pajak_advance = obj.no_faktur_pajak_advance
                                    , CashNo=obj.CashNo
                                    , amount_cash_advance=obj.amount_cash_advance
                                    , total_amount=obj.total_amount
                                    , incentive = obj.incentive
                                    , ppn=obj.ppn
                                    , AccessID = obj.AccessID
                                }, commandType: CommandType.StoredProcedure);
                return result != 0;
            }
        }


    }
}
