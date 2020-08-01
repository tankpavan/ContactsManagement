##Contacts Management

#### Introduction
The application is for maintaining contact information. The application is built on Asp.Net Core 2.1 and Angular 5. 

#### Features
* List contacts
* Add a contact
* Edit contact
* Delete/Inactivate a contact

#### Projects
 * **ContactsManagement** - Asp.Net Core 2.1 application having Web API and uses Angular 5 template. Used PrimeNg library for UI template.
 * **ContactsManagement.DataAccess** - .Net Core 2.1 class library consists of database context files and models and repository class to access database for add, edit, delete and get records from the database. 
 * **ContactsManagement.Domain** - .Net Core 2.1 class library consists of all domains models and interfaces used to implement DI.
 * **ContactsManagement.Tests** - NUnit Test Project consists of unit tests for Web API methods of **ContactsManagement** project.

#### Installation 
* Download the source code from git.
* Open solution file in Visual Studio.
* Build whole solution. It will take some time to build solution first time because it will install npm packages required in Angular application. It might give error for some npm packages which will be resolved after building project again.
* Set **ContactsManagement** project as default project.
* Run the application using either profiles **IIS Express** or **ContactsManagement**.
* For convenience database mdf file directly added in **ContactsManagement.DataAccess** projects. So there is no need to setup the database. Connection string added in appsettings.json file, so that can you create and use SQL server database from different locations.
* The application has all the required configurations to deploy or host the application on the production environment. For this, **appsettings.json** files added for different environments (i.e. appsettings.Development.json, appsettings.Staging.json, and appsettings.Production.json). You may add additional environments as per the requirements.
* To run the application on the environment other than Development you just need to change **ASPNETCORE_ENVIRONMENT** variable.
* For deployment on production, the environment variable must be set on that environment.

#### Logging
NLog library is used for logging. Used File type for logging option. The location of log files set as **C:\temp**. **nlog.config** file used for all the logging configuration. You may configure log files location by changing the path in **nlog.config** file. You can also change the log levels in different environments in appsettings.{environment}.json file.

#### Unit Testing
NUnit Test project added with some basic unit tests for the Web API.

#### Authentication & Authorization
Currently, there is no authentication and authorization applied in the application. As per the requirement, we can add authentication and authorization by either using middleware or using Filters and Attributes.
