CREATE DATABASE EV
USE EV

-- Attendee Table for Registration
CREATE TABLE Attendees (
    AttendeeID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    ContactNo NVARCHAR(15) NOT NULL,
    Preferences NVARCHAR(MAX) -- JSON or text-based preferences
);

SELECT * FROM Attendees;

-- Event Booking Table
CREATE TABLE EventBooking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    AttendeeID INT NOT NULL,
    EventID INT NOT NULL, -- Assume Event table exists
    TicketType NVARCHAR(50), -- Example: 'VIP', 'General Admission'
    BookingDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AttendeeID) REFERENCES Attendees(AttendeeID)
);

SELECT * FROM EventBooking;

-- Tickets and Check-In
CREATE TABLE EventTickets (
    TicketID INT IDENTITY(1,1) PRIMARY KEY,
    BookingID INT NOT NULL,
    QRCode NVARCHAR(MAX), -- Encrypted QR Code or unique link
    CheckInStatus BIT DEFAULT 0, -- 0 = Not Checked In, 1 = Checked In
    FOREIGN KEY (BookingID) REFERENCES EventBooking(BookingID)
);

SELECT * FROM EventTickets;

-- Feedback and Ratings for Events
CREATE TABLE AttendeeFeedbacks (
    FeedbackID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL, -- Reference to Event table
    AttendeeID INT NOT NULL,
    FeedbackText NVARCHAR(MAX),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    FeedbackDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AttendeeID) REFERENCES Attendees(AttendeeID)
);

SELECT * FROM AttendeeFeedbacks;



-- Organizer Registration Table
CREATE TABLE Organizers (
    OrganizerID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    ContactNo NVARCHAR(15) NOT NULL,
    OrganizationName NVARCHAR(100)
);

SELECT * FROM Organizers;

-- Event Creation and Management Table
CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganizerID INT NOT NULL,
    EventTitle NVARCHAR(200) NOT NULL,
    EventDescription NVARCHAR(MAX),
    EventLocation NVARCHAR(200),
    EventStartDate DATETIME NOT NULL,
    EventEndDate DATETIME,
    TicketCategories NVARCHAR(MAX), -- Example JSON: {"VIP": 100, "General": 500}
    Status NVARCHAR(50) DEFAULT 'Pending', -- 'Pending', 'Approved', 'Rejected'
    FOREIGN KEY (OrganizerID) REFERENCES Organizers(OrganizerID)
);

SELECT * FROM Events;

ALTER TABLE Events
ADD EventCategory VARCHAR(255);







-- Inserting a single simple dummy event data into Events table
INSERT INTO Events (OrganizerID, EventTitle, EventDescription, EventLocation, EventStartDate, EventEndDate, TicketCategories, EventCategory, Status)
VALUES
(2, 'Music Concert', 'A live music concert featuring popular bands.', 'New York City, NY', '2024-12-15 18:00:00', '2024-12-15 23:00:00', '{"VIP": 200, "General": 1000}', 'Music', 'Approved');

-- Insert a dummy organizer into the Organizers table
INSERT INTO Organizers (UserName, Email, Password, ContactNo, OrganizationName)
VALUES 
('john_doe', 'john.doe@example.com', 'password123', '1234567890', 'Doe Events');

-- Insert a dummy user into the Users table
INSERT INTO Users (UserName, Email, Password, ContactNo, UserType, Status, RegistrationDate)
VALUES 
('john_doe', 'john.doe@example.com', 'password123', '1234567890', 'Attendee', 'Active', GETDATE());










-- Ticket and Sales Management
CREATE TABLE TicketSales (
    SaleID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    TicketType NVARCHAR(50), -- 'VIP', 'General Admission', etc.
    QuantitySold INT,
    TotalAmount DECIMAL(10,2),
    SaleDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (EventID) REFERENCES Events(EventID)
);

SELECT * FROM TicketSales;

-- Attendee Communication
CREATE TABLE OrganizerMessages (
    MessageID INT IDENTITY(1,1) PRIMARY KEY,
    OrganizerID INT NOT NULL,
    AttendeeID INT NOT NULL,
    Message NVARCHAR(MAX),
    SentDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (OrganizerID) REFERENCES Organizers(OrganizerID),
    FOREIGN KEY (AttendeeID) REFERENCES Attendees(AttendeeID)
);

SELECT * FROM OrganizerMessages;




-- Vendor Registration Table
CREATE TABLE Vendors (
    VendorID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL, -- Encrypted password
    ContactNo NVARCHAR(15), -- Optional contact number
    BusinessType NVARCHAR(100) NOT NULL, -- Example: Catering, Security
    ServicesOffered NVARCHAR(MAX), -- Details of services offered
    RegistrationDate DATETIME DEFAULT GETDATE(), -- Date of registration
    Status NVARCHAR(50) DEFAULT 'Active' -- Status of the vendor: 'Active', 'Pending', 'Blocked'
);

-- Example Query: Retrieve all active vendors
SELECT * 
FROM Vendors
WHERE Status = 'Active';

-- Example Insert Query: Add a new vendor
INSERT INTO Vendors (UserName, Email, Password, ContactNo, BusinessType, ServicesOffered)
VALUES 
('Catering Solutions', 'vendor@example.com', HASHBYTES('SHA2_256', 'SecurePass123'), '1234567890', 'Catering', 'Full-service event catering');



