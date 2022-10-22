USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblPPN]    Script Date: 6/29/2022 2:57:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







CREATE TRIGGER [dbo].[TRIGGER_tblPPN]
ON [dbo].[tblPPN]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblPPN_Audit
				([periode],[no_faktur_pajak],[ppn_type],[tgl_faktur_pajak],[tgl_posting]
				,[sap_company_code],[sap_vendor_code],[periode_awal],[periode_akhir]
				,[disetorKe],[ppn_penyelesaian],[no_faktur_pajak_advance],[CashNo]
				,[amount_cash_advance],[total_amount],[incentive],[ppn],[userID]
				,[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[periode],[no_faktur_pajak],[ppn_type],[tgl_faktur_pajak],[tgl_posting]
				,[sap_company_code],[sap_vendor_code],[periode_awal],[periode_akhir]
				,[disetorKe],[ppn_penyelesaian],[no_faktur_pajak_advance],[CashNo]
				,[amount_cash_advance],[total_amount],[incentive],[ppn],[userID]
				,[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblPPN_Audit
				([periode],[no_faktur_pajak],[ppn_type],[tgl_faktur_pajak],[tgl_posting]
				,[sap_company_code],[sap_vendor_code],[periode_awal],[periode_akhir]
				,[disetorKe],[ppn_penyelesaian],[no_faktur_pajak_advance],[CashNo]
				,[amount_cash_advance],[total_amount],[incentive],[ppn],[userID]
				,[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[periode],[no_faktur_pajak],[ppn_type],[tgl_faktur_pajak],[tgl_posting]
				,[sap_company_code],[sap_vendor_code],[periode_awal],[periode_akhir]
				,[disetorKe],[ppn_penyelesaian],[no_faktur_pajak_advance],[CashNo]
				,[amount_cash_advance],[total_amount],[incentive],[ppn],[userID]
				,[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblPPN_Audit
			([periode],[no_faktur_pajak],[ppn_type],[tgl_faktur_pajak],[tgl_posting]
				,[sap_company_code],[sap_vendor_code],[periode_awal],[periode_akhir]
				,[disetorKe],[ppn_penyelesaian],[no_faktur_pajak_advance],[CashNo]
				,[amount_cash_advance],[total_amount],[incentive],[ppn],[userID]
				,[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[periode],[no_faktur_pajak],[ppn_type],[tgl_faktur_pajak],[tgl_posting]
				,[sap_company_code],[sap_vendor_code],[periode_awal],[periode_akhir]
				,[disetorKe],[ppn_penyelesaian],[no_faktur_pajak_advance],[CashNo]
				,[amount_cash_advance],[total_amount],[incentive],[ppn],[userID]
				,[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblPPN] ENABLE TRIGGER [TRIGGER_tblPPN]
GO


