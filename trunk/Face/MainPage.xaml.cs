using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Controls;
using System;
using System.Windows.Media;
using System.Windows;
using System.ServiceModel;
using System.Net;
using Face.ServiceReference1;
using FaceLight.Core;
using System.Windows.Controls;
namespace Face
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Declare the CameraCaptureTask object with page scope.
        CameraCaptureTask cameraCaptureTask;
        PhotoChooserTask photoChooserTask;
        BitmapImage bmp;

        private Service1Client client = null;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Initialize the Chooser objects and assign the handlers for their "Completed" events
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(photoCaptureOrSelectionCompleted);
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoCaptureOrSelectionCompleted);
        }


        void photoCaptureOrSelectionCompleted(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                myImage.Source = bmp;
                myImage.Stretch = Stretch.Uniform;
                // swap UI element states
                savePhotoButton.IsEnabled = true;
                statusText.Text = "";
            }
            else
            {
                savePhotoButton.IsEnabled = false;
                statusText.Text = "Task Result Error: " + e.TaskResult.ToString();
            }
        }

        // The camera Chooser is shown in response to a button click.
        private void takePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            cameraCaptureTask.Show();
        }


        // The photo Chooser shown in response to a button click.
        private void choosePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }

        private void savePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a filename for JPEG file in isolated storage.
                String tempJPEG = "TempJPEG";
                // Create virtual store and file stream. Check for duplicate tempJPEG files.
                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (myStore.FileExists(tempJPEG))
                {
                    myStore.DeleteFile(tempJPEG);
                }
                IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);
                // Create a stream out of the sample JPEG file.
                // Instead of MyQuickApp in the URI, use the correct project name.
                // The use of TempJPEG was established earlier.
                Uri uri = new Uri("MyQuickApp;component/TempJPEG.jpg", UriKind.Relative);
                // Create a new WriteableBitmap object and set it to the JPEG stream.
                WriteableBitmap wb = new WriteableBitmap(bmp);
                // Encode WriteableBitmap object to a JPEG stream.
                // SaveJpeg(WriteableBitmap bitmap, Stream targetStream, int targetWidth,
                // int targetHeight, int orientation, int quality)
                Extensions.SaveJpeg(wb, myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                myFileStream.Close();
                // Create a new stream from isolated storage, and save the JPEG file to
                // the media library on Windows Phone.
                myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);
                MediaLibrary library = new MediaLibrary();
                Picture pic = library.SavePicture("SavedPicture.jpg", myFileStream);
                myFileStream.Close();
                savePhotoButton.IsEnabled = false;
                statusText.Text = "Saved!";
            }
            catch (Exception myError)
            {
                statusText.Text = myError.Message;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
                basicHttpBinding.MaxReceivedMessageSize = 2147483647;
                basicHttpBinding.MaxBufferSize = 2147483647;
                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
                EndpointAddress endpointAddress = new EndpointAddress("http://ws-bbai.websense.com:8096/Service1.svc");
                //EndpointAddress endpointAddress = new EndpointAddress("http://localhost:65360/Service1.svc");
                client = new Service1Client(basicHttpBinding, endpointAddress);

                //this.client.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(client_GetDataCompleted);

                this.client.FileUploadCompleted += new EventHandler<FileUploadCompletedEventArgs>(client_FileUploadCompleted);
            }

            //client.GetDataAsync(1);
            string imgName = DateTime.Now.Ticks.ToString();
            byte[] bytearray = null;
            MemoryStream ms = new MemoryStream();

            //WriteableBitmap btmMap = new WriteableBitmap
            //    (bmp.PixelWidth, bmp.PixelHeight);
            WriteableBitmap btmMap = new WriteableBitmap(bmp);
            // write an image into the stream
            Extensions.SaveJpeg(btmMap, ms,
                bmp.PixelWidth, bmp.PixelHeight, 0, 100);
            bytearray = ms.ToArray();


            client.FileUploadAsync(bytearray);

        }

        void client_FileUploadCompleted(object sender, FileUploadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.textBox1.Text = e.Result.ToString();
                this.gInfo.Visibility = System.Windows.Visibility.Visible;
                this.gPhoto.Visibility = System.Windows.Visibility.Collapsed;
                this.txtInfo.Text = "You are: \n" + e.Result;
            }
            else
            {
                this.textBox1.Text = e.Error.Message;
            }
        }

        void client_GetDataCompleted(object sender, GetDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.textBox1.Text = e.Result;
            }
            else
            {
                this.textBox1.Text = "failed";
            }
        }
    }


}