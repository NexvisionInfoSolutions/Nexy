using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    class bfBase : DbContext
    {
        public bfBase(System.Data.Common.DbConnection dbConnection, Boolean OwnsConnection) : base(dbConnection, OwnsConnection) { }
    }
}
