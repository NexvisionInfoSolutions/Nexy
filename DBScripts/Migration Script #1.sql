declare @LoanNo varchar(50), @InterestRate decimal(18,2), @PenalInterestRate decimal(18,2), @IssuedDate datetime, @AmountIssued decimal(18,2),
 @FirstinstDate datetime, @LastinstDate datetime , 
@Mno int, @NoofInstallment int, @period int, @CD datetime, @Voucher varchar(50), @Mode char(1),  @SAC char(1)

declare @1Name varchar(200)
declare @2UserId int, @2LoanId int, @CashAccId int,@CashBookLinkAcc int, @ReceiptMaxId int, @ReceiptHeaderIden int, @UserAccountId int

declare @RepayDate datetime, @Principal decimal(18,2)
declare @OpeningBalCount int, @AccScheduleId int, @DayBookCount int

SELECT    TOP (1) @CashAccId= CashBookId
FROM         LoanManagement.dbo.Settings

SELECT   @CashBookLinkAcc=  AccountHeadId
FROM         LoanManagement.dbo.AccountBook
WHERE     (AccountBookId = @CashAccId)

DECLARE @MyCursor CURSOR
SET @MyCursor = CURSOR FAST_FORWARD
FOR
				SELECT     LoanNo, InterestRate, PenalInterestRate, IssuedDate, AmountIssued, FirstinstDate, 
				LastinstDate, Mno, NoofInstallment, period, CD, Voucher, Mode,  SAC
				FROM         STOrdinaryLoan --WHERE     (LoanNo IN ('STOL1'))

OPEN @MyCursor
FETCH NEXT FROM @MyCursor
INTO @LoanNo, @InterestRate, @PenalInterestRate, @IssuedDate, @AmountIssued, @FirstinstDate, 
				@LastinstDate, @Mno, @NoofInstallment, @period, @CD, @Voucher, @Mode,  @SAC
WHILE @@FETCH_STATUS = 0
BEGIN
set @2UserId=0
SELECT    @1Name=name
FROM         personala
WHERE     (mno = @Mno)

SELECT      @2UserId= UserID 
FROM         LoanManagement.dbo.Users
WHERE     (Code = @1Name)
if(@2UserId >0)
begin
	select @ReceiptMaxId=(id+1)  from LoanManagement.dbo.AccReceiptsHeader order by id  desc
	
	SELECT  @UserAccountId=   AccountHeadId
