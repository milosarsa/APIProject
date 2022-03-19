# APIProject

### First edition of APIProject

APIProject API gives us the ability to easily store, manage and read Projects and Tasks.
	



## Task

Solution should provide an ability to easily add new fields in Task entity. 
Each task should be a part of only one project. Project – is an instance which contains name, id and code (and also keep Tasks entities).
You need to deploy your service in Microsoft Azure and use tables (Azure Table Storage) for storing your projects and tasks (you can use free subscription).
To access to your service you need to implement Rest API.

	Project
	 - Id
	 - Name
	 - Description
	 - Tasks
	 - Code
	 - State

	 Task
	 - Id
	 - ProjectId
	 - Name
	 - Description
	 - State
	 - Owner (User)


## Technologies used
- .Net Core (6.0)
- AzureStorage


API is currently not published on Azure.
For testing purposed please use local azure storage emulator

## Implemented
- n-Layer architecture with repository-service pattern
- JWT Bearer Authentication
- Policy based authorization
- Azure Storage Table
- REST API
- Async/Await
- Swagger
- Dependency injection


## TODO
- Blazor web client
- Comments :)
- Definitions
- Performance improvements
- MyLogger - Custom logger library for capturing, processing, printing and storing logs. (Azure Table, Console, Local File, Azure Blob File)
- Azure Key Vault
- Azure Blob Storage 
- Azure Queue
- Memory cache
- Soft delete
- Proper user storing managing and authentication (Password hashing, DB)
- Security
- Proper Exception handling
- API Definition
- API Versioning
- Handle and log Unauthorized and Forbidden request (to better track any unwanted requests)
