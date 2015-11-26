[![Build status](https://ci.appveyor.com/api/projects/status/5c9qgnprl14bxkla?svg=true)](https://ci.appveyor.com/project/edewit/saml-windows-app)

# SAML Windows Client

## What does the demo do?

This is an example SAML Client App, designed to be used in conjunction with our [SAML Service](https://github.com/feedhenry-templates/saml-service). 

The Cloud App proxies the login client call to a SAML Service (e.g. fetching a URL to display in the WebView for IdP login) through the use of a SAML service (See Configuration section).

The client app demoes the usage of the two SAML Service endpoints:
- `sso/session/login_host` to get the SAML login page URL. A WebView is used to display the SAML login page.
- `sso/session/valid` to get the user information once the login was successful. 

Before running the project you will need some configuration.

## Configuration

As a pre-requisite, you need:
- to have project created with SAML template
- to have created and configured a SAML service in your RHMAP platform.
All the pre-requisites steps are well described in the [SAML notes](https://github.com/feedhenry-templates/saml-service/blob/master/NOTES.md).

Here is the steps you will need to do on your client app in fh-ngui console:
- Go to your SAML Demo project, associate it with your new SAML Service (click the + in the MBaaS Services area)
- Pick your SAML Service, and click Associate
- Navigate into your service and grab its "Service ID" (e.g. qhgvcppenzcquhlipr3dldat)
- Go into your SAML Cloud app, choose Environment Variables icon on left hand side navigation
- Add a new environement variable
    - Name: SAML_SERVICE
    - Value: YOUR_SERVICE_ID (e.g. qhgvcppenzcquhlipr3dldat)
- Re-deploy your SAML Cloud app

You should be good to go.

## Build and deploy your app

Open VisualStudio project ```saml-windows.sln```.
Run it.

## Code snippets and SAML usage

### Login call
When the user clicks the `Sign In` button, `sso/session/login_host` end point is called. The resulting `sso` url is loaded in the WebView. 

In ```saml-windows\saml-windows.Shared\MainPage.xaml.cs``` file, we define the ```Button_Click``` method called once the user hits login button, as below:

```csharp
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
```

### User information call

Once the user is successfully logged in, we can call `sso/session/valid` to fetch the user details. 

In ```saml-windows\saml-windows.Shared\MainPage.xaml.cs```  file, we define the ```ShowSignedIn``` method called once the user is logged in and closes the WebView. In here we call the user information details.

```csharp
private async void ShowSignedIn()
{
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
```
