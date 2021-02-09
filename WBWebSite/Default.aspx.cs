using System;
using System.Threading;
using System.IO;

public partial class _Default : System.Web.UI.Page 
{
    IEBrowser browser;

    // this is in the main thread

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            AutoResetEvent resultEvent = new AutoResetEvent(false);
            string result = null;
            bool visible = true;

            browser = new IEBrowser(visible, resultEvent);

            // wait for the third thread getting result and setting result event
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });
            // the result is ready later than the result event setting somtimes 
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            result = browser.HtmlResult;
            if (!visible) browser.Dispose();

            string path = Request.PhysicalApplicationPath;
            TextWriter tw = new StreamWriter(path + "result.html");
            tw.Write(result);
            tw.Close();

            Response.Output.WriteLine("<script>window.open ('result.html','mywindow','location=1,status=0,scrollbars=1,resizable=1,width=600,height=600');</script>");
        }
        catch (Exception ex)
        {

        }

    }
   
}
