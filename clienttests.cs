/* Software License Agreement (BSD License)
 * 
 * Copyright (c) 2010-2011, Rustici Software, LLC
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the <organization> nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL Rustici Software, LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections;

namespace RusticiSoftware.HostedEngine.Client
{
    public class clienttests
    {
        public ScormEngineService svc;
        public ServiceRequest req;
        public Configuration cfg;

        private string _course;
        private string _regid2;

        public string tstCourse
        {
            get
            {
                if (_course == null)
                {
                    // find a test course
                    try
                    {
                        if (cfg == null)
                        {
                            fixtureSetup();
                        }
                        CourseService cs = new CourseService(cfg, svc);
                        _course = cs.GetCourseList()[0].CourseId;
                    }
                    catch (Exception ex)
                    {
                        throw new IgnoreException("Failed to select a test course", ex);
                    }
                }
                return _course;
            }
        }
        public string regId2
        {
            get
            {
                if (_regid2 == null)
                {
                    // find a test course
                    try
                    {
                        if (cfg == null)
                        {
                            fixtureSetup();
                        }
                        RegistrationService rs = new RegistrationService(cfg, svc);
                        List<RegistrationData> regs = rs.GetRegistrationList();
                        if (regs.Count > 0)
                        {
                            _regid2 = regs[0].RegistrationId;
                        }
                        else
                        {
                            _regid2 = Guid.NewGuid().ToString();
                            rs.CreateRegistration(_regid2, tstCourse, 0, tstLearner, fName, lName, postback, authType, pbLogin, pbPass, resultFmt);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new IgnoreException("Failed to select a test Registration", ex);
                    }
                }
                return _regid2;
            }
        }
        public string regId;
        public string courseId;
        public string courseFile = "Golf_RunTimeAdvancedCalls_SCORM20043rdEdition.zip";
        public string courseFilePath = @"Z:\shared\samplecourses\Scorm2004";
        public string upldFile = @"Z:\shared\samplecourses\Scorm2004\Golf_RunTimeAdvancedCalls_SCORM20043rdEdition.zip";
        public string tstLearner = "nunitLearnerId_" + Guid.NewGuid().ToString();
        public string fName = "Test";
        public string lName = "User";
        public string postback = "http://localhost";
        public RegistrationResultsAuthType authType = RegistrationResultsAuthType.HTTPBASIC;
        public string pbLogin = "testpostbackusername";
        public string pbPass = "password";
        public RegistrationResultsFormat resultFmt = RegistrationResultsFormat.FULL;
        public string domain = "default"; //TODO: should local WS handle other domains?

        [TestFixtureSetUp]
        public virtual void fixtureSetup()
        {
            if (cfg == null)
            {
                cfg = new Configuration(@"http://cloud.scorm.com/EngineWebServices/", "[App Id]", "[Secret Key]");
                //cfg = new Configuration(@"http://120.50.41.175/EngineWebServices", "defaultID", "changeme_lgpjkqk5JK");//"4JloSCYfXMancDw267peGZoL8s4GNlGhQkIV5qch");
                //cfg = new Configuration(@"http://bclark.local/EngineWebServices", "defaultID", "4JloSCYfXMancDw267peGZoL8s4GNlGhQkIV5qch");
                //cfg = new Configuration(@"http://test/EngineWebServices", "defaultID", "changeme_lgpjkqk5JK");
                //http://cloud.scorm.com/EngineWebServices/
                svc = new ScormEngineService(cfg);
            }
        }

        [SetUp]
        public virtual void caseSetup()
        {
            req = svc.CreateNewRequest();
            courseId = Guid.NewGuid().ToString();
            regId = Guid.NewGuid().ToString();
        }

        protected String testRegMissingMsg
        {
            get { return string.Format("Could not find registration [{0}] associated with appid [{1}]", regId2, cfg.AppId); }
        }
        protected String testCourseMissingMsg
        {
            get { return string.Format("Could not find course [{0}], versionid [{1}] associated with appid [{2}]", tstCourse, 0, cfg.AppId); }
        }

        protected String courseServerPath
        {
            //TODO: should local keep PD when saving uploads, if so, use PD here, not "default"
            // not correct for cloud WS unless "default" pd is used.
            get { return "default/"+courseFile; }
        }

        //
    }

    [TestFixture]
    public class DbgSvc : clienttests
    {
        DebugService debugSvc;

        public override void caseSetup()
        {
            base.caseSetup();
            debugSvc = new DebugService(cfg, svc);
        }

        [Test]
        public void ping()
        {
            bool resp = debugSvc.CloudPing();
            Debug.WriteLine(Environment.NewLine + resp.ToString());
            Assert.IsTrue(resp,"Ping Failed");
        }

        [Test]
        public void Authping()
        {
            bool resp = debugSvc.CloudAuthPing();
            Debug.WriteLine(Environment.NewLine + resp.ToString());
            Assert.IsTrue(resp, "Auth Ping Failed");
        }

        [Test]
        public void getTime()
        {
            XmlDocument response = req.CallService("rustici.debug.getTime");
            DateTime serverTime = DateTime.ParseExact(response.SelectSingleNode(@"/rsp/currenttime").InnerText, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            TimeSpan diff = DateTime.UtcNow - serverTime;
            Assert.LessOrEqual(Math.Abs(diff.Seconds), 60, "clock times too far off - server time is: " + serverTime.ToLocalTime());
        }
    }
    [TestFixture]
    public class Upload : clienttests
    {
        UploadService upload;


        public override void caseSetup()
        {
            base.caseSetup();
            upload = new UploadService(cfg, svc);
        }

        [Test]
        public void getUploadToken()
        {
            XmlDocument response = req.CallService("rustici.upload.getUploadToken");
            Console.WriteLine(response.InnerXml);
        }

        [Test]
        public void UploadShowDelete()
        {
            upload.UploadFile(upldFile, domain);
            foreach (FileData fd in upload.GetFileList(domain))
            {
                Console.WriteLine();
                Console.WriteLine("File: " + fd.Name);
                Console.WriteLine(fd.LastModified);
                Console.WriteLine(fd.Size);
                Console.WriteLine();

                if (fd.Name != courseFile)
                {
                    upload.DeleteFile(fd.Name, domain);
                }
            }
        }

        [Test]
        public void ListFiles()
        {
            List<FileData> files = upload.GetFileList();
            foreach (FileData file in files)
            {
                Console.WriteLine(file.Name);
            }
        }

        [Test]
        public void url()
        {
            Console.WriteLine(Environment.NewLine + upload.GetUploadUrl("http://scorm.com"));
        }

        public void EnsureTestFile()
        {
            try
            {
                if (upload == null)
                {
                    fixtureSetup();
                    caseSetup();
                }

                bool found = false;
                List<FileData> files = upload.GetFileList(domain);
                foreach (FileData file in files)
                {
                    if (file.Name == courseFile)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    UploadResult ur = upload.UploadFile(Path.Combine(courseFilePath,courseFile),domain);
                }
            }
            catch (InvalidProgramException ex)
            {
                Assert.Ignore("Prerequisite failure: error ensuring test file was present on server. " + Environment.NewLine + ex.ToString());
            }
        }
    }

    [TestFixture]
    public class Registration : clienttests
    {
        private RegistrationService reg;


        public override void caseSetup()
        {
            base.caseSetup();
            reg = new RegistrationService(cfg, svc);
        }

        [Test]
        public void createDelete()
        {
            try
            {
                reg.CreateRegistration(regId, tstCourse, 0, tstLearner, fName, lName, postback, authType, pbLogin, pbPass, resultFmt);
            }
            catch (ServiceException se)
            {
                if (se.Message == testCourseMissingMsg)
                {
                    Assert.Ignore("test course does not exist on the server");
                }
                else throw;
            }

            Assert.AreEqual(1, reg.GetRegistrationList(regId, null).Count, "Registration ID " + regId + " just created not found on server." );

            reg.ResetGlobalObjectives(regId, false);
            reg.ResetRegistration(regId);
            reg.DeleteRegistration(regId, false);
        }

        [Test]
        public void List()
        {
            foreach (RegistrationData rd in reg.GetRegistrationList())
            {
                Console.WriteLine(rd.CourseId + " : " + rd.RegistrationId);
            }
        }


        [Test]
        public void Info()
        {
            foreach (RegistrationData rd in reg.GetRegistrationList(regId2, tstCourse))
            {
                Console.WriteLine(rd.CourseId + " : " + rd.RegistrationId);
            }

            Console.WriteLine(reg.GetRegistrationSummary(regId2).Complete);
            Console.WriteLine(reg.GetRegistrationResult(regId2, RegistrationResultsFormat.FULL, DataFormat.JSON));
        }

        [Test]
        public void History()
        {
            string launchID = "";
            foreach (LaunchInfo li in reg.GetLaunchHistory(regId2))
            {
                Console.WriteLine(string.Format("{0} : {1}", li.Id, li.LaunchTime));
                Console.WriteLine(li.Log);
                launchID = li.Id;
            }

            if (!String.IsNullOrEmpty(launchID))
            {
                LaunchInfo li = reg.GetLaunchInfo(launchID);
                Console.WriteLine(li.Log);
            }
            else
            {
                Assert.Ignore("no history");
            }
        }

        [Test]
        public void LaunchExistingRegistration()
        {
            List<RegistrationData> regs = reg.GetRegistrationList();
            if (regs.Count < 1)
            {
                Assert.Ignore("No existing registration to launch");
            }
            string url = reg.GetLaunchUrl(regs[0].RegistrationId, "closer.html");
            Process browser = new Process();
            Console.WriteLine(url);
            browser.StartInfo = new ProcessStartInfo("firefox", " -new-window " + url);
            browser.Start();
            if (!browser.WaitForExit(1000 * 60 * 1))
            {
                Assert.Inconclusive("Browser did not close");
            }
            
        }

        [Test]
        public void uploadAndShowLaunchLink()
        {
            CourseService pkg = new CourseService(cfg, svc);

            List<ImportResult> results = pkg.ImportCourse(courseId, upldFile,null,domain);
            Assert.AreEqual(1, results.Count, "Expected 1 import result");
            Assert.IsTrue(results[0].WasSuccessful, results[0].Message);

            reg.CreateRegistration(regId, courseId, 0, tstLearner, fName, lName, postback, authType, pbLogin, pbPass, resultFmt);
            string url = reg.GetLaunchUrl(regId, @"closer.htm");
            showResponse(url);
        }
        private void showResponse(String url)
        {
            HttpWebRequest wreq = (HttpWebRequest)HttpWebRequest.Create(url);
            //wreq.UserAgent = @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";

            try
            {
                using (WebResponse response = wreq.GetResponse())
                {
                    showResponse(response);
                }
            }
            catch (WebException we)
            {
                showResponse(we.Response);
                ((IDisposable)we.Response).Dispose();
                throw;
            }
        }

        private void showResponse(WebResponse response)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in response.Headers.AllKeys)
            {
                sb.AppendLine(key + ": " + response.Headers[key]);
            }

            sb.AppendLine("length: " + response.ContentLength);
            if (response.ContentLength >= 0)
            {
                byte[] content = new byte[response.ContentLength];
                response.GetResponseStream().Read(content, 0, (int)response.ContentLength);

                sb.AppendLine(Encoding.UTF8.GetString(content));
            }

            Console.WriteLine(sb);
        }

    }


    [TestFixture]
    public class Course : clienttests
    {
        CourseService pkg;

        public override void fixtureSetup()
        {
            base.fixtureSetup();
            new Upload().EnsureTestFile();
        }

        public override void caseSetup()
        {
            base.caseSetup();
            pkg = new CourseService(cfg, svc);
        }

        [Test]
        public void CourseList()
        {
            List<CourseData> courseList = pkg.GetCourseList();
            foreach (CourseData course in courseList)
            {
                Console.WriteLine(course.CourseId + " - " + course.Title);
            }
        }

        [Test]
        public void CourseListFilter()
        {
            List<CourseData> courseList = pkg.GetCourseList();
            if (courseList.Count < 2)
            {
                Assert.Ignore("Can't test filtering, only " + courseList.Count + " courses.");
            }

            string courseId = courseList[0].CourseId;
            courseList = pkg.GetCourseList(courseId);
            Assert.AreEqual(1, courseList.Count);
            Assert.AreEqual(courseId, courseList[0].CourseId);
        }



        [Test]
        public void import_delete()
        {

            pkg.ImportUploadedCourse(courseId, courseServerPath);
            pkg.VersionUploadedCourse(courseId, courseServerPath, null);
            pkg.VersionUploadedCourse(courseId, courseServerPath, null);

            RegistrationService reg = new RegistrationService(cfg, svc);
            reg.CreateRegistration(regId, courseId, 0, tstLearner, fName, lName, postback, authType, pbLogin, pbPass, resultFmt);

            pkg.DeleteCourse(courseId, true);
            pkg.DeleteCourseVersion(courseId, 1);
            pkg.DeleteCourse(courseId);
        }


        [Test]
        public void import_async()
        {
            AsyncImportResult res = null;
            string token = pkg.ImportUploadedCourseAsync(courseId, courseServerPath, "", "wherever");
            while (res == null || res.Status == AsyncImportResult.ImportStatus.RUNNING)
            {
                Thread.Sleep(1000);
                res = pkg.GetAsyncImportResult(token);
                Console.WriteLine(res.Status);
            }
        }

        [Test]
        public void import_asset_ops()
        {
            pkg.ImportUploadedCourse(courseId, courseServerPath);
            pkg.GetAttributes(courseId);
            pkg.GetFileStructure(courseId);
            pkg.GetPreviewUrl(courseId);
            pkg.GetPropertyEditorUrl(courseId);
            pkg.GetAssets(@"test.zip", courseId);
        }


        [Test]
        public void import_asset_structure()
        {
            pkg.ImportUploadedCourse(courseId, courseServerPath);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pkg.GetFileStructure(courseId));
            string topdir = doc.DocumentElement.GetAttribute("name");
            foreach (XmlElement file in doc.SelectNodes("/dir/file"))
            {
                string path = topdir + @"\" + file.GetAttribute("name");
                Console.WriteLine(path);
                Console.WriteLine(pkg.GetAssetUrl(courseId, path));
            }
        }



        [Test]
        public void courseMetaData()
        {
            pkg.ImportUploadedCourse(courseId, courseServerPath);
            foreach (MetadataScope scope in Enum.GetValues(typeof(MetadataScope)))
            {
                foreach (MetadataFormat format in Enum.GetValues(typeof(MetadataFormat)))
                {
                    Console.WriteLine(scope.ToString() + " - " + format.ToString());
                    Console.WriteLine(pkg.GetMetadata(courseId, scope, format) + Environment.NewLine);
                }
            }
        }


        [TearDown]
        public void teardown()
        {
            try
            {
                pkg.DeleteCourse(courseId);
            }
            catch { }
        }
    }

    public class Reporting : clienttests
    {
        [Test]
        public void getAccountInfo()
        {
            ServiceRequest req = new ServiceRequest(cfg);
            Console.WriteLine(req.CallService("rustici.reporting.getAccountInfo").OuterXml);
        }

        /*[Test]
        public void getUsageInfo()
        {
            ServiceRequest req = new ServiceRequest(cfg);
            Console.WriteLine(req.CallService("rustici.reporting.getUsageInfo").OuterXml);
        }*/


        [Test]
        public void launch()
        {
            ServiceRequest req = new ServiceRequest(cfg);
            req.Parameters.Add("auth", "no auth");
            req.Parameters.Add("reporturl", "http://scorm.com");
            string url = req.ConstructUrl("rustici.reporting.launchReport");

            WebRequest wr = HttpWebRequest.Create(url);
            ((HttpWebRequest)wr).AllowAutoRedirect = false;
            WebResponse rsp = wr.GetResponse();

            Console.WriteLine(((HttpWebResponse)rsp).StatusCode);
            Console.WriteLine(((HttpWebResponse)rsp).StatusDescription);

            Console.WriteLine("Header: ");
            foreach (string key in ((HttpWebResponse)rsp).Headers.AllKeys)
            {
                Console.WriteLine(string.Format("{0} : {1}", key, rsp.Headers[key]));
            }
            Console.WriteLine("end of header");

            Console.WriteLine(rsp.ResponseUri);


            Console.WriteLine(new StreamReader(rsp.GetResponseStream()).ReadToEnd());
        }

        [Test]
        public void auth()
        {
            ServiceRequest req = new ServiceRequest(cfg);
            req.Parameters.Add("navpermission", "NONAV");
            Console.WriteLine(req.CallService("rustici.reporting.getReportageAuth").OuterXml);
        }

    }

    public class Tagging : clienttests
    {
        private const string testTag = "testTag";
        private const string testTag2 = "second test tag";


        [Test]
        public void course()
        {
            {
                ServiceRequest req = svc.CreateNewRequest();
                req.Parameters.Add("courseid", tstCourse);
                req.Parameters.Add("tag", testTag);
                Console.WriteLine("Add: " + req.CallService("rustici.tagging.addCourseTag").OuterXml);
            }

            {
                ServiceRequest req = svc.CreateNewRequest();
                req.Parameters.Add("courseid", tstCourse);
                req.Parameters.Add("tag", testTag);
                Console.WriteLine("Remove: " + req.CallService("rustici.tagging.removeCourseTag").OuterXml);
            }
        }

        [Test]
        public void learner()
        {
            addLearnerTag(tstLearner, testTag,0);
            addLearnerTag(tstLearner, testTag2, 1);
            removeLearnerTag(tstLearner, testTag, 2);
            removeLearnerTag(tstLearner, testTag2, 1);
        }

        private void addLearnerTag(string learnerId, string tag, int existingTagCount)
        {
            IList<string> learnerTags;
            Assert.AreEqual(existingTagCount, getLearnerTags(tstLearner).Count, "expected tag count");

            addLearnerTag(learnerId, tag);
            learnerTags = getLearnerTags(learnerId);
            Assert.AreEqual(existingTagCount + 1, learnerTags.Count, "expected tag count after adding tag");
            Assert.Contains(tag, (ICollection)learnerTags, "missing added tag");

            addLearnerTag(learnerId, tag);
            learnerTags = getLearnerTags(learnerId);
            Assert.AreEqual(existingTagCount + 1, learnerTags.Count, "expected tag count after adding tag (dup)");
            Assert.Contains(tag, (ICollection)learnerTags, "missing added tag");
        }

        private void removeLearnerTag(string learnerId, string tag, int existingTagCount)
        {
            IList<string> learnerTags;
            Assert.AreEqual(existingTagCount, getLearnerTags(tstLearner).Count, "expected tag count");

            removeLearnerTag(learnerId, tag);
            learnerTags = getLearnerTags(learnerId);
            Assert.AreEqual(existingTagCount - 1, learnerTags.Count, "expected tag count after removing tag");
            Assert.IsFalse(learnerTags.Contains(tag), "tag not removed");

            removeLearnerTag(learnerId, tag);
            learnerTags = getLearnerTags(learnerId);
            Assert.AreEqual(existingTagCount - 1, learnerTags.Count, "expected tag count after removing tag (dup)");
            Assert.IsFalse(learnerTags.Contains(tag), "tag not removed");
        }


        private void addLearnerTag(string learnerId, string tag)
        {
            ServiceRequest req = svc.CreateNewRequest();
            req.Parameters.Add("learnerid", learnerId);
            req.Parameters.Add("tag", tag);
            XmlDocument rsp =req.CallService("rustici.tagging.addLearnerTag");
            Assert.IsNotNull(rsp.SelectSingleNode(@"/rsp/success"),rsp.OuterXml);
        }

        private void removeLearnerTag(string learnerId, string tag)
        {
            ServiceRequest req = svc.CreateNewRequest();
            req.Parameters.Add("learnerid", learnerId);
            req.Parameters.Add("tag", tag);
            XmlDocument rsp = req.CallService("rustici.tagging.removeLearnerTag");
            Assert.IsNotNull(rsp.SelectSingleNode(@"/rsp/success"), rsp.OuterXml);
        }


        private IList<string> getLearnerTags(string learnerId)
        {
            IList<string> tags = new List<string>();

            ServiceRequest req = svc.CreateNewRequest();
            req.Parameters.Add("learnerid", learnerId);
            XmlDocument rsp = req.CallService("rustici.tagging.getLearnerTags");
            XmlNode responseNode = rsp.SelectSingleNode(@"/rsp[@stat='ok']");
            Assert.IsNotNull(responseNode, rsp.OuterXml);
            XmlNodeList tagNodes = responseNode.SelectNodes(@"tags/tag");

            foreach (XmlNode tagNode in tagNodes)
            {
                tags.Add(tagNode.InnerText);
            }

            return tags;
        }
    
    }

}
