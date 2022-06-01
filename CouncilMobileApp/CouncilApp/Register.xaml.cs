using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CouncilApp.Models;
using CouncilApp.Services.Core;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace CouncilApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        private string accountNumber, firstName, lastName, phone, email, password, path;
        private bool isPassConfirmed = false;
        private PreAccount pAccount;
        private Account account;
        private User user;

        public Register()
        {
            InitializeComponent();
        }

        private async void BtnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void RegisterBtnClicked(object sender, EventArgs e)
        {
            if(isPassConfirmed)
            {
                accountNumber = AccountNumberEntry.Text;
                firstName = FirstNameEntry.Text;
                lastName = LastNameEntry.Text;
                phone = PhonenumberEntry.Text;
                email = EmailEntry.Text;
                password = PasswordEntry.Text;

                path = $"{accountNumber}/{firstName}/{lastName}/{email}/{phone}/{password}";
                if(APIService.CheckInternetConnection())
                {
                    UserDialogs.Instance.ShowLoading("Registering...");
                    pAccount = await APIService.GetAccountAsync(path);
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    await DisplayAlert("No Internet Connection", "Your need to be connected to the internet to register!", "OK");
                    return;
                }
                account = new Account();
                user = new User();

                if (pAccount != null)
                {
                    account.AccountID = pAccount.AccountID;
                    account.AccountNumber = pAccount.AccountNumber;
                    account.CurrentBalance = pAccount.CurrentBalance;

                    user.AccountID = pAccount.User.AccountID;
                    user.FirstName = pAccount.User.FirstName;
                    user.LastName = pAccount.User.LastName;
                    user.Address = pAccount.User.Address;
                    user.Email = pAccount.User.Email;
                    user.Phone = pAccount.User.Phone;
                    user.Sex = pAccount.User.Sex;
                    user.IsRegistered = pAccount.User.IsRegistered;
                    user.ConnectionID = pAccount.User.ConnectionID;
                    user.Password = pAccount.User.Password;


                    DBOperator.ResetDB();
                    DBOperator.DatabasePath = account.AccountNumber;
                    await DBOperator.AddAccount(account);
                    await DBOperator.AddUser(user);

                    await DisplayAlert("Registration Successful", "You have been registered successfully. Return to Login to access your account.", "OK");
                    
                }

                else
                {
                    await DisplayAlert("Registration failed", "Registration failed. Either the details given didn't match any house hold account or something else went wrong.", "OK");
                }
            }

            else
            {
                lblPasswordWarning.IsVisible = true;
                isPassConfirmed = false;
            }
        }

        private void ConfirmPasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(PasswordEntry.Text == ConfirmPasswordEntry.Text)
            {
                lblPasswordWarning.IsVisible = false;
                isPassConfirmed = true;
            }
            else
            {
                lblPasswordWarning.IsVisible = true;
                isPassConfirmed = false;
            }
        }
    }

    public class PreAccount
    {

        public int AccountID { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }


        //// foreign key relationships with other tables
        public ICollection<Message> Messages { get; set; }
        public ICollection<Statement> Statements { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public virtual User User { get; set; }

        public PreAccount()
        {
            this.Messages = new List<Message>();
            this.Statements = new List<Statement>();
            this.Payments = new List<Payment>();
        }
    }
}