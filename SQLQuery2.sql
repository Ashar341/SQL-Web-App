DECLARE @SearchTerm Varchar(100);


SET @SearchTerm = 'PartNumber 1';


SELECT * FROM PartNumbers 
LEFT JOIN Customers 
ON PartNumbers.FKCustomers = Customers.PKCustomers
LEFT JOIN Buildings
ON  Customers.FKBuilding = Buildings.PKBuilding
WHERE PartNumber LIKE @SearchTerm;
