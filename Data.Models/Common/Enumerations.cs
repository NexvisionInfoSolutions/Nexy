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

        public enum UserType {User=1, Executive = 2, Member = 3 }

        public enum RepaymentInterval { Daily = 1, Weekly, Monthly, BiMonthly, Quaterly, Yearly }

        public enum LoanStatus { Inactive = 0, Active = 1 }

        public enum RepaymentStatus { Pending = 0, Paid, Posted }

        public enum ModeOfPayment { Cash = 1, Cheque = 2, Others = 9 }
    }
}