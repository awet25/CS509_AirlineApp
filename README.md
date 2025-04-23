# CS509_AirlineApp
Airline application final project for design of software systems CS509 at Worcester Polytechnic Institute

Launch Process:

Ensure that in the program.cs fil of the backend the policy.withorigin line properly references your local host servers

--Using 2 terminals--
In terminal 1 'cd AppBackend' and use 'dotnet run' to start backend server

In terminal 2 'cd AppFrontend' and use 'npm run dev' to start frontend server



Please add Environment variable in your your system that is called "DefaultConnection" that has the connecting string to your database.

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [Stripe CLI](https://stripe.com/docs/stripe-cli) for webhook testing
- [Node.js & npm](https://nodejs.org/) (if using frontend)

### 🔧 Setup Instructions
Set the following Environmetal variables
STRIPE_SECRET_KEY=sk_test_xxx
STRIPE_WEBHOOK_SECRET=whsec_xxx
DefaultConnection=server=localhost;port=3306;user=root;password=yourpass;database=yourdbname;