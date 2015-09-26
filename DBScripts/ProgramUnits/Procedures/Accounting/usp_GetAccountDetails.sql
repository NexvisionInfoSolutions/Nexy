
----------------------------------------------------------
-- Author: Digeesh
-- Date: 11/Nov/14
-- Purpose: procedure usp_RptEmployeeShifts to get all the employee shifts
-- Returns the list of all users
----------------------------------------------------------
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE type = 'P'
			AND NAME = 'usp_GetAccountDetails'
		)
	DROP PROCEDURE usp_GetAccountDetails
GO

CREATE PROCEDURE [dbo].[usp_GetAccountDetails] @vAccountBookId bigint
  ,@vTranType varchar(20)
  ,@vSessionKey varchar(100)
AS
  
  /*		public int FromModule { get; set; } //0 for "From Accounts", 1 for "From Posting"
        public TransactionType Transaction { get; set; } //0 for deposit 1 for withdrawal, 2 for "Loan Entry", 3 for "Loan repayment"
        public long TransId { get; set; }// Transaction id 
        public int Cancelled { get; set; }//0 or 1
		TransactionType { CashReceipt = 0, CashPayment = 1, LoanEntry = 2, LoanRepayment = 3, DepositEntry = 4, DepositWithdrawal = 5 }		*/
    
Select distinct ab.AccountBookId
,ab.BookName
,ab.BookCode
,ab.ReceiptVoucherPrefix
,ab.ReceiptVoucherSuffix
,ab.PaymentVoucherPrefix
,ab.PaymentVoucherSuffix
,isnull(ob.ClosingBalance,0) as ClosingBalance
,isnull(case when @vTranType = 'BANK' then max(bdhead.Id) over(partition by 1) when @vTranType = 'CASH' then max(rchead.Id) over(partition by 1) when @vTranType = 'JOURNAL' then max(jnhead.Id) over(partition by 1) else 0 end,0) + 1 as VoucherNo
FROM AccountBook ab
left join OpeningBalance ob on ob.AccountHeadId = ab.AccountHeadId
left join dbo.AccReceiptsHeader rchead ON rchead.BookId = ab.AccountBookId and @vTranType = 'CASH'
left join dbo.AccJournalHeader jnhead ON jnhead.BookId = ab.AccountBookId and @vTranType = 'JOURNAL'
left join dbo.AccBankDepositHeader bdhead ON bdhead.BookId = ab.AccountBookId and @vTranType = 'BANK'
where ab.AccountBookId = @vAccountBookId
GO

PRINT 'usp_GetAccountDetails is created successfully..'
GO
