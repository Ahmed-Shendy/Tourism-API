USE Tourism;

CREATE TABLE Users (
    Id VARCHAR(255) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Country VARCHAR(100),
    Phone VARCHAR(50),
    BirthDate DATE,
    Role VARCHAR(50),
    Gender VARCHAR(50),
    Photo VARCHAR(355),
    Tourguid_id VARCHAR(255),
    FOREIGN KEY (Tourguid_id) REFERENCES Users(Id) 
);



CREATE TABLE Governorates (
    Name VARCHAR(255) PRIMARY KEY,
    Photo VARCHAR(355)
);


CREATE TABLE Places (
    Name VARCHAR(255) PRIMARY KEY,
    Photo VARCHAR(355),
    Location VARCHAR(355),
    Description TEXT,
    Rate DECIMAL(5,2),
    Government_name VARCHAR(255),
    FOREIGN KEY (Government_name) REFERENCES Governorates(Name) ON DELETE SET NULL
);


CREATE TABLE Tourguid_Places (
    Tourguid_id VARCHAR(255),
    Place_Name VARCHAR(255),
    PRIMARY KEY (Tourguid_id, Place_Name),
    FOREIGN KEY (Tourguid_id) REFERENCES Users(Id) ,
    FOREIGN KEY (Place_Name) REFERENCES Places(Name) 
);


CREATE TABLE Comments (
    Id INT PRIMARY KEY Identity(1,1),
    Content varchar(max) NOT NULL,
    Place_Name VARCHAR(255),
    FOREIGN KEY (Place_Name) REFERENCES Places(Name) ON DELETE CASCADE
);


CREATE TABLE Type_of_Tourism (
    Name VARCHAR(255) PRIMARY KEY,
    Photo VARCHAR(255)
);


CREATE TABLE Type_of_Tourism_Places (
    Tourism_Name VARCHAR(255),
    Place_Name VARCHAR(255),
    PRIMARY KEY (Tourism_Name, Place_Name),
    FOREIGN KEY (Tourism_Name) REFERENCES Type_of_Tourism(Name) ,
    FOREIGN KEY (Place_Name) REFERENCES Places(Name) 
);


CREATE TABLE Programs (
    Name VARCHAR(255) PRIMARY KEY,
    Description TEXT,
    Price DECIMAL(10,2) CHECK (Price >= 0),
    Activities TEXT
);


CREATE TABLE Programs_Photo (
    Program_Name VARCHAR(255),
    Photo VARCHAR(355),
    PRIMARY KEY (Program_Name, Photo),
    FOREIGN KEY (Program_Name) REFERENCES Programs(Name) 
);


CREATE TABLE Program_Places (
    Program_Name VARCHAR(255),
    Place_Name VARCHAR(255),
    PRIMARY KEY (Program_Name, Place_Name),
    FOREIGN KEY (Program_Name) REFERENCES Programs(Name) ON DELETE CASCADE,
    FOREIGN KEY (Place_Name) REFERENCES Places(Name) ON DELETE CASCADE
);
