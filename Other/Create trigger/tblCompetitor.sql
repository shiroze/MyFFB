USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblCompetitor]    Script Date: 6/29/2022 2:34:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE TRIGGER [dbo].[TRIGGER_tblCompetitor]
ON [dbo].[tblCompetitor]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblCompetitor_Audit
				([CompetitorID],[CompetitorName],[CompetitorLocation]
				,[Capacity],[CompetitorGroup],[PMKSID],[UserID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[CompetitorID],[CompetitorName],[CompetitorLocation]
				,[Capacity],[CompetitorGroup],[PMKSID],[UserID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblCompetitor_Audit
				([CompetitorID],[CompetitorName],[CompetitorLocation]
				,[Capacity],[CompetitorGroup],[PMKSID],[UserID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[CompetitorID],[CompetitorName],[CompetitorLocation]
				,[Capacity],[CompetitorGroup],[PMKSID],[UserID],[LastAccess]
				,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblCompetitor_Audit
			([CompetitorID],[CompetitorName],[CompetitorLocation]
			,[Capacity],[CompetitorGroup],[PMKSID],[UserID],[LastAccess]
			,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
			,[ActionAudit],[TimeAudit])
		SELECT 
			[CompetitorID],[CompetitorName],[CompetitorLocation]
			,[Capacity],[CompetitorGroup],[PMKSID],[UserID],[LastAccess]
			,[FCCreatedBy],[FCCreatedDT],[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd]
			,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblCompetitor] ENABLE TRIGGER [TRIGGER_tblCompetitor]
GO

