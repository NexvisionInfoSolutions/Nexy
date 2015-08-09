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

        public enum UserType { Executive = 1, Member = 2 }

    }
}