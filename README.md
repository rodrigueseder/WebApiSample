# WebApiSample

### Pre-requisites
	Windows features
	- Enable .Net Framwork 3.5 (Root feature only)
		- If error when trying to download set the gpo to download directly from windows update (http://www.mswiki.com.br/erro-ao-tentar-instalar-o-net-framework-3-5-no-windows-8/)
	- Enable Internet Information Services (All child features)
		- If error use the Web Platform Installation (http://177.205.9.201/pdata/a6c5d0a0008dd3e4/download.microsoft.com/download/F/4/2/F42AB12D-C935-4E65-9D98-4E56F9ACBC8E/wpilauncher.exe)
	
	Windows SDK tools (it installs makecert tool, that will be used on next steps)
	- Programs and Features > Visual Studio > Modify > Select Windows SDK to be installed
	
	Certificate Store
	- Create a folder for certificates i.e C:\Certificates
	- Open the Developer Command Prompt for VS as Administrator
	- Go to C:\Certificates
	- Run the following commands (this will create the CA Root Certificate and pack it with its Private Key)
		- makecert.exe -r -n "CN=RootCertificate" -pe -sv RootCertificate.pvk -a sha1 -len 2048 -b 02/10/2017 -e 02/10/2030 -cy authority RootCertificate.cer
		- pvk2pfx.exe -pvk RootCertificate.pvk -spc RootCertificate.cer -pfx RootCertificate.pfx
		
### Server
	Certificate Store
	- Open the Developer Command Prompt for VS as Administrator
	- Go to C:\Certificates
	- Run the following commands (this will create the Server Certificate and pack it with its Private Key)
		- makecert.exe -ic RootCertificate.cer -iv RootCertificate.pvk -pe -sv localhostServer.pvk -a sha1 -n "CN=localhostServer" -len 2048 -b 02/10/2017 -e 02/10/2030 -sky exchange localhostServer.cer -eku 1.3.6.1.5.5.7.3.1
		- pvk2pfx.exe -pvk localhostServer.pvk -spc localhostServer.cer -pfx localhostServer.pfx
	- Open the certificate store for Computer Account (mmc.exe > File > Add/Remove Snap-In > Certificates > Computer Account)
		- Go to Trusted Roo Certification Authorities node
		- Right-click and import the RootCertificate.cer created on the previous steps
		- Go to Web Hosting node
		- Right-click and import the localhostServer.pfx created on the previous steps
	
	IIS
	- Create a new WebSite on IIS named localhostServer
	- Physical path to WebApiServer project's folder (will be created on the next section)
	- Bindings
		- http on Port: 80
		- https on Port: 443, Host Name: localhostServer, SSL Certificate: localhostServer
	- App pool using .NET v4.0 and an admin account as identity
	- Change C:\Windows\System32\drivers\etc\hosts to redirect localhostServer calls
		- 127.0.0.1       localhostServer

	Visual Studio
	- Start VS as admin
	- MVC Web Application
	- Set server to Local IIS using URL http://localhostServer
	- Click on Create Virtual Directory (an Error will be thrown if no admin rights, if so restart VS as admin)
	- Create a HttpWebRequest including User/Pwd on the header, as well as the client certificate		
		
### Client
	Certificate Store
	- Open the Developer Command Prompt for VS as Administrator
	- Go to C:\Certificates
	- Run the following commands (this will create the Client Certificate and pack it with its Private Key)
		- makecert.exe -ic RootCertificate.cer -iv RootCertificate.pvk -pe -sv localhostClient.pvk -a sha1 -n "CN=localhostClient" -len 2048 -b 02/10/2017 -e 02/10/2030 -sky exchange localhostClient.cer -eku 1.3.6.1.5.5.7.3.2
		- pvk2pfx.exe -pvk localhostClient.pvk -spc localhostClient.cer -pfx localhostClient.pfx
	- Open the certificate store for Current User Account (mmc.exe > File > Add/Remove Snap-In > Certificates > My User Account)
		- Go to Trusted Roo Certification Authorities node
		- Right-click and import the RootCertificate.cer created on the previous steps
		- Go to Personal node
		- Right-click and import the localhostClient.pfx created on the previous steps
	
	IIS
	- Create a new WebSite on IIS named localhostClient
	- Physical path to WebApiClient project's folder (will be created on the next section)
	- Bindings
		- http on Port: 80
	- App pool using .NET v4.0 and an admin account as identity
	- Change C:\Windows\System32\drivers\etc\hosts to redirect localhostClient calls
		- 127.0.0.1       localhostClient

	Visual Studio
	- Start VS as admin
	- MVC Web Application
	- Set server to Local IIS using URL http://localhostClient
	- Click on Create Virtual Directory (an Error will be thrown if no admin rights, if so restart VS as admin)
	- Create a HttpWebRequest including User/Pwd on the header, as well as the client certificate

### References

http://blog.anthonybaker.me/2013/05/how-to-consume-json-rest-api-in-net.html
http://stackoverflow.com/questions/1842770/consume-rest-api-from-net
https://www.codeproject.com/tips/1015550/create-and-consume-asp-net-webapi-services-mvc
https://msdn.microsoft.com/en-us/library/jj823172(v=vs.110).aspx
http://www.dotnettricks.com/learn/webapi/difference-between-wcf-and-web-api-and-wcf-rest-and-web-service
https://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
http://stackoverflow.com/questions/1639707/asp-net-mvc-requirehttps-in-production-only

https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/basic-authentication
https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/working-with-ssl-in-web-api

http://www.diogonunes.com/blog/webclient-vs-httpclient-vs-httpwebrequest/

https://dotnetcodr.com/2015/05/28/https-and-x509-certificates-in-net-part-1-introduction/
https://dotnetcodr.com/2016/01/11/using-client-certificates-in-net-part-1-introduction/
http://www.hanselman.com/blog/WorkingWithSSLAtDevelopmentTimeIsEasierWithIISExpress.aspx

https://jamielinux.com/docs/openssl-certificate-authority/create-the-intermediate-pair.html
http://www.oid-info.com/cgi-bin/display?oid=1.3.6.1.5.5.7.3&submit=Display&action=display

http://stackoverflow.com/questions/26247462/http-error-403-16-client-certificate-trust-issue