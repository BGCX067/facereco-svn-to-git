using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using FaceLight.Core;
using System.IO;
using ImageHash;

namespace Compare
{
    class Program
    {
        //C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\Profile\WindowsPhone71\System.Windows.dll
        //private static FaceDetector _faceDetector = new FaceDetector();
        static void Main(string[] args)
        {
            try
            {

                string fileName = args[0];
                string httpBasePath = args[1];
                //Console.WriteLine(fileName);
                //Console.WriteLine(httpBasePath);
            //string fileName = @"634753467709034943.jpg";
            //string httpBasePath = @"C:\test\WP7\Face\PhotoService\";

            string path = httpBasePath + @"\" + fileName;
            string corpedPath = httpBasePath + "corped" + fileName;

            //BitmapImage bmpImage = new BitmapImage();
            //bmpImage.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            //WriteableBitmap face = _faceDetector.Corp(new WriteableBitmap(bmpImage));
            //FileStream corped = new FileStream(corpedPath, FileMode.Create);

            //BinaryWriter corpedWriter = null;
            //byte[] bytes = new byte[corped.Length];
            //corpedWriter = new BinaryWriter(corped);
            //corpedWriter.Write(face.ToByteArray());
            //corped.Close();
            //corped.Dispose();
            string foundLogin = string.Empty;
            ulong targetSimilarity = 95;
            foreach (string item in Directory.GetFiles(httpBasePath + @"\pic","*.jpg"))
            {
                //Console.WriteLine(path);
                //Console.WriteLine(item);
                if (Utility.IsSimilar(path, item, targetSimilarity))
                {
                    //Console.WriteLine("Found" + item);
                    string foundJpg = item.Substring(item.LastIndexOf(@"\"));
                    foundLogin = foundJpg.Substring(1, foundJpg.LastIndexOf(".") - 1);
                    break;
                }
            }

                Console.WriteLine(foundLogin);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
