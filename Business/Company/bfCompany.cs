using Business.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Company
{
    class bfCompany : bfBase
    {
        public bfCompany(System.Data.Common.DbConnection dbConnection, Boolean OwnsConnection) : base(dbConnection, OwnsConnection) { }
    }
}
