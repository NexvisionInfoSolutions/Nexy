using Data.Models.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LoanManagementSystem.Models
{
    public class sdtoViewReportFilter
    {
        public string CompanyId { get; set; }

        public int OperationId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MutipleSelectionModel Accounts { get; set; }

        public sdtoViewReportFilter()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public SelectList Companies { get; set; }
    }
}
