using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CouncilApp.Models;
using CouncilApp.Services.Core;
using Acr.UserDialogs;

namespace CouncilApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaynowPayment : ContentPage
    {
        private Account account;
        public PaynowPayment(Account account)
        {
            InitializeComponent();
            this.account = account;

            lblAccountNumber.Text = account.AccountNumber;
            stackLtPaymentSuccess.IsVisible = false;
            lblPaymentFail.IsVisible = false;
            lblWarning.IsVisible = false;
        }

        protected async void PayButtonClicked(object sender, EventArgs e)
        {
            stackLtPaymentSuccess.IsVisible = false;
            lblPaymentFail.IsVisible = false;
            lblWarning.IsVisible = false;

            string accountNumber, email, method, phone, path;
            decimal amount = 0;
            accountNumber = account.AccountNumber;
            email = entryEmail.Text;
            method = GetMethod();
            phone = entryPhone.Text;
            try
            {
                amount = Convert.ToDecimal(entryAmount.Text);
                if (amount <= 0) { InvalidDataWarning(); return; }
            }
            catch (FormatException) { InvalidDataWarning(); return; }
            catch (Exception) { InvalidDataWarning(); return; }

            path = $"{accountNumber}/{email}/{method}/{phone}/{amount}";

            UserDialogs.Instance.ShowLoading("processing payment...");
            Payment receipt = null;
            
            if(APIService.CheckInternetConnection())
            {
                if (!APIService.pingServer())
                {
                    await DisplayAlert("Server Down.", "The Council server is currently down. Please try again later.", "OK");
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                receipt = await APIService.MakePaynowPaymentAsync(path);
            }
            else
            {
                await DisplayAlert("No Internet Connection", "Your device is not connected to the internet!", "OK");
            }

            UserDialogs.Instance.HideLoading();

            if(receipt != null)
            {
                lblPaymentFail.IsVisible = false;
                stackLtPaymentSuccess.IsVisible = true;

                await DBOperator.AddPaymnent(receipt);
                await DBOperator.UpdateBalance(receipt.RemainingBalance);
            }
            else
            {
                stackLtPaymentSuccess.IsVisible = false;
                lblPaymentFail.IsVisible = true;
            }
        }

        private string GetMethod()
        {
            string phone = entryPhone.Text;
            if(phone != null)
            {
                if (phone.StartsWith("077") || phone.StartsWith("078"))
                {
                    return "ecocash";
                }
                else if (phone.StartsWith("071"))
                {
                    return "onemoney";
                }
            }
            return null;
        }

        private void InvalidDataWarning()
        {
            lblWarning.Text = "Your details are invalid!";
            lblWarning.IsVisible = true;
        }
    }
}