-- Sponsor Registration Table
CREATE TABLE Sponsors (
    SponsorID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL, -- Sponsor representative or company name
    Email NVARCHAR(100) UNIQUE NOT NULL, -- Unique email for sponsorship communication
    Password NVARCHAR(255) NOT NULL, -- Encrypted password
    ContactNo NVARCHAR(15), -- Optional contact number
    CompanyName NVARCHAR(100) NOT NULL, -- Name of the sponsor's company
    SponsoredEvents NVARCHAR(MAX), -- Example JSON: {"EventIDs": [1, 2, 3]}
    Budget DECIMAL(10, 2), -- Sponsorship budget in monetary terms
    RegistrationDate DATETIME DEFAULT GETDATE(), -- Date of sponsor registration
    Status NVARCHAR(50) DEFAULT 'Active' -- Status of the sponsor: 'Active', 'Pending', 'Blocked'
);

-- Example Query: Retrieve all sponsors with active status
SELECT * 
FROM Sponsors
WHERE Status = 'Active';

-- Example Insert Query: Add a new sponsor
INSERT INTO Sponsors (UserName, Email, Password, ContactNo, CompanyName, SponsoredEvents, Budget)
VALUES 
('Jane Doe', 'sponsor@example.com', HASHBYTES('SHA2_256', 'SponsorPass456'), '0987654321', 'Tech Corp', '{"EventIDs": [1, 2]}', 10000.00);



-- Create Admin table
CREATE TABLE Admin (
    AdminID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    ContactNo NVARCHAR(15) NOT NULL
);

-- Select all from Admin table
SELECT * FROM Admin;

-- Create UserActivity table
CREATE TABLE UserActivity (
    UserActivityID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100),
    Activity NVARCHAR(255),
    ActivityDate DATETIME
);

-- Select all from UserActivity table
SELECT * FROM UserActivity;

-- Create Complaints table
CREATE TABLE Complaints (
    ComplaintID INT IDENTITY(1,1) PRIMARY KEY,
    ComplaintText NVARCHAR(255),
    ComplaintDate DATETIME,
    UserID INT -- Assuming a relation to a User table
);

-- Select all from Complaints table
SELECT * FROM Complaints;

-- Create Accounts table
CREATE TABLE Accounts (
    AccountID INT IDENTITY(1,1) PRIMARY KEY,
    AccountName NVARCHAR(100),
    Status NVARCHAR(50) -- 'Pending', 'Approved', 'Rejected', etc.
);

-- Create PendingAccounts table
CREATE TABLE PendingAccounts (
    AccountID INT IDENTITY(1,1) PRIMARY KEY,
    AccountName NVARCHAR(100),
    Status NVARCHAR(50) -- 'Pending' by default
);

-- Select all from PendingAccounts table
SELECT * FROM PendingAccounts;

-- Update Accounts table example
UPDATE Accounts
SET Status = 'Approved'
WHERE AccountID = 1;

-- Create Feedbacks table
CREATE TABLE Feedbacks (
    FeedbackID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing FeedbackID
    FeedbackText NVARCHAR(MAX), -- Text of the feedback
    Rating INT -- Rating (Assumed to be an integer, can be 1-5 or any scale you prefer)
);

-- Select FeedbackID, FeedbackText, Rating from Feedbacks table
SELECT FeedbackID, FeedbackText, Rating FROM Feedbacks;



CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    ContactNo NVARCHAR(15) NOT NULL,
    UserType NVARCHAR(50) NOT NULL, -- 'Admin', 'Attendee', 'Organizer', 'Vendor', 'Sponsor'
    Status NVARCHAR(50) DEFAULT 'Active', -- 'Active', 'Pending', 'Blocked'
    RegistrationDate DATETIME DEFAULT GETDATE()
);

-- Example Query to Retrieve Specific UserType
SELECT * 
FROM Users
WHERE UserType = 'Attendee';

-- Example Insert to Add a New User
INSERT INTO Users (UserName, Email, Password, ContactNo, UserType)
VALUES ('John Doe', 'john@example.com', HASHBYTES('SHA2_256', 'password123'), '1234567890', 'Organizer');


ALTER TABLE Admin
ADD FOREIGN KEY (Email) REFERENCES Users(Email);


ALTER TABLE Attendees
ADD FOREIGN KEY (Email) REFERENCES Users(Email);


ALTER TABLE Organizers
ADD FOREIGN KEY (Email) REFERENCES Users(Email);


-- Check for emails in Vendors table that don't exist in Users table
SELECT Email
FROM Vendors
WHERE Email NOT IN (SELECT Email FROM Users);

-- Check for emails in Sponsors table that don't exist in Users table
SELECT Email
FROM Sponsors
WHERE Email NOT IN (SELECT Email FROM Users);



-- Example of inserting missing vendors' emails into Users table
INSERT INTO Users (UserName, Email, Password, ContactNo, UserType)
SELECT 'VendorName', Email, 'password', 'contact', 'Vendor'
FROM Vendors
WHERE Email NOT IN (SELECT Email FROM Users);


-- Add foreign key to Vendors table
ALTER TABLE Vendors
ADD FOREIGN KEY (Email) REFERENCES Users(Email);

-- Add foreign key to Sponsors table
ALTER TABLE Sponsors
ADD FOREIGN KEY (Email) REFERENCES Users(Email);


-- Check for emails in Sponsors table that don't exist in Users table
SELECT Email
FROM Sponsors
WHERE Email NOT IN (SELECT Email FROM Users);


-- Insert missing sponsor emails into Users table
INSERT INTO Users (UserName, Email, Password, ContactNo, UserType)
SELECT 'SponsorName', Email, 'password', 'contact', 'Sponsor'
FROM Sponsors
WHERE Email NOT IN (SELECT Email FROM Users);

-- Add foreign key to Sponsors table
ALTER TABLE Sponsors
ADD FOREIGN KEY (Email) REFERENCES Users(Email);



