/*--------------------------------------------------------------------------------------+
//----------------------------------------------------------------------------
// DOCUMENT ID:   
// LIBRARY:       
// CREATOR:       Mark Anderson
// DATE:          02-15-2017
//
// NAME:          SmartDispatcherExtLoader.cs
//
// DESCRIPTION:   Utility.
//
// REFERENCES:    ProjectWise.
//
// ---------------------------------------------------------------------------
// NOTICE
//    NOTICE TO ALL PERSONS HAVING ACCESS HERETO:  This document or
//    recording contains computer software or related information
//    constituting proprietary trade secrets of Black & Veatch, which
//    have been maintained in "unpublished" status under the copyright
//    laws, and which are to be treated by all persons having acdcess
//    thereto in manner to preserve the status thereof as legally
//    protectable trade secrets by neither using nor disclosing the
//    same to others except as may be expressly authorized in advance
//    by Black & Veatch.  However, it is intended that all prospective
//    rights under the copyrigtht laws in the event of future
//    "publication" of this work shall also be reserved; for which
//    purpose only, the following is included in this notice, to wit,
//    "(C) COPYRIGHT 1997 BY BLACK & VEATCH, ALL RIGHTS RESERVED"
// ---------------------------------------------------------------------------
/*
/* CHANGE LOG
 * $Archive: /ProjectWise/ASFramework/HPEDocumentProcessorWebService/HPWDocProcessorWebService/HPEDocProcessorWebService.asmx.cs $
 * $Revision: 1 $
 * $Modtime: 2/15/17 7:30a $
 * $History: HPEDocProcessorWebService.asmx.cs $
 * 
 * *****************  Version 1  *****************
 * User: Mark.anderson Date: 2/15/17    Time: 7:39a
 * Created in $/ProjectWise/ASFramework/HPEDocumentProcessorWebService/HPWDocProcessorWebService
 * The WebService for the generic document processor framework
 * 
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Web.Services;

namespace HPEDocProcessorWebService
{
    /// <summary>
    /// This service provides a way for calling the RunSCJob application.  This is used to create AS Jobs
    /// that will be processed in Automation Services. The service is needed to bridge the gap from desktop to 
    /// server side processing.
    /// </summary>
   /* [WebService(Namespace = "http://localhost/HPEDocProcessorWebService/")] */
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HPEDocProcessorWebService : System.Web.Services.WebService
    {
        //these need to be set to the location of the exe to be called.  On the test site the service needs to point
        // to the d drive.
        private static string sServiceLocation = @"c:\Program Files (x86)\Bentley\AutomationServices\RunSCJob.exe ";
        private static string sWorkingDir = @"c:\Program Files (x86)\Bentley\AutomationServices\";
        /// <summary>
        /// This method will take in the commandline parameters to the DWGRunJob exe and pass them through from a web service
        /// </summary>
        /// <param name="unparsed"></param>
        /// <returns></returns>
        [WebMethod]
        public int RunJob(string unparsed)
        {
            ProcessStartInfo info = new ProcessStartInfo(sServiceLocation);
            
            info.UseShellExecute = false;
            info.Arguments = unparsed;
            info.WorkingDirectory = sWorkingDir;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;

            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }
            return 0;
        }
        /// <summary>
        /// This method will take in six parameters.  It divides the docset, 
        /// and other parameters into seperate paramters for the application 
        /// command line.
        /// </summary>
        /// <param name="docset">The set of one or more documents in project:document id form</param>
        /// <param name="appName">the application to run</param>
        /// <param name="appKeyin">the keyin to run</param>
        /// <param name="pwLoginCMD"> the function in the app that logs in to PW</param>
        /// <param name="pwdatasource">the encoded PW datasource</param>
        /// <param name="pwusername">the user name to login to PW</param>
        /// <param name="pwpwd">the encoded password</param>
        /// <returns></returns>
        [WebMethod]
        public int RunJobEx
        (
            string docset,  // in the format projID:docID
            string appName,
            string appKeyin,
            string pwLoginCMD,
            string pwdatasource,
            string pwusername,
            string pwpwd
        )
        {
            ProcessStartInfo info = new ProcessStartInfo(sServiceLocation);
            
            string decode_pwdatasource = HttpUtility.UrlDecode(pwdatasource);
            string decode_pwpwd = HttpUtility.UrlDecode(pwpwd);
            string decode_docset = HttpUtility.UrlDecode(docset);
            string decode_appName = HttpUtility.UrlDecode(appName);
            string decode_appKeyin = HttpUtility.UrlDecode(appKeyin);
            string decode_pwLoginCMD = HttpUtility.UrlDecode(pwLoginCMD);
            string decode_pwusername = HttpUtility.UrlDecode(pwusername);

            StringBuilder unparsed = new StringBuilder(
                string.Format("-doclist {0} -doclist -appname {1}  -keyin {2} -pwlogincmd {3} -pwdatasourcename {4} -pwuser {5} -pwpassword \"{6}\"",
            decode_docset,decode_appName,decode_appKeyin,decode_pwLoginCMD,decode_pwdatasource,decode_pwusername,decode_pwpwd));

            info.UseShellExecute = false;
            info.Arguments = unparsed.ToString();
            info.WorkingDirectory = sWorkingDir;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }
            return 0;
        }
        /// <summary>
        /// This will  take in the project  id  as a string.  This will then run the DWGRunJob with the command line
        /// -projectid and will process the entire project/folder.
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [WebMethod]
        public string SendFolderId(string ProjectId)
        {
            ProcessStartInfo info = new ProcessStartInfo(sServiceLocation);
            StringBuilder progArgs = new StringBuilder(" -projectid ");
            progArgs.Append(ProjectId);

            info.UseShellExecute = false;
            info.Arguments = progArgs.ToString();
            info.WorkingDirectory = sWorkingDir;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }
            return "completed";

        }
    }
}