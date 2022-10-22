USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_TblHolidayBank]    Script Date: 6/29/2022 2:48:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE TRIGGER [dbo].[TRIGGER_TblHolidayBank]
ON [dbo].[TblHolidayBank]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into TblHolidayBank_Audit
				([Date],[Remarks]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[Date],[Remarks]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into TblHolidayBank_Audit
				([Date],[Remarks]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[Date],[Remarks]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into TblHolidayBank_Audit
			([Date],[Remarks]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[Date],[Remarks]
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[TblHolidayBank] ENABLE TRIGGER [TRIGGER_TblHolidayBank]
GO

