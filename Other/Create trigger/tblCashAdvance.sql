USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblCashAdvance]    Script Date: 6/29/2022 2:44:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE TRIGGER [dbo].[TRIGGER_tblCashAdvance]
ON [dbo].[tblCashAdvance]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblCashAdvance_Audit
				([CashNo],[Period],[PMKSID],[SupplierID],[SupplierName],[Code]
				,[Description],[Amount],[DeductAmount],[StatusApproval],[Week]
				,[UserID],[LastAccess],[TransferDate],[UserApproval],[LastAccessApproval]
				,[Tanggal],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[CashNo],[Period],[PMKSID],[SupplierID],[SupplierName],[Code]
				,[Description],[Amount],[DeductAmount],[StatusApproval],[Week]
				,[UserID],[LastAccess],[TransferDate],[UserApproval],[LastAccessApproval]
				,[Tanggal],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblCashAdvance_Audit
				([CashNo],[Period],[PMKSID],[SupplierID],[SupplierName],[Code]
				,[Description],[Amount],[DeductAmount],[StatusApproval],[Week]
				,[UserID],[LastAccess],[TransferDate],[UserApproval],[LastAccessApproval]
				,[Tanggal],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[CashNo],[Period],[PMKSID],[SupplierID],[SupplierName],[Code]
				,[Description],[Amount],[DeductAmount],[StatusApproval],[Week]
				,[UserID],[LastAccess],[TransferDate],[UserApproval],[LastAccessApproval]
				,[Tanggal],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblCashAdvance_Audit
			([CashNo],[Period],[PMKSID],[SupplierID],[SupplierName],[Code]
				,[Description],[Amount],[DeductAmount],[StatusApproval],[Week]
				,[UserID],[LastAccess],[TransferDate],[UserApproval],[LastAccessApproval]
				,[Tanggal],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[CashNo],[Period],[PMKSID],[SupplierID],[SupplierName],[Code]
				,[Description],[Amount],[DeductAmount],[StatusApproval],[Week]
				,[UserID],[LastAccess],[TransferDate],[UserApproval],[LastAccessApproval]
				,[Tanggal],[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT]
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblCashAdvance] ENABLE TRIGGER [TRIGGER_tblCashAdvance]
GO


