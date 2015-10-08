/*EXEC [msdb].[dbo].[sp_delete_database_backuphistory] @database_name = N'LoanManagement';
ALTER DATABASE [LoanManagement] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
USE [master];
DROP DATABASE [LoanManagement];
truncate table Address
truncate table Contact
truncate table User
Truncate table Village;
truncate table Taluk;
truncate table District;
truncate table State;
truncate table Country;
truncate table Address;
truncate table Contact;
GO*/

select * from Country

insert into Country(CountryName, CountryAbbr, IsDeleted ) VALUES
(N'India', N'IN',0), 
(N'United States of America', N'USA',0), 
(N'United Arab Emirates', N'UAE',0)

select * from State
Insert into State(StateName, StateAbbr, CountryId, IsDeleted) VALUES
(N'Kerala', N'KL', 1,0), 
(N'Karnataka', N'KA', 1,0), 
(N'Tamil Nadu', N'TN', 1,0)

select * from District
Insert into District(DistrictName, DistrictAbbr, StateId, IsDeleted) VALUES
(N'Ernakulam', N'EKM', 1,0), 
(N'Thiruvananthapuram', N'TVM', 1,0), 
(N'KA1', N'KA1', 2,0), 
(N'KA2', N'KA2', 2,0), 
(N'TND1', N'TND1', 3,0), 
(N'TND2', N'TND2', 3,0)

select * from Taluk
Insert into Taluk(TalukName, TalukAbbr, DistrictId, IsDeleted) VALUES
(N'EKMTK1', N'EKMTk1', 1,0), 
(N'EKMTK2', N'EKMTk2', 1,0), 
(N'TVMTK1', N'TVMTk1', 2,0), 
(N'TVMTK2', N'TVMTk2', 2,0)

select * from Village
Insert into Village(VillageName, VillageAbbr, TalukId, IsDeleted) VALUES
(N'EKMV1', N'EKMV1', 1,0), 
(N'EKMV2', N'EKMV2', 1,0), 
(N'dd', N'fff', 2,0), 
(N'fhd', N'dh', 2,0)

select * from Address
Insert into Address(Address1, Address2, City, StateId, CountryId, Zipcode, Place, Post, DistrictId, TalukId, VillageId, IsDeleted) VALUES
(N'Address of Owner', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0)

select * from Contact
Insert into Contact(ContactName, Telephone1, Telephone2, Mobile1, Mobile2, Fax, Email1, Email2, IsDeleted) VALUES
(N'Name of Owner', NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,0)

select * from Users
Insert into Users(Code, FirstName, LastName, UserName, Password, Description, UserGroupId, IsActive, Designation, UserType, UserContactId, UserAddressId, PermanentAddressId, GuaranterAddressId, PermanentContactId, GuaranterContactId, FatherName, GuaranterName, Occupation, CompanyId, AccountHeadId, IsDeleted) VALUES
(N'EMP-01', N'test', N'admin', N'adminuser', N'test123', N'1344', 1, 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL,0)

--truncate table AccountHead
select * from AccountHead
Insert into AccountHead( AccountCode, AccountName, ScheduleId, AccountTypeId, CreditLimit, CreditDays, ContactId, AddressId, TIN, CST, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
( N'AC_HD_#01', N'CASH', 9, 1, NULL, NULL, 3, 3, NULL, NULL, '10/03/2015 13:01:47', NULL, 1, NULL, 0, NULL, NULL), 
( N'AC_HD_#02', N'BANK', 3, 2, NULL, NULL, 3, 3, NULL, NULL, '10/03/2015 13:02:20', NULL, 1, NULL, 0, NULL, NULL), 
( N'AC_HD_#03', N'Debit', 2, 3, NULL, NULL, 4, 4, NULL, NULL, '10/03/2015 13:02:38', NULL, 1, NULL, 0, NULL, NULL), 
( N'AC_HD_#04', N'Credit', 2, 4, NULL, NULL, 5, 5, NULL, NULL, '10/03/2015 13:03:01', NULL, 1, NULL, 0, NULL, NULL), 
( N'AC_HD_#05', N'Other', 2, 5, NULL, NULL, 6, 6, NULL, NULL, '10/03/2015 13:03:23', NULL, 1, NULL, 0, NULL, NULL)

--truncate table AccountBook
select * from AccountBook
Insert into AccountBook(BookCode, BookName, BookDescription, AccountBookTypeId, AccountHeadId, BankInterest, BankCharges, ReceiptVoucherPrefix, ReceiptVoucherSuffix, PaymentVoucherPrefix, PaymentVoucherSuffix, Status, IsDeleted) VALUES
(N'BK/01/15', N'Cash Book', NULL, 1, 1, 10, 10, N'C/', N'/15', N'P/', N'/15', 1,0), 
(N'BK/02/15', N'Bank Book', NULL, 2, 2, 10, 10, N'B/', N'/15', N'BP/', N'/15', 1,0), 
(N'BK/03/15', N'Journal Book', NULL, 3, 1, 10, 10, N'J/', N'/15', N'JP/', N'/15', 1,0)

select * from OpeningBalance
Insert into OpeningBalance(AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, IsDeleted) VALUES
(1, 9, 0.00, 0.00, 0.00, 1,0), 
(2, 3, 0.00, 0.00, 0.00, 1,0), 
(3, 2, 0.00, 0.00, 0.00, 1,0), 
(4, 2, 0.00, 0.00, 0.00, 1,0), 
(5, 2, 0.00, 0.00, 0.00, 1,0)

select * from DayBook