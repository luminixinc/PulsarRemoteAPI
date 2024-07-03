### What is this repository for? ###

  

This Project exists to demonstrate how an unpackaged (non-UWP) Microsoft Windows app might communicate with the Pulsar API directly on the same Windows system. Our current version of Pulsar is a packaged UWP application and due to this has some limitations on how other apps can connect to it. In short, if the connecting application is another packaged application, then explicit access rules can be added to both Pulsar and the connecting application to allow for communication. This is not preferred as both apps would have to hardcode those specific app access rules in the builds themselves.

  

If the connecting application is unpackaged, there is also a way to communicate, but it will require administrative permissions and an update to the local firewall to explicitly allow communication through the port Pulsar listens on (17014).

  

Here is the Windows documentation describing these limitations:

https://learn.microsoft.com/en-us/windows/uwp/communication/interprocess-communication#loopback

  

### Steps for communicating with Pulsar from an UNPACKAGED app: ###

  

- Start and login to Pulsar on your Windows PC so that the internal Pulsar web server begins listening for requests on port 17014.

  

- Per the Microsoft documentation above, in order to allow inbound connections, we need to run the following command with administrator permissions:

```
CheckNetIsolation.exe LoopbackExempt -is -n=E3D1A000.PulsarforSalesforce_ta6n8xy84gcpg
```  

**Please Note: this command needs to be running continuously while performing connection tests.**

- Next, you should permit access to port 17014 through your local firewall.

	- If you use Windows Defender, you can do this by:

		- Navigating to: Control Panel -> System and Security -> Windows Defender Firewall.

		- Click on 'Advanced Settings'

		- Click on 'Inbound Rules'

		- Tap 'New Rule...' and allow TCP port 17014

* This test app can be built and run using Visual Studio 2022 or Visual Studio Code (with C#/DotNet extensions installed)