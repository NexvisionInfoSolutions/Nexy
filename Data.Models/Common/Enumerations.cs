using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    namespace Enumerations
    {
        public enum UserGroupStatus { Inactive = 0, Active = 1 }

        public enum CompanyStatus { Inactive = 0, Active }

        public enum AccountTypeStatus { Inactive = 0, Active }

        public enum AccountBookStatus { Inactive = 0, Active }

        public enum AccountBookTypeStatus { Inactive = 0, Active }

        public enum UrlInfoStatus { Inactive = 0, Active = 1 }

        public enum UserType { User = 1, Executive = 2, Member = 3 }

        public enum RepaymentInterval { Daily = 1, Weekly, Monthly, BiMonthly, Quaterly, Yearly }

        public enum LoanStatus { Inactive = 0, Active = 1, Approved, Completed, Cancelled, Recalled }

        public enum DepositType { Fixed = 1, Recurring }

        public enum DepositStatus { Inactive = 0, Active = 1, Matured, Closed, Unclaimed }

        public enum RepaymentStatus { Pending = 0, Paid, Posted, Cancelled }

        public enum WithdrawalStatus { Pending = 0, Paid, Posted }

        public enum ModeOfPayment { Cash = 1, Cheque = 2, Others = 9 }

        public enum ReceiptType { Receipt = 0, Payment = 1 }

        public enum BankTransType { Deposit = 0, Withdrawal = 1 }

        public enum Instrument { Cash = 0, Cheque = 1, CreditCard = 2 }

        public enum TransactionType { CashReceipt = 0, CashPayment = 1, LoanEntry = 2, LoanRepayment = 3, DepositEntry = 4, DepositWithdrawal = 5, BankDeposit = 6, BankWithdrawal = 7, JournalEntry }
    }
}