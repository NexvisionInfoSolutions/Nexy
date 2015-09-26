using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UtilityHelper
{
    public class UtilityHelper
    {
        public static string FormatAddress(string addrLine1, string addrLine2, string addrPlace, string addrPost, string addrDistrict, string addrPin, string Taluk, string Village, string State, string Country)
        {
            string NewLine = "<br/>";
            //string comma = ",";
            return addrLine1 + NewLine + addrLine2 + NewLine + addrPlace + NewLine + addrPost + NewLine + addrDistrict + NewLine + addrPin + NewLine + State + "," + Country;
        }

        public static string GetBrowser()
        {
            HttpRequest Request = HttpContext.Current.Request;
            StringBuilder strb = new StringBuilder();
            strb.AppendFormat("User Agent: {0}{1}", Request.ServerVariables["http_user_agent"].ToString(), Environment.NewLine);
            strb.AppendFormat("Browser: {0}{1}", Request.Browser.Browser.ToString(), Environment.NewLine);
            strb.AppendFormat("Version: {0}{1}", Request.Browser.Version.ToString(), Environment.NewLine);
            strb.AppendFormat("Major Version: {0}{1}", Request.Browser.MajorVersion.ToString(), Environment.NewLine);
            strb.AppendFormat("Minor Version: {0}{1}", Request.Browser.MinorVersion.ToString(), Environment.NewLine);
            strb.AppendFormat("Platform: {0}{1}", Request.Browser.Platform.ToString(), Environment.NewLine);
            strb.AppendFormat("ECMA Script version: {0}{1}", Request.Browser.EcmaScriptVersion.ToString(), Environment.NewLine);
            strb.AppendFormat("Type: {0}{1}", Request.Browser.Type.ToString(), Environment.NewLine);
            /*strb.AppendFormat("-------------------------------------------------------------------------------{0}", Environment.NewLine);
            strb.AppendFormat("ActiveX Controls: {0}{1}", Request.Browser.ActiveXControls.ToString(), Environment.NewLine);
            strb.AppendFormat("Background Sounds: {0}{1}", Request.Browser.BackgroundSounds.ToString(), Environment.NewLine);
            strb.AppendFormat("AOL: {0}{1}", Request.Browser.AOL.ToString(), Environment.NewLine);
            strb.AppendFormat("Beta: {0}{1}", Request.Browser.Beta.ToString(), Environment.NewLine);
            strb.AppendFormat("CDF: {0}{1}", Request.Browser.CDF.ToString(), Environment.NewLine);
            strb.AppendFormat("ClrVersion: {0}{1}", Request.Browser.ClrVersion.ToString(), Environment.NewLine);
            strb.AppendFormat("Cookies: {0}{1}", Request.Browser.Cookies.ToString(), Environment.NewLine);
            strb.AppendFormat("Crawler: {0}{1}", Request.Browser.Crawler.ToString(), Environment.NewLine);
            strb.AppendFormat("Frames: {0}{1}", Request.Browser.Frames.ToString(), Environment.NewLine);
            strb.AppendFormat("Tables: {0}{1}", Request.Browser.Tables.ToString(), Environment.NewLine);
            strb.AppendFormat("JavaApplets: {0}{1}", Request.Browser.JavaApplets.ToString(), Environment.NewLine);
            strb.AppendFormat("JavaScript: {0}{1}", Request.Browser.JavaScript.ToString(), Environment.NewLine);
            strb.AppendFormat("MSDomVersion: {0}{1}", Request.Browser.MSDomVersion.ToString(), Environment.NewLine);
            strb.AppendFormat("TagWriter: {0}{1}", Request.Browser.TagWriter.ToString(), Environment.NewLine);
            strb.AppendFormat("VBScript: {0}{1}", Request.Browser.VBScript.ToString(), Environment.NewLine);
            strb.AppendFormat("W3CDomVersion: {0}{1}", Request.Browser.W3CDomVersion.ToString(), Environment.NewLine);
            strb.AppendFormat("Win16: {0}{1}", Request.Browser.Win16.ToString(), Environment.NewLine);
            strb.AppendFormat("Win32: {0}{1}", Request.Browser.Win32.ToString(), Environment.NewLine);
            strb.AppendFormat("-------------------------------------------------------------------------------{0}", Environment.NewLine);*/
            strb.AppendFormat("MachineName: {0}{1}", Environment.MachineName, Environment.NewLine);
            /*strb.AppendFormat("OSVersion: {0}{1}", Environment.OSVersion, Environment.NewLine);
            strb.AppendFormat("ProcessorCount: {0}{1}", Environment.ProcessorCount, Environment.NewLine);*/
            strb.AppendFormat("UserName: {0}{1}", Environment.UserName, Environment.NewLine);
            /*strb.AppendFormat("Version: {0}{1}", Environment.Version, Environment.NewLine);
            strb.AppendFormat("UserInteractive: {0}{1}", Environment.UserInteractive, Environment.NewLine);*/
            strb.AppendFormat("UserDomainName: {0}{1}", Environment.UserDomainName, Environment.NewLine);
            return strb.ToString();
        }
    }
}
