# 🐫 Camel Registry API

A lightweight **ASP.NET Core Minimal API** service designed to manage camel records. This project was developed as a technical assessment for the software developer internship position.

## 🚀 Features

* **Full CRUD Operations**: Create, Read, Update, and Delete camel records.
* **Data Validation**: Enforces business rules (e.g., `HumpCount` must be 1 or 2).
* **Automatic Database Setup**: Initializes the SQLite database on startup using Entity Framework Core (`Migrate`).
* **Interactive Documentation**: Integrated Swagger UI for testing endpoints.
* **Unit Testing**: Comprehensive test coverage using xUnit.

## 🛠 Tech Stack

* **Framework**: .NET 8
* **Architecture**: ASP.NET Core Minimal API
* **Database**: SQLite
* **ORM**: Entity Framework Core
* **Testing**: xUnit
* **Documentation**: Swashbuckle (OpenAPI/Swagger)

## 📦 Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed on your machine.

### Installation

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/JacintDev/CamelRegistry.git
    ```

2.  **Navigate to the project directory:**
    ```bash
    cd CamelRegistry
    ```

3.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

### Running the Application

To start the API, run the following command. The application will automatically create the `database.db` SQLite file if it does not exist.

```bash
dotnet run
```

## 📖 API Documentation (Swagger)

Swagger UI is enabled for easy endpoint testing and exploration. Once the application is running, navigate to:
```bash
http://localhost:5201/swagger/index.html
```

### Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| **GET** | `/camels` | Retrieves a list of all camels. |
| **GET** | `/camels/{id}` | Retrieves a specific camel by ID. |
| **POST** | `/camels` | Creates a new camel. |
| **PUT** | `/camels/{id}` | Updates an existing camel's details. |
| **DELETE** | `/camels/{id}` | Deletes a camel from the database. |

## 🧪 Running Tests

The solution includes **xUnit** tests to verify business logic and validation rules.

To run the tests, execute the following command in the root directory:

```bash
dotnet test
```


## 📬 Contact

* **Website:** [jacintkovacs.hu](https://jacintkovacs.hu)
* **LinkedIn:** [linkedin.com/in/jacint-kovacs/](https://www.linkedin.com/in/jacint-kovacs/)
* **Email:** kovacs.jacint02@gmail.com
---
*© 2026 Jácint Kovács. All rights reserved.*
