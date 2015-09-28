/*EXEC [msdb].[dbo].[sp_delete_database_backuphistory] @database_name = N'LoanManagement';
ALTER DATABASE [LoanManagement] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
USE [master];
DROP DATABASE [LoanManagement];
Truncate table Village;
truncate table Taluk;
truncate table District;
truncate table State;
truncate table Country;
truncate table Address;
truncate table Contact;
GO*/


select * from Country

insert into Country(CountryName, CountryAbbr, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'India', N'IN', '09/26/2015 23:36:27', NULL, NULL, NULL, 0, NULL, NULL), 
(N'United States of America', N'USA', '09/26/2015 23:36:27', NULL, NULL, NULL, 0, NULL, NULL), 
(N'United Arab Emirates', N'UAE', '09/26/2015 23:36:27', NULL, NULL, NULL, 0, NULL, NULL)

select * from State
Insert into State(StateName, StateAbbr, CountryId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'Kerala', N'KL', 1, '09/26/2015 23:36:27', NULL, NULL, NULL, 0, NULL, NULL), 
(N'Karnataka', N'KA', 1, '09/26/2015 23:36:27', NULL, NULL, NULL, 0, NULL, NULL), 
(N'Tamil Nadu', N'TN', 1, '09/26/2015 23:36:27', NULL, NULL, NULL, 0, NULL, NULL), 
(N'Orissa', N'OR', 1, '09/26/2015 23:41:31', NULL, NULL, NULL, 0, NULL, NULL)

select * from District
Insert into District(DistrictName, DistrictAbbr, StateId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'Ernakulam', N'EKM', 1, '09/26/2015 23:42:38', NULL, NULL, NULL, 0, NULL, NULL), 
(N'Kollam', N'KLM', 1, '09/26/2015 23:43:01', NULL, NULL, NULL, 0, NULL, NULL)

select * from Taluk
Insert into Taluk(TalukName, TalukAbbr, DistrictId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'TKL', N'lKO', 1, '09/26/2015 23:43:50', NULL, NULL, NULL, 0, NULL, NULL), 
(N'LPK', N'KOL', 2, '09/26/2015 23:44:19', NULL, NULL, NULL, 0, NULL, NULL)

select * from Village
Insert into Village(VillageName, VillageAbbr, TalukId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'kdfgk', N'lksdglk', 1, '09/26/2015 23:53:08', NULL, NULL, NULL, 0, NULL, NULL), 
(N'slsdgg', N'sdgpdf', 2, '09/26/2015 23:53:45', NULL, NULL, NULL, 0, NULL, NULL)

select * from Address
Insert into Address(Address1, Address2, City, StateId, CountryId, Zipcode, Place, Post, DistrictId, TalukId, VillageId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'Address of Owner', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, 1, 1, 1, '09/27/2015 12:17:00', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, 1, 1, 1, '09/27/2015 12:18:16', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, 1, 1, 1, '09/27/2015 12:19:09', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, 1, 1, 1, '09/27/2015 12:20:03', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, 1, 1, 1, '09/27/2015 12:41:14', NULL, NULL, NULL, 0, NULL, NULL)

select * from Contact
Insert into Contact(ContactName, Telephone1, Telephone2, Mobile1, Mobile2, Fax, Email1, Email2, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'Name of Owner', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/27/2015 12:17:00', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/27/2015 12:18:16', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/27/2015 12:19:09', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/27/2015 12:20:03', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/27/2015 12:41:14', NULL, NULL, NULL, 0, NULL, NULL)

select * from Users
Insert into Users(Code, FirstName, LastName, UserName, Password, Description, UserGroupId, IsActive, Designation, UserType, UserContactId, UserAddressId, PermanentAddressId, GuaranterAddressId, PermanentContactId, GuaranterContactId, FatherName, GuaranterName, Occupation, CompanyId, AccountHeadId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'test', N'dfsd', N'dsg', N'test', N'test123', N'dgll', 1, 0, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, '09/26/2015 23:38:31', NULL, NULL, NULL, 0, NULL, NULL)

--truncate table AccountHead
select * from AccountHead
Insert into AccountHead(AccountCode, AccountName, ScheduleId, AccountTypeId, CreditLimit, CreditDays, ContactId, AddressId, TIN, CST, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'hjjk', N'cash', 1, 1, NULL, NULL, 2, 2, NULL, NULL, '09/27/2015 12:17:00', '09/27/2015 12:17:30', 1, 1, 0, NULL, NULL), 
(N'bank', N'bank', 2, 2, NULL, NULL, 3, 3, NULL, NULL, '09/27/2015 12:18:16', NULL, 1, NULL, 0, NULL, NULL), 
(N'journal', N'debit', 3, 3, NULL, NULL, 4, 4, NULL, NULL, '09/27/2015 12:19:09', NULL, 1, NULL, 0, NULL, NULL), 
(N'credit', N'credit', 2, 4, NULL, NULL, 5, 5, NULL, NULL, '09/27/2015 12:20:03', NULL, 1, NULL, 0, NULL, NULL), 
(N'Other', N'Other', 2, 5, NULL, NULL, 6, 6, NULL, NULL, '09/27/2015 12:41:14', NULL, 1, NULL, 0, NULL, NULL)

--truncate table AccountBook
select * from AccountBook
Insert into AccountBook(BookCode, BookName, BookDescription, AccountBookTypeId, AccountHeadId, BankInterest, BankCharges, ReceiptVoucherPrefix, ReceiptVoucherSuffix, PaymentVoucherPrefix, PaymentVoucherSuffix, Status, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'Cash', N'Cash Book', NULL, 1, 1, 10, 10, N'CH', N'O', N'CH', N'P', 1, '09/27/2015 12:37:07', NULL, NULL, NULL, 0, NULL, NULL), 
(N'Bank', N'Bank Book', NULL, 2, 2, 10, 10, N'BNK', N'IF', N'OL', N'PLP', 1, '09/27/2015 12:37:32', NULL, NULL, NULL, 0, NULL, NULL), 
(N'KIL', N'JOURNAL', NULL, 3, 1, 10, 10, N'IFL', N'IF', N'IG', N'GOI', 1, '09/27/2015 12:38:04', NULL, NULL, NULL, 0, NULL, NULL)

select * from OpeningBalance
Insert into OpeningBalance(AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(1, 1, 0.00, 0.00, 0.00, 1, '09/27/2015 12:17:01', NULL, 1, NULL, 0, NULL, NULL), 
(2, 2, 0.00, 0.00, 0.00, 1, '09/27/2015 12:18:16', NULL, 1, NULL, 0, NULL, NULL), 
(3, 3, 0.00, 0.00, 0.00, 1, '09/27/2015 12:19:10', NULL, 1, NULL, 0, NULL, NULL), 
(4, 2, 0.00, 0.00, 0.00, 1, '09/27/2015 12:20:03', NULL, 1, NULL, 0, NULL, NULL), 
(5, 2, 0.00, 0.00, 0.00, 1, '09/27/2015 12:41:14', NULL, 1, NULL, 0, NULL, NULL)

select * from DayBook