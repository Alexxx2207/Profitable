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
  
![image](https://user-images.githubusercontent.com/61185321/182321525-02dd831b-2a63-4d71-aaa7-b1893ed3035d.png)
![image](https://user-images.githubusercontent.com/61185321/182321593-cdc71aab-7e36-4f87-a952-4fe0424bebcb.png)
  
    
2. After Visual Studio is loaded, go to View->Solution Explorer **(Skip this step if you already see a Solution Explorer section on your screen)**
  
![image](https://user-images.githubusercontent.com/61185321/182322449-08b080b8-e2a0-4e0b-a09c-6ede73d6540d.png)
![image](https://user-images.githubusercontent.com/61185321/182322515-427f9103-ffb2-4c05-a017-37aa9e8c12f8.png)
  
3. In Solution Explorer open folder AdminPanel and right click on Profitable.AdminPanel
  
![image](https://user-images.githubusercontent.com/61185321/208537052-eda766f0-6814-41db-9d26-3f40e95e2db2.png)


**A dialog with many options is opened.**  
  
4.In this dialog click **'Set as Startup Project'**

![image](https://user-images.githubusercontent.com/61185321/208537174-995e58f5-79e8-4d1d-b2df-e41db928b190.png)


5. Start the application, a console app is started, make sure **all enitities from the list are seeded in your database!**


6. In Solution Explorer open folder Web and right click on Profitable.Web
  
![image](https://user-images.githubusercontent.com/61185321/182330356-6590855b-3d76-4b86-ab9a-8e5d7375d5e8.png)
  
**A dialog with many options is opened.**
  
  
7. In this dialog click **'Set as Startup Project'**  
  
![image](https://user-images.githubusercontent.com/61185321/182323855-25bd40a0-98e2-4038-ab47-baa551f8110d.png)
  
8. Click on the empty green arrow in the navigation bar
  
![image](https://user-images.githubusercontent.com/61185321/182324277-8c4b9498-f0ce-4e44-acbe-e641316ad5e3.png)
  
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
