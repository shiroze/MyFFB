USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblTender]    Script Date: 6/29/2022 3:21:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









CREATE TRIGGER [dbo].[TRIGGER_tblTender]
ON [dbo].[tblTender]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblTender_Audit
				([ProductID],[Region],[DateTender],[Remarks],[Price]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[ActionAudit],[TimeAudit])
			SELECT 
				[ProductID],[Region],[DateTender],[Remarks],[Price]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblTender_Audit
				([ProductID],[Region],[DateTender],[Remarks],[Price]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[ActionAudit],[TimeAudit])
			SELECT 
				[ProductID],[Region],[DateTender],[Remarks],[Price]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblTender_Audit
			([ProductID],[Region],[DateTender],[Remarks],[Price]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[ActionAudit],[TimeAudit])
			SELECT 
				[ProductID],[Region],[DateTender],[Remarks],[Price]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblTender] ENABLE TRIGGER [TRIGGER_tblTender]
GO


