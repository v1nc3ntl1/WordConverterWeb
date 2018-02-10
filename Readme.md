## Pre-requisite
1. Visual studio 2017 / 2015 with .Net Framework 4.6.2
2. NPM

## Instruction
## Running web api
1. Open the WordConverterAPI.sln in Visual Studio with Administrator right.
2. Press F5.
3. Browser will launch with Web Api running on background.

## Running the website
1. Open Command Prompt with Adminstrator right.
2. Go to WordConvertWeb folder. For example, cd C:\Workspace\wcw\WordConvertWeb
3. Type "npm install" and wait until packages added. For example, you will see "added 953 packages in 29.079s"
4. type "ng serve -o"
5. Browser will launch with page. 

## To use the website.
1. Click Converter tab from the Navigation bar.
2. Enter a name and number and press "Show"
3. Number in words will be shown

## To exit or shut down
1. Click Ctrl + C and type y to exit angular website.
2. Stop Debugging from Visual Studio to exit Web Api.

## Technologies used
1. ASP.NET Web API
2. C# .NET
3. Angular & Bootstrap - Front End
4. Unity Container as IOC Container
5. Nuget packages
6. NUnit - Test runner
7. NSubstitute - Mocking & Faking

## Concepts used
1. Dependency Injection - through WebApiHelper & used in WebApiConfig
2. SOLID Principles 
3. TDD
4. Decorator Design Patterns 
- through IpFilteringHandler. Served like a pipeline that can chain multiple IRequestFilter to allow or block Api Controller action. 
  It starts from IpWhiteListFilter and can chain any other implementation of IRequestFilter such as Development machine allow Filter and etc.

