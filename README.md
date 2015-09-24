# SAML Client Windows

This is an example SAML Client App, designed to be used in conjunction with our [SAML Service](https://github.com/feedhenry-templates/saml-service). The Cloud App here only proxies calls from client to an SAML Service (e.g. fetching a URL to display in the WebView for IdP login). You must provide set a SAML_SERVICE environment variable with an appropriate SAML Service ID for this to work.

The SAML Service has 2 endpoint that can be called `sso/session/login_host` to get the SAML login page URL and `sso/session/valid` to get the user information once the login was successful. The parameters we need to send to both these calls is the `deviceId`

In this client we use a webview to display the SAML login page clicking the `Sign In` button will result in a call to `sso/session/login_host` the resulting `sso` url is loaded in the webview. The webview has a listener attached that when the login is successful the the call to `sso/session/valid` is performed to fetch the user details. When the user was already logged in the webeview will directly contain the `login/ok` url and fetching the details will be invoked imidiately.

## Example fetch login url

```csharp
var response = await FHClient.GetCloudRequest("sso/session/login_host", "POST", null, GetRequestParams()).ExecAsync();
var resData = response.GetResponseAsJObject();
var sso = (string)resData["sso"];
if (!string.IsNullOrEmpty(sso))
{
  webView.Visibility = Visibility.Visible;
  webView.Navigate(new Uri(sso));
}
```

## Example fetch user information:

```csharp
var response = await FHClient.GetCloudRequest("sso/session/valid", "POST", null, GetRequestParams()).ExecAsync();
if (response.Error == null)
{
  var data = response.GetResponseAsDictionary();
  //show the user information on the screen
  name.Text = string.Format("{0} {1}", data["first_name"], data["last_name"]);
  email.Text = (string) data["email"];
  expires.Text = ((DateTime) data["expires"]).ToString();
}
else
{
  await new MessageDialog(response.Error.ToString()).ShowAsync();
}
```
