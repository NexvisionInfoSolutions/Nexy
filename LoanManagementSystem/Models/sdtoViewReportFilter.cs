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

        [Required(ErrorMessage="Please enter the start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the end date")]
        public DateTime EndDate { get; set; }

        public string MiscFilter { get; set; }

        public List<string> MemberIds { get; set; }

        public List<string> AccountIds { get; set; }

        public List<string> StatusIds { get; set; }

        public List<string> LoanIds { get; set; }

        public List<string> DepositIds { get; set; }

        public MultipleSelectionModel Accounts { get; set; }

        public MultipleSelectionModel Members { get; set; }

        public MultipleSelectionModel Status { get; set; }

        public MultipleSelectionModel Loans { get; set; }

        public MultipleSelectionModel Deposits { get; set; }

        public sdtoViewReportFilter()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            MemberIds = new List<string>();
            AccountIds = new List<string>();
            StatusIds = new List<string>();
            LoanIds = new List<string>();
            DepositIds = new List<string>();

            Accounts = new MultipleSelectionModel();
            Members = new MultipleSelectionModel();
            Status = new MultipleSelectionModel();
            Loans = new MultipleSelectionModel();
            Deposits = new MultipleSelectionModel();
        }

        public SelectList Companies { get; set; }
    }
}
