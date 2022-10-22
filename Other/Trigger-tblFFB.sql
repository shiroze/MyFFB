USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_tblFFB]    Script Date: 6/27/2022 8:30:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE TRIGGER [dbo].[TRIGGER_tblFFB]
ON [dbo].[tblFFB]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	SET NOCOUNT ON;
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into tblFFB_Audit
				(Post2Payment,Number,PMKSID,Supplier,SupplierName,GroupPayment,Kendaraan
				,TanggalTimbang,Ticket,BeratNetto,Potongan,Netto,NettoTransfeer,Janjang
				,Komidel,HargaBeli,Harga,PPH22,VAT,TotalPembayaran,RealisasiPanjarAmount
				,PMKSTransfeer,Remarks,CalculateBy,CalculateDate,UserID,LastAccess,sap_process
				,ActionAudit,TimeAudit)
			SELECT 
				Post2Payment,Number,PMKSID,Supplier,SupplierName,GroupPayment,Kendaraan
				,TanggalTimbang,Ticket,BeratNetto,Potongan,Netto,NettoTransfeer,Janjang
				,Komidel,HargaBeli,Harga,PPH22,VAT,TotalPembayaran,RealisasiPanjarAmount
				,PMKSTransfeer,Remarks,CalculateBy,CalculateDate,UserID,LastAccess,sap_process
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into tblFFB_Audit
				(Post2Payment,Number,PMKSID,Supplier,SupplierName,GroupPayment,Kendaraan
				,TanggalTimbang,Ticket,BeratNetto,Potongan,Netto,NettoTransfeer,Janjang
				,Komidel,HargaBeli,Harga,PPH22,VAT,TotalPembayaran,RealisasiPanjarAmount
				,PMKSTransfeer,Remarks,CalculateBy,CalculateDate,UserID,LastAccess,sap_process
				,ActionAudit,TimeAudit)
			SELECT 
				Post2Payment,Number,PMKSID,Supplier,SupplierName,GroupPayment,Kendaraan
				,TanggalTimbang,Ticket,BeratNetto,Potongan,Netto,NettoTransfeer,Janjang
				,Komidel,HargaBeli,Harga,PPH22,VAT,TotalPembayaran,RealisasiPanjarAmount
				,PMKSTransfeer,Remarks,CalculateBy,CalculateDate,UserID,LastAccess,sap_process
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
		INSERT into tblFFB_Audit
				(Post2Payment,Number,PMKSID,Supplier,SupplierName,GroupPayment,Kendaraan
				,TanggalTimbang,Ticket,BeratNetto,Potongan,Netto,NettoTransfeer,Janjang
				,Komidel,HargaBeli,Harga,PPH22,VAT,TotalPembayaran,RealisasiPanjarAmount
				,PMKSTransfeer,Remarks,CalculateBy,CalculateDate,UserID,LastAccess,sap_process
				,ActionAudit,TimeAudit)
			SELECT 
				Post2Payment,Number,PMKSID,Supplier,SupplierName,GroupPayment,Kendaraan
				,TanggalTimbang,Ticket,BeratNetto,Potongan,Netto,NettoTransfeer,Janjang
				,Komidel,HargaBeli,Harga,PPH22,VAT,TotalPembayaran,RealisasiPanjarAmount
				,PMKSTransfeer,Remarks,CalculateBy,CalculateDate,UserID,LastAccess,sap_process
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[tblFFB] ENABLE TRIGGER [TRIGGER_tblFFB]
GO


