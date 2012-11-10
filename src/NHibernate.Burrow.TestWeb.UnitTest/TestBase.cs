using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using NUnit.Framework;
using WatiN.Core; 

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    /// <summary>
    /// based on code writen by Mikhail Dikov http://www.mikhaildikov.com/2008/01/using-asp.html
    /// </summary>
    public class TestBase
    {

        private const string devServerPort = "2612";
        private IE ie;
        private string rootUrl;
        private static Process cmdProcess;
        private readonly static Disposal dis = new Disposal();

        private const string rootpath = "NHibernate.Burrow.TestWeb/";
        
        protected  IE IE
        {
            get{ return ie;}
        }
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            bool IsWebStarted;
            rootUrl = string.Format("http://localhost:{0}", devServerPort);
          
                // Check if Dev WebServer runs
            string startPath = rootUrl + "/Default.aspx";
            ie = new IE(startPath, true);
                IsWebStarted = ie.ContainsText("Started");
            

            if (!IsWebStarted)
            {
                // If not start it
                string command = @"C:\Program Files (x86)\IIS Express\iisexpress.exe";

                string rootPhyPath = Environment.CurrentDirectory.Remove( Environment.CurrentDirectory.IndexOf(@".UnitTest")) ;
                    
                    //Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf('\\'));
                string commandArgs = string.Format(" /path:\"{0}\" /port:{1}", rootPhyPath, devServerPort);

                cmdProcess = new Process();
                cmdProcess.StartInfo.Arguments = commandArgs;
                cmdProcess.StartInfo.CreateNoWindow = true;
                cmdProcess.StartInfo.FileName = command;
                cmdProcess.StartInfo.UseShellExecute = false;
                cmdProcess.StartInfo.WorkingDirectory = command.Substring(0, command.LastIndexOf('\\'));
               if( !cmdProcess.Start())
                   throw new Exception("Cannot start webserver");
                
                // .. and try one more time to see if the server is up
                ie.GoTo(rootUrl);
            }
            Assert.IsTrue(ie.ProcessID>0);

            // Give some time to crank up
            Thread.Sleep(1000);
        }

        [TearDown]
        public void TearDown()
        {
            ie.Close();
        }
 
        protected  void GoTo(string path)
        {
            if (path.IndexOf(".aspx") < 0)
                path = path + "/Default.aspx";
            ie.GoTo(rootUrl+"/"+path);
        }

        
        #endregion


        protected void AssertText(string s)
        {
            Assert.IsTrue(IE.ContainsText(s));
        }

        protected  void AssertTestSuccessMessageShown()
        {
            AssertText("Congratulations! Test passed.");
        }

        private class Disposal
        {
            ~Disposal() //use a dusctruction method to kill the developer server at the end
            {
                if(TestBase.cmdProcess != null)
                    TestBase.cmdProcess.Kill();
            }
        }     
    }
}