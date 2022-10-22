USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblPriceDefault]    Script Date: 6/29/2022 3:13:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE TRIGGER [dbo].[TRIGGER_tblPriceDefault]
ON [dbo].[tblPriceDefault]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblPriceDefault_Audit
				([SupplierID],[SupplierName],[Efective_Date],[PMKSID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[SupplierID],[SupplierName],[Efective_Date],[PMKSID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblPriceDefault_Audit
				([SupplierID],[SupplierName],[Efective_Date],[PMKSID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[SupplierID],[SupplierName],[Efective_Date],[PMKSID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblPriceDefault_Audit
			([SupplierID],[SupplierName],[Efective_Date],[PMKSID],[LastAccess]
			,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
			,[ActionAudit],[TimeAudit])
		SELECT 
			[SupplierID],[SupplierName],[Efective_Date],[PMKSID],[LastAccess]
			,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
			,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblPriceDefault] ENABLE TRIGGER [TRIGGER_tblPriceDefault]
GO


