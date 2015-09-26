select * from Users
insert into Users(Code, FirstName, LastName, UserName, Password, Description, UserGroupId, IsActive, Designation, UserType, UserContactId, UserAddressId, PermanentAddressId, GuaranterAddressId, PermanentContactId, GuaranterContactId, FatherName, GuaranterName, Occupation, CompanyId, AccountHeadId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'test', N'fhf', N'dfgd', N'test', N'test123', N'dgd', 1, 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, '09/20/2015 18:36:03', NULL, NULL, NULL, 0, NULL, NULL)

select * from Address
insert into Address(Address1, Address2, City, StateId, CountryId, Zipcode, Place, Post, District, SubDivision, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'Address of Owner', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:37:02', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:37:40', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:38:34', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:39:00', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:39:20', NULL, NULL, NULL, 0, NULL, NULL)

select * from Contact
insert into Contact(ContactName, Telephone1, Telephone2, Mobile1, Mobile2, Fax, Email1, Email2, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'Name of Owner', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:37:02', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:37:40', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:38:34', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:39:00', NULL, NULL, NULL, 0, NULL, NULL), 
(NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '09/20/2015 18:39:20', NULL, NULL, NULL, 0, NULL, NULL)

--truncate table AccountHead
select * from AccountHead
insert into AccountHead(AccountCode, AccountName, ScheduleId, AccountTypeId, CreditLimit, CreditDays, ContactId, AddressId, TIN, CST, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'tste', N'tewgs', 1, 2, NULL, NULL, 2, 2, NULL, NULL, '09/20/2015 18:37:02', '09/20/2015 18:37:21', 1, 1, 0, NULL, NULL), 
(N'dhdf', N'dfhd', 2, 1, NULL, NULL, 3, 3, NULL, NULL, '09/20/2015 18:37:40', NULL, 1, NULL, 0, NULL, NULL), 
(N'dsfs', N'sds', 1, 3, NULL, NULL, 4, 5, NULL, NULL, '09/20/2015 18:38:34', NULL, 1, NULL, 0, NULL, NULL), 
(N'ddsd', N'sdfsd', 2, 4, NULL, NULL, 5, 6, NULL, NULL, '09/20/2015 18:39:00', NULL, 1, NULL, 0, NULL, NULL), 
(N'dsfs', N'sdfs', 4, 5, NULL, NULL, 6, 7, NULL, NULL, '09/20/2015 18:39:20', NULL, 1, NULL, 0, NULL, NULL)

--truncate table AccountBook
select * from AccountBook
insert into AccountBook(BookCode, BookName, BookDescription, AccountBookTypeId, AccountHeadId, BankInterest, BankCharges, ReceiptVoucherPrefix, ReceiptVoucherSuffix, PaymentVoucherPrefix, PaymentVoucherSuffix, Status, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(N'ss', N'ssg', N'sdgsdg', 2, 1, 10, 10, N'34', N'54', N'64', N'65', 1, '09/20/2015 18:40:23', '09/20/2015 18:41:41', NULL, NULL, 0, NULL, NULL), 
(N'sdfs', N'sdfs', N'sdfs', 1, 1, 10, 10, N'43', N'54', N'53', N'534', 1, '09/20/2015 18:40:48', NULL, NULL, NULL, 0, NULL, NULL), 
(N'sdfs', N'sdfs', N'sdfsd', 3, 1, 10, 10, N'54', N'65', N'76', N'85', 1, '09/20/2015 18:41:27', NULL, NULL, NULL, 0, NULL, NULL)

insert into OpeningBalance(AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted, DeletedBy, DeletedOn) VALUES
(1, 1, 0.00, 0.00, 0.00, 1, '09/20/2015 18:37:03', NULL, 1, NULL, 0, NULL, NULL), 
(2, 2, 0.00, 0.00, 0.00, 1, '09/20/2015 18:37:40', NULL, 1, NULL, 0, NULL, NULL), 
(3, 1, 0.00, 0.00, 0.00, 1, '09/20/2015 18:38:34', NULL, 1, NULL, 0, NULL, NULL), 
(4, 2, 0.00, 0.00, 0.00, 1, '09/20/2015 18:39:00', NULL, 1, NULL, 0, NULL, NULL), 
(5, 4, 0.00, 0.00, 0.00, 1, '09/20/2015 18:39:20', NULL, 1, NULL, 0, NULL, NULL)