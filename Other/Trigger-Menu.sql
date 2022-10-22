USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_t_Menu]    Script Date: 6/27/2022 8:22:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE TRIGGER [dbo].[TRIGGER_t_Menu]
ON [dbo].[t_Menu]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into t_Menu_Audit
				(FCMenuID,FCMenuCode,FCMenuDesc,FCMenuPID
				,FCMenuLink,FCOrderNo,FCIcon,FCHidden
				,FCCreatedBy,FCCreatedDT,FCDefunctInd
				,ActionAudit,TimeAudit,FCUpdatedBy,FCUpdatedDT)
			SELECT 
				FCMenuID,FCMenuCode,FCMenuDesc,FCMenuPID
				,FCMenuLink,FCOrderNo,FCIcon,FCHidden
				,FCCreatedBy,FCCreatedDT,FCDefunctInd
				,@ActionAudit,GETDATE(),FCUPdatedBy,FCUpdatedDT
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into t_Menu_Audit
				(FCMenuID,FCMenuCode,FCMenuDesc,FCMenuPID
				,FCMenuLink,FCOrderNo,FCIcon,FCHidden
				,FCCreatedBy,FCCreatedDT,FCDefunctInd
				,ActionAudit,TimeAudit,FCUpdatedBy,FCUpdatedDT)
			SELECT 
				FCMenuID,FCMenuCode,FCMenuDesc,FCMenuPID
				,FCMenuLink,FCOrderNo,FCIcon,FCHidden
				,FCCreatedBy,FCCreatedDT,FCDefunctInd
				,@ActionAudit,GETDATE(),FCUPdatedBy,FCUpdatedDT
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into t_Menu_Audit
				(FCMenuID,FCMenuCode,FCMenuDesc,FCMenuPID
				,FCMenuLink,FCOrderNo,FCIcon,FCHidden
				,FCCreatedBy,FCCreatedDT,FCDefunctInd
				,ActionAudit,TimeAudit,FCUPdatedBy,FCUpdatedDT)
			SELECT 
				FCMenuID,FCMenuCode,FCMenuDesc,FCMenuPID
				,FCMenuLink,FCOrderNo,FCIcon,FCHidden
				,FCCreatedBy,FCCreatedDT,FCDefunctInd
				,@ActionAudit,GETDATE(),FCUPdatedBy,FCUpdatedDT
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[t_Menu] ENABLE TRIGGER [TRIGGER_t_Menu]
GO


