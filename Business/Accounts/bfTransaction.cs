using Business.Base;
using Data.Models.Accounts;
using Data.Models.Enumerations;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Reports
{
    public class bfTransaction : bfBase
    {
        public bfTransaction(DbContext dbConnection) : base(dbConnection) { }

        public void InitiateMemberAccounts(LoanManagementSystem.Models.sdtoUser Member)
        {
            try
            {
                sdtoSettings settings = AppDb.GeneralSettings.FirstOrDefault();
                sdtoAccountType accTypeDebiter = AppDb.AccountTypes.Where(x => x.UniqueName.Equals("Debiter", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                var accHead = new sdtoAccountHead()
                {
                    AccountCode = "ACH_" + Member.Code,
                    AccountName = "AC_" + Member.Code,
                    ScheduleId = settings.SundryDebtorAccountId.Value,
                    AccountTypeId = accTypeDebiter.AccountTypeId,
                    CreditLimit = 0,
                    CreditDays = 0,
                    TIN = string.Empty,
                    CST = string.Empty,
                    AddressId = Member.UserAddressId.Value,
                    ContactId = Member.UserContactId.Value,
                    CreatedBy = Member.UserID,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                };
                AppDb.AccountHeads.Add(accHead);
                AppDb.SaveChanges();

                Member.AccountHeadId = accHead.AccountHeadId;
                Member.ModifiedOn = DateTime.Now;
                Member.ModifiedBy = Member.UserID;
                AppDb.Entry(Member).State = EntityState.Modified;
                AppDb.SaveChanges();

                sdtoOpeningBalance memberOpeniningBalance = new sdtoOpeningBalance()
                {
                    AccountHeadId = accHead.AccountHeadId,
                    ClosingBalance = 0,
                    CreditOpeningBalance = 0,
                    DebitOpeningBalance = 0,
                    FinancialYearId = 1,
                    ScheduleId = settings.SundryDebtorAccountId.Value,
                    IsDeleted = false,
                    CreatedBy = Member.UserID,
                    CreatedOn = DateTime.Now
                };

                AppDb.OpeningBalance.Add(memberOpeniningBalance);
                AppDb.SaveChanges();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
        }

        public bool CancelPostedLoanIssue(sdtoLoanInfo LoanInfo)
        {
            bool bFlag = true;
            var member = AppDb.User.Where(x => x.UserID == LoanInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    if (accCashBook != null)
                    {
                        var header = AppDb.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && x.BookId == accCashBook.AccountBookId && x.TransId == LoanInfo.LoanId && x.Transaction == TransactionType.LoanEntry).FirstOrDefault();
                        if (header != null)
                        {
                            header.Cancelled = 1;
                            AppDb.Entry(header).State = EntityState.Modified;
                            AppDb.SaveChanges();

                            var dtls = AppDb.ReceiptDetails.Where(x => x.ReceiptsId == header.Id && x.IsDeleted == false).ToList();
                            //dtls.ForEach(x => x.IsDeleted = true);

                            foreach (var dtlItem in dtls)
                            {
                                dtlItem.IsDeleted = true;
                                AppDb.Entry(dtlItem).State = EntityState.Modified;
                            }

                            AppDb.SaveChanges();
                        }
                    }
                }
            }
            return bFlag;
        }
        public bool CancelPostedLoanRepayment(sdtoLoanRepayment LoanRepaymentInfo)
        {
            bool bFlag = true;
            var LoanInfo = AppDb.sdtoLoanInfoes.Find(LoanRepaymentInfo.LoanId);
            var member = AppDb.User.Where(x => x.UserID == LoanInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    if (accCashBook != null)
                    {
                        var header = AppDb.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && x.BookId == accCashBook.AccountBookId && x.TransId == LoanRepaymentInfo.LoanRepaymentId && x.Transaction == TransactionType.LoanRepayment).FirstOrDefault();
                        if (header != null)
                        {
                            header.Cancelled = 1;
                            AppDb.Entry(header).State = EntityState.Modified;
                            AppDb.SaveChanges();

                            var dtls = AppDb.ReceiptDetails.Where(x => x.ReceiptsId == header.Id && x.IsDeleted == false).ToList();
                            //dtls.ForEach(x => x.IsDeleted = true);

                            foreach (var dtlItem in dtls)
                            {
                                dtlItem.IsDeleted = true;
                                AppDb.Entry(dtlItem).State = EntityState.Modified;
                            }

                            AppDb.SaveChanges();
                        }
                    }
                }
            }
            return bFlag;
        }
        public bool CancelPostedDepositIssue(sdtoDepositInfo DepositInfo)
        {
            bool bFlag = true;
            var member = AppDb.User.Where(x => x.UserID == DepositInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    var settingBankBookId = AppDb.GeneralSettings.FirstOrDefault().BankBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingBankBookId).FirstOrDefault();
                    if (accCashBook != null && accBankBook != null)
                    {
                        var header = AppDb.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && (x.BookId == accCashBook.AccountBookId || x.BookId == accBankBook.AccountBookId) && x.TransId == DepositInfo.DepositId && x.Transaction == TransactionType.DepositEntry).FirstOrDefault();
                        if (header != null)
                        {
                            header.Cancelled = 1;
                            AppDb.Entry(header).State = EntityState.Modified;
                            AppDb.SaveChanges();

                            var dtls = AppDb.ReceiptDetails.Where(x => x.ReceiptsId == header.Id && x.IsDeleted == false).ToList();
                            //dtls.ForEach(x => x.IsDeleted = true);

                            foreach (var dtlItem in dtls)
                            {
                                dtlItem.IsDeleted = true;
                                AppDb.Entry(dtlItem).State = EntityState.Modified;
                            }

                            AppDb.SaveChanges();
                        }
                    }
                }
            }
            return bFlag;
        }
        public bool CancelPostedDepositWithdrawal(sdtoWithdrawalInfo WithdrawalInfo)
        {
            bool bFlag = true;
            var DepositInfo = AppDb.sdtoDepositInfoes.Find(WithdrawalInfo.DepositId);
            var member = AppDb.User.Where(x => x.UserID == DepositInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    var settingBankBookId = AppDb.GeneralSettings.FirstOrDefault().BankBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingBankBookId).FirstOrDefault();
                    if (accCashBook != null && accBankBook != null)
                    {
                        var header = AppDb.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && (x.BookId == accCashBook.AccountBookId || x.BookId == accBankBook.AccountBookId) && x.TransId == WithdrawalInfo.WithdrawalId && x.Transaction == TransactionType.DepositWithdrawal).FirstOrDefault();
                        if (header != null)
                        {
                            header.Cancelled = 1;
                            AppDb.Entry(header).State = EntityState.Modified;
                            AppDb.SaveChanges();

                            var dtls = AppDb.ReceiptDetails.Where(x => x.ReceiptsId == header.Id && x.IsDeleted == false).ToList();
                            //dtls.ForEach(x => x.IsDeleted = true);

                            foreach (var dtlItem in dtls)
                            {
                                dtlItem.IsDeleted = true;
                                AppDb.Entry(dtlItem).State = EntityState.Modified;
                            }

                            AppDb.SaveChanges();
                        }
                    }
                }
            }
            return bFlag;
        }
        public bool PostLoanIssue(sdtoLoanInfo LoanInfo)
        {
            bool tranFlag = true;

            //      var member = AppDb.User.Join(AppDb.AccountHeads, // the source table of the inner join
            //post => post.AccountHead.AccountHeadId,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
            //meta => meta.AccountHeadId,   // Select the foreign key (the second part of the "on" clause)
            //(post, meta) => new { Post = post, Meta = meta }).Where(x => x.Post.UserID == LoanInfo.UserId).Select(x => new { x.Post.UserID, AccountHead = new { AccountHeadId = x.Meta.AccountHeadId, AccountCode = x.Meta.AccountCode, AccountName = x.Meta.AccountName, AccountType = x.Meta.AccountType, AccountTypeId = x.Meta.AccountTypeId } }).FirstOrDefault();

            //from f in AppDb.User
            //join b in AppDb.AccountHeads on f.AccountHeadId equals b.AccountHeadId
            //where f.UserID == LoanInfo.UserId
            //select new { f.UserID, AccountHead = new sdtoAccountHead() { AccountHeadId = b.AccountHeadId, AccountCode = b.AccountCode, AccountName = b.AccountName, AccountType = b.AccountType, AccountTypeId = b.AccountTypeId } };

            //var member = members.FirstOrDefault();

            var member = AppDb.User.Where(x => x.UserID == LoanInfo.UserId).FirstOrDefault();

            //var member = AppDb.User.Include(x => x.AccountHeadId).Where(x => x.UserID == LoanInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    if (accCashBook != null)
                    {
                        var receipt = new sdtoReceiptHeader()
                        {
                            BookId = accCashBook.AccountBookId,
                            TransDate = LoanInfo.TransactionDate.Value,
                            VoucherTotal = LoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                            TransType = ReceiptType.Receipt,
                            FinancialYearId = CurrentUser.UserSession.FinancialYearId.Value,
                            FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                            Transaction = TransactionType.LoanEntry, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                            TransId = LoanInfo.LoanId, //Doubt: Is transaction id loan id?
                            Cancelled = 0
                        };
                        AppDb.ReceiptHeader.Add(receipt);
                        AppDb.SaveChanges();

                        receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.Id + accCashBook.ReceiptVoucherSuffix;
                        AppDb.Entry(receipt).State = EntityState.Modified;
                        AppDb.SaveChanges();

                        // Member
                        var receiptDetailsDb = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accHeadMember.AccountHeadId,
                            Narration = "Loan issued",
                            Amount = LoanInfo.LoanAmount,
                            Display = 1
                        };

                        UpdateClosingBalance(accHeadMember.AccountHeadId, CurrentUser.UserSession.FinancialYearId.Value, LoanInfo.LoanAmount);
                        UpdateDayBookBalance(accHeadMember.AccountHeadId, CurrentUser.UserSession.FinancialYearId.Value, LoanInfo.TransactionDate.Value, LoanInfo.LoanAmount, TransactionType.LoanEntry);

                        // Cash Account
                        var receiptDetailsCr = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accCashBook.AccountHeadId.Value,
                            Narration = "Loan issued",
                            Amount = -1 * LoanInfo.LoanAmount,
                            Display = 1
                        };

                        UpdateClosingBalance(accCashBook.AccountHeadId.Value, CurrentUser.UserSession.FinancialYearId.Value, LoanInfo.LoanAmount);
                        UpdateDayBookBalance(accCashBook.AccountHeadId.Value, CurrentUser.UserSession.FinancialYearId.Value, LoanInfo.TransactionDate.Value, LoanInfo.LoanAmount, TransactionType.LoanEntry);

                        AppDb.ReceiptDetails.Add(receiptDetailsCr);
                        AppDb.ReceiptDetails.Add(receiptDetailsDb);
                        AppDb.SaveChanges();
                    }
                }
            }

            return tranFlag;
        }
        public bool PostLoanRepayment(sdtoLoanRepayment LoanRepaymentInfo)
        {
            bool tranFlag = true;
            var LoanInfo = AppDb.sdtoLoanInfoes.Find(LoanRepaymentInfo.LoanId);

            var member = AppDb.User.Where(x => x.UserID == LoanInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    var settingsInterestAccountId = AppDb.GeneralSettings.FirstOrDefault().InterestAccountId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    var accInterest = AppDb.AccountHeads.Where(x => x.AccountHeadId == settingsInterestAccountId).FirstOrDefault();
                    if (accCashBook != null)
                    {
                        var receipt = new sdtoReceiptHeader()
                        {
                            BookId = accCashBook.AccountBookId,
                            TransDate = DateTime.Now,
                            VoucherTotal = LoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                            TransType = ReceiptType.Receipt,
                            FinancialYearId = CurrentUser.UserSession.FinancialYearId.Value,
                            FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                            Transaction = TransactionType.LoanRepayment, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                            TransId = LoanRepaymentInfo.LoanRepaymentId, //Doubt: Is transaction id loan id?
                            Cancelled = 0
                        };
                        AppDb.ReceiptHeader.Add(receipt);
                        AppDb.SaveChanges();

                        receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.Id + accCashBook.ReceiptVoucherSuffix;
                        AppDb.Entry(receipt).State = EntityState.Modified;
                        AppDb.SaveChanges();

                        // Member
                        var receiptDetailsCr = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accHeadMember.AccountHeadId,
                            Narration = "Loan Repayment received",
                            Amount = -1 * LoanRepaymentInfo.RepaymentAmount,
                            Display = 1
                        };

                        UpdateClosingBalance(accHeadMember.AccountHeadId, CurrentUser.UserSession.FinancialYearId.Value, -1 * LoanRepaymentInfo.RepaymentAmount);
                        UpdateDayBookBalance(accHeadMember.AccountHeadId, CurrentUser.UserSession.FinancialYearId.Value, LoanInfo.TransactionDate.Value, -1 * LoanRepaymentInfo.RepaymentAmount, TransactionType.LoanRepayment);

                        // Cash Account
                        var receiptDetailsDb = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accCashBook.AccountHeadId.Value,
                            Narration = "Loan Repayment received",
                            Amount = (LoanRepaymentInfo.RepaymentAmount - LoanRepaymentInfo.InterestAmount),
                            Display = 1
                        };

                        UpdateClosingBalance(accCashBook.AccountHeadId.Value, CurrentUser.UserSession.FinancialYearId.Value, (LoanRepaymentInfo.RepaymentAmount - LoanRepaymentInfo.InterestAmount));
                        UpdateDayBookBalance(accCashBook.AccountHeadId.Value, CurrentUser.UserSession.FinancialYearId.Value, LoanInfo.TransactionDate.Value, (LoanRepaymentInfo.RepaymentAmount - LoanRepaymentInfo.InterestAmount), TransactionType.LoanRepayment);

                        var receiptDetailsDbInt = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accInterest.AccountHeadId,
                            Narration = "Loan Repayment received",
                            Amount = LoanRepaymentInfo.InterestAmount,
                            Display = 1
                        };

                        AppDb.ReceiptDetails.Add(receiptDetailsDbInt);
                        AppDb.ReceiptDetails.Add(receiptDetailsCr);
                        AppDb.ReceiptDetails.Add(receiptDetailsDb);
                        AppDb.SaveChanges();
                    }
                }
            }

            return tranFlag;
        }
        public bool PostDepositIssue(sdtoDepositInfo DepositInfo)
        {
            bool tranFlag = true;

            var member = AppDb.User.Where(x => x.UserID == DepositInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    var settingBankBookId = AppDb.GeneralSettings.FirstOrDefault().BankBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingBankBookId).FirstOrDefault();
                    if (accCashBook != null && accBankBook != null)
                    {
                        var receipt = new sdtoReceiptHeader()
                        {
                            BookId = accCashBook.AccountBookId,
                            TransDate = DateTime.Now,
                            VoucherTotal = DepositInfo.DepositAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                            TransType = ReceiptType.Receipt,
                            FinancialYearId = CurrentUser.UserSession.FinancialYearId.Value,
                            FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                            Transaction = TransactionType.DepositEntry, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                            TransId = DepositInfo.DepositId, //Doubt: Is transaction id loan id?
                            Cancelled = 0
                        };
                        AppDb.ReceiptHeader.Add(receipt);
                        AppDb.SaveChanges();

                        if (DepositInfo.InstrumentMode == Instrument.Cash)
                            receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.Id + accCashBook.ReceiptVoucherSuffix;
                        else
                            receipt.VoucherNo = accBankBook.ReceiptVoucherPrefix + receipt.Id + accBankBook.ReceiptVoucherSuffix;
                        AppDb.Entry(receipt).State = EntityState.Modified;
                        AppDb.SaveChanges();

                        // Member
                        var receiptDetailsCr = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accHeadMember.AccountHeadId,
                            Narration = "Deposit Account Opened",
                            Amount = -1 * DepositInfo.DepositAmount,
                            Display = 1
                        };

                        // Cash Account
                        var receiptDetailsDb = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = DepositInfo.InstrumentMode == Instrument.Cash ? accCashBook.AccountHeadId.Value : accBankBook.AccountHeadId.Value,
                            Narration = "Loan Repayment received",
                            Amount = DepositInfo.DepositAmount,
                            Display = 1
                        };

                        AppDb.ReceiptDetails.Add(receiptDetailsCr);
                        AppDb.ReceiptDetails.Add(receiptDetailsDb);
                        AppDb.SaveChanges();
                    }
                }
            }

            return tranFlag;
        }
        public bool PostDepositWithdrawal(sdtoWithdrawalInfo WithdrawalInfo)
        {
            bool tranFlag = true;

            var DepositInfo = AppDb.sdtoDepositInfoes.Where(x => x.DepositId == WithdrawalInfo.DepositId).FirstOrDefault();
            var member = AppDb.User.Where(x => x.UserID == DepositInfo.UserId).FirstOrDefault();
            if (member != null)
            {
                var accHeadMember = AppDb.AccountHeads.Find(member.AccountHeadId);
                if (accHeadMember != null)
                {
                    var settingCashBookId = AppDb.GeneralSettings.FirstOrDefault().CashBookId;
                    var settingBankBookId = AppDb.GeneralSettings.FirstOrDefault().BankBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingBankBookId).FirstOrDefault();

                    var settingsInterestAccId = AppDb.GeneralSettings.FirstOrDefault().InterestAccountId;
                    var accInterest = AppDb.AccountHeads.Where(x => x.AccountHeadId == settingsInterestAccId).FirstOrDefault();
                    if (accCashBook != null && accBankBook != null)
                    {
                        var receipt = new sdtoReceiptHeader()
                        {
                            BookId = WithdrawalInfo.InstrumentMode == Instrument.Cash ? accCashBook.AccountBookId : accBankBook.AccountBookId,
                            TransDate = DateTime.Now,
                            VoucherTotal = WithdrawalInfo.WithdrawalAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                            TransType = ReceiptType.Receipt,
                            FinancialYearId = CurrentUser.UserSession.FinancialYearId.Value,
                            FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                            Transaction = TransactionType.DepositWithdrawal, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                            TransId = WithdrawalInfo.WithdrawalId, //Doubt: Is transaction id loan id?
                            Cancelled = 0
                        };
                        AppDb.ReceiptHeader.Add(receipt);
                        AppDb.SaveChanges();

                        if (WithdrawalInfo.InstrumentMode == Instrument.Cash)
                            receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.Id + accCashBook.ReceiptVoucherSuffix;
                        else
                            receipt.VoucherNo = accBankBook.ReceiptVoucherPrefix + receipt.Id + accBankBook.ReceiptVoucherSuffix;
                        AppDb.Entry(receipt).State = EntityState.Modified;
                        AppDb.SaveChanges();

                        // Member
                        var receiptDetailsDb = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accHeadMember.AccountHeadId,
                            Narration = "Deposit Amount Withdrawn",
                            Amount = WithdrawalInfo.WithdrawalAmount,
                            Display = 1
                        };

                        // Cash Account
                        var receiptDetailsCr = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = WithdrawalInfo.InstrumentMode == Instrument.Cash ? accCashBook.AccountHeadId.Value : accBankBook.AccountHeadId.Value,
                            Narration = "Deposit Amount Withdrawn",
                            Amount = -1 * (WithdrawalInfo.WithdrawalAmount - WithdrawalInfo.InterestAmount),
                            Display = 1
                        };

                        var receiptDetailsCrInt = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accInterest.AccountHeadId,
                            Narration = "Deposit Amount Withdrawn",
                            Amount = -1 * WithdrawalInfo.InterestAmount,
                            Display = 1
                        };

                        AppDb.ReceiptDetails.Add(receiptDetailsCrInt);
                        AppDb.ReceiptDetails.Add(receiptDetailsCr);
                        AppDb.ReceiptDetails.Add(receiptDetailsDb);
                        AppDb.SaveChanges();
                    }
                }
            }

            return tranFlag;
        }

        public sdtoTransactionAccountDetails GetAccountDetails(long AccountBookId, string TranType)
        {
            sdtoTransactionAccountDetails rptCollection = new sdtoTransactionAccountDetails();
            try
            {
                AppDb.Database.Connection.Open();
                DbRawSqlQuery<sdtoTransactionAccountDetails> result = AppDb.Database.SqlQuery<sdtoTransactionAccountDetails>("usp_GetAccountDetails @vAccountBookId, @vTranType, @vSessionKey",
                    new SqlParameter("@vAccountBookId", AccountBookId)
                    , new SqlParameter("@vTranType", TranType)
                    , new SqlParameter("@vSessionKey", CurrentUser.UserSession.SessionKey));

                if (result != null)
                    rptCollection = result.ToList().FirstOrDefault();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public bool AddCashPayment(sdtoViewAccCashReceiptPayment objCashReceiptPayment)
        {
            var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == objCashReceiptPayment.Book.AccountBookId).FirstOrDefault();
            if (accBankBook != null)
            {
                sdtoReceiptHeader hdrBankDeposit = new sdtoReceiptHeader()
                {
                    BookId = accBankBook.AccountBookId,
                    Cancelled = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CurrentUser.UserID,
                    TransDate = objCashReceiptPayment.Date,
                    FinancialYearId = CurrentUser.UserSession.FinancialYearId.Value,
                    FromModule = 0,
                    IsDeleted = false,
                    // VoucherNo = objCashReceiptPayment.Voucher,
                    VoucherTotal = objCashReceiptPayment.Details.Sum(x => x.Amount),
                    Transaction = TransactionType.CashPayment,
                    TransType = ReceiptType.Payment
                };

                AppDb.ReceiptHeader.Add(hdrBankDeposit);
                AppDb.SaveChanges();

                hdrBankDeposit.VoucherNo = accBankBook.PaymentVoucherPrefix + hdrBankDeposit.Id + accBankBook.PaymentVoucherSuffix;
                AppDb.Entry(hdrBankDeposit).State = EntityState.Modified;
                AppDb.SaveChanges();

                foreach (sdtoViewAccCashReceiptPaymentDetails dtl in objCashReceiptPayment.Details)
                {
                    sdtoReceiptDetails bankDepositDtlCr = new sdtoReceiptDetails()
                    {
                        ReceiptsId = hdrBankDeposit.Id,
                        AccountId = dtl.AccountHeadId,
                        Narration = dtl.Narration,
                        Amount = dtl.Amount,
                        CreatedBy = CurrentUser.UserID,
                        CreatedOn = DateTime.Now,
                        Display = 1,
                        IsDeleted = false,
                    };

                    UpdateClosingBalance(dtl.AccountHeadId, CurrentUser.UserSession.FinancialYearId.Value, bankDepositDtlCr.Amount);
                    UpdateDayBookBalance(dtl.AccountHeadId, CurrentUser.UserSession.FinancialYearId.Value, objCashReceiptPayment.Date, bankDepositDtlCr.Amount, TransactionType.CashPayment);
                    AppDb.ReceiptDetails.Add(bankDepositDtlCr);
                }

                sdtoReceiptDetails bankDepositDtlDb = new sdtoReceiptDetails()
                {
                    ReceiptsId = hdrBankDeposit.Id,
                    AccountId = accBankBook.AccountHeadId.Value,
                    Narration = "Cash Receipt - " + hdrBankDeposit.VoucherNo,
                    Amount = -1 * objCashReceiptPayment.Details.Sum(x => x.Amount),
                    CreatedBy = CurrentUser.UserID,
                    CreatedOn = DateTime.Now,
                    Display = 0,
                    IsDeleted = false
                };
                UpdateClosingBalance(accBankBook.AccountHeadId.Value, CurrentUser.UserSession.FinancialYearId.Value, bankDepositDtlDb.Amount);
                UpdateDayBookBalance(accBankBook.AccountHeadId.Value, CurrentUser.UserSession.FinancialYearId.Value, objCashReceiptPayment.Date, bankDepositDtlDb.Amount, TransactionType.CashPayment);
                AppDb.ReceiptDetails.Add(bankDepositDtlDb);
                AppDb.SaveChanges();
            }
            return true;
        }

        public bool AddCashReceipt(sdtoViewAccCashReceiptPayment objCashReceiptPayment)
        {
            var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == objCashReceiptPayment.Book.AccountBookId).FirstOrDefault();
            if (accBankBook != null)
            {
                sdtoReceiptHeader hdrBankDeposit = new sdtoReceiptHeader()
                {
                    BookId = accBankBook.AccountBookId,
                    Cancelled = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CurrentUser.UserID,
                    TransDate = objCashReceiptPayment.Date,
                    FinancialYearId = CurrentUser.UserSession.FinancialYearId.Value,
                    FromModule = 0,
                    IsDeleted = false,
                    //VoucherNo = objCashReceiptPayment.Voucher,
                    VoucherTotal = objCashReceiptPayment.Details.Sum(x => x.Amount),
                    Transaction = TransactionType.CashReceipt,
                    TransType = ReceiptType.Receipt
                };

                AppDb.ReceiptHeader.Add(hdrBankDeposit);
                AppDb.SaveChanges();

                hdrBankDeposit.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrBankDeposit.Id + accBankBook.ReceiptVoucherSuffix;
                AppDb.Entry(hdrBankDeposit).State = EntityState.Modified;
                AppDb.SaveChanges();

                foreach (sdtoViewAccCashReceiptPaymentDetails dtl in objCashReceiptPayment.Details)
                {
                    sdtoReceiptDetails bankDepositDtlCr = new sdtoReceiptDetails()
                    {
                        ReceiptsId = hdrBankDeposit.Id,
                        AccountId = dtl.AccountHeadId,
                        Narration = dtl.Narration,
                        Amount = -1 * dtl.Amount,
                        CreatedBy = CurrentUser.UserID,
                        CreatedOn = DateTime.Now,
                        Display = 1,
                        IsDeleted = false,
                    };
                    UpdateClosingBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, bankDepositDtlCr.Amount);
                    UpdateDayBookBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, objCashReceiptPayment.Date, bankDepositDtlCr.Amount, TransactionType.CashReceipt);
                    AppDb.ReceiptDetails.Add(bankDepositDtlCr);
                }

                sdtoReceiptDetails bankDepositDtlDb = new sdtoReceiptDetails()
                {
                    ReceiptsId = hdrBankDeposit.Id,
                    AccountId = accBankBook.AccountHeadId.Value,
                    Narration = "Cash Receipt - " + hdrBankDeposit.VoucherNo,
                    Amount = objCashReceiptPayment.Details.Sum(x => x.Amount),
                    CreatedBy = CurrentUser.UserID,
                    CreatedOn = DateTime.Now,
                    Display = 0,
                    IsDeleted = false
                };

                UpdateClosingBalance(bankDepositDtlDb.AccountId, CurrentUser.UserSession.FinancialYearId.Value, bankDepositDtlDb.Amount);
                UpdateDayBookBalance(bankDepositDtlDb.AccountId, CurrentUser.UserSession.FinancialYearId.Value, objCashReceiptPayment.Date, bankDepositDtlDb.Amount, TransactionType.CashReceipt);
                AppDb.ReceiptDetails.Add(bankDepositDtlDb);
                AppDb.SaveChanges();
            }
            return true;
        }

        public bool UpdateClosingBalance(long AccountId, long FinancialYearId, decimal amount)
        {
            var openingBalance = AppDb.OpeningBalance.Where(x => x.AccountHeadId == AccountId && x.FinancialYearId == FinancialYearId).FirstOrDefault();
            if (openingBalance != null)
            {
                openingBalance.ClosingBalance += amount;
                AppDb.Entry<sdtoOpeningBalance>(openingBalance).State = EntityState.Modified;
                AppDb.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool UpdateDayBookBalance(long AccountId, long FinancialYearId, DateTime transactionDate, decimal Amount, TransactionType transType)
        {
            if (AccountId > 0)
            {
                var dayBookEntry = AppDb.DayBook.Where(x => x.AccountHeadId == AccountId && x.IsDeleted == false && x.FinancialYearId == FinancialYearId).FirstOrDefault(); //x.Date.Date == transactionDate.Date &&
                if (dayBookEntry == null)
                {
                    dayBookEntry = new sdtoDayBook() { AccountHeadId = AccountId, CreatedBy = CurrentUser.UserID, CreatedOn = DateTime.Now, TransDate = transactionDate, FinancialYearId = FinancialYearId };
                    AppDb.DayBook.Add(dayBookEntry);
                    AppDb.SaveChanges();
                }

                if (dayBookEntry != null)
                {
                    switch (transType)
                    {
                        case TransactionType.CashPayment:
                            dayBookEntry.Payment += Amount;
                            break;
                        case TransactionType.CashReceipt:
                            dayBookEntry.Receipt += Amount;
                            break;
                        case TransactionType.BankDeposit:
                            break;
                        case TransactionType.BankWithdrawal:
                            break;
                        case TransactionType.DepositEntry:
                            dayBookEntry.Payment += Amount;
                            break;
                        case TransactionType.DepositWithdrawal:
                            dayBookEntry.Receipt += Amount;
                            break;
                        case TransactionType.LoanEntry:
                            dayBookEntry.Receipt += Amount;
                            break;
                        case TransactionType.LoanRepayment:
                            dayBookEntry.Payment += Amount;
                            break;
                    }
                    dayBookEntry.ClosingBalance += Amount;
                    AppDb.Entry(dayBookEntry).State = EntityState.Modified;
                    return true;
                }
                else return false;
            }
            else
                return false;
        }

        public bool CancelCashAccountHeader(long AccountHeaderId)
        {
            var accHeaderEntry = AppDb.ReceiptHeader.Find(AccountHeaderId);
            if (accHeaderEntry != null)
            {
                var accHeaderSub = AppDb.ReceiptDetails.Where(x => x.ReceiptsId == AccountHeaderId);
                if (accHeaderSub != null)
                {
                    foreach (sdtoReceiptDetails dtl in accHeaderSub)
                    {
                        dtl.IsDeleted = false;
                        AppDb.Entry(dtl).State = EntityState.Modified;

                        var openingBal = AppDb.OpeningBalance.Where(x => x.AccountHeadId == dtl.AccountId && x.IsDeleted == false && x.FinancialYearId == accHeaderEntry.FinancialYearId).FirstOrDefault();
                        if (openingBal != null)
                        {
                            openingBal.ClosingBalance += -1 * dtl.Amount;
                            AppDb.Entry<sdtoOpeningBalance>(openingBal).State = EntityState.Modified;
                        }

                        var dayBookEntry = AppDb.DayBook.Where(x => x.AccountHeadId == dtl.AccountId && x.IsDeleted == false && x.TransDate.Date == accHeaderEntry.TransDate.Date && x.FinancialYearId == accHeaderEntry.FinancialYearId).FirstOrDefault();
                        if (dayBookEntry != null)
                        {
                            switch (accHeaderEntry.Transaction)
                            {
                                case TransactionType.CashPayment:
                                    dayBookEntry.Payment += -1 * dtl.Amount;
                                    break;
                                case TransactionType.CashReceipt:
                                    dayBookEntry.Receipt += -1 * dtl.Amount;
                                    break;
                                case TransactionType.BankDeposit:
                                    break;
                                case TransactionType.BankWithdrawal:
                                    break;
                            }
                            dayBookEntry.ClosingBalance += -1 * dtl.Amount;
                            AppDb.Entry(dayBookEntry).State = EntityState.Modified;
                        }
                    }
                    AppDb.SaveChanges();
                }

                accHeaderEntry.Cancelled = 1;
                AppDb.Entry<sdtoReceiptHeader>(accHeaderEntry).State = EntityState.Modified;
                AppDb.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool AddBankWithdrawal(sdtoViewAccDepositWithdrawal objDepositWithdrawal)
        {
            //sdtoUser user = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            //var settingCashBookId = db.GeneralSettings.FirstOrDefault().BankBookId;
            //Post for Bank book
            var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == objDepositWithdrawal.Book.AccountBookId).FirstOrDefault();
            if (accBankBook != null)
            {
                sdtoBankDepositHeader hdrBankDeposit = new sdtoBankDepositHeader()
                {
                    BookId = accBankBook.AccountBookId,
                    Cancelled = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CurrentUser.UserID,
                    TransDate = objDepositWithdrawal.Date,
                    FinYear = 1,
                    FromModule = 0,
                    IsDeleted = false,
                    VoucherNo = objDepositWithdrawal.Voucher,
                    VoucherTotal = objDepositWithdrawal.VoucherTotal
                };

                AppDb.BankDepositHeader.Add(hdrBankDeposit);
                AppDb.SaveChanges();

                hdrBankDeposit.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrBankDeposit.Id + accBankBook.ReceiptVoucherSuffix;
                AppDb.Entry(hdrBankDeposit).State = EntityState.Modified;
                AppDb.SaveChanges();

                foreach (sdtoViewAccDepositWithdrawalDetails dtl in objDepositWithdrawal.Details)
                {
                    sdtoBankDepositDetails bankDepositDtlCr = new sdtoBankDepositDetails()
                    {
                        BankDepositId = hdrBankDeposit.Id,
                        AccountId = dtl.AccountHeadId,
                        Narration = dtl.Narration,
                        Amount = dtl.Amount,
                        CreatedBy = CurrentUser.UserID,
                        CreatedOn = DateTime.Now,
                        Display = 1,
                        IsDeleted = false,
                        Instrument = dtl.InstrumentType,
                        InstrumentNo = dtl.InstrumentNo,
                        InstrumentDate = dtl.InstrumentDate
                    };
                    UpdateClosingBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, Convert.ToDecimal(bankDepositDtlCr.Amount));
                    UpdateDayBookBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, objDepositWithdrawal.Date, Convert.ToDecimal(bankDepositDtlCr.Amount), TransactionType.BankWithdrawal);
                    AppDb.BankDepositDetails.Add(bankDepositDtlCr);
                }

                sdtoBankDepositDetails bankDepositDtlDb = new sdtoBankDepositDetails()
                {
                    BankDepositId = hdrBankDeposit.Id,
                    AccountId = accBankBook.AccountHeadId.Value,
                    Narration = "Bank deposit/withdrawal",
                    Amount = -1 * objDepositWithdrawal.Details.Sum(x => x.Amount),
                    CreatedBy = CurrentUser.UserID,
                    CreatedOn = DateTime.Now,
                    Display = 0,
                    IsDeleted = false,
                    Instrument = objDepositWithdrawal.Details.FirstOrDefault().InstrumentType,
                    InstrumentNo = objDepositWithdrawal.Details.FirstOrDefault().InstrumentNo,
                    InstrumentDate = objDepositWithdrawal.Details.FirstOrDefault().InstrumentDate
                };

                UpdateClosingBalance(bankDepositDtlDb.AccountId, CurrentUser.UserSession.FinancialYearId.Value, Convert.ToDecimal(bankDepositDtlDb.Amount));
                UpdateDayBookBalance(bankDepositDtlDb.AccountId, CurrentUser.UserSession.FinancialYearId.Value, objDepositWithdrawal.Date, Convert.ToDecimal(bankDepositDtlDb.Amount), TransactionType.BankWithdrawal);
                AppDb.BankDepositDetails.Add(bankDepositDtlDb);
                AppDb.SaveChanges();

                return true;
            }
            else
                return false;
        }

        public bool AddBankDeposit(sdtoViewAccDepositWithdrawal objDepositWithdrawal)
        {
            var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == objDepositWithdrawal.Book.AccountBookId).FirstOrDefault();
            if (accBankBook != null)
            {
                sdtoBankDepositHeader hdrBankDeposit = new sdtoBankDepositHeader()
                {
                    BookId = objDepositWithdrawal.Book.AccountBookId,
                    Cancelled = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CurrentUser.UserID,
                    TransDate = objDepositWithdrawal.Date,
                    FinYear = 1,
                    FromModule = 0,
                    IsDeleted = false,
                    VoucherNo = objDepositWithdrawal.Voucher,
                    VoucherTotal = objDepositWithdrawal.VoucherTotal
                };

                AppDb.BankDepositHeader.Add(hdrBankDeposit);
                AppDb.SaveChanges();

                hdrBankDeposit.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrBankDeposit.Id + accBankBook.ReceiptVoucherSuffix;
                AppDb.Entry(hdrBankDeposit).State = EntityState.Modified;
                AppDb.SaveChanges();

                foreach (sdtoViewAccDepositWithdrawalDetails dtl in objDepositWithdrawal.Details)
                {
                    sdtoBankDepositDetails bankDepositDtlCr = new sdtoBankDepositDetails()
                    {
                        BankDepositId = hdrBankDeposit.Id,
                        AccountId = dtl.AccountHeadId,
                        Narration = dtl.Narration,
                        Amount = -1 * dtl.Amount,
                        CreatedBy = CurrentUser.UserID,
                        CreatedOn = DateTime.Now,
                        Display = 1,
                        IsDeleted = false,
                        Instrument = dtl.InstrumentType,
                        InstrumentNo = dtl.InstrumentNo
                    };

                    if (dtl.InstrumentDate != DateTime.MinValue)
                        bankDepositDtlCr.InstrumentDate = dtl.InstrumentDate;

                    UpdateClosingBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, Convert.ToDecimal(bankDepositDtlCr.Amount));
                    UpdateDayBookBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, objDepositWithdrawal.Date, Convert.ToDecimal(bankDepositDtlCr.Amount), TransactionType.BankDeposit);
                    AppDb.BankDepositDetails.Add(bankDepositDtlCr);
                }
                AppDb.SaveChanges();

                sdtoBankDepositDetails bankDepositDtlDb = new sdtoBankDepositDetails()
                {
                    BankDepositId = hdrBankDeposit.Id,
                    AccountId = accBankBook.AccountHeadId.Value,
                    Narration = "Bank deposit/withdrawal",
                    Amount = objDepositWithdrawal.Details.Sum(x => x.Amount),
                    CreatedBy = CurrentUser.UserID,
                    CreatedOn = DateTime.Now,
                    Display = 0,
                    IsDeleted = false,
                    Instrument = objDepositWithdrawal.Details.FirstOrDefault().InstrumentType,
                    InstrumentNo = objDepositWithdrawal.Details.FirstOrDefault().InstrumentNo,
                    InstrumentDate = objDepositWithdrawal.Details.FirstOrDefault().InstrumentDate
                };

                UpdateClosingBalance(bankDepositDtlDb.AccountId, CurrentUser.UserSession.FinancialYearId.Value, Convert.ToDecimal(bankDepositDtlDb.Amount));
                AppDb.BankDepositDetails.Add(bankDepositDtlDb);
                AppDb.SaveChanges();

                return true;
            }
            else
                return false;
        }

        public bool CancelBankAccountHeader(long AccountHeaderId)
        {
            var accHeaderEntry = AppDb.BankDepositHeader.Find(AccountHeaderId);
            if (accHeaderEntry != null)
            {
                var accHeaderSub = AppDb.BankDepositDetails.Where(x => x.BankDepositId == AccountHeaderId);
                if (accHeaderSub != null)
                {
                    foreach (sdtoBankDepositDetails dtl in accHeaderSub)
                    {
                        dtl.IsDeleted = false;
                        AppDb.Entry(dtl).State = EntityState.Modified;

                        var openingBal = AppDb.OpeningBalance.Where(x => x.AccountHeadId == dtl.AccountId && x.IsDeleted == false && x.FinancialYearId == accHeaderEntry.FinYear).FirstOrDefault();
                        if (openingBal != null)
                        {
                            openingBal.ClosingBalance += -1 * Convert.ToDecimal(dtl.Amount);
                            AppDb.Entry<sdtoOpeningBalance>(openingBal).State = EntityState.Modified;
                        }

                        //var dayBookEntry = AppDb.DayBook.Where(x => x.AccountHeadId == dtl.AccountId && x.IsDeleted == false && x.Date.Date == accHeaderEntry.TransDate.Date && x.FinancialYearId == accHeaderEntry.FinYear).FirstOrDefault();
                        //if (dayBookEntry != null)
                        //{
                        //    switch (accHeaderEntry.Transaction)
                        //    {
                        //        case TransactionType.CashPayment:
                        //            dayBookEntry.Payment += -1 * Convert.ToDecimal(dtl.Amount);
                        //            break;
                        //        case TransactionType.CashReceipt:
                        //            dayBookEntry.Receipt += -1 * Convert.ToDecimal(dtl.Amount);
                        //            break;
                        //        case TransactionType.BankDeposit:
                        //            break;
                        //        case TransactionType.BankWithdrawal:
                        //            break;
                        //    }
                        //    dayBookEntry.ClosingBalance += -1 * Convert.ToDecimal(dtl.Amount);
                        //    AppDb.Entry(dayBookEntry).State = EntityState.Modified;
                        //}
                    }
                    AppDb.SaveChanges();
                }

                accHeaderEntry.Cancelled = 1;
                AppDb.Entry<sdtoBankDepositHeader>(accHeaderEntry).State = EntityState.Modified;
                AppDb.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool CancelJournalAccountHeader(long AccountHeaderId)
        {
            var accHeaderEntry = AppDb.JournalHeader.Find(AccountHeaderId);
            if (accHeaderEntry != null)
            {
                var accHeaderSub = AppDb.JournalDetails.Where(x => x.JournalId == AccountHeaderId);
                if (accHeaderSub != null)
                {
                    foreach (sdtoJournalDetails dtl in accHeaderSub)
                    {
                        dtl.IsDeleted = false;
                        AppDb.Entry(dtl).State = EntityState.Modified;

                        var openingBal = AppDb.OpeningBalance.Where(x => x.AccountHeadId == dtl.AccountId && x.IsDeleted == false && x.FinancialYearId == accHeaderEntry.FinancialYearId).FirstOrDefault();
                        if (openingBal != null)
                        {
                            openingBal.ClosingBalance += -1 * Convert.ToDecimal(dtl.CrAmount + dtl.DrAmount);
                            AppDb.Entry<sdtoOpeningBalance>(openingBal).State = EntityState.Modified;
                        }

                        var dayBookEntry = AppDb.DayBook.Where(x => x.AccountHeadId == dtl.AccountId && x.IsDeleted == false && x.TransDate.Date == accHeaderEntry.TransDate.Value.Date && x.FinancialYearId == accHeaderEntry.FinancialYearId).FirstOrDefault();
                        if (dayBookEntry != null)
                        {
                            switch (accHeaderEntry.Transaction)
                            {
                                case TransactionType.CashPayment:
                                    dayBookEntry.Payment += -1 * Convert.ToDecimal(dtl.CrAmount + dtl.DrAmount);
                                    break;
                                case TransactionType.CashReceipt:
                                    dayBookEntry.Receipt += -1 * Convert.ToDecimal(dtl.CrAmount + dtl.DrAmount);
                                    break;
                                case TransactionType.BankDeposit:
                                    break;
                                case TransactionType.BankWithdrawal:
                                    break;
                            }
                            dayBookEntry.ClosingBalance += -1 * Convert.ToDecimal(dtl.CrAmount + dtl.DrAmount);
                            AppDb.Entry(dayBookEntry).State = EntityState.Modified;
                        }
                    }
                    AppDb.SaveChanges();
                }

                accHeaderEntry.Cancelled = 1;
                AppDb.Entry<sdtoJournalHeader>(accHeaderEntry).State = EntityState.Modified;
                AppDb.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool AddJournalEntry(sdtoViewAccJournalEntry objJournalEntry)
        {
            sdtoUser user = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            var accBankBook = AppDb.AccountBooks.Where(x => x.AccountBookId == objJournalEntry.Book.AccountBookId).FirstOrDefault();
            if (accBankBook != null)
            {
                sdtoJournalHeader hdrCashReceiptPayment = new sdtoJournalHeader()
                {
                    BookId = accBankBook.AccountBookId,
                    Cancelled = 0,
                    CreatedOn = DateTime.Now,
                    CreatedBy = user.UserID,
                    FinancialYearId = 1,
                    FromModule = 0,
                    IsDeleted = false,
                    TransDate = objJournalEntry.Date,
                    VoucherNo = objJournalEntry.Voucher,
                    VoucherTotal = Convert.ToDecimal((-1 * objJournalEntry.Details.Sum(x => x.CreditAmount)) + objJournalEntry.Details.Sum(x => x.DebitAmount))
                };

                AppDb.JournalHeader.Add(hdrCashReceiptPayment);
                AppDb.SaveChanges();

                hdrCashReceiptPayment.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrCashReceiptPayment.Id + accBankBook.ReceiptVoucherSuffix;
                AppDb.Entry(hdrCashReceiptPayment).State = EntityState.Modified;
                AppDb.SaveChanges();

                foreach (sdtoViewAccJournalEntryDetails dtl in objJournalEntry.Details)
                {
                    sdtoJournalDetails bankDepositDtlCr = new sdtoJournalDetails()
                    {
                        JournalId = hdrCashReceiptPayment.Id,
                        AccountId = dtl.AccountHeadId,
                        Narration = dtl.Narration,
                        CrAmount = dtl.CreditAmount,
                        DrAmount = dtl.DebitAmount,
                        CreatedBy = user.UserID,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };

                    UpdateClosingBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, Convert.ToDecimal(bankDepositDtlCr.CrAmount + bankDepositDtlCr.DrAmount));
                    UpdateDayBookBalance(bankDepositDtlCr.AccountId, CurrentUser.UserSession.FinancialYearId.Value, objJournalEntry.Date, Convert.ToDecimal(bankDepositDtlCr.CrAmount + bankDepositDtlCr.DrAmount), TransactionType.JournalEntry);
                    AppDb.JournalDetails.Add(bankDepositDtlCr);
                }

                AppDb.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
