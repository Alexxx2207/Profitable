## Profitable - Overview
This project is a platform that helps traders to make trading decisions, observe the financial markets, communicate with each other, journal their performance and never miss news or events. It is suitable for investment banks, hedge funds and retail traders' communities.

## Project Structure
### In folder Profitable there are all files for both the backend and the frontend application.

#### **Link for the clientside application folder: https://github.com/Alexxx2207/Profitable/tree/main/Profitable/Profitable.Web/ClientApp** ####

## Prerequisites

1. Visual Studio 2022 and its required workloads from the Visual Studio Installer (see images below):

![image](https://user-images.githubusercontent.com/61185321/197560581-d7cfa221-83b9-427e-9f52-68221e854ce6.png)
![image](https://user-images.githubusercontent.com/61185321/182320887-25f449a1-3b03-49a8-a48f-49f098c10f14.png)


2. Database Engine for Database (Microsoft SQL Engine Developer Edition preffered)

&emsp;&emsp; https://www.microsoft.com/en-us/sql-server/sql-server-downloads


## Configuration

1. Change the WEB_API_BASE_URL in Profitable.Web/ClientApp/src/common/config.js to meet your backend web API address

&emsp;&emsp; :warning: Be careful not to forget '/api' path string after the port number! :warning:

2. Add "JWT_KEY" key/value pair in user secrets

3. Set '.' as decimal point for numbers with decimal part. That is configured in your OS settings.

4. Set Connection String for your database on lines:
https://github.com/Alexxx2207/Profitable/blob/2764f87d0b1a26990fdc42416da16ce2387954e2/Profitable/Profitable.Web/appsettings.json#L3
https://github.com/Alexxx2207/Profitable/blob/2764f87d0b1a26990fdc42416da16ce2387954e2/Profitable/Profitable.Data/appsettings.json#L3

## How To Start The Project
    
1. Open folder Profitable and click Profitable.sln from your file explorer  
    
2. After Visual Studio is loaded, go to View->Solution Explorer **(Skip this step if you already see a Solution Explorer section on your screen)**
  
3. Make sure you have applied migrations 
    
    3.1 In Solution Explorer open folder Common and right click on Porfitable.Data

    3.2 Search for Open In Terminal
    
    3.3 A Developer PowerShell window is opened -> Run the command "**dotnet ef database update**"
    
 
4. In Solution Explorer open folder AdminPanel and right click on Profitable.AdminPanel

    ![image](https://user-images.githubusercontent.com/61185321/208537052-eda766f0-6814-41db-9d26-3f40e95e2db2.png)


    **A dialog with many options is opened.**  
  
  
5. In this dialog click **'Set as Startup Project'**

   ![image](https://user-images.githubusercontent.com/61185321/208653302-5db2ba41-aeec-4abe-8d29-e50dd1c0bdfa.png)


6. Start the application. A console app is started, make sure **all enitities from the list are seeded in your database!**


7. In Solution Explorer open folder Web and right click on Profitable.Web
  
    ![image](https://user-images.githubusercontent.com/61185321/182330356-6590855b-3d76-4b86-ab9a-8e5d7375d5e8.png)
  
    **A dialog with many options is opened.**
  
  
8. In this dialog click **'Set as Startup Project'** 
  
   ![image](https://user-images.githubusercontent.com/61185321/208653255-e2d4dbef-ad4d-4f78-ab71-e88a7b425d11.png)


9. Make sure you have installed all JS packages
    
    9.1 Open Profitable.Web -> ClientApp with VS Code
    
    9.2 Run "**npm install**" in Terminal

10. Start the application from the Visual Studio

    ![image](https://user-images.githubusercontent.com/61185321/208654548-c1b03a63-48ae-4074-a4e6-1d47eb0e5130.png)


***This will start both the backend and the frontend servers and automatically open a browser tab with the project application.***
  
> :warning: **Do NOT close the console windows that were opened after clicking the empty green arrow!** :warning:  
> :warning: **Close them when you finish the web app usage!** :warning: 

## Tech Stack

**Backend**

- ASP.NET Core Web API
- Entity Framework
- SQL Server
- Identity JWT
- Automapper
- SignalR
- Argon2 (hashing algorithm)

**Frontend**

- React
- HTML
- CSS
- chart.js
- Material UI
- react-tradingview-embed
- dayjs
