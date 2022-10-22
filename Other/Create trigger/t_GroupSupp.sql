USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_t_GroupSupp]    Script Date: 6/29/2022 1:47:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE TRIGGER [dbo].[TRIGGER_t_GroupSupp]
ON [dbo].[t_GroupSupp]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into t_GroupSupp_Audit
				([GroupSuppID],[GroupSuppName],[Regional],[FCCreatedBy]
				,[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[GroupSuppID],[GroupSuppName],[Regional],[FCCreatedBy]
				,[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into t_GroupSupp_Audit
				([GroupSuppID],[GroupSuppName],[Regional],[FCCreatedBy]
				,[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[GroupSuppID],[GroupSuppName],[Regional],[FCCreatedBy]
				,[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
			INSERT into t_GroupSupp_Audit
				([GroupSuppID],[GroupSuppName],[Regional],[FCCreatedBy]
				,[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[GroupSuppID],[GroupSuppName],[Regional],[FCCreatedBy]
				,[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[t_GroupSupp] ENABLE TRIGGER [TRIGGER_t_GroupSupp]
GO


