# FlightBooker-backend

This is the backend application built with ASP.NET for the Flight Booker Application. It provides the API endpoints and handles data storage and retrieval.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) must be installed on the local system.
- An instance of a PostgreSQL database must be set up and accessible.

## Installation

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/abhi8080/FlightBooker-backend.git
   ```

2. Navigate to the project directory:

   ```bash
   cd FlightBooker-backend
   ```

3. Restore dependencies

   ```bash
   dotnet restore
   ```

4. Build

   ```bash
   dotnet build --configuration Release
   ```

5. Update the database connection details:

   - Open the `env.cs` file.
   - Update the `connectionString` with your PostgreSQL database connection details, including the server, port, database name, username, and password.

6. Run the database migrations:

   ```bash
   dotnet ef database update
   ```

7. Start the development server:

   ```bash
   dotnet run
   ```

   The backend server will now be running at `http://localhost:5228`.

## Usage

The backend provides the following API endpoints:

- **Endpoint 1**: `/airports`

  - `GET`: Retrieves all airports.

- **Endpoint 2**: `/flights/dates`

  - `GET`: Retrieves a list of distinct flight dates.

- **Endpoint 3**: `/flights`

  - `GET`: Retrieves a list of flights based on search criteria.

- **Endpoint 4**: `/book`

  - `POST`: Creates a new booking.

### Transactions

Transactions are used to ensure data consistency and integrity when performing database operations. The backend application utilizes the transaction support provided by Entity Framework Core. Database operations within the `using` statement are wrapped in a transaction, ensuring that they are executed atomically.

### Unit Tests

The backend includes unit tests to ensure the correctness of the application's functionality. The tests cover different scenarios and edge cases to verify that the API endpoints and database operations are working as expected.

To run the unit tests, use the following command:

```bash
dotnet test
```

The tests will be executed, and the results will be displayed in the terminal.

## License

This project is licensed under the [MIT License](LICENSE).
