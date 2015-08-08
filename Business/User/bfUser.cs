using Business.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.User
{
    class bfUser : bfBase
    {
        public bfUser(DbContext dbConnection) : base(dbConnection) { }
    }
}
