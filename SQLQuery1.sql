CREATE TABLE Buildings (
    PKBuilding INT IDENTITY(1,1) PRIMARY KEY,
    Building NVARCHAR(255) NOT NULL
);

-- Create the Customers table with the foreign key
CREATE TABLE Customers (
    PKCustomers INT IDENTITY(1,1) PRIMARY KEY,
    Customer NVARCHAR(255) NOT NULL,
    Prefix NVARCHAR(5) NOT NULL,
    FKBuilding INT FOREIGN KEY REFERENCES Buildings(PKBuilding)
);

--Create the third table with foregeign key also

CREATE TABLE PartNumbers (
	PKPartNumber INT IDENTITY(1,1) PRIMARY KEY,
	PartNumber NVARCHAR(50),
	FKCustomers INT FOREIGN KEY REFERENCES Customers(PKCustomers),
	Available BIT
);

-- Example data for Buildings table
INSERT INTO Buildings (Building) VALUES ('Building A'), ('Building B');

-- Example data for Customers table
INSERT INTO Customers (Customer, Prefix, FKBuilding) VALUES
    ('Customer 1', 'C1', 1),
    ('Customer 2', 'C2', 2),
    ('Customer 3', 'C2', 2);

-- Example data for PartNumbers table
INSERT INTO PartNumbers (PartNumber, FKCustomers, Available) VALUES
    ('PartNumber 1', 1, 1),
    ('PartNumber 2', 1, 1),
    ('PartNumber 3', 2, 1),
    ('PartNumber 4', 2, 0);
