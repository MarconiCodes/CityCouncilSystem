using System;
using System.Collections.Generic;
using System.Text;
using CouncilApp.Models.Events;

namespace CouncilApp.Services.Interfaces
{
    public interface IFileService
    {
        string StorageDirectory { get; }
        void DownloadFile(string url, string folder);
        void OpenFile(string fileName);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
    }
}
