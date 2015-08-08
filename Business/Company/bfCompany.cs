using Business.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Company
{
    class bfCompany : bfBase
    {
        public bfCompany(DbContext dbConnection) : base(dbConnection) { }
    }
}
