DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccBankDepositDetail];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccBankDepositDetail];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccBankDepositDetail];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccBankDepositHeader];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccBankDepositHeader];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccBankDepositHeader];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccJournalDetail];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccJournalDetail];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccJournalDetail];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccJournalHeader];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccJournalHeader];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccJournalHeader];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccountBook];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccountBook];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccountBook];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccountHead];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccountHead];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccountHead];
GO

IF EXISTS
      (SELECT TOP 1 1
         FROM sys.indexes
        WHERE     name = 'Idx_AccountBookType_UniqueName'
              AND object_id = OBJECT_ID ('AccountBookType'))
   DROP INDEX dbo.AccountBookType.Idx_AccountBookType_UniqueName
GO

CREATE NONCLUSTERED INDEX Idx_AccountBookType_UniqueName
   ON dbo.AccountBookType (UniqueName)

PRINT 'Index is created successfully [Table:AccountBookType; Name:Idx_AccountBookType_UniqueName]'
GO

IF EXISTS
      (SELECT TOP 1 1
         FROM sys.indexes
        WHERE     name = 'Idx_AccountType_UniqueName'
              AND object_id = OBJECT_ID ('AccountType'))
   DROP INDEX dbo.AccountType.Idx_AccountType_UniqueName
GO

CREATE NONCLUSTERED INDEX Idx_AccountType_UniqueName
   ON dbo.AccountType (UniqueName)

PRINT 'Index is created successfully [Table:AccountType; Name:Idx_AccountType_UniqueName]'
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccReceiptsDetail];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccReceiptsDetail];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccReceiptsDetail];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[AccReceiptsHeader];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[AccReceiptsHeader];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[AccReceiptsHeader];
GO


DROP INDEX [IX_CreatedBy] 
ON [dbo].[Address];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Address];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Address];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[Company];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Company];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Company];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[Contact];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Contact];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Contact];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[Country];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Country];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Country];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[DayBook];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[DayBook];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[DayBook];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[DepositInfo];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[DepositInfo];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[DepositInfo];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[District];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[District];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[District];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[FinancialPeriod];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[FinancialPeriod];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[FinancialPeriod];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[LoanInfo];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[LoanInfo];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[LoanInfo];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[LoanRepayment];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[LoanRepayment];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[LoanRepayment];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[OpeningBalance];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[OpeningBalance];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[OpeningBalance];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[Settings];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Settings];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Settings];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[State];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[State];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[State];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[Taluk];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Taluk];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Taluk];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[UserGroup];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[UserGroup];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[UserGroup];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[Users];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Users];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Users];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[Village];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[Village];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[Village];
GO

DROP INDEX [IX_CreatedBy] 
ON [dbo].[WithdrawalInfo];
GO

DROP INDEX [IX_DeletedBy] 
ON [dbo].[WithdrawalInfo];
GO

DROP INDEX [IX_ModifiedBy] 
ON [dbo].[WithdrawalInfo];
GO
