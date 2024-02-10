# Loan-Web-API

LOAN-WEB-API is an ASP.NET CORE WEB API PROJECT that allows users to apply for and manage different types of loans. The API supports eight types of loans: 
Car Loan, Fast Loan, Buy With Credit, Personal Loan, Home Loan, Education Loan, Business Loan, Emergency Loan. 
Loans can be requested in three different currencies: GEL, USD, and EUR. The loan application period ranges from 1 day to 1825 days.

## Features:

- Users can apply for loans and manage loans that are in progress.
- Users can take multiple loans in different currencies.
- Users can view their previously availed loans.
- The bank operator, registered as `Admin123` in the database, has administrative rights.
  - The operator can log in and retrieve information about all loans based on the user ID.
  - The operator can delete loans.
  - The operator can change loan status from InProcessing to Approved or Rejected.
  - The operator can block or unblock any User with role 'User'.(When the user is blocked, he can not make any loan.)
  - The operator can see information about any User(Their balance, Age, every details)
  - He can also see all loans and specific loan by id.
  - The operator also can get information about blocked users.
  - The operator can get information about users by specific Country or City or by Age.
  - He can also get information about all loggs(that is saved in the database) by User id, email, logg datetime ect.
  - The operator can also get information about paid or unpaid loans.
  - It can also refresh the page, during which users with overdue and unpaid loans are automatically blocked and prohibited from performing any actions.
  - The operator also can to delete loan by status InProcessing and also delete expired and paid loans.

## Getting Started:

To use the API and access its services, you need to register as a user. Use the following registration form:

### Registration Form:

{
  "firstName": "string",
  "lastName": "string",
  "age": int,
  "contactNumber": int(from 500000000 to 599999999),
  "identityNumber": "string",
  "email": "string",
  "userName": "string",
  "password": "string",
  "userAddress": {
    "country": "string",
    "city": "string"
  }
}
(FirstName must not be empty.)
(LastName must not be empty)
(Email must be valid e-mail address)
(UserName must not be empty. It must be  between  6-15 chars!)
(Password must not be empty. It must be  between  6-15 chars!)
(Age must be from 21 to 65 to make a loan!)
(ContactNumber must be from 500000000 to 599999999)
(IdentityNumber must be valid identity number)
(Country and City must not be empty!)

Once registered, you can log in using the following user login form:

### User Login Form:
{
  "email": "string",
  "userName": "string",
  "password": "string"
}
### Users can get their balance and details about their profile:
/api/User/GetMyBalance
/api/User/GetMyProfile

## Loan Management:
### Adding a Loan:
Authorized users can add a loan using the following request body:
/api/Loan/AddNewLoan:
{
  "amount": int,
  "currency": "string",
  "type": "string",
  "loanPeriodDays": int
}
(Amount must be greater than 500 and less than 300,000)
(Loan time frame should be from minimum 1 day to 1825 days)

Authorized users can update a loan(which status is InProcessing) using the following request body:
/api/Loan/UpdateLoan?loanId={?id?}:
{
  "amount": int,
  "currency": "string",
  "type": "string",
  "loanPeriodDays": int
}

Users can get their loan information:
/api/Loan/GetMyLoans

Also can get information about their loans by loanId:
/api/Loan/GetMyLoanById?loanId={?id?}

When the loan status is Approved by Admin, User can cash out it and transfer to his balance:
/api/Loan/TransferMoneyToMyBalance?loanId={?id?}

He should Cover loan before expire date, in order to avoid block by Admin:
/api/Loan/CoverLoan:
{
  "amount": int,
  "loanId": int
}

### Get all Loan service: This service allows you to check and view all loans registered under your name. It provides loan information such as currency, date, amount, type, and loan ID. You can use the loan ID for further services such as the delete service or modify.

### Accountant (Admin) Services:
Accountant has the right to everything. He is an administrator and can change everything in the system, including operations that are prohibited to the user!
When the loan is approved by the administrator, the user is allowed to cash out the amount, at which time it is transferred from the bank account (GEL, USD, EUR) to the user's account. The user receives a loan of any kind and any periodicity at 12% and he has to return this amount. At the time of return, the GEL on the user's balance is automatically converted into => USD and EURO and thus credited to the bank account. If the user is late and misses the deadline for depositing the loan, it is automatically blocked and it is necessary to contact Support (admin).

## What have I made:
I have made models: User,Loan,Bank. I put validations on user registration, loan taking, I have used FluentValidation for it.
I have made identity, register and login form, I have used Microsoft.AspNetCore.Identity.EntityFrameworkCore for it and Microsoft.AspNetCore.Authentication.JwtBearer for generate Access token.
When the user registeres, password saves in the database as Hashpassword in order to be secured. I have user TweetinviAPI for it.
I have used common ORM - Entity FrameWork Core to connect my code in database. I have used Microsoft.EntityFrameworkCore.SqlServer and
Microsoft.EntityFrameworkCore.Tools" Version="8.0.1 for it.
I have made Testing NUnit project for testing my Services, that I have made in FakeServices folder, and then write testing code. I have used Swagger and Postman to test my status codes and not to be any exception in my code.
Everything Works perfectly.
