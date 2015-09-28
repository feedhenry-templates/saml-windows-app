using FHSDK.Config;
using FHSDKPortable;
using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            await FHClient.Init();
            progress.Visibility = Visibility.Collapsed;
            app.Visibility = Visibility.Visible;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var response = await FHClient.GetCloudRequest("sso/session/login_host", "POST", null, GetRequestParams()).ExecAsync();

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

            var response = await FHClient.GetCloudRequest("sso/session/valid", "POST", null, GetRequestParams()).ExecAsync();
            if (response.Error == null)
            {
                var data = response.GetResponseAsDictionary();
                name.Text = string.Format("{0} {1}", data["first_name"], data["last_name"]);
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
            var data = new JObject();
            data.Add("token", FHConfig.GetInstance().GetDeviceId());
            return data;
        }
    }
}
