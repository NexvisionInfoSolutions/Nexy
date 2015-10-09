IF EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'Date'
			AND Object_ID = Object_ID(N'dbo.DayBook')
		)
BEGIN
	EXECUTE sys.sp_rename @objname = N'[dbo].[DayBook].[Date]', @newname = N'TransDate', @objtype = 'COLUMN'

	PRINT 'Date is renamed to TransDate to DayBook'
END
ELSE
	PRINT 'Date already renamed to TransDate in DayBook'
GO

IF EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'Code'
			AND Object_ID = Object_ID(N'dbo.Users')
		)
BEGIN
	ALTER TABLE [dbo].[Users] ALTER COLUMN [Code] nvarchar(150) NULL

	PRINT 'Code is altered to Users'
END
ELSE
	PRINT 'Code is already altered in Users'
GO

IF NOT EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'NomineeName'
			AND Object_ID = Object_ID(N'dbo.Users')
		)
BEGIN
	ALTER TABLE [dbo].[Users] ADD [NomineeName] nvarchar(200) NULL

	PRINT 'NomineeName is added to Users'
END
ELSE
	PRINT 'NomineeName is already added in Users'
GO

IF NOT EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'NomineeAddress'
			AND Object_ID = Object_ID(N'dbo.Users')
		)
BEGIN
	ALTER TABLE [dbo].[Users] ADD [NomineeAddress] nvarchar(250) NULL

	PRINT 'NomineeAddress is added to Users'
END
ELSE
	PRINT 'NomineeAddress is already added in Users'
GO

IF NOT EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'NomineeRelationship'
			AND Object_ID = Object_ID(N'dbo.Users')
		)
BEGIN
	ALTER TABLE [dbo].[Users] ADD [NomineeRelationship] nvarchar(100) NULL

	PRINT 'NomineeRelationship is added to Users'
END
ELSE
	PRINT 'NomineeRelationship is already added in Users'
GO

IF EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'BankInterest'
			AND Object_ID = Object_ID(N'dbo.Settings')
		)
BEGIN
	ALTER TABLE [dbo].[Settings] ALTER COLUMN [BankInterest] decimal(18,2) NULL

	PRINT 'BankInterest is altered to Settings'
END
ELSE
	PRINT 'BankInterest is already altered in Settings'
GO

IF EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'BankCharges'
			AND Object_ID = Object_ID(N'dbo.Settings')
		)
BEGIN
	ALTER TABLE [dbo].[Settings] ALTER COLUMN [BankCharges] decimal(18,2) NULL

	PRINT 'BankCharges is altered in Settings'
END
ELSE
	PRINT 'BankCharges is already altered in Settings'
GO

IF EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'AccountHeadId'
			AND Object_ID = Object_ID(N'dbo.AccountBook')
		)
BEGIN
	ALTER TABLE [dbo].[AccountBook] ALTER COLUMN [AccountHeadId] BIGINT NULL

	PRINT 'AccountHeadId is altered in AccountBook'
END
ELSE
	PRINT 'AccountHeadId is already altered in AccountBook'
GO

IF EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'InstrumentDate'
			AND Object_ID = Object_ID(N'dbo.AccBankDepositDetail')
		)
BEGIN
	ALTER TABLE [dbo].[AccBankDepositDetail] ALTER COLUMN [InstrumentDate] DateTime NULL

	PRINT 'InstrumentDate is altered in AccBankDepositDetail'
END
ELSE
	PRINT 'InstrumentDate is already altered in AccBankDepositDetail'
GO