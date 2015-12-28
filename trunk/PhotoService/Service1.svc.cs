using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.ServiceModel.Activation;
using System.Web;
using FaceLight.Core;
using System.Windows.Media.Imaging;
using ImageHash;
using System.Diagnostics;
namespace PhotoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.

    [DataContract(IsReference = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults=true)]
    public class Service1 : IService1
    {
        private FaceDetector _faceDetector = new FaceDetector();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string FileUpload(byte[] fileStream)
        {
            try
            {
                string fileName = DateTime.Now.Ticks.ToString() + ".jpg";
                string path = HttpContext.Current.Server.MapPath(".") + "\\" + fileName;
                FileStream fileToupload = new FileStream(path, FileMode.Create);
                BinaryWriter writer = null;
                byte[] bytearray = new byte[fileStream.Length];
                writer = new BinaryWriter(fileToupload);
                writer.Write(fileStream);
                fileToupload.Close();
                fileToupload.Dispose();

                string foundLogin = RunShellCmd(HttpContext.Current.Server.MapPath(".") + "\\Bin\\" + "Compare.exe", fileName + " " + HttpContext.Current.Server.MapPath("."));

                return foundLogin;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string RunShellCmd(string fileName, string cmdArgs)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = cmdArgs;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            string cmdStdText = process.StandardOutput.ReadToEnd();
            string cmdErrText = process.StandardError.ReadToEnd();

            process.WaitForExit(30000);  // Use timeout, just in case.

            return string.IsNullOrEmpty(cmdErrText) ? cmdStdText : cmdErrText;
        }
    }
}
