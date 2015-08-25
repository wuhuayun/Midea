using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;

namespace MideaAscm.Code
{
    public class AscmReportViewer : Microsoft.Reporting.WebForms.ReportViewer
    {
        /// <summary>谷歌浏览器无法显示报表，主要原因是是ReportViewer中的Js在Chrome下会造成死循环</summary>
        protected override void Render(HtmlTextWriter writer)
        {
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter tmpWriter = new HtmlTextWriter(sw);
                base.Render(tmpWriter);
                string val = sw.ToString();
                val = val.Replace(@"!= 'javascript:\'\''", @"!= 'javascript:\'\'' && false");
                writer.Write(val);
            }
        }
    }
}