FROM         LoanManagement.dbo.Users
WHERE     (UserID = @2UserId)

	INSERT INTO LoanManagement.dbo.LoanInfo
                      (UserId,transactiondate, RepaymentStartDate, RePaymentInterval, RequestedAmount, ProposedAmount, LoanAmount, TotalInstallments, 
						Status, InteresRate, SanctionedDate, 
                       Notes, InstallmentAmount, IsDeleted,SanctionedBy)
	VALUES     (@2UserId,@IssuedDate,@FirstinstDate,1,@AmountIssued,@AmountIssued,@AmountIssued,@NoofInstallment,
						1,@InterestRate,@FirstinstDate,'',@AmountIssued/@NoofInstallment,0,0)
	select @2LoanId=@@IDENTITY
	
	INSERT     
	INTO      LoanManagement.dbo.AccReceiptsHeader
	(BookId, TransDate, VoucherNo, VoucherTotal, TransType, FinYear, FromModule, [Transaction], TransId, Cancelled, IsDeleted)
	VALUES     (@CashAccId,@IssuedDate,'CASHPAY-'+ convert(varchar(10),@ReceiptMaxId) ,@AmountIssued, 1, 1, 1, 2,@2LoanId, 0, 0)
	select @ReceiptHeaderIden=@@IDENTITY
 
	INSERT     
	INTO      LoanManagement.dbo.AccReceiptsDetail(ReceiptsId, AccountId, Narration, Amount, Display, IsDeleted)
	VALUES     (@ReceiptHeaderIden,@CashBookLinkAcc,'Loan Issue',(-1)*@AmountIssued,0,0)

	INSERT     
	INTO      LoanManagement.dbo.AccReceiptsDetail(ReceiptsId, AccountId, Narration, Amount, Display, IsDeleted)
	VALUES     (@ReceiptHeaderIden,@UserAccountId,'Loan Issue',@AmountIssued,1,0)
	
	--Day book start
	--===============================================================================================
	SELECT   @DayBookCount=  COUNT(*)  
	FROM         LoanManagement.dbo.DayBook
	WHERE     (AccountHeadId = @CashBookLinkAcc) AND (TransDate = CONVERT(DATETIME, @IssuedDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
		if(@DayBookCount>0)
			begin
				UPDATE    LoanManagement.dbo.DayBook
				SET              Receipt =Receipt, Payment =Payment+@AmountIssued  
				WHERE     (AccountHeadId = @CashBookLinkAcc) AND (TransDate = CONVERT(DATETIME, @IssuedDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
			end
		else
			begin
				INSERT INTO LoanManagement.dbo.DayBook
                      (		AccountHeadId,		TransDate,						Receipt,	Payment,		ClosingBalance, IsDeleted,FinancialYearId)
				VALUES     (@CashBookLinkAcc, CONVERT(DATETIME, @IssuedDate, 102),0,		@AmountIssued,	@AmountIssued,	0,			1)
			end
	
	SELECT   @DayBookCount=  COUNT(*)  
	FROM         LoanManagement.dbo.DayBook
	WHERE     (AccountHeadId = @UserAccountId) AND (TransDate = CONVERT(DATETIME, @IssuedDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
		if(@DayBookCount>0)
			begin
				UPDATE    LoanManagement.dbo.DayBook
				SET              Receipt =Receipt+@AmountIssued  , Payment =Payment
				WHERE     (AccountHeadId = @UserAccountId) AND (TransDate = CONVERT(DATETIME, @IssuedDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
			end
		else
			begin
				INSERT INTO LoanManagement.dbo.DayBook
                      (		AccountHeadId,		TransDate,						Receipt,	Payment,		ClosingBalance, IsDeleted,FinancialYearId)
				VALUES     (@UserAccountId, CONVERT(DATETIME, @IssuedDate, 102),@AmountIssued,	0	,	@AmountIssued,	0,			1)
			end
	--Daybook ending
	--===============================================================================================		
	--Cash book opening balance
	--==================================================================
	SELECT   @OpeningBalCount=  COUNT(*) 
	FROM         LoanManagement.dbo.OpeningBalance
	WHERE     (AccountHeadId = @CashBookLinkAcc)  AND (FinancialYearId = 1) AND (IsDeleted = 0)
	if(@OpeningBalCount>0)
	begin
		UPDATE    LoanManagement.dbo.OpeningBalance
		SET       ClosingBalance =ClosingBalance -@AmountIssued
		WHERE     (AccountHeadId = @CashBookLinkAcc) AND (FinancialYearId = 1) AND (IsDeleted = 0)

	end
	else
	begin
		SELECT   @AccScheduleId=   ScheduleId
		FROM        LoanManagement.dbo.AccountHead
		WHERE     (AccountHeadId = @CashBookLinkAcc) 
		INSERT INTO LoanManagement.dbo.OpeningBalance
                      (AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, IsDeleted)
		VALUES     (@CashBookLinkAcc,@AccScheduleId,0,0,(-1)*@AmountIssued,1,0)--@AmountIssued
	end
	--==================================================================
	SELECT   @OpeningBalCount=  COUNT(*) 
	FROM         LoanManagement.dbo.OpeningBalance
	WHERE     (AccountHeadId = @UserAccountId)
	if(@OpeningBalCount>0)
	begin
		UPDATE    LoanManagement.dbo.OpeningBalance
		SET       ClosingBalance =ClosingBalance +@AmountIssued
		WHERE     (AccountHeadId = @UserAccountId) AND (FinancialYearId = 1) AND (IsDeleted = 0)

	end
	else
	begin
		SELECT   @AccScheduleId=   ScheduleId
		FROM        LoanManagement.dbo.AccountHead
		WHERE     (AccountHeadId = @UserAccountId) 
		INSERT INTO LoanManagement.dbo.OpeningBalance
                      (AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, IsDeleted)
		VALUES     (@UserAccountId,@AccScheduleId,0,0,@AmountIssued,1,0)
	end
	--====================================================================
	
	INSERT INTO LoanManagement.dbo.LoanRepayment
                      (LoanId, RepaymentCode, RepaymentDate, PrincipalAmount, InterestAmount, 
                      InterestRate, RepaymentAmount, PreviousPaymentDueAmount, PendingPrincipalAmount, 
                      PendingInstallments, Status, PaymentMode, ChequeDetails, IsDeleted)
                      
	SELECT     @2LoanId, '', trans_date, Principal, 0, @InterestRate, Principal, 0, 0, 
						  0, 1, 1,challan , 0
	FROM         STOrdinaryLoanRep where (LoanNo = @LoanNo)

	--================================================================================
		DECLARE @Repayment CURSOR
		SET @Repayment = CURSOR FAST_FORWARD
		FOR
						SELECT       trans_date, Principal 
						FROM         STOrdinaryLoanRep where (LoanNo = @LoanNo)

		OPEN @Repayment
		FETCH NEXT FROM @Repayment
		INTO @RepayDate , @Principal
		WHILE @@FETCH_STATUS = 0
		BEGIN
			select @ReceiptMaxId=(id+1)  from LoanManagement.dbo.AccReceiptsHeader order by id  desc
			INSERT     
			INTO      LoanManagement.dbo.AccReceiptsHeader
			(BookId, TransDate, VoucherNo, VoucherTotal, 
			TransType, FinYear, FromModule, [Transaction], TransId, Cancelled, IsDeleted)
			VALUES     (@CashAccId,@RepayDate,'CASHREC-'+ convert(varchar(10),@ReceiptMaxId) ,@Principal, 
			0, 1, 1, 3,@2LoanId, 0, 0)
			select @ReceiptHeaderIden=@@IDENTITY
			
			INSERT     
			INTO      LoanManagement.dbo.AccReceiptsDetail(ReceiptsId, AccountId, Narration, Amount, Display, IsDeleted)
			VALUES     (@ReceiptHeaderIden,@CashBookLinkAcc,'Loan Repayment',@Principal,0,0)

			INSERT     
			INTO      LoanManagement.dbo.AccReceiptsDetail(ReceiptsId, AccountId, Narration, Amount, Display, IsDeleted)
			VALUES     (@ReceiptHeaderIden,@UserAccountId,'Loan Repayment',(-1)*@Principal,1,0)
			
			
			--Cash book opening balance
			--==================================================================
			SELECT   @OpeningBalCount=  COUNT(*) 
			FROM         LoanManagement.dbo.OpeningBalance
			WHERE     (AccountHeadId = @CashBookLinkAcc)
			if(@OpeningBalCount>0)
			begin
				UPDATE    LoanManagement.dbo.OpeningBalance
				SET       ClosingBalance =ClosingBalance +@Principal
				WHERE     (AccountHeadId = @CashBookLinkAcc) AND (FinancialYearId = 1) AND (IsDeleted = 0)

			end
			else
			begin
				SELECT   @AccScheduleId=   ScheduleId
				FROM        LoanManagement.dbo.AccountHead
				WHERE     (AccountHeadId = @CashBookLinkAcc) 
				INSERT INTO LoanManagement.dbo.OpeningBalance
							  (AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, IsDeleted)
				VALUES     (@CashBookLinkAcc,@AccScheduleId,0,0, @Principal,1,0)
			end
			--==================================================================
			SELECT   @OpeningBalCount=  COUNT(*) 
			FROM         LoanManagement.dbo.OpeningBalance
			WHERE     (AccountHeadId = @UserAccountId)
			if(@OpeningBalCount>0)
			begin
				UPDATE    LoanManagement.dbo.OpeningBalance
				SET       ClosingBalance =ClosingBalance -@Principal
				WHERE     (AccountHeadId = @UserAccountId) AND (FinancialYearId = 1) AND (IsDeleted = 0)

			end
			else
			begin
				SELECT   @AccScheduleId=   ScheduleId
				FROM        LoanManagement.dbo.AccountHead
				WHERE     (AccountHeadId = @UserAccountId) 
				INSERT INTO LoanManagement.dbo.OpeningBalance
							  (AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, IsDeleted)
				VALUES     (@UserAccountId,@AccScheduleId,0,0,(-1)*@Principal,1,0)
			end
			--====================================================================
			
			
			--Day book start
			--===============================================================================================
			SELECT   @DayBookCount=  COUNT(*)  
			FROM         LoanManagement.dbo.DayBook
			WHERE     (AccountHeadId = @CashBookLinkAcc) AND (TransDate = CONVERT(DATETIME, @RepayDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
				if(@DayBookCount>0)
					begin
						UPDATE    LoanManagement.dbo.DayBook
						SET              Receipt =Receipt+@Principal, Payment =Payment  
						WHERE     (AccountHeadId = @CashBookLinkAcc) AND (TransDate = CONVERT(DATETIME, @RepayDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
					end
				else
					begin
						INSERT INTO LoanManagement.dbo.DayBook
							  (		AccountHeadId,		TransDate,						Receipt,	Payment,		ClosingBalance, IsDeleted,FinancialYearId)
						VALUES     (@CashBookLinkAcc, CONVERT(DATETIME, @RepayDate, 102),@Principal,0,	@Principal,	0,			1)
					end
			
			SELECT   @DayBookCount=  COUNT(*)  
			FROM         LoanManagement.dbo.DayBook
			WHERE     (AccountHeadId = @UserAccountId) AND (TransDate = CONVERT(DATETIME, @RepayDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
				if(@DayBookCount>0)
					begin
						UPDATE    LoanManagement.dbo.DayBook
						SET              Receipt =Receipt , Payment =Payment + @Principal
						WHERE     (AccountHeadId = @UserAccountId) AND (TransDate = CONVERT(DATETIME, @RepayDate, 102)) AND (FinancialYearId = 1) AND (IsDeleted = 0)
					end
				else
					begin
						INSERT INTO LoanManagement.dbo.DayBook
							  (		AccountHeadId,		TransDate,						Receipt,	Payment,		ClosingBalance, IsDeleted,FinancialYearId)
						VALUES     (@UserAccountId, CONVERT(DATETIME, @RepayDate, 102),0,	@Principal	,	@Principal,	0,			1)
					end
			--Daybook ending
			--===============================================================================================
	
		FETCH NEXT FROM @Repayment
		INTO @RepayDate , @Principal
		END
		CLOSE @Repayment
		DEALLOCATE @Repayment
	--================================================================================
	
	
	

end
else
	print 'Not Found'



FETCH NEXT FROM @MyCursor
INTO @LoanNo, @InterestRate, @PenalInterestRate, @IssuedDate, @AmountIssued, @FirstinstDate, 
				@LastinstDate, @Mno, @NoofInstallment, @period, @CD, @Voucher, @Mode,  @SAC
END
CLOSE @MyCursor
DEALLOCATE @MyCursor
