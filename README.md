# SCORM Cloud C#.NET API Bindings

This library provides implementations for the majority, but not all, of the
[SCORM Cloud API][1]. It is provided under the BSD 3-clause License (see
[LICENSE](LICENSE.md)).

You can sign up for a SCORM Cloud account at
[https://cloud.scorm.com](https://cloud.scorm.com).

See our [API quick start][1] guide for more information.

## Requirements

Requires .NET 2.0+

## Installation

### Via NuGet

This library is available [via NuGet][2].

From the command line:

    nuget install RusticiSoftware.HostedEngine.Client

From Package Manager:

    Install-Package RusticiSoftware.HostedEngine.Client -Version 1.6.0

From .NET CLI:

    dotnet add package RusticiSoftware.HostedEngine.Client --version 1.6.0

### Configuration

#### Single App ID

If you're only using a single application (app ID), you can use the
`ScormCloud` convenience singleton class:

```csharp
ScormCloud.Configuration =
    new RusticiSoftware.HostedEngine.Client.Configuration(
        "https://cloud.scorm.com/EngineWebServices",
        "your app id",
        "your secret key",
        "your origin string");
```

where *your app id* is the app ID in question, *your secret key* is a secret
key for that app ID, and *your origin string* is a company/app description.
(The origin string is used for debugging on the SCORM Cloud developers' side.)
See [API quick start][1] for information on app ID / secret keys.

#### Multiple App IDs

If you're using multiple applications in SCORM Cloud, you'll need to
build the `ScormEngineService` class manually.

```csharp
Configuration cfg =
    new RusticiSoftware.HostedEngine.Client.Configuration(
        "https://cloud.scorm.com/EngineWebServices",
        "your app id",
        "your secret key",
        "your origin string");

ScormEngineService service = new ScormEngineService(cfg);
```

This readme assumes the single app ID case, but the interface of
`ScormCloud` and `ScormEngineService` are effectively the same. For example,
instead of `ScormCloud.CourseService`, you'd use `service.CourseService`.

## Example API Calls

Once installed and configured, it's time to make API calls. As explained in
the [quick start guide][1], the library implements the scaffolding used to
interact with the [actual web API][3].

We also have a [.NET demo app](https://github.com/RusticiSoftware/SCORMCloud_NetDemoApp)
built using this client library, which contains far more example code,
but may not necessarily be following best practices.

### Registration Exists

Corresponds to [rustici.registration.exists][4].

```csharp
bool exists = ScormCloud.RegistrationService.RegistrationExists("reg id");
```

### Create Registration

Corresponds to [rustici.registration.createRegistration][5].

```csharp
ScormCloud.RegistrationService.CreateRegistration(
    "registration id",
    "course id",
    "learner id",
    "learner first name",
    "learner last name");
```

As explained in the [LMS Integration Guide][6], the registration, course, and 
learner IDs are provided by your system. The course ID needs to be an ID
for a course that's already been imported. See the LMS integration guide for
more information about integrating a typical LMS with SCORM Cloud.

### Building Launch Link

As explained in [rustici.registration.launch][7], the launch API call is
different from nearly every other API call in that it's intended for integrating
systems to *redirect* learners to that API call URL instead of calling it
server-side.

The client library facilitates this by providing a `GetLaunchUrl` method:

```csharp
string launchUrl = ScormCloud.RegistrationService.GetLaunchUrl(
    "registration id",
    "redirect on exit url");
```

where *registration id* is the registration to launch and *redirect on exit url*
is the URL to which the learner should be redirected if they exit the course.
(Note: as discussed in the [LMS Integration Guide][6], don't rely on learners
hitting the redirect on exit URL.)

In a typical .NET web-app, you might redirect to this:

```csharp
Response.Redirect(launchUrl);
```

The library provides other overrides for GetLaunchUrl that have more parameters.

## Implementing Other API Calls

Although this library implements most of the SCORM Cloud API, it doesn't
implement all of it. In cases where the library is insufficient, you can use
the `ServiceRequest` class to directly call API methods described in the
[API reference][3]:

```csharp
ServiceRequest request = ScormCloud.CreateNewRequest();

request.Parameters.Add("regid", "your reg id");

XmlDocument response = request.CallService("rustici.registration.exists");
XmlElement attrEl = (XmlElement) response.GetElementsByTagName("result")[0];

bool exists = Convert.ToBoolean(attrEl.InnerXml.ToString());
```

This snippet of code manually builds a `ServiceRequest` with one parameter
and then uses `CallService` to invoke the request for a particular API method.

As it happens, this is (close to) the implementation of the RegistrationExists
method above.

If you find methods missing and would like to implement them, we would be
eternally grateful for any pull requests. 

## Support

Need to get in touch with us? Contact us at
[support@scorm.com](mailto:support@scorm.com). Ask us anything.

Don't hesitate to submit technical questions. Our support staff are excellent,
and even if they can't answer a question or resolve a problem, tickets get
escalated quickly to real, live developers.


[1]: https://cloud.scorm.com/docs/quick_start.html
[2]: https://www.nuget.org/packages/RusticiSoftware.HostedEngine.Client/1.6.0
[3]: https://cloud.scorm.com/docs/api_reference/index.html
[4]: https://cloud.scorm.com/docs/api_reference/registration.html#exists
[5]: https://cloud.scorm.com/docs/api_reference/registration.html#createRegistration
[6]: https://cloud.scorm.com/docs/lms_integration.html#ids-are-yours
[7]: https://cloud.scorm.com/docs/api_reference/registration.html#launch
