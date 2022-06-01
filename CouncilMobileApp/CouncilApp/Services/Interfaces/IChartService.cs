using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace CouncilApp.Services.Interfaces
{
    public interface IChartService : IDisposable
    {
        void Connect();
        Task Disconnect();
        Task sendMessage(string userid, string message);
        Task Register(string accountNumber);
        void ReceiveMessage(Action<string, string> GetMessageAndUser);
    }
}
