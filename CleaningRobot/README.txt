HOW TO OPEN AND COMPILE

Cleaning robot is implemented in C#. 
To open the solution file CleaningRobot.sln, please use Visual Studio 2015 or newer.
Use menu command "Build -> Build solution" in Visual Studio to compile the solution.
Nuget package restoration during build must be turned on.


SOLUTION STRUCTURE

Solution contains several projects:

CleaningRobot
    console application cleaning_robot.exe

CleaningRobot.Core
    base assembly with cleaning robot itself

CleaningRobot.Core.Test
    tests for CleaningRobot.Core

CleaningRobot.Json
    Separation of JSON (de)serialization and cleaning robot.

CleaningRobot.WebApi
    ASP.NET Web API service

The cleaning robot is separated from JSON format to allow use different
input/output formats in the future.  
I decided to use library Newtonsoft.Json for JSON (de)serialization.
Therefore I add DTOs which correspond to requested JSON message formats.


DELIVERABLES

Console application is named cleaning_robot.exe and compilation output
is in directory CleaningRobot\bin\Debug or ClenaingRobot\bin\Release 
according to choosen build configuration.
It can be invoked with the following command:
cleaning_robot.exe source.json result.json

To test Web API service right click on the CleaningRobot.WebApi project 
and from context menu choose "Set as StartUp project". 
Then use menu command "Debug -> Start debugging".
Web browser shows up with error message, because I removed all MVC parts
from the project and therefore there is no web page to display.
You can use for example freeware application Postman to send request to the
service.

Parameters for the request:
URL:           http://localhost:53074/api/CleaningRobot 
(the port may differ, you can find it out in the web browser window mentioned above)
Method:        POST
Content type:  application/json
Raw body:      you can use content of test1.json or test2.json file