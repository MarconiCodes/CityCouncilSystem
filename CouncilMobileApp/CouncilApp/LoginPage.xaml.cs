using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using CouncilApp.Models;
using CouncilApp.Services.Core;
using Acr.UserDialogs;
using System.Collections.ObjectModel;

namespace CouncilApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private User user;
        private Account account;
        public TabbedPageViewModel viewModel;
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            lblWarning.IsVisible = false;

            if (File.Exists(Path.Combine(FileSystem.AppDataDirectory, $"{accountNumEntry.Text}.db")))
            {
                DBOperator.ResetDB();
                DBOperator.DatabasePath = accountNumEntry.Text;

                UserDialogs.Instance.ShowLoading("Logging in..");

                user = await DBOperator.GetUserAsync();
                account = await DBOperator.GetAccountAsync();

                if (user != null && account != null)
                {
                    if (accountNumEntry.Text == account.AccountNumber && passwordEntry.Text == user.Password)
                    {
                        ThaGrid.IsVisible = false;

                        List<Payment> paymentList = await DBOperator.GetPaymentsAsync();
                        List<Message> msgList = await DBOperator.GetMessagesAsync();
                        List<Statement> statmntList = await DBOperator.GetStatementsAsync();

                        ObservableCollection<Statement> Statements = new ObservableCollection<Statement>();
                        ObservableCollection<Payment> Receipts = new ObservableCollection<Payment>();
                        ObservableCollection<Message> Messages = new ObservableCollection<Message>();

                        foreach (var item in statmntList)
                        {
                            Statements.Add(item);
                        }

                        foreach (var item in paymentList)
                        {
                            Receipts.Add(item);
                        }

                        foreach (var item in msgList)
                        {
                            Messages.Add(item);
                        }

                        viewModel = new TabbedPageViewModel
                        {
                            Statements = Statements,
                            Receipts = Receipts,
                            Messages = Messages,
                            User = user,
                            Account = account,
                            Balance = account.CurrentBalance
                        };
                        await Navigation.PushAsync(new TabbedPage1(viewModel));

                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        lblWarning.Text = "Wrong account number or password.";
                        lblWarning.IsVisible = true;
                        UserDialogs.Instance.HideLoading();
                    }
                }

                else
                {
                    NotRegistered();
                    UserDialogs.Instance.HideLoading();
                }

            }

            else
            {
                NotRegistered();
            }
        }

        protected void OnAppearing(object sender, EventArgs e)
        {
            ThaGrid.IsVisible = true;
        }

        protected void RegisterBtnClicked(object sender, EventArgs e)
        {
            lblWarning.IsVisible = false;
            Navigation.PushAsync(new Register());
        }

        private void NotRegistered()
        {
            lblWarning.Text = "You're not registered yet! Please register.";
            lblWarning.IsVisible = true;
        }
    }
}