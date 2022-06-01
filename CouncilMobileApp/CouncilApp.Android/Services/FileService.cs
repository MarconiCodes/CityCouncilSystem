using CouncilApp.Services.Interfaces;
using CouncilApp.Models.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Plugin.XamarinFormsSaveOpenPDFPackage;

[assembly: Dependency(typeof(CouncilApp.Droid.Services.FileService))]
namespace CouncilApp.Droid.Services
{
    public class FileService : IFileService
    {
        public string StorageDirectory => Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
        public static string AndroidStotageDirectory = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public void DownloadFile(string url, string folder)
        {
            string pathToNewFolder = Path.Combine(StorageDirectory, folder);
            Directory.CreateDirectory(pathToNewFolder);
            try
            {
                WebClient webClient = new WebClient();

                //
                //This is for development purposes only since during development we didn't have a live-server
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback
                (
                    delegate { return true; }
                );
                //

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DonwloadCompleted);
                string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                //webClient.DownloadFileAsync(new UriBuilder(url).Uri, pathToNewFile);
                webClient.DownloadFile(url, pathToNewFile);
            }
            catch (Exception exc)
            {
                File.AppendAllText($"{pathToNewFolder}error.txt", exc.Message);
                if(OnFileDownloaded != null)
                {
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
                }
            }
        }

        private void DonwloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                if (OnFileDownloaded != null)
                {
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
                }
            }
            else
            {
                if (OnFileDownloaded != null)
                {
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
                }
            }
        }

        public async void OpenFile(string fileName)
        {
            var stream = File.Open(Path.Combine(StorageDirectory, "PdfFiles", fileName), FileMode.Open, FileAccess.Read, FileShare.Read);
            File.Delete(Path.Combine(StorageDirectory, "PdfFiles", fileName));

            using(var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(Path.Combine(StorageDirectory, "ReadingFiles", fileName), "application/pdf", memoryStream, PDFOpenContext.InApp);
            }
        }
        
    }
}
