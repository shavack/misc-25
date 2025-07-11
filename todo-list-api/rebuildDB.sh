    rm -rf Migrations
    rm app.db app.db-wal app.db-shm
    dotnet ef migrations add InitialCreate
    dotnet ef database update