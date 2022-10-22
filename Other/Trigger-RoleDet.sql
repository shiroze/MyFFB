USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_t_RoleDet]    Script Date: 6/27/2022 8:26:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE TRIGGER [dbo].[TRIGGER_t_RoleDet]
ON [dbo].[t_RoleDet]
AFTER UPDATE, INSERT, DELETE
AS
SET NOCOUNT ON
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into t_RoleDet_Audit
				(FCRoleID,FCMenuID,FCAdd,FCEdit
				,FCDelete,FCUndelete,FCApprove
				,FCCreatedBy,FCCreatedDT
				,FCUpdatedBy,FCUpdatedDT,FCDefunctInd
				,ActionAudit,TimeAudit)
			SELECT 
				FCRoleID,FCMenuID,FCAdd,FCEdit
				,FCDelete,FCUndelete,FCApprove
				,FCCreatedBy,FCCreatedDT
				,FCUpdatedBy,FCUpdatedDT,FCDefunctInd
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into t_RoleDet_Audit
				(FCRoleID,FCMenuID,FCAdd,FCEdit
				,FCDelete,FCUndelete,FCApprove
				,FCCreatedBy,FCCreatedDT
				,FCUpdatedBy,FCUpdatedDT,FCDefunctInd
				,ActionAudit,TimeAudit)
			SELECT 
				FCRoleID,FCMenuID,FCAdd,FCEdit
				,FCDelete,FCUndelete,FCApprove
				,FCCreatedBy,FCCreatedDT
				,FCUpdatedBy,FCUpdatedDT,FCDefunctInd
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into t_RoleDet_Audit
				(FCRoleID,FCMenuID,FCAdd,FCEdit
				,FCDelete,FCUndelete,FCApprove
				,FCCreatedBy,FCCreatedDT
				,FCUpdatedBy,FCUpdatedDT,FCDefunctInd
				,ActionAudit,TimeAudit)
			SELECT 
				FCRoleID,FCMenuID,FCAdd,FCEdit
				,FCDelete,FCUndelete,FCApprove
				,FCCreatedBy,FCCreatedDT
				,FCUpdatedBy,FCUpdatedDT,FCDefunctInd
				,@ActionAudit,GETDATE()
		FROM deleted i
	END
GO

ALTER TABLE [dbo].[t_RoleDet] ENABLE TRIGGER [TRIGGER_t_RoleDet]
GO


