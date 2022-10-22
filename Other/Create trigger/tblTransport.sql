USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblTransport]    Script Date: 6/29/2022 3:28:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO











CREATE TRIGGER [dbo].[TRIGGER_tblTransport]
ON [dbo].[tblTransport]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblTransport_Audit
				([ProductID],[PMKSID],[TransportDate],[Price],[Destination]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[ActionAudit],[TimeAudit])
			SELECT 
				[ProductID],[PMKSID],[TransportDate],[Price],[Destination]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblTransport_Audit
				([ProductID],[PMKSID],[TransportDate],[Price],[Destination]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[ActionAudit],[TimeAudit])
			SELECT 
				[ProductID],[PMKSID],[TransportDate],[Price],[Destination]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblTransport_Audit
			([ProductID],[PMKSID],[TransportDate],[Price],[Destination]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[ActionAudit],[TimeAudit])
			SELECT 
				[ProductID],[PMKSID],[TransportDate],[Price],[Destination]
				,[UserID],[LastAccess],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblTransport] ENABLE TRIGGER [TRIGGER_tblTransport]
GO


