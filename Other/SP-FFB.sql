USE [FFB_Dbase]
GO
/****** Object:  UserDefinedFunction [dbo].[GeneratePeriode]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[GeneratePeriode](@Date varchar(20))
RETURNS varchar(25)
AS
BEGIN
	Declare 
	--@Date varchar(10) ='2021-12-16',
	@Bulan int, @NamaBulan varchar(20),
	@Periode varchar(50)

	SET @Bulan = convert(int,SUBSTRING(@Date,6,2))
	SELECT @NamaBulan=
	CASE 
		WHEN @Bulan = 1 THEN 'Januari'
		WHEN @Bulan = 2 THEN 'Februari'
		WHEN @Bulan = 3 THEN 'Maret'
		WHEN @Bulan = 4 THEN 'April'
		WHEN @Bulan = 5 THEN 'Mei'
		WHEN @Bulan = 6 THEN 'Juni'
		WHEN @Bulan = 7 THEN 'July'
		WHEN @Bulan = 8 THEN 'Agustus'
		WHEN @Bulan = 9 THEN 'September'
		WHEN @Bulan = 10 THEN 'Oktober'
		WHEN @Bulan = 11 THEN 'November'
		WHEN @Bulan = 12 THEN 'Desember'
	ELSE 'ERROR'
	END

	IF SUBSTRING(@Date,9,2)=''
		SET @Periode=IIF(@NamaBulan='ERROR','',@NamaBulan+' '+LEFT(@Date,4))
	ELSE
		SET @Periode=SUBSTRING(@Date,9,2)+' '+IIF(@NamaBulan='ERROR','',@NamaBulan+' '+LEFT(@Date,4))

	--INSERT INTO @returnList
	--SELECT 

	Return @Periode
END
GO
/****** Object:  UserDefinedFunction [dbo].[splitstring]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[splitstring](@stringToSplit varchar(max))
RETURNS
	@returnList TABLE ([Name] [varchar](25))
AS
BEGIN
	DECLARE @name varchar(25),@pos int
	WHILE CHARINDEX(',',@stringToSplit)>0
	BEGIN
		Select @pos=CHARINDEX(',',@stringToSplit)
		Select @name=SUBSTRING(@stringToSplit,1,@pos-1)

		INSERT INTO @returnList
		SELECT @name
		
		SELECT @stringToSplit =SUBSTRING(@stringToSplit,@pos+1,LEN(@stringToSplit)-@pos)
	END

	INSERT INTO @returnList
	SELECT @stringToSplit

	Return
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDAreaRegional]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_CRUDAreaRegional]
(
@action varchar(9),
@AreaID int output,
@AreaOperational varchar(100),
@Regional varchar(50),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO t_AreaRegional
				(AreaOperational, Regional, FCCreatedBy, FCCreatedDT			)
			VALUES 
				(@AreaOperational, @Regional, @ACCESSID, getdate()
				)
			SET @AreaID=SCOPE_IDENTITY()
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM t_AreaRegional_Audit WHERE AreaID=@AreaID)
			BEGIN
				INSERT INTO t_AreaRegional_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM t_AreaRegional WHERE AreaID=@AreaID
			END
		END

		IF @action='EDIT'
		BEGIN
			Update t_AreaRegional
			SET
				AreaOperational = @AreaOperational
				, Regional = @Regional
				, FCUpdatedBy = @ACCESSID
				, FCUpdatedDT = getdate()
			WHERE AreaID=@AreaID
		END

		IF @action='DELETE'
		BEGIN
			Update t_AreaRegional 
				SET FCDEFUNCTIND=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=getdate()
			WHERE AreaID=@AreaID
		END

		INSERT INTO t_AreaRegional_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM t_AreaRegional WHERE AreaID=@AreaID
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH

GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDBudget]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [dbo].[sp_CRUDBudget]
(
@action varchar(9),
@PMKSID varchar(50),
@Periode varchar(20),
@VolumeFFB_KG float,
@VolumeCPO_KG float,
@VolumePK_KG float,
@OER float,
@KER float,
@NetMargin_USD_MT_CPO float,
@HK float,
@ExchangeRate float,
@Capacity float,
@ProduksiCangkang_KG float,
@BakarCangkang_KG float,
@EBITDA_Cangkang float,
@ProduksiBA_KG float,
@Price_BunchAsh float,
@AccessID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	SET @Periode=REPLACE(@Periode,'-','')
		IF @action='ADD'
		BEGIN
			INSERT INTO dbo.tblBudget
				(PMKSID,Periode,VolumeFFB_KG,VolumeCPO_KG,VolumePK_KG
				,OER,KER,NetMargin_USD_MT_CPO,HK,ExchangeRate,Capacity
				,ProduksiCangkang_KG,BakarCangkang_KG,EBITDA_Cangkang
				,ProduksiBA_KG,Price_BunchAsh,FCCreatedBy,FCCreatedDT)
			VALUES
				(@PMKSID,@Periode,@VolumeFFB_KG,@VolumeCPO_KG,@VolumePK_KG
				,@OER,@KER,@NetMargin_USD_MT_CPO,@HK,@ExchangeRate,@Capacity
				,@ProduksiCangkang_KG,@BakarCangkang_KG,@EBITDA_Cangkang
				,@ProduksiBA_KG,@Price_BunchAsh,@ACCESSID,GETDATE())
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblBudget_Audit 
							WHERE PMKSID=@PMKSID 
							AND Periode=@Periode)
			BEGIN
				INSERT INTO tblBudget_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblBudget 
				WHERE PMKSID=@PMKSID 
						AND Periode=@Periode
			END
		END

		IF @action='EDIT'
		BEGIN
			UPDATE tblBudget 
			SET
				VolumeFFB_KG=@VolumeFFB_KG,VolumeCPO_KG=@VolumeCPO_KG,
				VolumePK_KG=@VolumePK_KG,OER=@OER,KER=@KER,
				NetMargin_USD_MT_CPO=@NetMargin_USD_MT_CPO,
				HK=@HK,ExchangeRate=@ExchangeRate,Capacity=@Capacity,
				ProduksiCangkang_KG=@ProduksiCangkang_KG,
				BakarCangkang_KG=@BakarCangkang_KG,
				EBITDA_Cangkang=@EBITDA_Cangkang,
				ProduksiBA_KG=@ProduksiBA_KG,
				Price_BunchAsh=@Price_BunchAsh,
				FCUpdatedBy=@AccessID, FCUpdatedDT=GETDATE()
			WHERE
			PMKSID=@PMKSID 
				AND Periode=@Periode
		END

		INSERT INTO tblBudget_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblBudget 
		WHERE PMKSID=@PMKSID 
				AND Periode=@Periode

		IF @action='DELETE'
		BEGIN
			DELETE tblBudget 
			WHERE
				PMKSID=@PMKSID 
				AND Periode=@Periode
		END

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDCashAdvance]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_CRUDCashAdvance]
(
@action varchar(9),
@CashNo varchar(25) output,
@Period	varchar(6),
@Tanggal date,
@PMKSID varchar(50),
@SupplierID varchar(10),
@SupplierName varchar(50),
@Code varchar(8),
@Description varchar(100),
@Amount float,
@DeductAmount float,
@Week varchar(10),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO tblCashAdvance
				(CashNo,[Period],Tanggal,PMKSID,SupplierID,SupplierName,Code
				,[Description],Amount,[Week],FCCreatedBy,FCCreatedDT)
			VALUES 
				(@CashNo,@Period,@Tanggal,@PMKSID,@SupplierID,@SupplierName,@Code
				,@Description,@Amount,@Week,@ACCESSID,getdate())
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblCashAdvance_Audit WHERE CashNo=@CashNo)
			BEGIN
				INSERT INTO tblCashAdvance_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblCashAdvance WHERE CashNo=@CashNo
			END
		END

		IF @action='EDIT'
		BEGIN

			Declare @Panjar float
			SELECT @Panjar=SUM(RealisasiPanjarAmount) 
			FROM tblffb WHERE Ltrim(Rtrim(Number))=@CashNo
			GROUP BY number

			IF (select @Amount-@Panjar)<0
				BEGIN
					RAISERROR ( 'Sorry Amount Panjar < Realisasi Panjar',16,1)
				END
			ELSE
				BEGIN
					UPDATE tblCashAdvance 
					SET Tanggal=cast(@Tanggal as date),[Description]=@Description,
						Amount=@Amount,FCUpdatedBY=@ACCESSID,FCUpdatedDT=GETDATE() 
					WHERE CashNo=@CashNo
				END
		END

		INSERT INTO tblCashAdvance_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblCashAdvance WHERE CashNo=@CashNo

		IF @action='DELETE'
		BEGIN
			DELETE tblCashAdvance WHERE CashNo=@CashNo
			UPDATE tblFFB SET Number=NULL WHERE ISNULL(NUMBER,'')=@CashNo
		END

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDCashDeduct]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_CRUDCashDeduct]
(
@action varchar(9),
@Ticket	varchar(15),
@Number varchar(25),
@TotalPembayaran float,
@RealisasiPanjarAmount float,
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	Declare @msg varchar(500)
		IF @action='EDIT'
		BEGIN
			DECLARE @Panjar float,@Amount float
			SELECT @Panjar=SUM(RealisasiPanjarAmount) 
			FROM tblffb WHERE Ltrim(Rtrim(Number))=@Number and Ticket<>@Ticket
			GROUP BY number

			SELECT @Amount=Amount FROM tblCashAdvance WHERE CashNo=@Number

			IF NOT EXISTS (SELECT * FROM tblCashAdvance_Audit WHERE CashNo=@Number)
			BEGIN
				INSERT INTO tblCashAdvance_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblCashAdvance WHERE CashNo=@Number
			END
			IF NOT EXISTS (SELECT * FROM tblFFB_Audit WHERE Ticket=@Ticket)
			BEGIN
				INSERT INTO tblFFB_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblFFB WHERE Ticket=@Ticket
			END

			IF (select @Amount-@Panjar-@RealisasiPanjarAmount)<0
				BEGIN
					RAISERROR ( 'Sorry Sisa Total Amount < Realisasi Panjar',16,1)
				END
			ELSE
				BEGIN TRY
					BEGIN TRANSACTION;
						UPDATE tblCashAdvance SET DeductAmount=ISNULL(@Panjar,0)+@RealisasiPanjarAmount, FCUpdatedBY=@ACCESSID,FCUpdatedDT=GETDATE() 
						WHERE CashNo=@Number
					
						UPDATE tblFFB SET Number=IIF(@RealisasiPanjarAmount=0,null,@Number), RealisasiPanjarAmount=@RealisasiPanjarAmount--, FCUpdatedBY=@ACCESSID,FCUpdatedDT=GETDATE()
						WHERE Ticket=@Ticket

						INSERT INTO tblCashAdvance_Audit
						SELECT *,@ACCESSID,@action,GETDATE()
						FROM tblCashAdvance WHERE CashNo=@Number

						INSERT INTO tblFFB_Audit
						SELECT *,@ACCESSID,@action+'-CA',GETDATE()
						FROM tblFFB WHERE Ticket=@Ticket

					COMMIT TRANSACTION;
				END TRY

				BEGIN CATCH
					ROLLBACK TRANSACTION;
					Declare @SisaPanjar float=@Amount-@Panjar
					SET @msg='Sisa Panjar : '+cast(format(@SisaPanjar, 'N', 'en-US') as varchar(20))
					RAISERROR (@msg,16,1)
				END CATCH
		END
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDCompetitor]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE  PROCEDURE [dbo].[sp_CRUDCompetitor]
(
@action varchar(9),
@CompetitorID int,
@CompetitorName varchar(50),
@CompetitorLocation varchar(50),
@Capacity int,
@CompetitorGroup varchar(50),
@PMKSID varchar(50),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO tblCompetitor
				(CompetitorName, CompetitorLocation 
				, Capacity, CompetitorGroup, PMKSID 
				, FCCreatedBy, FCCreatedDT
				)
			VALUES 
				(@CompetitorName, @CompetitorLocation 
				, @Capacity, @CompetitorGroup, @PMKSID 
				, @ACCESSID, getdate()
				)
			SET @CompetitorID=SCOPE_IDENTITY()
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblCompetitor_Audit WHERE CompetitorID = @CompetitorID)
			BEGIN
				INSERT INTO tblCompetitor_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblCompetitor WHERE CompetitorID = @CompetitorID
			END
		END

		IF @action='EDIT'
		BEGIN
			Update tblCompetitor
			SET
				CompetitorName = @CompetitorName
				, CompetitorLocation = @CompetitorLocation
				, Capacity = @Capacity
				, CompetitorGroup = @CompetitorGroup
				, PMKSID = @PMKSID
				, FCUpdatedBy = @ACCESSID
				, FCUpdatedDT = getdate()
			WHERE CompetitorID = @CompetitorID
		END

		INSERT INTO tblCompetitor_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblCompetitor WHERE CompetitorID = @CompetitorID

		IF @action='DELETE'
		BEGIN
			DELETE tblCompetitor WHERE CompetitorID = @CompetitorID

			--UPDATE tblCompetitor 
			--	SET FCDefunctInd=1, FCUpdatedBy = @ACCESSID, FCUpdatedDT = getdate() 
			--WHERE CompetitorID = @CompetitorID
		END

		--IF @action='UNDELETE'
		--BEGIN
			--UPDATE tblCompetitor 
			--	SET FCDefunctInd=0, FCUpdatedBy = @ACCESSID, FCUpdatedDT = getdate() 
			--WHERE CompetitorID = @CompetitorID
		--END

		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDFFB]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[sp_CRUDFFB]
(
@action varchar(20),
@TanggalTimbang as varchar(10),
@TanggalPOST as varchar(10),
@Ticket as varchar(50),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	SET NOCOUNT ON;
	DECLARE @UserID VARCHAR(100)
	SELECT @UserID=FCUserName FROM t_User WHERE FCUserID=1

	IF @action='POST'
	BEGIN
		UPDATE tblFFB SET Post2Payment=@TanggalPOST,UserID=@UserID,LastAccess=GETDATE()
		WHERE CAST(TanggalTimbang as date)=@TanggalTimbang AND Ticket=@Ticket
	END

	IF @action='DELETE' or @action='UNPOST'
	BEGIN
		UPDATE tblFFB SET UserID=@UserID,LastAccess=GETDATE()
		WHERE CAST(TanggalTimbang as date)=@TanggalTimbang AND Ticket=@Ticket

		DELETE tblFFB WHERE CAST(TanggalTimbang as date)=@TanggalTimbang AND Ticket=@Ticket
	END

	IF @action='DOWNLOADFFB' or @action='UNPOST'
	BEGIN
		SELECT a.No_Tiket,
		IIF(s.komidelCalculate = 'N', 0, -- JIKA TIDAK PERLU CALCULATE KOMIDEL, MAKA POTONGAN OTOMATIS NOL
			iif(ISNULL(s.BibitTopaz,'N')='Y',
				CASE
					WHEN LTRIM(RTRIM(s.Category))in ('1. Agent Local','2. Own Estate','3. Agent Ramp') 
						THEN iif(substring(a.Spesifikasi_Komoditi, 76, 5)<c.KomidelMin,c.hargamin,iif(substring(a.Spesifikasi_Komoditi, 76, 5)<c.KomidelPlus,c.hargaplus,0))
					WHEN LTRIM(RTRIM(s.Category))in ('4. CSV') 
						THEN iif(substring(a.Spesifikasi_Komoditi, 76, 5)<c.KomidelMin2,c.hargamin2,iif(substring(a.Spesifikasi_Komoditi, 76, 5)<c.KomidelPlus2,c.hargaplus2,0))
					ELSE 0
				END,iif(substring(a.Spesifikasi_Komoditi, 76, 5)<c.KomidelMin1,c.hargamin1,iif(substring(a.Spesifikasi_Komoditi, 76, 5)<c.KomidelPlus1,c.hargaplus1,0))
				))
			as potongan
		INTO #TempPotongan1
		From 
			OPENDATASOURCE('SQLNCLI',  
				'Data Source=172.21.8.100;Initial Catalog=db_WBSc;User ID=ffb_reader;Password=@reader123')  
				.db_WBSc.dbo.[t_trx_wb] a
			LEFT JOIN
				dbo.tblPrice b 
					ON b.DatePrice=CAST(a.Tgl_Laporan as date) 
						AND b.SupplierID=a.PMS_EstateCode_Supplier and b.PMKSID=a.pmks
			INNER JOIN tblPMKS c
				ON a.PMKS=c.PMKSID
			LEFT JOIN tblSupplier s
				ON a.pmks=s.pmksid and a.PMS_EstateCode_Supplier=s.supplierid
			LEFT JOIN tblFFB d
				ON a.No_Tiket=d.Ticket
		WHERE 
			a.Transaksi ='2 - BONGKAR MUAT' 
			AND a.Komoditi_Group = '11 - TBS' 
			AND a.Komoditi = 'TBS LUAR'
			AND CAST(a.Tgl_Laporan as date)=@TanggalTimbang


		SELECT null as Post2Payment, null as Number,
			a.pmks as PMKSID, a.PMS_EstateCode_Supplier as Supplier, b.SupplierName,
			null as GroupPayment, a.No_Kendaraan as Kendaraan, a.Tgl_Laporan as TanggalTimbang, a.No_Tiket as Ticket,
			a.Berat_Netto as BeratNetto, a.Potongan,(a.Berat_Netto-a.Potongan) as Netto, 0 as NettoTransfeer,
			a.Jumlah_Janjang as Janjang, substring(a.Spesifikasi_Komoditi, 76, 5) As Komidel,
			Round(b.Price-pot.potongan+((b.Price-pot.potongan)*b.PPH22/100),0) as HargaBeli, b.Price-pot.potongan as Harga, b.PPH22, b.VAT,
			(a.Berat_Netto-a.Potongan)*(b.Price-pot.potongan) as TotalPembayaran, 'FFB-WEB' as UserID, getdate() as LastAccess
			INTO #Temp1
		From 
			OPENDATASOURCE('SQLNCLI',  
				'Data Source=172.21.8.100;Initial Catalog=db_WBSc;User ID=ffb_reader;Password=@reader123')  
				.db_WBSc.dbo.[t_trx_wb] a
			LEFT JOIN
				dbo.tblPrice b 
					ON b.DatePrice=CAST(a.Tgl_Laporan as date) 
						AND b.SupplierID=a.PMS_EstateCode_Supplier and b.PMKSID=a.pmks
			INNER JOIN tblPMKS c
				ON a.PMKS=c.PMKSID
			LEFT JOIN tblFFB d
				ON a.No_Tiket=d.Ticket
			LEFT JOIN #TempPotongan1 pot
				ON a.No_Tiket=pot.No_Tiket
		WHERE 
			a.Transaksi ='2 - BONGKAR MUAT' 
			AND a.Komoditi_Group = '11 - TBS' 
			AND a.Komoditi = 'TBS LUAR'
			AND CAST(a.Tgl_Laporan as date)=@TanggalTimbang
			AND d.Ticket is null

		DROP TABLE #TempPotongan1

		SET NOCOUNT OFF;
		INSERT INTO tblFFB
		(
		Post2Payment, Number, PMKSID, Supplier, SupplierName, GroupPayment, Kendaraan, 
		TanggalTimbang, Ticket, BeratNetto, Potongan, Netto, NettoTransfeer, Janjang, 
		Komidel, HargaBeli, Harga, PPH22, VAT, TotalPembayaran, UserID, LastAccess
		)
		SELECT * FROM #Temp1

		DROP TABLE #Temp1
	END

	Declare @msg varchar(500),@Row_tblPrice int = 0,@rowcount int
	SET @msg='Sorry Transaction '+@action+' Failed.'
	SELECT @Row_tblPrice=Count(*) from tblPrice where datePrice=@TanggalTimbang

	IF @action='CALCULATE'
	BEGIN
		IF @Row_tblPrice<=0
		BEGIN
			SET @msg='Sorry Check Table Price Date '+@TanggalTimbang
			RAISERROR (@msg,16,1)
			RETURN
		END
		select a.ticket,b.price,
			IIF(s.komidelCalculate = 'N', 0, -- JIKA TIDAK PERLU CALCULATE KOMIDEL, MAKA POTONGAN OTOMATIS NOL
			iif(ISNULL(s.BibitTopaz,'N')='Y',
				CASE
					WHEN LTRIM(RTRIM(s.Category))in ('1. Agent Local','2. Own Estate','3. Agent Ramp') 
						THEN iif(a.komidel<c.KomidelMin,c.hargamin,iif(a.komidel<c.KomidelPlus,c.hargaplus,0))
					WHEN LTRIM(RTRIM(s.Category))in ('4. CSV') 
						THEN iif(a.komidel<c.KomidelMin2,c.hargamin2,iif(a.komidel<c.KomidelPlus2,c.hargaplus2,0))
					ELSE 0
				END,iif(a.komidel<c.KomidelMin1,c.hargamin1,iif(a.komidel<c.KomidelPlus1,c.hargaplus1,0))
				)) as potongan

			INTO #TempPotongan
			FROM tblFFB a 
					LEFT JOIN tblPrice b
						ON CAST(a.TanggalTimbang AS DATE)=CAST(b.DatePrice AS DATE) 
							and a.PMKSID=b.PMKSID 
							and a.Supplier=b.SupplierID

					LEFT JOIN tblPMKS c
							ON a.PMKSID=c.PMKSID

					LEFT JOIN tblSupplier s
						ON a.PMKSID=s.pmksid and a.Supplier=s.supplierid

					WHERE CAST(a.TanggalTimbang AS DATE)=@TanggalTimbang
			order by a.Ticket

		SET NOCOUNT OFF;
		UPDATE a
		SET a.HargaBeli=
			Round(b.Price-pot.potongan+((b.Price-pot.potongan)*b.PPH22/100),0), 
			a.Harga=b.Price-pot.potongan,
			a.PPH22=b.PPH22, a.VAT=b.VAT, a.TotalPembayaran=a.Netto*(b.Price-pot.potongan),
			a.CalculateBy=@ACCESSID, a.CalculateDate=getdate()
		FROM tblFFB a 
		LEFT JOIN tblPrice b
			ON CAST(a.TanggalTimbang AS DATE)=CAST(b.DatePrice AS DATE) 
				and a.PMKSID=b.PMKSID 
				and a.Supplier=b.SupplierID
		LEFT JOIN tblPMKS c
				ON a.PMKSID=c.PMKSID
		LEFT JOIN tblSupplier s
			ON a.PMKSID=s.pmksid and a.Supplier=s.supplierid
		LEFT JOIN #TempPotongan pot
			ON a.Ticket=pot.Ticket
		WHERE CAST(a.TanggalTimbang AS DATE)=@TanggalTimbang
				AND a.Post2Payment IS NULL

		DROP TABLE #TempPotongan

		--SET NOCOUNT OFF;
		--INSERT tblFFB_Audit
		--SELECT *,@ACCESSID,@action,GETDATE()
		--FROM tblFFB 
		--WHERE CAST(TanggalTimbang AS DATE)=@TanggalTimbang
		--	AND Post2Payment IS NULL
	END

	

	
	COMMIT TRANSACTION

	IF @action='TotalTRX'
	BEGIN
	SET NOCOUNT OFF
		DECLARE @setpmksid varchar(max),@pos int, @name varchar(25)
        SELECT @setpmksid=SetPMKSID from t_user WHERE FCUserID=@ACCESSID

        SELECT * into #Temptrx
        FROM tblffb 
        WHERE cast(TanggalTimbang as date) = @TanggalTimbang
            AND (PMKSID in (select * from dbo.splitstring(@setpmksid)) or isnull(@setpmksid,'')='')
		drop table #Temptrx
	END
	
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDGroupIncentive]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE  PROCEDURE [dbo].[sp_CRUDGroupIncentive]
(
@action varchar(9)
,@NoId INT output
,@GroupSuppID INT
,@Approval BIT
,@Incentive INT
,@IncentiveQty1 FLOAT
,@IncentivePrice1 INT
,@IncentiveQty2 FLOAT
,@IncentivePrice2 INT
,@IncentiveQty3 FLOAT
,@IncentivePrice3 INT
,@IncentiveQty4 FLOAT
,@IncentivePrice4 INT
,@IncentiveQty5 FLOAT
,@IncentivePrice5 INT
,@IncentiveQty6 FLOAT
,@IncentivePrice6 INT
,@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO t_GroupIncentive
				(GroupSuppID, Approval,Incentive
				, IncentiveQty1, IncentivePrice1, IncentiveQty2, IncentivePrice2
				, IncentiveQty3, IncentivePrice3, IncentiveQty4, IncentivePrice4
				, IncentiveQty5, IncentivePrice5, IncentiveQty6, IncentivePrice6
				, FCCreatedBy, FCCreatedDT
				)
			VALUES 
				(@GroupSuppID, @Approval, @Incentive
				, @IncentiveQty1, @IncentivePrice1, @IncentiveQty2, @IncentivePrice2
				, @IncentiveQty3, @IncentivePrice3, @IncentiveQty4, @IncentivePrice4
				, @IncentiveQty5, @IncentivePrice5, @IncentiveQty6, @IncentivePrice6
				, @ACCESSID, getdate()
				)
			SET @NoId=SCOPE_IDENTITY()
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM t_GroupIncentive_Audit WHERE NoId=@NoId)
			BEGIN
				INSERT INTO t_GroupIncentive_Audit
				SELECT *,@ACCESSID, 'AWAL', GETDATE()
				FROM t_GroupIncentive 
				WHERE NoId=@NoId
			END
		END

		IF @action='EDIT'
		BEGIN
			Update t_GroupIncentive
			SET
				GroupSuppID = @GroupSuppID
				, Approval = 0
				, Incentive = @Incentive
				, IncentiveQty1 = @IncentiveQty1
				, IncentivePrice1 = @IncentivePrice1
				, IncentiveQty2 = @IncentiveQty2
				, IncentivePrice2 = @IncentivePrice2
				, IncentiveQty3 = @IncentiveQty3
				, IncentivePrice3 = @IncentivePrice3
				, IncentiveQty4 = @IncentiveQty4
				, IncentivePrice4 = @IncentivePrice4
				, IncentiveQty5 = @IncentiveQty5
				, IncentivePrice5 = @IncentivePrice5
				, IncentiveQty6 = @IncentiveQty6
				, IncentivePrice6 = @IncentivePrice6
				, FCUpdatedBy = @ACCESSID
				, FCUpdatedDT = getdate()
			WHERE NoId=@NoId
		END

		IF @action='DELETE'
		BEGIN
			UPDATE t_GroupIncentive SET Approval=0,FCDefunctInd=1, FCUpdatedBy = @ACCESSID, FCUpdatedDT = getdate() WHERE NoId=@NoId
		END

		IF @action='UNDELETE'
		BEGIN
			UPDATE t_GroupIncentive SET FCDefunctInd=0, FCUpdatedBy = @ACCESSID, FCUpdatedDT = getdate() WHERE NoId=@NoId
		END

		IF @action='APPROVE'
		BEGIN
			UPDATE t_GroupIncentive SET Approval=1, FCApproveBy=@ACCESSID, FCApproveDT = getdate() WHERE NoId=@NoId
		END
		--untuk Log History Audit Table
		INSERT INTO t_GroupIncentive_Audit
		SELECT *,@ACCESSID, @action, GETDATE()
		FROM t_GroupIncentive 
		WHERE NoId=@NoId
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDGroupSupp]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_CRUDGroupSupp]
(
@action varchar(9),
@GroupSuppID int output,
@GroupSuppName varchar(max),
@Regional varchar(25),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO t_GroupSupp
				(GroupSuppName,Regional,FCCreatedBy,FCCreatedDT,FCUpdatedBy,FCUpdatedDT) 
			VALUES 
				(@GroupSuppName,@Regional,@ACCESSID,GETDATE(),@ACCESSID,GETDATE())
			SET @GroupSuppID=SCOPE_IDENTITY()
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM t_GroupSupp_Audit 
							WHERE GroupSuppID=@GroupSuppID)
			BEGIN
				INSERT INTO t_GroupSupp_Audit
				SELECT *,@ACCESSID, 'AWAL', GETDATE()
				FROM t_GroupSupp 
				WHERE GroupSuppID=@GroupSuppID
			END
		END

		IF @action='EDIT'
		BEGIN
			UPDATE t_GroupSupp SET GroupSuppName=@GroupSuppName,Regional=@Regional,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
			WHERE GroupSuppID=@GroupSuppID
		END
		IF @action='DELETE'
		BEGIN
			UPDATE t_GroupSupp SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
				WHERE GroupSuppID=@GroupSuppID

			UPDATE t_GroupIncentive SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
				WHERE GroupSuppID=@GroupSuppID and ISNULL(FCDefunctInd,0)=0

			--untuk delete tblGroupSupp
			DELETE gs
			FROM TblGroupSupp gs
			INNER JOIN
				(
				SELECT b.GroupSuppName,b.Regional,a.* 
				FROM t_GroupSuppDet a
				LEFT JOIN t_GroupSupp b
					ON a.GroupSuppID=b.GroupSuppID
				WHERE b.GroupSuppID=@GroupSuppID
				) a
			ON gs.GroupSupp=a.GroupSuppName 
				AND gs.PMKSID=a.PMKSID 
				AND gs.SupplierID=a.SupplierID
				AND gs.Regional=a.Regional

			INSERT INTO t_GroupSuppDet_Audit
			SELECT *,@ACCESSID, @action, GETDATE()
			FROM t_GroupSuppDet 
			WHERE GroupSuppID=@GroupSuppID and ISNULL(FCDefunctInd,0)=0

			INSERT INTO t_GroupIncentive_Audit
			SELECT *,@ACCESSID, @action, GETDATE()
			FROM t_GroupIncentive 
			WHERE GroupSuppID=@GroupSuppID

			UPDATE t_GroupSuppDet SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
				WHERE GroupSuppID=@GroupSuppID and ISNULL(FCDefunctInd,0)=0
		END
		IF @action='UNDELETE'
		BEGIN
			UPDATE t_GroupSupp SET FCDefunctInd=0,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
				WHERE GroupSuppID=@GroupSuppID
		END

		--untuk Log History Audit Table
		INSERT INTO t_GroupSupp_Audit
		SELECT *,@ACCESSID, @action, GETDATE()
		FROM t_GroupSupp 
		WHERE GroupSuppID=@GroupSuppID
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDGroupSuppDet]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_CRUDGroupSuppDet]
(
@action varchar(9),
@NoId int output,
@GroupSuppID int,
@PMKSID varchar(50),
@SupplierID varchar(50),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	Declare @msg varchar(500)
		IF @action='ADD'
		BEGIN
			INSERT INTO t_GroupSuppDet
				(GroupSuppID, PMKSID, SupplierID, FCCreatedBy,FCCreatedDT,FCUpdatedBy,FCUpdatedDT) 
			VALUES 
				(@GroupSuppID, @PMKSID, @SupplierID, @ACCESSID,GETDATE(),@ACCESSID,GETDATE())
				SET @NoId=SCOPE_IDENTITY()

			--untuk update ke tblGroupSupp
			DECLARE @Ro varchar(10),@groupname varchar(200),@suppliername varchar(200),@userid varchar(50)
			SELECT @groupname=GroupSuppName,@Ro=Regional FROM t_GroupSupp WHERE GroupSuppID=@GroupSuppID
			SELECT @suppliername=SupplierName FROM tblSupplier WHERE PMKSID=@PMKSID and SupplierID=@SupplierID
			SELECT @userid=FCName FROM t_User WHERE FCUserID=@ACCESSID

			INSERT INTO TblGroupSupp
				(GroupSupp,PMKSID,SupplierID,SupplierName,Regional,UserID,LastAccess)
			VALUES
				(@groupname,@PMKSID,@SupplierID,@suppliername,@Ro,@userid,GETDATE())
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM t_GroupSuppDet_Audit 
							WHERE NoId=@NoId)
			BEGIN
				INSERT INTO t_GroupSuppDet_Audit
				SELECT *,@ACCESSID, 'AWAL', GETDATE()
				FROM t_GroupSuppDet 
				WHERE NoId=@NoId
			END
		END

		IF @action='EDIT'
		BEGIN
			UPDATE t_GroupSuppDet SET PMKSID=@PMKSID, SupplierID=@SupplierID,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
			WHERE NoId=@NoId
		END
		IF @action='DELETE'
		BEGIN
			UPDATE t_GroupSuppDet SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
				WHERE NoId=@NoId

			-- untuk delete di tblGroupSupp
			DELETE gs
			FROM TblGroupSupp gs
			INNER JOIN
				(
				SELECT b.GroupSuppName,b.Regional,a.* 
				FROM t_GroupSuppDet a
				LEFT JOIN t_GroupSupp b
					ON a.GroupSuppID=b.GroupSuppID
				WHERE NoId=@NoId
				) a
			ON gs.GroupSupp=a.GroupSuppName 
				AND gs.PMKSID=a.PMKSID 
				AND gs.SupplierID=a.SupplierID
				AND gs.Regional=a.Regional
		END
		IF @action='UNDELETE'
		BEGIN
			UPDATE t_GroupSuppDet SET FCDefunctInd=0,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
				WHERE NoId=@NoId
		END
		--untuk Log History Audit Table
		INSERT INTO t_GroupSuppDet_Audit
		SELECT *,@ACCESSID, @action, GETDATE()
		FROM t_GroupSuppDet 
		WHERE NoId=@NoId
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDHolidayBank]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[sp_CRUDHolidayBank]
(
@action varchar(9),
@Date date,
@Remarks varchar(50),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO TblHolidayBank([Date],Remarks) VALUES (@Date,@Remarks)
		END
		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM TblHolidayBank_Audit
							WHERE Date=@Date AND Remarks=@Remarks)
			BEGIN
				INSERT INTO TblHolidayBank_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM TblHolidayBank WHERE Date=@Date AND Remarks=@Remarks
			END
		END

		INSERT INTO TblHolidayBank_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM TblHolidayBank WHERE Date=@Date AND Remarks=@Remarks

		IF @action='DELETE'
		BEGIN
			DELETE TblHolidayBank WHERE Date=@Date AND Remarks=@Remarks
		END
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDIncentive]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_CRUDIncentive]
(
@action varchar(9),
@PERIODE VARCHAR(10),
@IncentiveID int,
@GroupSuppID int,
@GroupSuppName varchar(200),
@CompanyCode varchar(50),
@VendorCode varchar(50),
@PMKSID VARCHAR(10),
@SupplierID VARCHAR(50),
@SupplierName varchar(200),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	Declare @UserID varchar(50)
	SELECT @UserID=FCName FROM t_User Where FCUserID=@ACCESSID
		IF @action='ADD'
		BEGIN
			Declare
				--@SupplierName VARCHAR(255)
				@Netto float
				, @Incentive int
				, @PPH22 decimal(18,2)
				, @Remarks VARCHAR(255)
			SELECT 
				--@SupplierName=SupplierName,
				@PPH22=PPH22
			FROM tblSupplier WHERE PMKSID=@PMKSID and SupplierID=@SupplierID

			SELECT @NETTO=SUM(NETTO) FROM tblFFB 
			WHERE PMKSID=@PMKSID and Supplier=@SupplierID
					and CAST(TanggalTimbang as date)>=@PERIODE+'-01'
					and CAST(TanggalTimbang as date)<=EOMONTH(@PERIODE+'-01')

			SELECT @Incentive=
				IIF(IncentiveQty6<>0 AND @Netto>=IncentiveQty6,IncentivePrice6,
					IIF(IncentiveQty5<>0 AND @Netto>=IncentiveQty5,IncentivePrice5,
						IIF(IncentiveQty4<>0 AND @Netto>=IncentiveQty4,IncentivePrice4,
							IIF(IncentiveQty3<>0 AND @Netto>=IncentiveQty3,IncentivePrice3,
								IIF(IncentiveQty2<>0 AND @Netto>=IncentiveQty2,IncentivePrice2,
									IIF(IncentiveQty1<>0 AND @Netto>=IncentiveQty1,IncentivePrice1,
										0))))))
			FROM t_GroupIncentive WHERE GroupSuppID=@GroupSuppID and ISNULL(FCDefunctInd,0)=0

			INSERT INTO [dbo].[t_Incentive]
				([Periode],[GroupSuppID],[PMKSID],[SupplierID],[NETTO],[Incentive],[PPH22],[Remarks],[FCCreatedBy],[FCCreatedDT])
			VALUES
			(REPLACE(@PERIODE,'-',''),@GroupSuppID,@PMKSID,@SupplierID,@Netto,@Incentive,@PPH22,@GroupSuppName+' - Total Netto = '+LTRIM(RTRIM(STR(@Netto, 25, 2))),@ACCESSID,getdate())
			SET @IncentiveID=SCOPE_IDENTITY()

			--untuk tblIncentive
			INSERT INTO tblIncentive
				(GroupSupp,[Period],sap_company_code,sap_vendor_code
				,SupplierID,SupplierName,PMKSID,Netto,Incentive
				,PPH22,Remarks,UserID,LastAccess)
			VALUES
				(@GroupSuppName,REPLACE(@PERIODE,'-',''),@CompanyCode,@VendorCode
				,@SupplierID,@SupplierName,@PMKSID,@Netto,@Incentive
				,@PPH22,@SupplierName+' - Total Netto = '+LTRIM(RTRIM(STR(@Netto, 25, 2)))
				,@UserID,GETDATE())
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM t_Incentive_Audit WHERE IncentiveID=@IncentiveID)
			BEGIN
				INSERT INTO t_Incentive_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM t_Incentive WHERE IncentiveID=@IncentiveID
			END
		END

		IF @action='DELETE'
		BEGIN
			UPDATE [dbo].[t_Incentive] 
			SET FCDefunctInd=1, FCUpdatedBy=@ACCESSID, FCUpdatedDT=getdate() 
			WHERE IncentiveID=@IncentiveID
			--delete tblIncentive
			DELETE FROM tblIncentive 
			WHERE GroupSupp=@GroupSuppName
				AND [Period]=REPLACE(@PERIODE,'-','')
				AND SupplierID=@SupplierID
				AND PMKSID=@PMKSID
		END

		IF @action='APPROVE'
		BEGIN
			UPDATE [dbo].[t_Incentive]
			SET Approval=1, FCApproveBy=@ACCESSID, FCApproveDT=getdate() 
			WHERE IncentiveID=@IncentiveID

			UPDATE tblIncentive 
			SET Approval=1, ApprovalBy=@UserID, ApprovalDT=GETDATE()
			WHERE GroupSupp=@GroupSuppName
				AND [Period]=REPLACE(@PERIODE,'-','')
				AND SupplierID=@SupplierID
				AND PMKSID=@PMKSID
		END

		INSERT INTO t_Incentive_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM t_Incentive WHERE IncentiveID=@IncentiveID
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDMenu]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[sp_CRUDMenu]
(
@action varchar(9),
@FCMENUID int output,
@FCMENUCODE nvarchar(50),
@FCMENUDESC	nvarchar(50),
@FCMENUPID int,
@FCICON varchar(50),
@FCMENULINK bit,
@FCORDERNO int,
@FCHIDDEN bit,
@ACCESSID int
)
AS
	IF @action='ADD'
	BEGIN
		INSERT INTO t_Menu(FCMenuCode,FCMenuDesc,FCMenuPID,FCMenuLink,FCOrderNo,FCIcon,FCHidden,FCCreatedBy,FCCreatedDT) VALUES (@FCMENUCODE,@FCMENUDESC,@FCMENUPID,@FCMENULINK,@FCORDERNO,@FCICON,@FCHIDDEN,@ACCESSID,GETDATE())
		SET @FCMENUID=SCOPE_IDENTITY()
	END

	IF @action='EDIT'
	BEGIN
		UPDATE t_Menu SET FCMenuCode=@FCMENUCODE,FCMenuDesc=@FCMENUDESC,FCMenuPID=@FCMENUPID,FCMenuLink=@FCMENULINK,FCOrderNo=@FCORDERNO,FCIcon=@FCICON,FCHidden=@FCHIDDEN,FCCreatedBy=@ACCESSID,FCCreatedDT=GETDATE() WHERE  FCMenuID=@FCMENUID
	END
	IF @action='DEFUNCT'
	BEGIN
		UPDATE t_Menu SET FCDEFUNCTIND=1,FCCREATEDBY=@ACCESSID,FCCREATEDDT=GETDATE() WHERE FCMenuID=@FCMENUID
	END
	IF @action='UNDEFUNCT'
	BEGIN
		UPDATE t_Menu SET FCDEFUNCTIND=0,FCCREATEDBY=@ACCESSID,FCCREATEDDT=GETDATE() WHERE FCMenuID=@FCMENUID
	END

	IF @action='DELETE'
	BEGIN
		UPDATE t_menu SET FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCMenuID=@FCMENUID

		DELETE FROM t_Menu WHERE FCMenuID=@FCMENUID
		DELETE FROM t_RoleDet WHERE FCMenuID=@FCMENUID
	END
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDPMKS]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[sp_CRUDPMKS]
(
@action varchar(20),
--@Approval bit=null,
@AreaOperational varchar(100),
@Regional varchar(25),
--@Urut bit = null,
@PMKSID varchar(3),
@PMKSName varchar(50),
@Company varchar(50),
@KomidelMin float,
@HargaMin int,
@KomidelPlus float,
@HargaPlus int,
@CompanyCode varchar(6),
@BusinessArea varchar(6),
@BusinessAreaCoP varchar(6),
@PMKSGroup varchar(50),
@HouseBank varchar(5),
--@UserID varchar(15),
--@LastAccess datetime,
@PaySaturday bit,
@KomidelMin1 float,
@HargaMin1 int,
@KomidelPlus1 float,
@HargaPlus1 int,
@KomidelMin2 float,
@HargaMin2 int,
@KomidelPlus2 float,
@HargaPlus2 int,
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO tblPMKS
				(AreaOperational, Regional, PMKSID, PMKSName, Company
				, KomidelMin, HargaMin, KomidelPlus, HargaPlus
				, CompanyCode, BusinessArea, BusinessAreaCoP, PMKSGroup
				, HouseBank, PaySaturday, KomidelMin1, HargaMin1
				, KomidelPlus1, HargaPlus1, KomidelMin2, HargaMin2
				, KomidelPlus2, HargaPlus2, FCCreatedBy, FCCreatedDT
				)
			VALUES 
				(@AreaOperational, @Regional, @PMKSID, @PMKSName, @Company
				, @KomidelMin, @HargaMin, @KomidelPlus, @HargaPlus
				, @CompanyCode, @BusinessArea, @BusinessAreaCoP, @PMKSGroup
				, @HouseBank, @PaySaturday, @KomidelMin1, @HargaMin1
				, @KomidelPlus1, @HargaPlus1, @KomidelMin2, @HargaMin2
				, @KomidelPlus2, @HargaPlus2, @ACCESSID, getdate()
				)
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblPMKS_Audit WHERE PMKSID=@PMKSID)
			BEGIN
				INSERT INTO tblPMKS_Audit
				SELECT *,@ACCESSID, 'AWAL', GETDATE()
				FROM tblPMKS WHERE PMKSID=@PMKSID
			END
		END

		IF @action='EDIT'
		BEGIN
			Update tblPMKS
			SET
				Approval=0
				,AreaOperational = @AreaOperational
				, Regional = @Regional
				, PMKSName = @PMKSName
				, Company = @Company
				, KomidelMin = @KomidelMin
				, HargaMin = @HargaMin
				, KomidelPlus = @KomidelPlus
				, HargaPlus = @HargaPlus
				, CompanyCode = @CompanyCode
				, BusinessArea = @BusinessArea
				, BusinessAreaCoP = @BusinessAreaCoP
				, PMKSGroup = @PMKSGroup
				, HouseBank = @HouseBank
				, PaySaturday = @PaySaturday
				, KomidelMin1 = @KomidelMin1
				, HargaMin1 = @HargaMin1
				, KomidelPlus1 = @KomidelPlus1
				, HargaPlus1 = @HargaPlus1
				, KomidelMin2 = @KomidelMin2
				, HargaMin2 = @HargaMin2
				, KomidelPlus2 = @KomidelPlus2
				, HargaPlus2 = @HargaPlus2
				, FCUpdatedBy = @ACCESSID
				, FCUpdatedDT = getdate()
			WHERE PMKSID=@PMKSID
		END

		IF @action='APPROVE'
		BEGIN
			UPDATE tblPMKS 
				SET Approval=1, FCUpdatedBy = @ACCESSID, FCUpdatedDT = getdate() 
			WHERE PMKSID=@PMKSID
		END

		IF @action='UNAPPROVE'
		BEGIN
			UPDATE tblPMKS 
				SET Approval=0, FCUpdatedBy = @ACCESSID, FCUpdatedDT = getdate() 
			WHERE PMKSID=@PMKSID
		END

		INSERT INTO tblPMKS_Audit
		SELECT *,@ACCESSID, @action, GETDATE()
		FROM tblPMKS WHERE PMKSID=@PMKSID

		IF @action='DELETE'
		BEGIN
			DELETE tblPMKS WHERE PMKSID=@PMKSID
		END
		COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDPPN]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_CRUDPPN]
(
@action varchar(20),
@periode varchar(6),
@no_faktur_pajak varchar(25),
@ppn_type varchar(1),
@tgl_faktur_pajak date,
--@tgl_posting date,
@sap_company_code varchar(4),
@sap_vendor_code varchar(6),
@periode_awal date,
@periode_akhir date,
@disetorKe varchar(1),
@ppn_penyelesaian bit,
@no_faktur_pajak_advance varchar(25),
@CashNo varchar(25),
@amount_cash_advance bigint,
@total_amount bigint,
@incentive bigint,
@ppn bigint,
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO tblPPN
				(periode, no_faktur_pajak, ppn_type
				, tgl_faktur_pajak, sap_company_code, sap_vendor_code,tgl_posting
				, periode_awal, periode_akhir, disetorKe, ppn_penyelesaian
				, no_faktur_pajak_advance, CashNo, amount_cash_advance
				, total_amount, incentive, ppn, FCCreatedBy, FCCreatedDT
				)
			VALUES 
				(@periode, @no_faktur_pajak, @ppn_type
				, @tgl_faktur_pajak, @sap_company_code, @sap_vendor_code,GETDATE()
				, cast(@periode_awal as date), cast(@periode_akhir as date), @disetorKe, @ppn_penyelesaian
				, @no_faktur_pajak_advance, @CashNo, @amount_cash_advance
				, @total_amount, @incentive, @ppn, @ACCESSID, getdate()
				)
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblPPN WHERE no_faktur_pajak=@no_faktur_pajak)
			BEGIN
				INSERT INTO tblPPN_Audit
				SELECT *,@ACCESSID,@action,GETDATE()
				FROM tblPPN WHERE no_faktur_pajak=@no_faktur_pajak
			END
		END

		IF @action='ADDADVANCE'
		BEGIN
			INSERT INTO tblPPN
				(periode, no_faktur_pajak, ppn_type
				, tgl_faktur_pajak, sap_company_code, sap_vendor_code
				, periode_awal, periode_akhir, disetorKe, ppn_penyelesaian
				, no_faktur_pajak_advance, CashNo, amount_cash_advance
				, total_amount, incentive, ppn, FCCreatedBy, FCCreatedDT
				)
			VALUES 
				(@periode, @no_faktur_pajak, @ppn_type
				, cast(@tgl_faktur_pajak as date), @sap_company_code, @sap_vendor_code
				, null, null, @disetorKe, @ppn_penyelesaian
				, @no_faktur_pajak_advance, @CashNo, @amount_cash_advance
				, @total_amount, @incentive, @ppn, @ACCESSID, getdate()
				)
		END

		INSERT INTO tblPPN_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblPPN WHERE no_faktur_pajak=@no_faktur_pajak

		IF @action='DELETE'
		BEGIN
			DELETE tblPPN WHERE no_faktur_pajak=@no_faktur_pajak
		END
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDPrice]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[sp_CRUDPrice]
(
@action varchar(9),
@SupplierID nvarchar(50),
@SupplierName nvarchar(50),
@DatePrice varchar(20),
@PMKSID nvarchar(50),
@Price int,
@PPH22 float,
@VAT float,

@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @UserID varchar(15)
	SELECT @UserID=FCUserName FROM t_User WHERE FCUserID=@ACCESSID
		IF @action='ADD'
		BEGIN
			INSERT INTO tblPrice
				(SupplierID, SupplierName,DatePrice, PMKSID
				, Price, PPH22, VAT,UserID
				, FCCreatedBy, FCCreatedDT
				)
			VALUES 
				(@SupplierID, @SupplierName, @DatePrice, @PMKSID
				, @Price, @PPH22, @VAT,@UserID
				, @ACCESSID, getdate()
				)
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblPrice_Audit 
                   WHERE SupplierID=@SupplierID 
					and SupplierName=@SupplierName 
					and dateprice=@DatePrice 
					and PMKSID=@PMKSID)
			BEGIN
				INSERT INTO tblPrice_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblPrice 
				WHERE SupplierID=@SupplierID 
						and SupplierName=@SupplierName 
						and dateprice=@DatePrice 
						and PMKSID=@PMKSID
			END
		END

		IF @action='EDIT'
		BEGIN
			Update tblPrice
			SET
				Price = @Price
				, PPH22 = @PPH22
				, VAT = @VAT
				, UserID = @UserID
				, FCUpdatedBy = @ACCESSID
				, FCUpdatedDT = getdate()
			WHERE SupplierID=@SupplierID 
				and SupplierName=@SupplierName 
				and PMKSID=@PMKSID 
				and DatePrice= @DatePrice
		END

		INSERT INTO tblPrice_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblPrice 
		WHERE SupplierID=@SupplierID 
				and SupplierName=@SupplierName 
				and dateprice=@DatePrice 
				and PMKSID=@PMKSID

		IF @action='DELETE'
		BEGIN
			DELETE tblPrice 
			WHERE SupplierID=@SupplierID 
				and SupplierName=@SupplierName 
				and dateprice=@DatePrice 
				and PMKSID=@PMKSID
		END

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDPriceCompetitor]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_CRUDPriceCompetitor]
(
@action varchar(9),
--@PriceCompetitorID as varchar(50),
@Date as date,
@PMKSID as varchar(50),
--@CompetitorID as int,
@CompetitorName as varchar(200),
@Price as int,
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO TblPriceCompetitor
				([Date],PMKSID,CompetitorName,Price,FCCreatedBy,FCCreatedDT)
			VALUES
				(@Date,@PMKSID,@CompetitorName,@Price,@ACCESSID,GETDATE())
			--Declare
			--	@ID as varchar(50),
			--	@IDFilter as varchar(50)
			--SET @IDFilter='%'+convert(varchar,@Date)+'/'+@PMKSID+'-'+CONVERT(varchar,@CompetitorID)+'/%'
			--SELECT TOP 1 @ID=RIGHT(PriceCompetitorID,2)
			--FROM t_PriceCompetitor 
			--WHERE PriceCompetitorID like @IDFilter and [Date]=@Date
			--ORDER BY PriceCompetitorID Desc

			--SET @PriceCompetitorID=convert(varchar,@Date)+'/'+@PMKSID+'-'+CONVERT(varchar,@CompetitorID)+'/'+FORMAT(ISNULL(RIGHT(@ID,2),0)+1,'00')

			--INSERT INTO t_PriceCompetitor
			--	(PriceCompetitorID, [Date], PMKSID, CompetitorID, Price, FCCreatedBy, FCCreatedDT)
			--VALUES
			--	(@PriceCompetitorID,@Date,@PMKSID,@CompetitorID, @Price, @ACCESSID, getdate())
		END

		--IF @action<>'ADD'
		--BEGIN
		--	IF NOT EXISTS (SELECT * FROM t_PriceCompetitor_Audit 
		--					WHERE PriceCompetitorID=@PriceCompetitorID and [Date]=@Date)
		--	BEGIN
		--		INSERT t_PriceCompetitor_Audit
		--		SELECT *,@ACCESSID,'AWAL',GETDATE()
		--		FROM t_PriceCompetitor 
		--		WHERE PriceCompetitorID=@PriceCompetitorID and [Date]=@Date
		--	END
		--END

		IF @action='EDIT'
		BEGIN
			UPDATE TblPriceCompetitor
			SET Price=@Price, FCUpdatedBy=@ACCESSID, FCUpdatedDT=getdate()
			WHERE [Date]=@Date AND PMKSID=@PMKSID AND CompetitorName=@CompetitorName
			--UPDATE t_PriceCompetitor 
			--SET Price=@Price, FCUpdatedBy=@ACCESSID, FCUpdatedDT=getdate() 
			--WHERE PriceCompetitorID=@PriceCompetitorID and [Date]=@Date
		END


		IF @action='DELETE'
		BEGIN
			DELETE TblPriceCompetitor
			WHERE [Date]=@Date AND PMKSID=@PMKSID AND CompetitorName=@CompetitorName
			--UPDATE t_PriceCompetitor 
			--UPDATE t_PriceCompetitor 
			--SET FCDefunctInd=1, FCUpdatedBy=@ACCESSID, FCUpdatedDT=getdate() 
			--WHERE PriceCompetitorID=@PriceCompetitorID and [Date]=@Date
		END

		--INSERT t_PriceCompetitor_Audit
		--SELECT *,@ACCESSID,@action,GETDATE()
		--FROM t_PriceCompetitor 
		--WHERE PriceCompetitorID=@PriceCompetitorID and [Date]=@Date
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDPriceCompetitorTarikData]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [dbo].[sp_CRUDPriceCompetitorTarikData]
(
@action varchar(9),
@Date as date,
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	IF @action='TARIKDATA'
	BEGIN
		--DECLARE @Counter INT, @TotalCount INT, @PMKSID varchar(50),@CompetitorID INT,@Price INT
		----DELETE t_PriceCompetitor WHERE Date=@Date
		--UPDATE t_PriceCompetitor SET FCDefunctInd=1, FCUpdatedBy=@ACCESSID, FCUpdatedDT=getdate() WHERE Date=@Date
		--SET @Counter = 1
		--SET @TotalCount = (SELECT COUNT(*) FROM t_PriceCompetitor WHERE Date=DATEADD(day, -1,@Date)and ISNULL(FCDefunctInd,0)=0)

		--SELECT ROW_NUMBER() OVER(order by PriceCompetitorID) AS RowId,* 
		--	into #Temp1
		--FROM t_PriceCompetitor 
		--WHERE Date=DATEADD(day, -1,@Date)
		--	and ISNULL(FCDefunctInd,0)=0

		--WHILE (@Counter <=@TotalCount)
		--BEGIN
		--	SELECT @PMKSID=PMKSID,@CompetitorID=CompetitorID,@Price=Price FROM #Temp1 WHERE RowId = @Counter
		--	Declare
		--		@ID as varchar(50),
		--		@IDFilter as varchar(50),
		--		@PriceCompetitorID as varchar(50)
		--	SET @IDFilter='%'+convert(varchar,@Date)+'/'+@PMKSID+'-'+CONVERT(varchar,@CompetitorID)++'/%'
		--	SELECT TOP 1 @ID=RIGHT(PriceCompetitorID,2)
		--	FROM t_PriceCompetitor 
		--	WHERE PriceCompetitorID like @IDFilter and [Date]=@Date
		--	ORDER BY PriceCompetitorID Desc

		--	SET @PriceCompetitorID=convert(varchar,@Date)+'/'+@PMKSID+'-'+CONVERT(varchar,@CompetitorID)+'/'+FORMAT(ISNULL(RIGHT(@ID,2),0)+1,'00')

		--	INSERT INTO t_PriceCompetitor
		--		(PriceCompetitorID, [Date], PMKSID, CompetitorID, Price)
		--	VALUES
		--		(@PriceCompetitorID,@Date,@PMKSID,@CompetitorID, @Price)
 
		--	SET @Counter = @Counter + 1
		--	CONTINUE;
		--END
		--DROP TABLE #Temp1

		INSERT INTO TblPriceCompetitor
		([Date],PMKSID,CompetitorName,Price,FCCreatedBy,FCCreatedDT)
		SELECT a.* 
		FROM
			(SELECT 
				@Date as [Date],PMKSID,CompetitorName,Price,
				@ACCESSID as FCCreateBy,GETDATE() as FCCreateDT
			FROM TblPriceCompetitor WHERE [Date]=DATEADD(day, -1,@Date)
			)a
		LEFT JOIN
			(SELECT * FROM TblPriceCompetitor WHERE [Date]=@Date) b
		ON a.PMKSID=b.PMKSID AND a.CompetitorName=b.CompetitorName
		WHERE b.PMKSID IS NULL
	END

	--INSERT t_PriceCompetitor_Audit
	--SELECT *,@ACCESSID,@action,GETDATE()
	--FROM t_PriceCompetitor WHERE [Date]=@Date
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDPriceDefault]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [dbo].[sp_CRUDPriceDefault]
(
@action varchar(9),
@Efective_Date date,
@SupplierID varchar(50),
@SupplierName varchar(50),
@PMKSID varchar(50),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO tblPriceDefault
			(SupplierID,SupplierName,Efective_Date,PMKSID,FCCreatedBy,FCCreatedDT)
			VALUES
			(@SupplierID,@SupplierName,@Efective_Date,@PMKSID,@ACCESSID,GETDATE())
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblPriceDefault_Audit 
							WHERE SupplierID=@SupplierID 
								AND Efective_Date=@Efective_Date 
								AND PMKSID=@PMKSID)
			BEGIN
				INSERT INTO tblPriceDefault_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblPriceDefault 
				WHERE SupplierID=@SupplierID 
					AND Efective_Date=@Efective_Date 
					AND PMKSID=@PMKSID
			END
		END

		INSERT INTO tblPriceDefault_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblPriceDefault 
		WHERE SupplierID=@SupplierID 
			AND Efective_Date=@Efective_Date 
			AND PMKSID=@PMKSID

		IF @action='DELETE'
		BEGIN
			DELETE tblPriceDefault 
			WHERE
				SupplierID=@SupplierID 
				AND Efective_Date=@Efective_Date 
				AND PMKSID=@PMKSID
		END
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH

GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDPriceUpHarga]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[sp_CRUDPriceUpHarga]
(
@json varchar(max),
@ACCESSID int
)
AS

SELECT * INTO #Temp1 FROM  
 OPENJSON ( @json )  
WITH (   
		SupplierID   varchar(50) '$.SupplierID' ,  
		PMKSID     varchar(50)     '$.PMKSID',  
		DatePrice date '$.DatePrice',
		UpPrice int '$.UpPrice'
 )

BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @UserID varchar(15)
	SELECT @UserID=FCUserName FROM t_User WHERE FCUserID=@ACCESSID
		INSERT INTO tblPrice_Audit
		SELECT 
			a.SupplierID,a.SupplierName,a.DatePrice,a.PMKSID,(a.Price+b.UpPrice) as Price,
			a.PPH22,a.vat,a.KomidelCalculate,@UserID,a.LastAccess,FCCreatedBy,FCCreatedDT,
			FCUpdatedBy,FCUpdatedDT,@ACCESSID as UserAudit,'B-UPHARGA' as ActionAudit,GETDATE() as TimeAudit
		FROM tblPrice a
			INNER JOIN #Temp1 b
				ON a.SupplierID=b.SupplierID and a.PMKSID=b.PMKSID and a.DatePrice=b.DatePrice

		UPDATE a
			SET a.Price=a.Price+b.UpPrice,a.UserID=@UserID,a.FCUpdatedBy=@ACCESSID,a.FCUpdatedDT=GETDATE()
		FROM tblPrice a
			INNER JOIN #Temp1 b
				ON a.SupplierID=b.SupplierID and a.PMKSID=b.PMKSID and a.DatePrice=b.DatePrice

		INSERT INTO tblPrice_Audit
		SELECT 
			a.SupplierID,a.SupplierName,a.DatePrice,a.PMKSID,(a.Price+b.UpPrice) as Price,
			a.PPH22,a.vat,a.KomidelCalculate,@UserID,a.LastAccess,FCCreatedBy,FCCreatedDT,
			FCUpdatedBy,FCUpdatedDT,@ACCESSID as UserAudit,'A-UPHARGA' as ActionAudit,GETDATE() as TimeAudit
		FROM tblPrice a
			INNER JOIN #Temp1 b
				ON a.SupplierID=b.SupplierID and a.PMKSID=b.PMKSID and a.DatePrice=b.DatePrice
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction UPHARGA Failed.'
	RAISERROR (@msg,16,1)
END CATCH

DROP TABLE #Temp1
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDRole]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_CRUDRole]
(
@action varchar(9),
@FCROLEID int output,
@FCROLEDESC	varchar(50),
@ACCESSID int
)
AS
	IF @action='ADD'
	BEGIN
		INSERT INTO t_Role(FCRoleDesc,FCCreatedBy,FCCreatedDT) VALUES (@FCROLEDESC,@ACCESSID,GETDATE())
		SET @FCROLEID=SCOPE_IDENTITY()
	END
	IF @action='EDIT'
	BEGIN
		UPDATE t_Role SET FCRoleDesc=@FCROLEDESC,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID
	END
	IF @action='DEFUNCT'
	BEGIN
		UPDATE t_Role SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID
		UPDATE t_RoleDet SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID
	END
	IF @action='UNDEFUNCT'
	BEGIN
		UPDATE t_Role SET FCDefunctInd=0,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID
	END

	IF @action='DELETE'
	BEGIN
		UPDATE t_RoleDet SET FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID
		DELETE FROM t_RoleDet WHERE FCRoleID=@FCROLEID
		UPDATE t_Role SET FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID
		DELETE FROM t_Role WHERE FCRoleID=@FCROLEID
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDRoleDet]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_CRUDRoleDet]
(
@action varchar(9),
@FCROLEID int,
@FCMENUID int,
@FCADD bit,
@FCEDIT bit,
@FCDELETE bit,
@FCUNDELETE bit,
@FCApprove bit,
@ACCESSID int
)
AS
	--UPDATE t_Role SET FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID
	IF @action='ADD'
	BEGIN
		INSERT INTO t_RoleDet(FCRoleID,FCMenuID,FCAdd,FCEdit,FCDelete,FCUndelete,FCApprove,FCCreatedBy,FCCreatedDT) VALUES (@FCROLEID,@FCMENUID,@FCADD,@FCEDIT,@FCDELETE,@FCUNDELETE,@FCApprove,@ACCESSID,GETDATE())
	END
	IF @action='EDIT'
	BEGIN
		UPDATE t_RoleDet SET FCAdd=@FCADD,FCEdit=@FCEDIT,FCDelete=@FCDELETE,FCUndelete=@FCUNDELETE,FCApprove=@FCApprove,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID AND FCMenuID=@FCMENUID
	END
	IF @action='DEFUNCT'
	BEGIN
		UPDATE t_RoleDet SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID AND FCMenuID=@FCMENUID
	END
	IF @action='UNDEFUNCT'
	BEGIN
		UPDATE t_RoleDet SET FCDefunctInd=0,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID AND FCMenuID=@FCMENUID
	END

	IF @action='DELETE'
	BEGIN
		UPDATE t_RoleDet SET FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCRoleID=@FCROLEID AND FCMenuID=@FCMENUID
		DELETE FROM t_RoleDet WHERE FCRoleID=@FCROLEID AND FCMenuID=@FCMENUID
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDSetReport]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [dbo].[sp_CRUDSetReport]
(
@action varchar(9),
@ReportID varchar(100),
@Name1 varchar(100),
@Name2 varchar(100),
@Name3 varchar(100),
@Name4 varchar(100),
@Name5 varchar(100),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO set_RptName
			(ReportID,FCUserID,Name1,Name2,Name3,Name4,Name5,FCCreatedBy,FCCreatedDT)
			VALUES
			(@ReportID,@ACCESSID,@Name1,@Name2,@Name3,@Name4,@Name5,@ACCESSID,GETDATE())
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM set_RptName_Audit 
							WHERE ReportID=@ReportID AND FCUserID=@ACCESSID)
			BEGIN
				INSERT INTO [dbo].[set_RptName_Audit]
				([ReportID],[FCUserID],[Name1],[Name2],[Name3],[Name4],[Name5]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[UserAudit],[ActionAudit],[TimeAudit])
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM set_RptName WHERE ReportID=@ReportID AND FCUserID=@ACCESSID
			END
		END

		IF @action='EDIT'
		BEGIN
			UPDATE set_RptName 
			SET 
				Name1=@Name1, 
				Name2=@Name2, 
				Name3=@Name3, 
				Name4=@Name4, 
				Name5=@Name5, 
				FCUpdatedBy=@ACCESSID, FCUpdatedDT=GETDATE()
			WHERE
				ReportID=@ReportID AND FCUserID=@ACCESSID
		END

		INSERT INTO [dbo].[set_RptName_Audit]
			([ReportID],[FCUserID],[Name1],[Name2],[Name3],[Name4],[Name5]
			,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
			,[UserAudit],[ActionAudit],[TimeAudit])
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM set_RptName WHERE ReportID=@ReportID AND FCUserID=@ACCESSID

		IF @action='DELETE'
		BEGIN
			DELETE set_RptName 
			WHERE ReportID=@ReportID AND FCUserID=@ACCESSID
		END
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDSupplier]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[sp_CRUDSupplier]
(
@action varchar(9),
@SupplierID nvarchar(15),
@SupplierName nvarchar(50),
@PMKSID nvarchar(50),
@PMKSName nvarchar(50),
@Initial nvarchar(15),
@CashAdvance varchar(1),
@Code nvarchar(8),
@NPWP nvarchar(25),
@GroupSupplier nchar(50),
@Category nchar(50),
@VAT int,
@VATCondition  varchar(1),
@PPH22 float,
@PPH22Condition  varchar(1),
@Remarks nvarchar(60),
@Remarks1 nvarchar(60),
@Remarks2 nvarchar(60),
@PaymentTerm varchar(8),
@KomidelCalculate  bit,
@BibitTopaz  varchar(1),
@StatusActive  varchar(1),
@ACCESSID int
)
AS
DECLARE @KomidelCalculateChar  varchar(1)
SET @KomidelCalculateChar=IIF(@KomidelCalculate=0,'N','Y')
IF ISNULL(@PMKSID,'')=''
BEGIN
	RAISERROR ( 'Sorry Fill PMKS ID First.',16,1)
	Return
END

BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			Declare
				@ID as varchar(50),
				@IDFilter as varchar(50)

			SET @IDFilter='%'+RTRIM(LTRIM(@PMKSID))+'-%'

			SELECT TOP 1 @ID=RIGHT(SupplierID,4) FROM tblSupplier 
			WHERE SupplierID like @IDFilter	ORDER BY SupplierID Desc

			SET @SupplierID=@PMKSID+'-'+FORMAT(ISNULL(RIGHT(@ID,4),0)+1,'0000')

			INSERT INTO tblSupplier
				(SupplierID, SupplierName, PMKSID, PMKSName
				, Initial, CashAdvance, Code, NPWP
				, GroupSupplier, Category, VAT, VATCondition
				, PPH22, PPH22Condition, Remarks, Remarks1, Remarks2
				, PaymentTerm, BibitTopaz,KomidelCalculate,StatusActive
				, FCCreatedBy, FCCreatedDT
				)
			VALUES 
				(@SupplierID, @SupplierName, @PMKSID, @PMKSName
				, @Initial, @CashAdvance, @Code, @NPWP
				, @GroupSupplier, @Category, @VAT, @VATCondition
				, @PPH22, @PPH22Condition, @Remarks, @Remarks1, @Remarks2
				, @PaymentTerm, @BibitTopaz,@KomidelCalculateChar,@StatusActive
				, @ACCESSID, getdate()
				)
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblSupplier_Audit 
							WHERE SupplierID=@SupplierID and SupplierName=@SupplierName 
								and PMKSID=@PMKSID)
			BEGIN
				INSERT INTO tblSupplier_Audit
				SELECT *,@ACCESSID, 'AWAL', GETDATE()
				FROM tblSupplier 
				WHERE SupplierID=@SupplierID and SupplierName=@SupplierName and PMKSID=@PMKSID
			END
		END

		IF @action='EDIT'
		BEGIN
			Update tblSupplier
			SET
				Initial = @Initial
				, CashAdvance = @CashAdvance
				, Code = @Code
				, NPWP = @NPWP
				, GroupSupplier = @GroupSupplier
				, Category = @Category
				, VAT = @VAT
				, VATCondition = @VATCondition
				, PPH22 = @PPH22
				, PPH22Condition = @PPH22Condition
				, Remarks = @Remarks
				, Remarks1 = @Remarks1
				, Remarks2 = @Remarks2
				, PaymentTerm = @PaymentTerm
				, KomidelCalculate = @KomidelCalculateChar
				, BibitTopaz = @BibitTopaz
				, StatusActive=@StatusActive
				, FCUpdatedBy = @ACCESSID
				, FCUpdatedDT = getdate()
				, Approval=0
				, FCApproveBy=null
				, FCApproveDT=null
			WHERE SupplierID=@SupplierID and SupplierName=@SupplierName and PMKSID=@PMKSID
		END

		IF @action='APPROVE'
		BEGIN
			UPDATE tblSupplier 
				SET Approval=1, FCApproveBy = @ACCESSID, FCApproveDT = getdate() 
			WHERE SupplierID=@SupplierID and SupplierName=@SupplierName and PMKSID=@PMKSID
		END

		--untuk Log History Audit Table
		INSERT INTO tblSupplier_Audit
		SELECT *,@ACCESSID, @action, GETDATE()
		FROM tblSupplier 
		WHERE SupplierID=@SupplierID and SupplierName=@SupplierName and PMKSID=@PMKSID

		IF @action='UNAPPROVE'
		BEGIN
			UPDATE tblSupplier 
				SET Approval=0, FCUnApproveBy = @ACCESSID, FCUnApproveDT = getdate() 
			WHERE SupplierID=@SupplierID and SupplierName=@SupplierName and PMKSID=@PMKSID
		END

		IF @action='DELETE'
		BEGIN
			DELETE tblSupplier 
			WHERE SupplierID=@SupplierID and SupplierName=@SupplierName and PMKSID=@PMKSID
		END
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDTender]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_CRUDTender]
(
@action varchar(9),
@DateTender date,
@ProductID varchar(50),
@Region varchar(50),
@Remarks varchar(50),
@Price decimal(18,2),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	Declare @msg varchar(500)
	SET @msg='Sorry Transaction '+@action+' Failed.'
		IF @action='ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblTender 
						WHERE DateTender=@DateTender 
							AND ProductID=@ProductID 
							AND Region=@Region) 
			BEGIN
				INSERT INTO tblTender
				(DateTender,ProductID,Region,Remarks,Price,FCCreatedBy,FCCreatedDT)
				VALUES
				(@DateTender,@ProductID,@Region,@Remarks,@Price,@ACCESSID,GETDATE())
			END
			ELSE
			BEGIN
				SET @msg='ADD Data Failed Because Data Already Exist'
				RAISERROR (@msg,16,1)
			END
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblTender_Audit 
							WHERE DateTender=@DateTender 
								AND ProductID=@ProductID 
								AND Region=@Region
								AND Price=@Price)
			BEGIN
				INSERT INTO tblTender_Audit
				SELECT *,@ACCESSID,'AWAL',GETDATE()
				FROM tblTender 
				WHERE DateTender=@DateTender 
					AND ProductID=@ProductID 
					AND Region=@Region
					AND Price=@Price
			END
		END

		IF @action='EDIT'
		BEGIN
			UPDATE tblTender 
			SET Price=@Price, Remarks=@Remarks, FCUpdatedBy=@ACCESSID, FCUpdatedDT=GETDATE()
			WHERE
				DateTender=@DateTender 
				AND ProductID=@ProductID 
				AND Region=@Region
		END

		INSERT INTO tblTender_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblTender 
		WHERE DateTender=@DateTender 
			AND ProductID=@ProductID 
			AND Region=@Region
			AND Price=@Price

		IF @action='DELETE'
		BEGIN
			DELETE tblTender 
			WHERE
				DateTender=@DateTender 
				AND ProductID=@ProductID 
				AND Region=@Region
				AND Price=@Price
		END

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDTenderTarikData]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE  PROCEDURE [dbo].[sp_CRUDTenderTarikData]
(
@action varchar(9),
@Tanggal as date,
@Pilih bit,
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	IF @action='TARIKDATA'
	BEGIN
		INSERT tblTender_Audit
		SELECT *,@ACCESSID,'DELETE-TD',GETDATE()
		FROM tblTender WHERE DateTender=@Tanggal and FCUpdatedBy is null

		DELETE tblTender WHERE DateTender=@Tanggal and FCUpdatedBy is null

		INSERT INTO tblTender
			(ProductID,Region,DateTender,Remarks,Price,FCCreatedBy,FCCreatedDT)
		SELECT a.*
		FROM
			(
			SELECT 
				ProductID,Region,@Tanggal as DateTender,Remarks,IIF(@Pilih=1,0,Price) as Price,
				@AccessID as FCCreatedBy,GETDATE() as FCCreatedDT
			FROM tblTender 
			WHERE DateTender=DATEADD(day, -1,@Tanggal)
			) a
		LEFT JOIN
			(
			SELECT * FROM tblTender where DateTender=@Tanggal
			) b
		ON a.ProductID=b.ProductID and a.Region=b.Region and a.DateTender=b.DateTender
		WHERE b.ProductID IS NULL
	END

	INSERT tblTender_Audit
	SELECT *,@ACCESSID,@action,GETDATE()
	FROM tblTender WHERE DateTender=@Tanggal
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDTransport]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [dbo].[sp_CRUDTransport]
(
@action varchar(9),
@TransportDate date,
@ProductID varchar(50),
@PMKSID varchar(50),
@Destination varchar(50),
@Price decimal(18,2),
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		IF @action='ADD'
		BEGIN
			INSERT INTO tblTransport
			(TransportDate,ProductID,PMKSID,Destination,Price,FCCreatedBy,FCCreatedDT)
			VALUES
			(@TransportDate,@ProductID,@PMKSID,@Destination,@Price,@ACCESSID,GETDATE())
		END

		IF @action<>'ADD'
		BEGIN
			IF NOT EXISTS (SELECT * FROM tblTransport_Audit 
							WHERE TransportDate=@TransportDate 
									AND ProductID=@ProductID 
									AND PMKSID=@PMKSID
									AND Price=@Price)
			BEGIN
				INSERT INTO tblTransport_Audit
				SELECT *,@ACCESSID,@action,GETDATE()
				FROM tblTransport 
				WHERE TransportDate=@TransportDate 
						AND ProductID=@ProductID 
						AND PMKSID=@PMKSID
						AND Price=@Price
			END
		END

		IF @action='EDIT'
		BEGIN
			UPDATE tblTransport 
			SET Price=@Price, Destination=@Destination, FCUpdatedBy=@ACCESSID, FCUpdatedDT=GETDATE()
			WHERE
				TransportDate=@TransportDate 
				AND ProductID=@ProductID 
				AND PMKSID=@PMKSID
		END

		INSERT INTO tblTransport_Audit
		SELECT *,@ACCESSID,@action,GETDATE()
		FROM tblTransport 
		WHERE TransportDate=@TransportDate 
				AND ProductID=@ProductID 
				AND PMKSID=@PMKSID
				AND Price=@Price

		IF @action='DELETE'
		BEGIN
			DELETE tblTransport 
			WHERE
				TransportDate=@TransportDate 
				AND ProductID=@ProductID 
				AND PMKSID=@PMKSID
				AND Price=@Price
		END

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDTransportTarikData]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE  PROCEDURE [dbo].[sp_CRUDTransportTarikData]
(
@action varchar(9),
@Tanggal as date,
@Pilih bit,
@ACCESSID int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	IF @action='TARIKDATA'
	BEGIN
		INSERT tblTransport_Audit
		SELECT *,@ACCESSID,'DELETE-TD',GETDATE()
		FROM tblTransport WHERE TransportDate=@Tanggal and FCUpdatedBy is null

		DELETE tblTransport WHERE TransportDate=@Tanggal and FCUpdatedBy is null

		INSERT INTO tblTransport
			(ProductID,PMKSID,TransportDate,Price,Destination,FCCreatedBy,FCCreatedDT)
		SELECT a.*
		FROM
			(
			SELECT 
				ProductID,PMKSID,@Tanggal as Tanggal,IIF(@Pilih=1,0,Price) as Price,Destination,
				@AccessID as FCCreatedBy,GETDATE() as FCCreatedDT
			FROM tblTransport
			WHERE TransportDate=DATEADD(day, -1,@Tanggal)
			)a
		LEFT JOIN 
			(SELECT * FROM tblTransport WHERE TransportDate=@Tanggal)b
		ON a.ProductID=b.ProductID AND a.PMKSID=b.PMKSID AND a.Tanggal=b.TransportDate
		WHERE b.ProductID is null
	END

	INSERT tblTransport_Audit
	SELECT *,@ACCESSID,@action,GETDATE()
	FROM tblTransport WHERE TransportDate=@Tanggal
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Declare @msg varchar(500)
		SET @msg='Sorry Transaction '+@action+' Failed.'
	RAISERROR (@msg,16,1)
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_CRUDUser]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[sp_CRUDUser]
(
@action varchar(20),
@FCUSERID int output,
@FCUSERNAME varchar(15),
@FCUSERPASS varchar(100),
@FCNAME varchar(50),
@SetPMKSID varchar(max),
@FCROLEID int,
@ACCESSID int
)
AS
	Declare @age int
	SELECT @age=a.Age 
	FROM t_Role a
		INNER JOIN t_User b
		ON a.FCRoleID=b.FCRoleID
	WHERE b.FCUserID=@ACCESSID

	IF @action='ADD'
	BEGIN
		INSERT INTO t_User(FCUserName,FCUserPass,FCName,FCRoleID,FCPassExpDT,SetPMKSID,FCCreatedBy,FCCreatedDT,FCUpdatedBy,FCUpdatedDT,FCFirstLogin) 
		VALUES (@FCUSERNAME,pwdencrypt(@FCUSERPASS),@FCNAME,@FCROLEID,DATEADD(DAY,@age,GETDATE()),@SetPMKSID,@ACCESSID,GETDATE(),@ACCESSID,GETDATE(),1)
		SET @FCUSERID=SCOPE_IDENTITY()
	END

	IF @action='EDIT'
	BEGIN
		UPDATE t_User 
		SET FCUSERNAME=@FCUSERNAME,FCName=@FCNAME,FCRoleID=@FCROLEID,
			SetPMKSID=@SetPMKSID,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() 
		WHERE  FCUserID=@FCUSERID
	END
	IF @action='RESET'
	BEGIN
		UPDATE t_User 
		SET FCName=@FCNAME,FCUserPass=pwdencrypt(@FCUSERPASS),FCPassExpDT=DATEADD(DAY,@age,GETDATE())
			,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE(),FCFirstLogin=1 
		WHERE  FCUserID=@FCUSERID
	END
	IF @action='EDITPASS'
	BEGIN
		UPDATE t_User 
		SET FCName=@FCNAME,FCUserPass=pwdencrypt(@FCUSERPASS),FCPassExpDT=DATEADD(DAY,@age,GETDATE())
			,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE(),FCFirstLogin=0
		WHERE  FCUserID=@FCUSERID
	END
	IF @action='FIRSTLOGIN'
	BEGIN
		UPDATE t_User 
		SET FCName=@FCNAME,FCUserPass=pwdencrypt(@FCUSERPASS),FCPassExpDT=DATEADD(DAY,@age,GETDATE())
			,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE(),FCFirstLogin=0 
		WHERE  FCUserID=@FCUSERID
	END
	IF @action='UNBLOCK'
	BEGIN
		UPDATE t_User SET FCLoginAttempt=0,FCBlocked=0,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE  FCUserID=@FCUSERID
	END
	IF @action='BLOCK'
	BEGIN
		UPDATE t_User SET FCBlocked=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE  FCUserID=@FCUSERID
	END
	IF @action='DEFUNCT'
	BEGIN
		UPDATE t_User SET FCDefunctInd=1,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCUserID=@FCUSERID
	END
	IF @action='UNDEFUNCT'
	BEGIN
		UPDATE t_User SET FCDefunctInd=0,FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCUserID=@FCUSERID
	END

	IF @action='DELETE'
	BEGIN
		UPDATE t_User SET FCUpdatedBy=@ACCESSID,FCUpdatedDT=GETDATE() WHERE FCUserID=@FCUSERID
		DELETE FROM t_User WHERE FCUserID=@FCUSERID
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_Login]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_Login]
(
@FCUSERNAME varchar(15),
@FCUSERPASS varchar(100),
@FCUSERID int output,
@FCROLEID int output,
@FCROLEDESC varchar(50) output,
@FCFirstLogin bit output,
--@AreaOperational varchar(100) output,
--@Regional varchar(100) output,
@FCNAME varchar(50) output,
@MESSAGE varchar(100) output
)
AS
	BEGIN
		
		DECLARE @CNT int, @ATT int, @BLOCK bit, @DEFUNCT bit, @EXP varchar(10)
		SET @CNT = (SELECT COUNT(*) FROM t_User WHERE FCUserName=@FCUSERNAME COLLATE SQL_Latin1_General_CP1_CS_AS)

		IF(@CNT=1)
		BEGIN
			SELECT @BLOCK=FCBlocked, @DEFUNCT=FCDefunctInd FROM t_User WHERE FCUserName=@FCUSERNAME COLLATE SQL_Latin1_General_CP1_CS_AS
			IF(@BLOCK=1)
			BEGIN
				SET @MESSAGE='USER BLOCKED'
			END
			ELSE IF(@DEFUNCT=1)
			BEGIN
				SET @MESSAGE='USER DELETED'
			END
			ELSE
			BEGIN
				SELECT @FCUSERID=a.FCUserID, @FCROLEID=a.FCRoleID, @FCROLEDESC=b.FCRoleDesc, --@AreaOperational=a.AreaOperational, @Regional=a.Regional, 
						@FCNAME=a.FCNAME, @EXP=CONVERT(VARCHAR(10), a.FCPassExpDT, 111), @FCFirstLogin=ISNULL(a.FCFirstLogin,0)
				FROM t_User a INNER JOIN t_Role b ON a.FCRoleID=b.FCRoleID
				WHERE a.FCUserName=@FCUSERNAME COLLATE SQL_Latin1_General_CP1_CS_AS AND PWDCOMPARE(@FCUSERPASS,a.FCUserPass)=1 
				
				IF(@FCUSERID IS NULL)
				BEGIN
					SET @MESSAGE='WRONG PASSWORD'
					UPDATE t_User SET FCLoginAttempt=FCLoginAttempt+1 WHERE FCUserName=@FCUSERNAME
				
					SELECT @ATT=FCLoginAttempt FROM t_User WHERE FCUserName=@FCUSERNAME COLLATE SQL_Latin1_General_CP1_CS_AS
					IF(@ATT>=3)
					BEGIN
						UPDATE t_User SET FCBlocked=1 WHERE FCUserName=@FCUSERNAME
						SET @MESSAGE='USER BLOCKED'
					END
				END
				ELSE
				BEGIN
					UPDATE t_User SET FCLoginAttempt=0,LastTimeAccess=GETDATE() WHERE FCUserName=@FCUSERNAME	
					
					IF(@EXP<=(SELECT CONVERT(VARCHAR(10), getdate(), 111)))
					BEGIN
						SET @MESSAGE='PASSWORD EXPIRED'
					END
					ELSE IF(@FCFirstLogin=1)
					BEGIN
						SET @MESSAGE='FIRST LOGIN'
					END
					ELSE
					BEGIN	
						SET @MESSAGE='OK'
					END
										
				END
			END
			
		END
		ELSE
		BEGIN
			SET @MESSAGE='USER NOT FOUND'
		END
	END
	
	
GO
/****** Object:  StoredProcedure [dbo].[sp_RptCashAdvance]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [dbo].[sp_RptCashAdvance]
(
	@PMKSID varchar(50),
	@DateFrom date,
	@DateTo date,
	@AccessID int
)
AS
IF @PMKSID=''
BEGIN
	SELECT @PMKSID=SetPMKSID FROM t_User Where FCUserID=@AccessID
END

SELECT 
	PMKSID, Supplier, SupplierName, 
	SUM(ISNULL(Netto,0)) as Netto,
	SUM(ISNULL(TotalPembayaran,0)) as TotalPembayaran,
	SUM(ISNULL(RealisasiPanjarAmount,0)) as RealisasiPanjarAmount
	INTO #Temp1
FROM tblFFB 
WHERE (PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
	and cast(TanggalTimbang as date)>=@DateFrom
	and cast(TanggalTimbang as date)<=@DateTo
	and ISNULL(Number,'')<>''
GROUP BY PMKSID, Supplier, SupplierName

SELECT *,cast(TotalPembayaran as decimal(18,2))/cast(Netto as decimal(18,2)) as Harga FROM #Temp1
ORDER BY PMKSID, Supplier

DROP TABLE #Temp1

GO
/****** Object:  StoredProcedure [dbo].[sp_RptCashAdvanceRequest]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[sp_RptCashAdvanceRequest]
(
	@PMKSID varchar(50),
	@DateFrom varchar(10),
	@AccessID int
)
AS
Declare @Date date=@DateFrom+'-01'
SELECT 
	a.CashNo,a.PMKSID,a.SupplierName,a.[Description],a.Amount,
	ISNULL(b.Code,'') as Code,c.HouseBank,
	'Panjar '+a.PMKSID+' untuk Periode '+DATENAME(month, @Date)
	+' '+DATENAME(year, @Date) as Perihal,
	d.Name1,d.Name2,d.Name3,d.Name4,d.Name5
FROM tblCashAdvance a
	LEFT JOIN tblSupplier b
		ON a.PMKSID=b.PMKSID AND a.SupplierID=b.SupplierID and ISNULL(b.Approval,0)=1
	LEFT JOIN tblPMKS c
		ON a.PMKSID=c.PMKSID
	LEFT JOIN set_RptName d
		ON d.ReportID='FFB-T_Incentive'	AND d.FCUserID=@AccessID
WHERE a.PMKSID=@PMKSID
	AND a.[Period]=Replace(@DateFrom,'-','')

GO
/****** Object:  StoredProcedure [dbo].[sp_RptDetailListPayment]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[sp_RptDetailListPayment]
(
	@PMKSID varchar(50),
	@SupplierID varchar(50),
	@DateFrom date,
	@DateTo date,
	@AccessID int
)
AS

SELECT 
	PMKSID,Supplier,SupplierName,VAT,
	CAST(CAST(TanggalTimbang as date) as varchar(7))as Periode,
	CAST(CAST(TanggalTimbang as date) as varchar)as Timbang,Kendaraan,Ticket,
	Netto,ROUND(Harga-(Harga*PPH22/100),0) as HargaNetto,TotalPembayaran as TotalPembelian,
	ROUND(TotalPembayaran*PPH22/100,2) as PPh22Rp,
	ROUND(TotalPembayaran-TotalPembayaran*PPH22/100,2) as Pembayaran,
	Harga as HargaBeli,RealisasiPanjarAmount as Panjar_Amount,
	ROUND(TotalPembayaran-TotalPembayaran*PPH22/100,2)-RealisasiPanjarAmount as Selisih
INTO #Temp1
FROM tblFFB
WHERE
CAST(Post2Payment as date)>=@DateFrom
and CAST(Post2Payment as date)<=@DateTo
and PMKSID=@PMKSID and Supplier=@SupplierID
ORDER BY TanggalTimbang

SELECT 
	a.*,b.NettoToDate,b.ToDate,e.*,
	c.Remarks,c.Remarks1,c.Remarks2,
	d.FCName as Name1
FROM #Temp1 a
INNER JOIN
	(SELECT 
		t1.Ticket, SUM(t2.Netto) as NettoToDate,
		SUM(t2.TotalPembelian) as ToDate
	FROM #Temp1 t1
	INNER JOIN #Temp1 t2 on t1.Ticket >= t2.Ticket
	GROUP BY t1.Ticket) b
	ON a.Ticket=b.Ticket
LEFT JOIN tblSupplier c
	ON a.PMKSID=c.PMKSID and a.Supplier=c.SupplierID and ISNULL(c.Approval,0)=1
LEFT JOIN t_user d
	ON d.FCUserID=@AccessID and d.FCUserID=@AccessID
LEFT JOIN
	(SELECT 
	 SUM(TotalPembelian*vat/100)as VATRp,
	 SUM(TotalPembelian*vat/100)+SUM(TotalPembelian)as TotalALL
	 FROM #Temp1) e
	ON 1=1

DROP TABLE #Temp1

GO
/****** Object:  StoredProcedure [dbo].[sp_RptFFBOutStanding]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[sp_RptFFBOutStanding]
(
	@PMKSID varchar(50),
	@DateFrom date,
	@DateTo date,
	@AccessID int
)
AS
IF @PMKSID=''
BEGIN
	SELECT @PMKSID=SetPMKSID FROM t_User Where FCUserID=@AccessID
END

SELECT
	PMKSID,Supplier,SupplierName,Kendaraan,convert(varchar,TanggalTimbang,120) as TanggalTimbang,
	Ticket,BeratNetto,Potongan,Netto,Janjang,Komidel,HargaBeli,PPH22,HargaBeli*PPH22/100 as PPHrpkg,
	Harga,Netto*HargaBeli as Pembelian,VAT as ppn,ROUND(Netto*HargaBeli*VAT/100,0) as PPNRP,
	Netto*HargaBeli*PPH22/100 as PPHrp,Harga*Netto as TotalPembayaran
FROM tblFFB 
WHERE (PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
	AND CAST(TanggalTimbang as date) >= @DateFrom
	AND CAST(TanggalTimbang as date) <= @DateTo
	AND CAST(Post2Payment as date) >= @DateFrom
ORDER BY PMKSID,Supplier,TanggalTimbang,Kendaraan
GO
/****** Object:  StoredProcedure [dbo].[sp_RptFFBRecap]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_RptFFBRecap]
(
	@Type varchar(50),
	@PMKSID varchar(50),
	@DateFrom date,
	@DateTo date,
	@AccessID int
)
AS
IF @PMKSID=''
BEGIN
	SELECT @PMKSID=SetPMKSID FROM t_User Where FCUserID=@AccessID
END
--Type char1 I=Include, E=Exclue || char2 D=Detail, S=Summary
IF @type='D'
	BEGIN
		SELECT Cast(CAST(TanggalTimbang as date) as varchar) as TanggalTimbang,
			PMKSID, Supplier,SUM(Netto) as Netto,
			SUM(TotalPembayaran) as Pembelian,SUM(Round(TotalPembayaran*pph22/100,0)) as PPh22,
			SUM(TotalPembayaran-Round(TotalPembayaran*pph22/100,0)) as Pembayaran
			INTO #Temp1
		FROM tblFFB
		WHERE (PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
			AND CAST(TanggalTimbang as date)>=@DateFrom
			AND CAST(TanggalTimbang as date)<=@DateTo
		Group BY CAST(TanggalTimbang as date),PMKSID,Supplier

		SELECT a.*,b.SupplierName 
		FROM #Temp1 a
			INNER JOIN tblSupplier b
				ON a.PMKSID=b.PMKSID and a.Supplier=b.SupplierID
		ORDER BY a.PMKSID,a.Supplier
		DROP Table #Temp1

		--SELECT Cast(CAST(TanggalTimbang as date) as varchar) as TanggalTimbang,
		--	PMKSID, Supplier, sum(Netto) as Netto, 
		--	IIF(LEFT(@type,1)='I',SUM(TotalPembayaran),SUM(ROUND(TotalPembayaran/(100-pph22)*100,0))) as Pembelian
		--	INTO #Temp1
		--FROM tblFFB
		--WHERE (PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
		--	AND CAST(TanggalTimbang as date)>=@DateFrom
		--	AND CAST(TanggalTimbang as date)<=@DateTo
		--Group BY CAST(TanggalTimbang as date),PMKSID,Supplier

		--SELECT a.*,b.SupplierName 
		--FROM #Temp1 a
		--	INNER JOIN tblSupplier b
		--		ON a.PMKSID=b.PMKSID and a.Supplier=b.SupplierID
		--ORDER BY a.PMKSID,a.Supplier
		--DROP Table #Temp1
	END
ELSE
	BEGIN
		SELECT 
			PMKSID, Supplier, sum(Netto) as Netto,
			SUM(TotalPembayaran) as Pembelian,SUM(Round(TotalPembayaran*pph22/100,0)) as PPh22,
			SUM(TotalPembayaran-Round(TotalPembayaran*pph22/100,0)) as Pembayaran
			INTO #Temp2
		FROM tblFFB
		WHERE (PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
			AND CAST(TanggalTimbang as date)>=@DateFrom
			AND CAST(TanggalTimbang as date)<=@DateTo
		Group BY PMKSID,Supplier

		SELECT a.*,b.SupplierName 
		FROM #Temp2 a
			INNER JOIN tblSupplier b
				ON a.PMKSID=b.PMKSID and a.Supplier=b.SupplierID
		ORDER BY a.PMKSID,a.Supplier
		DROP Table #Temp2



		--SELECT 
		--	PMKSID, Supplier, sum(Netto) as Netto,
		--	IIF(LEFT(@type,1)='I',SUM(TotalPembayaran),SUM(ROUND(TotalPembayaran/(100-pph22)*100,0))) as Pembelian
		--	INTO #Temp2
		--FROM tblFFB
		--WHERE (PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
		--	AND CAST(TanggalTimbang as date)>=@DateFrom
		--	AND CAST(TanggalTimbang as date)<=@DateTo
		--Group BY PMKSID,Supplier

		--SELECT a.*,b.SupplierName 
		--FROM #Temp2 a
		--	INNER JOIN tblSupplier b
		--		ON a.PMKSID=b.PMKSID and a.Supplier=b.SupplierID
		--ORDER BY a.PMKSID,a.Supplier
		--DROP Table #Temp2
	END
GO
/****** Object:  StoredProcedure [dbo].[sp_RptFFBReceive]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[sp_RptFFBReceive]
(
	--@Type varchar(50),
	@PMKSID varchar(50),
	@DateFrom date,
	@DateTo date,
	@AccessID int
)
AS
IF @PMKSID=''
BEGIN
	SELECT @PMKSID=SetPMKSID FROM t_User Where FCUserID=@AccessID
END

SELECT 
	PMKSID, Supplier, SupplierName, Kendaraan, convert(varchar,TanggalTimbang,120) as TanggalTimbang,
	Ticket, BeratNetto, Potongan, Netto, Janjang, Komidel, Harga-(Harga*PPH22/100) as Harga, PPH22, Harga as HargaBeli,
	--Round(iif(@Type='I',TotalPembayaran,TotalPembayaran/(100-PPH22)*100),0) as Pembelian,
	TotalPembayaran as Pembelian,
	VAT as PPN Into #Temp1
FROM tblFFB
WHERE (PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
	AND CAST(TanggalTimbang as date)>=@DateFrom
	AND CAST(TanggalTimbang as date)<=@DateTo

SELECT *,
	ROUND(Pembelian*PPN/100,0) as PPNRP, ROUND(Pembelian*PPH22/100,0) as PPHRP,
	Pembelian-ROUND(Pembelian*PPH22/100,0) as Pembayaran
FROM #Temp1
ORDER BY PMKSID,Supplier,Ticket

Drop Table #Temp1

	
GO
/****** Object:  StoredProcedure [dbo].[sp_RptMenuRole]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_RptMenuRole]
(
	@Action varchar(50),
	@FCRoleID int
)
AS

IF @Action='MenuRole'
BEGIN
	SELECT 
		d.FCRoleDesc,c.FCMenuDesc,b.FCMenuDesc as TitleMenu,
		IIF(ISNULL(a.FCAdd,0)=1,'Y','N') as FCAdd,
		IIF(ISNULL(a.FCEdit,0)=1,'Y','N') as FCEdit,
		IIF(ISNULL(a.FCDelete,0)=1,'Y','N') as FCDelete,
		IIF(ISNULL(a.FCDefunctInd,0)=1,'Y','N') as FCDefunctInd,
		IIF(ISNULL(a.FCApprove,0)=1,'Y','N') as FCApprove
	FROM t_RoleDet a
		LEFT JOIN t_Menu b ON a.FCMenuID=b.FCMenuID
		LEFT JOIN t_Menu c on b.FCMenuPID=c.FCMenuID
		LEFT JOIN t_Role d on a.FCRoleID=d.FCRoleID
	WHERE (d.FCRoleID=@FCRoleID or @FCRoleID=0)
END

IF @Action='UserRole'
BEGIN
	SELECT 
		a.FCUserID,a.FCUserName,a.FCName,b.FCRoleDesc,FORMAT (a.FCPassExpDT, 'yyyy/MM/dd ') as FCPassExpDT,
		ISNULL(a.FCLoginAttempt,0) as FCLoginAttempt,IIF(ISNULL(a.FCBlocked,0)=1,'Y','N') as FCBlocked,
		ISNULL(u1.FCName,'') as CreatedBy,FORMAT (a.FCCreatedDT, 'yyyy/MM/dd ') as FCCreatedDT,
		ISNULL(u2.FCName,'') as UpdatedBy,FORMAT (a.FCUpdatedDT, 'yyyy/MM/dd ') as FCUpdatedDT,
		IIF(ISNULL(a.FCDefunctInd,0)=1,'Y','N') as FCDefunctInd,ISNULL(a.SetPMKSID,'') as SetPMKSID,
		FORMAT (a.LastTimeAccess, 'yyyy-MM-dd hh:mm:ss') as LastTimeAccess
	FROM t_User a
		LEFT JOIN t_Role b ON a.FCRoleID=b.FCRoleID
		LEFT JOIN t_User u1 ON a.FCCreatedBy=u1.FCUserID
		LEFT JOIN t_User u2 ON a.FCUpdatedBy=u2.FCUserID
	WHERE (a.FCRoleID=@FCRoleID or @FCRoleID=0)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_RptPriceAverage]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_RptPriceAverage]
(
	@Regional varchar(50),
	@DateFrom varchar(10),
	@AccessID int
)
AS

SELECT 
	day(a.TanggalTimbang) as NoUrut,
	a.PMKSID,b.Regional,
	SUM(cast(a.Netto as float)*a.Harga) as TotHarga,SUM(a.Netto) as Netto,
	round(AVG(cast(a.Netto as float)*a.Harga)/AVG(a.Netto),2) as Rata
	INTO #Temp1
FROM tblFFB a
	INNER JOIN tblPMKS b
		ON a.PMKSID=b.PMKSID
WHERE 
	(b.Regional=@Regional or @Regional='')
	AND LEFT(cast(a.TanggalTimbang as date),7)=@DateFrom and a.Harga <> 0
GROUP BY day(a.TanggalTimbang),a.PMKSID,b.Regional


SELECT a.*,b.GroupRata
FROM #Temp1 a
INNER JOIN
	(SELECT 
		Regional,NoUrut,
		Round((SUM(TotHarga)/COUNT(TotHarga))/
		(SUM(Netto)/COUNT(Netto)),2) as GroupRata
	from #Temp1
	Group BY Regional,NoUrut) b
ON a.Regional=b.Regional and a.NoUrut=b.NoUrut

DROP TABLE #Temp1

GO
/****** Object:  StoredProcedure [dbo].[sp_RptPriceIndicator]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[sp_RptPriceIndicator]
(
	@PMKSID varchar(50),
	@DateFrom date,
	@DateTo date,
	@AccessID int
)
AS
IF @PMKSID=''
BEGIN
	SELECT @PMKSID=SetPMKSID FROM t_User WHERE FCUserID=@AccessID
END

DECLARE @LOCAL_TABLEVARIABLE TABLE
(NoUrut int)

DECLARE @i int = 0
WHILE @i <= 31
BEGIN
	INSERT INTO @LOCAL_TABLEVARIABLE(NoUrut) VALUES (@i)
	SET @i = @i + 1
END

INSERT INTO @LOCAL_TABLEVARIABLE(NoUrut) VALUES (99)

SELECT 
	PMKSID,SupplierID,SupplierName,Price,DAY(datePrice) as NoUrut
	INTO #Temp1
FROM tblPrice 
WHERE 
	(PMKSID in (select * from dbo.splitstring(@PMKSID)) or ISNULL(@PMKSID,'')='')
	AND dateprice>=@DateFrom 
	AND dateprice<=@DateTo

SELECT DISTINCT
	b.NoUrut,a.PMKSID,a.SupplierID,a.SupplierName
	INTO #TempFinal
FROM #Temp1 a
	INNER JOIN @LOCAL_TABLEVARIABLE b
	ON b.NoUrut<>''

SELECT * INTO #Temp2
FROM 
(
    SELECT 
		PMKSID,SupplierID,SupplierName,Price,
		ROW_NUMBER() OVER (PARTITION BY PMKSID,SupplierID Order by PMKSID,SupplierID,NoUrut DESC) AS Sno# 
    FROM #Temp1
)RNK 
WHERE Sno# <=2

SELECT a.* INTO #Temp3
FROM (
		SELECT * FROM #Temp1
		UNION ALL
		SELECT 
			a.PMKSID,a.SupplierID,a.SupplierName,
			(a.Price-b.Price) as Price, 99 as NoUrut
			--ABS(a.Price-b.Price) as Price
		from (SELECT * FROM #Temp2 WHERE Sno#=1)a
			INNER JOIN (SELECT * FROM #Temp2 WHERE Sno#=2)b
			ON a.PMKSID=b.PMKSID and a.SupplierID=b.SupplierID
	)a

SELECT 
	IIF(cast(a.NoUrut as varchar(20))='99','LST',CAST(a.NoUrut as varchar(20))) as Nomor,
	a.NoUrut,a.PMKSID,LTRIM(RTRIM(d.Category)) as Category,a.SupplierID,a.SupplierName,b.Price
FROM #TempFinal a
	LEFT JOIN #Temp3 b
		ON a.PMKSID=b.PMKSID AND a.SupplierID=b.SupplierID AND a.NoUrut=b.NoUrut
	LEFT JOIN tblPMKS c
		ON a.PMKSID=c.PMKSID
	LEFT JOIN tblSupplier d
		ON a.PMKSID=d.PMKSID and a.SupplierID=d.SupplierID
WHERE d.Approval=1 and d.StatusActive='Y'
ORDER BY c.Regional,c.AreaOperational,c.urut,a.NoUrut,a.SupplierID

DROP TABLE #Temp1
DROP TABLE #Temp2
DROP TABLE #Temp3
DROP TABLE #TempFinal

GO
/****** Object:  StoredProcedure [dbo].[sp_RptTransfer]    Script Date: 6/27/2022 9:14:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[sp_RptTransfer]
(
	@PMKSID varchar(50),
	@DateFrom varchar(50),
	@DateTo varchar(50),
	@AccessID int
)
AS

SELECT
	a.PMKSID,b.Remarks,b.Remarks1,b.Remarks2,sum(a.TotalPembayaran) as TotalPembayaran,@DateFrom AS DateFrom
	INTO #Temp1
FROM tblFFB a
	LEFT JOIN tblSupplier b
		ON a.PMKSID=b.PMKSID and a.Supplier=b.SupplierID --and ISNULL(b.Approval,0)=1
WHERE
	a.PMKSID=@PMKSID
	And cast(Post2Payment as date) >= @DateFrom 
	And cast(Post2Payment as date) <= @DateTo
GROUP BY a.PMKSID,b.Remarks,b.Remarks1,b.Remarks2

SELECT
	b.Bank,b.Address1,b.Address2,b.Account,b.Remarks as Perusahaan,
	a.*
FROM #Temp1 a
	INNER JOIN tblbank b
		ON a.PMKSID=b.PMKSID
ORDER BY a.PMKSID,a.Remarks2

DROP TABLE #Temp1

	
GO
