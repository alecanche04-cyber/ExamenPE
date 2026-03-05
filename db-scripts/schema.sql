-- Esquema inicial de base de datos
CREATE DATABASE FoodCampus;
GO

USE FoodCampus;
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
