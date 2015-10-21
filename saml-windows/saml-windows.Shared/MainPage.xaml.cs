using FHSDK.Config;
using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FHSDK;
using static FHSDKPortable.FHClient;

namespace saml_windows
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            InitApp();
            webView.LoadCompleted += LoadCompleted;
        }

        private async void InitApp()
        {
            await Init();
            progress.Visibility = Visibility.Collapsed;
            app.Visibility = Visibility.Visible;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var response = await FH.GetCloudRequest("sso/session/login_host", "POST", null, GetRequestParams()).ExecAsync();

            var resData = response.GetResponseAsJObject();
            var sso = (string)resData["sso"];
            if (!string.IsNullOrEmpty(sso))
            {
                webView.Visibility = Visibility.Visible;
                webView.Navigate(new Uri(sso));

            }
        }

        void LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri.AbsolutePath.Contains("login/ok"))
            {
                ShowSignedIn();
            }
        }

        private async void ShowSignedIn()
        {
            signedIn.Visibility = Visibility.Visible;
            signIn.Visibility = Visibility.Collapsed;
            webView.Visibility = Visibility.Collapsed;

            var response = await FH.GetCloudRequest("sso/session/valid", "POST", null, GetRequestParams()).ExecAsync();
            if (response.Error == null)
            {
                var data = response.GetResponseAsDictionary();
                name.Text = $"{data["first_name"]} {data["last_name"]}";
                email.Text = (string) data["email"];
                expires.Text = ((DateTime) data["expires"]).ToString();
            }
            else
            {
                await new MessageDialog(response.Error.ToString()).ShowAsync();
            }
        }

        private static JObject GetRequestParams()
        {
            return new JObject { { "token", FHConfig.GetInstance().GetDeviceId() } };
        }
    }
}
