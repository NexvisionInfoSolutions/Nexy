using Business.Base;
using Data.Models.Accounts;
using Data.Models.Enumerations;
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
                sdtoSettings settings = AppDb.GeneralSettings.Where(x => x.SettingsId == 1).FirstOrDefault();
                sdtoAccountType accTypeDebiter = AppDb.AccountTypes.Where(x => x.UniqueName.Equals("Debiter", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                var accHead = new sdtoAccountHead()
                {
                    AccountCode = "ACH_" + Member.Code,
                    AccountName = "AC_" + Member.Code,
                    ScheduleId = settings.AssetScheduleId.Value,
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
                    ScheduleId = accHead.ScheduleId,
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
                            dtls.ForEach(x => x.IsDeleted = true);

                            AppDb.Entry(dtls).State = EntityState.Modified;
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
                            dtls.ForEach(x => x.IsDeleted = true);

                            AppDb.Entry(dtls).State = EntityState.Modified;
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
                            dtls.ForEach(x => x.IsDeleted = true);

                            AppDb.Entry(dtls).State = EntityState.Modified;
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
                            dtls.ForEach(x => x.IsDeleted = true);

                            AppDb.Entry(dtls).State = EntityState.Modified;
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
                            TransDate = DateTime.Now,
                            VoucherTotal = LoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                            TransType = ReceiptType.Receipt,
                            FinYear = 1,
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

                        // Cash Account
                        var receiptDetailsCr = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accCashBook.AccountHeadId,
                            Narration = "Loan issued",
                            Amount = -1 * LoanInfo.LoanAmount,
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
                    var settingsInterestBookId = AppDb.GeneralSettings.FirstOrDefault().InterestBookId;
                    //Post for Bank book
                    var accCashBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    var accInterestBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingsInterestBookId).FirstOrDefault();
                    if (accCashBook != null)
                    {
                        var receipt = new sdtoReceiptHeader()
                        {
                            BookId = accCashBook.AccountBookId,
                            TransDate = DateTime.Now,
                            VoucherTotal = LoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                            TransType = ReceiptType.Receipt,
                            FinYear = 1,
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

                        // Cash Account
                        var receiptDetailsDb = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accCashBook.AccountHeadId,
                            Narration = "Loan Repayment received",
                            Amount = (LoanRepaymentInfo.RepaymentAmount - LoanRepaymentInfo.InterestAmount),
                            Display = 1
                        };

                        var receiptDetailsDbInt = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accInterestBook.AccountHeadId,
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
                            FinYear = 1,
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
                            AccountId = DepositInfo.InstrumentMode == Instrument.Cash ? accCashBook.AccountHeadId : accBankBook.AccountHeadId,
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

                    var settingsInterestBookId = AppDb.GeneralSettings.FirstOrDefault().InterestBookId;
                    var accInterestBook = AppDb.AccountBooks.Where(x => x.AccountBookId == settingsInterestBookId).FirstOrDefault();
                    if (accCashBook != null && accBankBook != null)
                    {
                        var receipt = new sdtoReceiptHeader()
                        {
                            BookId = WithdrawalInfo.InstrumentMode == Instrument.Cash ? accCashBook.AccountBookId : accBankBook.AccountBookId,
                            TransDate = DateTime.Now,
                            VoucherTotal = WithdrawalInfo.WithdrawalAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                            TransType = ReceiptType.Receipt,
                            FinYear = 1,
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
                            AccountId = WithdrawalInfo.InstrumentMode == Instrument.Cash ? accCashBook.AccountHeadId : accBankBook.AccountHeadId,
                            Narration = "Deposit Amount Withdrawn",
                            Amount = -1 * (WithdrawalInfo.WithdrawalAmount - WithdrawalInfo.InterestAmount),
                            Display = 1
                        };

                        var receiptDetailsCrInt = new sdtoReceiptDetails()
                        {
                            ReceiptsId = receipt.Id,
                            AccountId = accInterestBook.AccountHeadId,
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
    }
}
