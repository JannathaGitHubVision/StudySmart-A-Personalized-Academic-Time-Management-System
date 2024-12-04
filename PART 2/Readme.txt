##Student Academic Time Management System

My program is developed using Visual Studio Code and consists of two projects: a WPF project and a DLL project. Both projects use the .NET Framework 4.7.2 version. 
The WPF project is the user interface for the time management application, which has the following features:

- Register: This feature allows new users to create an account with a username and password.
- Login: This feature authenticates the users and grants them access to the application.
- Opening Menu: This is the main screen of the application, where users can navigate to other features.
- Capture Modules: This feature enables users to enter the details of the modules they are taking in the semester, such as code, name, credits, and class hours. It feature allows users to specify the start date and the number of weeks of the semester
- Display Academics: This feature shows the users the list of modules they are taking, along with the calculated number of self-study hours per week and the remaining hours for the current week.

The DLL project is a custom class library that contains the classes related to the data and calculations of the application. The WPF project makes use of the DLL project to manipulate the data and perform the logic.

## Prerequisites

- .NET Framework 4.7.2 or higher
- Visual Studio Code

## System Requirements

- Operating System: Windows 10 or higher
- Memory: 4GB RAM or higher
- Disk Space: 1GB or higher

To execute the program, please adhere to the following instructions:

1. Extract the zip file of “ST10053561_Venkata Jannatha_PROG6212_POE_PART2”.
2. Open the “SampleProgPoe.sln” file by double clicking or using Visual Studio software on select 'File' at top left corner and choose 'Open' and select 'Project'.
and locate tot he zip folder that you exract.
3. Click on the ‘Start’ button (represented by a green arrow) located at the top of the IDE window to launch the application.

##Refrences
Dictionary objects:
https://stackoverflow.com/questions/7321235/dictionary-with-object-as-value

Dictionary LINQ :
https://stackoverflow.com/questions/55420571/how-to-find-an-element-in-di 

Checking Letters :
https://stackoverflow.com/questions/1181419/verifying-that-a-string-contains-only-letters-in-c-sharp 

Storing the YesorNo message :
https://stackoverflow.com/questions/3036829/how-do-i-create-a-message-box-with-yes-no-choices-and-a-dialogresult#:~:text=This%20should%20do%20it%3A%20DialogResult%20dialogResult%20%3D%20MessageBox.Show,%28dialogResult%20%3D%3D%20DialogResult.No%29%20%7B%20%2F%2Fdo%20something%20else%20%7D 

Adding columns to a GridView : 
https://stackoverflow.com/questions/704724/programmatically-add-column-rows-to-wpf-datagrid  

Menu Items:
https://wpf-tutorial.com/common-interface-controls/menu-control/ 

hashing password:
https://code-maze.com/csharp-hashing-salting-passwords-best-practices/ 
