using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper
{
    public class UtilityHelper
    {
        public static string FormatAddress(string addrLine1, string addrLine2, string addrPlace, string addrPost, string addrDistrict, string addrPin)
        {
            string NewLine = "<br/>";
            //string comma = ",";
            return addrLine1 + NewLine + addrLine2 + NewLine + addrPlace + NewLine + addrPost + NewLine + addrDistrict + addrPin;
        }
    }
}
