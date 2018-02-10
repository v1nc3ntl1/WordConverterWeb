## Technical Implementation Details
## Web Api
- The convert number Api is located at WordConverterController.Get action method. 
- The client will call the Convert Number end point at http(s)://<WebApiHostName>/api/Converter/{number} 
- The Controller is decorated with EnableCors at http://localhost:4200 to enable client (Angular Web site) running at http://localhost:4200 to call the Controller.
- If negative number is passed to api, a Http 400 Bad Request will be return.
- If RangeException is found when converting number, a Http 400 Bad Request will be return.
- If no exception during converting number, a HTTP Status 200 OK will be return with a WordModel encapsulating the converted number to client.
- The WordModel payload is return as camelCase as it was configured as CamelCasePropertyNamesContractResolver in ContractResolver
- The actual Converter process is encapsulated in SimpleWordConverterProvider which interface through IWordConverterProvider. 
- EnableCors is being enabled at WebApiConfig.Register method to enable different host name calling the web api.
- The authorization of Web Api is done through the IpFilterHandler in WebApiConfig.Register method by setting the MessageHandlers.
- The actual authorization / filter is done on IpFilterHandler.
- The Dependency injection of IWordConverterProvider through WebApiConfig.Register method is to allow other resolving dependency of IWordConverterProvider and also 
allow easier replacement of any other implementation of IWordConverterProvider such as ChineseConverter in future. WordConverterController is the example of the class which request instance of 
IWordConverterProvider through it's constructor.

## Converter Provider / Engine - SimpleWordConverterProvider
- The SimpleWordConverterProvider class is the actual engine that perform the conversion from number to words.
- It resides in WordConverterLibrary project as it can be a separate component on its own.
- SimpleWordConverterProvider can be configure to output number word in 
	1. UPPERCASE
	2. Include Dollar
	3. Include And in between hundred and ties. For example, nine hundred AND twenty.
	through its includeCurrency, includeAnd and upperCase constructor parameter.

## The Web Api authorization - IpFilterHandler
- The IpFilterHandler is a manager or provider to determine if a Controller Api is allow to execute. 
- It can call a series of IRequestFilter to validate the Api Request.
- IpWhiteListFilter is the request filter configured at this moment.
- It call the implementation of IIpHelper the get the client ip.
- It call the implementation of ISettings to get the white lists ip address.
- If requestFilter is being pass to the constructor, it will call the requestFilter.IsAllow() to determine whether to allow Api to execute.
- The algorithm of IpFilterHandler is as following.
	1. If "WhiteListedIPAddresses" is empty in appsettings, allow api to execute.
	2. If "WhiteListedIPAddresses" ie not empty, and client ip matches one of the lists in "WhiteListedIPAddresses" proceed to next step.
	3. If up to this point, allow api to execute is true and requestFilter is empty, return true.
	4. If up to this point, allow api to execute is true and requestFilter is being pass to the constructor, execute the requestFilter.IsAllow() and return the result.
- The IpFilteringHandler resides in Kernel project because it can be a component by itself.

## Unit Tests
- All tests method is written in Nunit except WordConverterAPI.Tests project.
- The unit test is written in AAA form. Arrange, Act and Assert.
- The unit test is written in DAMP (Descriptive And Meaningful Phrases) form
- Unit test can be run using Resharper Runner or Nunit Test Runner.

## WebSite - Front End Application
- Angular is used as the front-end javascript framework due to it's versatility and popularity.
- The css is powered mostly by bootstrap.
- The page is serve from angular component and it call the angular service which call the underlying web api to convert the number.
- The Converter component (/src/app/converter) describe the converter tab presentation. 
- When the Show button is clicked, the Converter component is calling wordConverterService (wordConverter.service.ts) that have the actual code to call the underlying web api.
- The wordConverterService uses promise / observable concept to fire asynchronous web api call and wait for the response to return. 
- Once response return from Web Api, it will return the model to Converter component which in turn set the model to Profile model 
and pass model to profile-output component (/src/app/profile-output) and display the number i words.
- The message service (message.service.ts) is used to capture the progress of calling web api and the progress is being stored and output in messages component (/src/app/messages)
at the bottom of the web page.

	 