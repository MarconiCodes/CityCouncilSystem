using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CouncilApp.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CouncilApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfJsPage : ContentPage
    {
        public PdfJsPage(string url)
        {
            InitializeComponent();

            PdfJsWebView.Uri = url;
        }
    }
}