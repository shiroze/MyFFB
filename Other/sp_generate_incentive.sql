USE [FFB_Dbase]
GO
/****** Object:  StoredProcedure [dbo].[sp_generate_incentive]    Script Date: 4/22/2022 11:34:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_generate_incentive] 
	@yesterday date = null
AS
BEGIN
	SET NOCOUNT ON;

	IF @yesterday is null
	BEGIN
		SET @yesterday = dateadd(day,-1, cast(getdate() as date))
	END
	

	INSERT INTO [dbo].[tblSAPIncentive]
			([yyyymm]
			,PMKSID
			,Supplier
			,doc_date
			,doc_timestamp
			,[doc_sap_timestamp]
			,[BUKRS]
			,[BLDAT]
			,[BUDAT]
			,[WAERS]
			,[KURSF]
			,[XBLNR]
			,[BKTXT]
			,[STBLG]
			,[BTRNS]
			,[ZTERM]
			,[ZFBDT]
			,[MWSKZ]
			,[TXBFW]
			,[WMWST]
			,[VKONT]
			,[QSSKZ]
			,[QSSHB]
			,[QBSHB]
			,[WKONT]
			,[MENGE]
			,[MEINS]
			,[HBKID]
			,[HKONT]
			,[GSBER]
			,[NEWBS]
			,[WRBTR]
			,[ZUONR]
			,[XREF1]
			,[XREF2]
			,[XREF3]
			,[KOSTL]
			,[PRCTR]
			,[SGTXT]
			,[LIFNR]
			,[GSBER2]
			,[NEWBS2]
			,[UMSKZ]
			,[WRBTR2]
			,[ZUONR2]
			,[XREF12]
			,[XREF22]
			,[XREF32]
			,[SGTXT2])
		SELECT yyyymm
			,PMKSID
			,sap_vendorcode
			,[doc_date]
			,GETDATE()
			,null
			,[BUKRS]
			,[BLDAT]
			,[BUDAT]
			,[WAERS]
			,[KURSF]
			,fruit_type
			,[BKTXT]
			,[STBLG]
			,[BTRNS]
			,[ZTERM]
			,[ZFBDT]
			,[MWSKZ]
			,[TXBFW]
			,[WMWST]
			,[VKONT]
			,[QSSKZ]
			,[QSSHB]
			,[QBSHB]
			,[WKONT]
			,[MENGE]
			,[MEINS]
			,[HBKID]
			,[HKONT]
			,[GSBER]
			,[NEWBS]
			,[WRBTR]
			,[ZUONR]
			,[XREF1]
			,[XREF2]
			,[XREF3]
			,[KOSTL]
			,[PRCTR]
			,[SGTXT]
			,[LIFNR]
			,[GSBER2]
			,[NEWBS2]
			,[UMSKZ]
			,[WRBTR2]
			,[ZUONR2]
			,[XREF12]
			,[XREF22]
			,[XREF32]
			,[SGTXT2]
		FROM [dbo].[v_SAP_Incentive] 
		WHERE [doc_date] = @yesterday 
		ORDER BY  PMKSID, sap_vendorcode
	
	IF @@ROWCOUNT > 0
	BEGIN
		DECLARE @ref_key VARCHAR(16) = '';
		DECLARE @pmks VARCHAR(5) = '';
		DECLARE @SupplierID VARCHAR(50) = '';
		
		DECLARE @yyyymm VARCHAR(6) = '';
		
		UPDATE i 
			SET UploadToSAP = 1
		FROM t_Incentive i
		INNER JOIN
			(SELECT PMKSID,Supplier,lifnr FROM tblSAPIncentive 
				WHERE LEFT(XBLNR,4) = 'OUTS' AND doc_date = @yesterday) si
			ON i.PMKSID=si.PMKSID and i.SupplierID=si.Supplier
		WHERE i.periode = convert(varchar(6), @yesterday, 112)

		--untuk update ke tblincentive
		UPDATE tblIncentive 
			SET upload_to_sap = 1
			WHERE sap_vendor_code in (SELECT lifnr FROM tblSAPIncentive WHERE doc_date = @yesterday AND XBLNR = 'OUTS')
			  AND period = convert(varchar(6), @yesterday, 112)


		DECLARE @count INT = 0, @i INT = 0
		SELECT @count = count(*) FROM [tblSAPIncentive] 		  
			WHERE XBLNR = 'OUTS'
			  --AND YEAR(doc_date) = YEAR(doc_date)
			  --AND MONTH(doc_date) = MONTH(doc_date)

		WHILE @i < @count
		BEGIN
			SET @i = @i + 1;
		

			SELECT TOP(1) 
				--@id = idx, 
				@yyyymm = yyyymm, 
				@pmks = pmksID, 
				@SupplierID = supplier FROM [tblSAPIncentive] WHERE XBLNR = 'OUTS'

			SET @ref_key = dbo.[fn_number_get_next_incentive](@yesterday, 'OUTS');

			UPDATE [tblSAPIncentive]
			   SET XBLNR = @ref_key
			 WHERE yyyymm = @yyyymm
			   AND PMKSID = @pmks
			   AND Supplier = @SupplierID

			--SELECT @pmks = pmksID, @SupplierID = supplier FROM [tblSAPIncentive] WHERE xblnr = @ref_key;

			--INSERT INTO [dbo].[tblSAPIncentive_i]
			--   ([XBLNR]
			--   ,[ticket_no])
			--SELECT @ref_key, Ticket FROM tblFFB 
			--	WHERE CONVERT(VARCHAR(8), TanggalTimbang,112) = CONVERT(VARCHAR(8), @yesterday, 112)
			--	  AND PMKSID = @pmks 
			--	  AND Supplier = @SupplierID;
		
		END
	END
END
