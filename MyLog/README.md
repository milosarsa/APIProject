#MyLogger

##Abandoned for now, will exchange with serilog(Too much work, will continuo later on)


## Needs to be implemented and renamed ty MyLog

## Goal
Provide an independent library for capturing, processing, printing and storing logs. (Azure Table, Console, File)

Needs to be implemented


Logs -> Queue -> Table(myLogs)

All data output can be configured to output a certain level of data
	Ex.Console output does not log inner exception message
	Full exception messages by default present only in file, table log form
	Maybe use a sepparate config file


MyException - 
- Message?
- Status code?
- InnerException?

Calls made using Logger.Level(MyException)
	- Log Levels
		- Info
		- Warning
		- Error

Logs output format: StackTrace\t Error message?, Status code?  \n Exception message?
Ex. Error log