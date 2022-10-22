USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblBudget]    Script Date: 6/29/2022 2:28:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE TRIGGER [dbo].[TRIGGER_tblBudget]
ON [dbo].[tblBudget]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblBudget_Audit
				([PMKSID],[Periode],[VolumeFFB_KG],[VolumeCPO_KG],[VolumePK_KG],[OER],[KER]
				,[NetMargin_USD_MT_CPO],[HK],[ExchangeRate],[Capacity],[ProduksiCangkang_KG]
				,[BakarCangkang_KG],[EBITDA_Cangkang],[ProduksiBA_KG],[Price_BunchAsh]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[PMKSID],[Periode],[VolumeFFB_KG],[VolumeCPO_KG],[VolumePK_KG],[OER],[KER]
				,[NetMargin_USD_MT_CPO],[HK],[ExchangeRate],[Capacity],[ProduksiCangkang_KG]
				,[BakarCangkang_KG],[EBITDA_Cangkang],[ProduksiBA_KG],[Price_BunchAsh]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblBudget_Audit
				([PMKSID],[Periode],[VolumeFFB_KG],[VolumeCPO_KG],[VolumePK_KG],[OER],[KER]
				,[NetMargin_USD_MT_CPO],[HK],[ExchangeRate],[Capacity],[ProduksiCangkang_KG]
				,[BakarCangkang_KG],[EBITDA_Cangkang],[ProduksiBA_KG],[Price_BunchAsh]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[PMKSID],[Periode],[VolumeFFB_KG],[VolumeCPO_KG],[VolumePK_KG],[OER],[KER]
				,[NetMargin_USD_MT_CPO],[HK],[ExchangeRate],[Capacity],[ProduksiCangkang_KG]
				,[BakarCangkang_KG],[EBITDA_Cangkang],[ProduksiBA_KG],[Price_BunchAsh]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblBudget_Audit
			([PMKSID],[Periode],[VolumeFFB_KG],[VolumeCPO_KG],[VolumePK_KG],[OER],[KER]
				,[NetMargin_USD_MT_CPO],[HK],[ExchangeRate],[Capacity],[ProduksiCangkang_KG]
				,[BakarCangkang_KG],[EBITDA_Cangkang],[ProduksiBA_KG],[Price_BunchAsh]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[PMKSID],[Periode],[VolumeFFB_KG],[VolumeCPO_KG],[VolumePK_KG],[OER],[KER]
				,[NetMargin_USD_MT_CPO],[HK],[ExchangeRate],[Capacity],[ProduksiCangkang_KG]
				,[BakarCangkang_KG],[EBITDA_Cangkang],[ProduksiBA_KG],[Price_BunchAsh]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblBudget] ENABLE TRIGGER [TRIGGER_tblBudget]
GO


