# saml-windows-app [![Build status](https://ci.appveyor.com/api/projects/status/5c9qgnprl14bxkla?svg=true)](https://ci.appveyor.com/project/edewit/saml-windows-app)

Author: Erik Jan de Wit   
Level: Intermediate  
Technologies: C#, windows, RHMAP
Summary: A demonstration of how to authenticate with SAML IdP with RHMAP. 
Community Project : [Feed Henry](http://feedhenry.org)
Target Product: RHMAP  
Product Versions: RHMAP 3.7.0+   
Source: https://github.com/feedhenry-templates/saml-window-app  
Prerequisites: fh-dotnet-sdk : 3.+, Visual Studio : 2013/2015, windows mobile sdk

## What is it?

Simple native Windows app to work with [```SAML Service``` connector service](https://github.com/feedhenry-templates/saml-service) in RHMAP. The user can login to the app using SAML authentication, user details available on SAML IdP are displayed once successfully logged in.To configure the service in your RHMAP platform read the [SAML notes](https://github.com/feedhenry-templates/saml-service/blob/master/NOTES.md).

If you do not have access to a RHMAP instance, you can sign up for a free instance at [https://openshift.feedhenry.com/](https://openshift.feedhenry.com/).

## How do I run it?  

### RHMAP Studio

This application and its cloud services are available as a project template in RHMAP as part of the "SAML Project" template.

### Local Clone (ideal for Open Source Development)
If you wish to contribute to this template, the following information may be helpful; otherwise, RHMAP and its build facilities are the preferred solution.

## Build instructions

1. Clone this project

2. Populate ```saml-windows/saml-windows.Shared/fhconfig.json``` with your values as explained [here](http://docs.feedhenry.com/v3/dev_tools/sdks/windows.html#windows-existing_app-set_up_configuration).

3. Open ```saml-windows.sln```

4. Run the project
 
## How does it work?

### Using FHClient
In this example we used ```FHClient.GetCloudRequest``` to make request on the REST endpoint setup to deal with SAML authentication.

