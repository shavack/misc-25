# To-Do List API

This project is a simple To-Do List API built with .NET 8. It allows users to manage tasks through a RESTful API. The API supports the following operations:

## API Endpoints

- **GET /tasks**: Retrieves a list of all tasks.
- **GET /tasks/{id}**: Retrieves a specific task by its ID.
- **POST /tasks**: Creates a new task.
- **PUT /tasks/{id}**: Updates an existing task by its ID.
- **DELETE /tasks/{id}**: Deletes a task by its ID.

## Setup Instructions

1. Clone the repository:
   ```
   git clone <repository-url>
   ```

2. Navigate to the project directory:
   ```
   cd todo-list-api
   ```

3. Restore the project dependencies:
   ```
   dotnet restore
   ```

4. Run the application:
   ```
   dotnet run
   ```

The API will be available at `http://localhost:5000`.

## Database

This project uses an InMemory database for data storage, which is suitable for development and testing purposes. 

## Technologies Used

- .NET 8
- Entity Framework Core
- InMemory Database

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.