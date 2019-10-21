# jobsityChat
Chat for jobsity company

To run the project you only need to download the solution, open it in visual studio 2019, and run the solution.
- when you run the solution 3 projects should start.

project ChatHub - This project is intended to handle the chat room this project should run on https://localhost:44306/.

project ChatServices -  this project handles the connection to the database, this project should run on https://localhost:44319/.

project JobsityChatFront - this is the frontend project this project should run on https://localhost:44349.


- we have some additional project for handling the connection to the database and the business logic.

- Since this is only a demo project I used an in memory database for storing the users so it is necessary to create the users again every time you restart the application.


Authentication:

I wanted to separate the services for connecting to the database from the chat so I created tw different project one for the services and another for the chat, for that reason I decided to use the JWT autentication
in order to allow me authenticate each service, I thought for this scenario windows authentication is not the most optimal choice. 

Assumptions:
The stock web service is working and the url is right.

Missing Items
I was not able to create the unit tests of the application