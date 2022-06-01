using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CouncilApp.Services.Core;
using CouncilApp.Services.Interfaces;
using CouncilApp.Models;
using Acr.UserDialogs;
using System.Windows.Input;
using Prism.Commands;

namespace CouncilApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        ChatService chatService;
        TabbedPageViewModel pageViewModel;

        private ObservableCollection<Message> Messages;
        private ObservableCollection<Payment> Receipts;
        private ObservableCollection<Statement> Statements;
        private System.Timers.Timer timer;
        private System.Timers.Timer downloadTimer;
        private ICommand RefreshCommand;
        
        public TabbedPage1(TabbedPageViewModel viewModel)
        {
            InitializeComponent();
            
            pageViewModel = viewModel;
            RefreshCommand = new Command(() =>
            {
                UpdateBalance();
                ThaRefreshView.IsRefreshing = false;
            });
            ThaRefreshView.Command = RefreshCommand;
            

            lblAccountBalance.BindingContext = pageViewModel.Balance;
            if (pageViewModel.User.Sex == "M")
            {
                lblName.Text = $"Mr {pageViewModel.User.FirstName} {pageViewModel.User.LastName}";
            }
            else if (pageViewModel.User.Sex == "F")
            {
                lblName.Text = $"Mrs {pageViewModel.User.FirstName} {pageViewModel.User.LastName}";
            }
            lblAccountNumber.Text = pageViewModel.Account.AccountNumber;
            lblAccountBalance.Text = $"ZWL {pageViewModel.Account.CurrentBalance}";
            ThaListView.ItemsSource = Messages;



            Messages = pageViewModel.Messages;
            ThaListView.ItemsSource = Messages;

            Receipts = pageViewModel.Receipts;
            LVhistory.ItemsSource = Receipts;

            Statements = pageViewModel.Statements;
            BListView.ItemsSource = Statements;

            if (APIService.CheckInternetConnection())
            {
                if (APIService.pingServer())
                {
                    chatService = new ChatService(pageViewModel.Account);
                    Initialize();
                }
                else
                {
                    DisplayAlert("Connection to Server failed", "The City Council Server appears to be down at the moment.", "OK");
                }
            }
            else
            {
                DisplayAlert("No Internet connection", "Connection to server failed! Please make sure you're connected to the internet for real-time feed.", "OK");
            }

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += UpdateStaff;
            ///downloadTimer = new System.Timers.Timer(500);
            //downloadTimer.Elapsed += CheckDownloadStatus;

            ScrollToLastMessage();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image image = (Image)sender;
            image.Opacity = .5;
            await Navigation.PushAsync(new PaynowPayment(pageViewModel.Account));
            image.Opacity = 1;
        }

        protected void LstViewMessageItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        protected void LstViewStatementItemTapped(object sender, ItemTappedEventArgs e)
        {
            Statement statement = (Statement)e.Item;
            string fileName = statement.FileName;  // "Database-Design-Concepts-Outline.pdf";
            string filePath = Path.Combine("/storage/emulated/0/Android/data/com.companyname.councilapp/files/PdfFiles", fileName);

            if (!statement.IsNotDownloaded && File.Exists(filePath))
            {
                Navigation.PushAsync(new PdfJsPage(filePath));
            }
            ((ListView)sender).SelectedItem = null;
        }

        protected void LstViewReceiptItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        protected async void SendButtonClicked(object sender, EventArgs e)
        {
            var message = new Message { Content = MessageEntry.Text, DateSent = DateTime.Now.ToShortTimeString(), FromUser = false, Sender = "You" };
            Messages.Add(message);
            ThaListView.ItemsSource = Messages;
            MessageEntry.Text = string.Empty;
            ScrollToLastMessage();

            await DBOperator.AddMessage(message);
            if(chatService != null)
            {
                await chatService.SendMessageToCouncil(pageViewModel.Account.AccountNumber, message.Content);
            }
            else
            {
                await DisplayAlert("Message not sent!", "Message not sent because server is unreachable. Make sure you're connected to the internet.", "OK");
            }
        }

        protected async void BtnDownloadFileClicked(object sender, EventArgs e)
        {
            ThreadStart childref = new ThreadStart(ShowLoading);
            Thread childThread = new Thread(childref);
            childThread.Start();

            //int? id = ((Button)sender).BindingContext as int?;
            Statement statement = (Statement)((Button)sender).BindingContext;
            //Statement statement = await DBOperator.GetStatementAsync(id);

            if (APIService.pingServer())
            {
                Button downloadBtn = (Button)sender;
                pageViewModel.DownloadPdfFile(statement.FileUrl);

                pageViewModel.isFileDownloaded = false;
                statement.IsNotDownloaded = false;
                await DBOperator.UpdateStatement(statement);
                downloadBtn.IsVisible = false;

                childThread.Abort();
                HideLoading();
            }
            else
            {
                childThread.Abort();
                HideLoading();
                await DisplayAlert("Server unreachable", "Can't donwload file because City Council server is currently unreachable.", "OK");
            }
        }

        //protected void CheckDownloadStatus(object sender, EventArgs e)
        //{
        //    if(pageViewModel.isFileDownloaded)
        //    {
        //        downloadTimer.Stop();
        //        downloadBtn.IsVisible = false;
        //        pageViewModel.isFileDownloaded = false;
        //    }
        //}

        private async void GetMessage(string userName, string message)
        {
            var mssg = new Message { Sender = userName, Content = message, DateSent = DateTime.Now.ToShortTimeString(), FromUser = false };
            await DBOperator.AddMessage(mssg);
            GetMessages();
            ScrollToLastMessage();
        }

        private async void GetStatement(string fileName, string fileUrl, string month)
        {
            var statement = new Statement { FileName = fileName, FileUrl = fileUrl, Month = month, IsNotDownloaded = true, DateSent = DateTime.Now.ToLongDateString(), Timesent = DateTime.Now.ToShortTimeString(), Published = true, AccountID = pageViewModel.Account.AccountID };
            await DBOperator.AddStatement(statement);
            GetStatements();
            ScrollToLastBill();
        }

        private void UpdateStaff(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                timer.Stop();
                GetReceipts();
                ScrollToLastReceipt();
            });
        }

        private void UpdateUI(object sender, EventArgs e)
        {
            timer.Start();
        }

        private async void UpdateBalance()
        {
            pageViewModel.Account.CurrentBalance = await DBOperator.GetBalanceAsync();
            lblAccountBalance.Text = $"ZWL {pageViewModel.Account.CurrentBalance}";
        }

        public async void Initialize()
        {
            try
            {
                chatService.ReceiveMessage(GetMessage);
                chatService.ReceiveBill(GetStatement);
                //chatService.UpdateReceipts(UpdateStaff);
            }
            catch (Exception exc)
            {
                await DisplayAlert("Exception", exc.Message, "OK");
            }
        }

        private void ScrollToLastMessage()
        {
            if (Messages.Count > 0)
            {
                ThaListView.ScrollTo(Messages[Messages.Count - 1], ScrollToPosition.End, true);
            }
        }

        private void ScrollToLastBill()
        {
            if(Statements.Count > 4)
            {
                BListView.ScrollTo(Statements[Statements.Count - 1], ScrollToPosition.End, true);
            }
        }

        private void ScrollToLastReceipt()
        {
            if(Receipts.Count > 3)
            {
                LVhistory.ScrollTo(Receipts[Receipts.Count - 1], ScrollToPosition.End, true);
            }
        }

        private async void GetMessages()
        {
            var msgList = await DBOperator.GetMessagesAsync();
            Messages.Clear();
            foreach (var msg in msgList)
            {
                Messages.Add(msg);
            }
            ThaListView.ItemsSource = Messages;
        }

        private async void GetReceipts()
        {
            var rcptList = await DBOperator.GetPaymentsAsync();
            Receipts.Clear();
            foreach (var receipt in rcptList)
            {
                Receipts.Add(receipt);
            }
            LVhistory.ItemsSource = Receipts;
        }

        private async void GetStatements()
        {
            var statementList = await DBOperator.GetStatementsAsync();
            Statements.Clear();
            foreach (var statement in statementList)
            {
                Statements.Add(statement);
            }
            BListView.ItemsSource = Statements;
        }

        private void ShowLoading()
        {
            UserDialogs.Instance.ShowLoading("Donwloading file..");
        }

        private void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }

    }

    public class TabbedPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        decimal balance;
        private readonly IFileService fileService;
        public ICommand DownloadPdfFileCommand { get; private set; }
        public bool isFileDownloaded { get; set; } = false;
        public User User { get; set; }
        public Account Account { get; set; }
        public ObservableCollection<Statement> Statements { get; set; }
        public ObservableCollection<Payment> Receipts { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        public TabbedPageViewModel()
        {
            DownloadPdfFileCommand = new DelegateCommand<object>(DownloadPdfFile);
            fileService = DependencyService.Get<IFileService>();
        }

        public void DownloadPdfFile(object parameter)
        {
            string fileUrl = (string)parameter;
            fileService.DownloadFile(fileUrl, "PdfFiles"); //www.africau.edu/images/default/sample.pdf
            fileService.OnFileDownloaded += FileService_OnFileDownloaded;
        }

        public async void OpenPdf(string fileName)
        {
            fileService.OpenFile(fileName);
        }

        private void FileService_OnFileDownloaded(object sender, Models.Events.DownloadEventArgs e)
        {
            isFileDownloaded = true;
        }

        public decimal Balance
        {
            set
            {
                if (!value.Equals(balance))
                {
                    balance = value;
                    OnPropertyChanged("Balance");
                }
            }
            get
            {
                return balance;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}