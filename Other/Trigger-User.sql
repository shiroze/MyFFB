USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_t_user]    Script Date: 6/27/2022 8:27:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE TRIGGER [dbo].[TRIGGER_t_user]
ON [dbo].[t_User]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into t_User_Audit
				(FCUserID,FCUserName, FCUserPass,FCName,FCRoleID,FCPassExpDT
				,FCLoginAttempt,FCBlocked,FCCreatedBy,FCCreatedDT,FCUpdatedBy
				,FCUpdatedDT,FCDefunctInd,FCFirstLogin,ActionAudit,TimeAudit,LastTimeAccess)
			SELECT 
				FCUserID,FCUserName, FCUserPass,FCName,FCRoleID,FCPassExpDT
				,FCLoginAttempt,FCBlocked,FCCreatedBy,FCCreatedDT,FCUpdatedBy
				,FCUpdatedDT,FCDefunctInd,FCFirstLogin,@ActionAudit,GETDATE(),LastTimeAccess
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into t_User_Audit
				(FCUserID,FCUserName, FCUserPass,FCName,FCRoleID,FCPassExpDT
				,FCLoginAttempt,FCBlocked,FCCreatedBy,FCCreatedDT,FCUpdatedBy
				,FCUpdatedDT,FCDefunctInd,FCFirstLogin,ActionAudit,TimeAudit,LastTimeAccess)
			SELECT 
				FCUserID,FCUserName, FCUserPass,FCName,FCRoleID,FCPassExpDT
				,FCLoginAttempt,FCBlocked,FCCreatedBy,FCCreatedDT,FCUpdatedBy
				,FCUpdatedDT,FCDefunctInd,FCFirstLogin,@ActionAudit,GETDATE(),LastTimeAccess
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into t_User_Audit
			(FCUserID,FCUserName, FCUserPass,FCName,FCRoleID,FCPassExpDT
			,FCLoginAttempt,FCBlocked,FCCreatedBy,FCCreatedDT,FCUpdatedBy
			,FCUpdatedDT,FCDefunctInd,FCFirstLogin,ActionAudit,TimeAudit,LastTimeAccess)
		SELECT
			FCUserID,FCUserName, FCUserPass,FCName,FCRoleID,FCPassExpDT
			,FCLoginAttempt,FCBlocked,FCCreatedBy,FCCreatedDT,FCUpdatedBy
			,FCUpdatedDT,FCDefunctInd,FCFirstLogin,@ActionAudit,GETDATE(),LastTimeAccess
		FROM deleted i
	END
GO

ALTER TABLE [dbo].[t_User] ENABLE TRIGGER [TRIGGER_t_user]
GO


