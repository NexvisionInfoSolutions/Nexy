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

IF NOT EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'LandMark'
			AND Object_ID = Object_ID(N'dbo.Address')
		)
BEGIN
	ALTER TABLE [dbo].[Address] ADD [LandMark] nvarchar(150) NULL

	PRINT 'LandMark is added to Address'
END
ELSE
	PRINT 'LandMark is already added in Address'
GO

ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountSchedule_AssetScheduleId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountBook_BankBookId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountBook_CashBookId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.Company_CompanyId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountHead_InterestAccountId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountSchedule_PLExpenseScheduleId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountSchedule_PLIncomeScheduleId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountHead_RoundOffAccountId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountSchedule_SundryCreditorAccountId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountSchedule_SundryDebtorAccountId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountSchedule_TradingExpenseScheduleId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.AccountSchedule_TradingIncomeScheduleId]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.Users_CreatedBy]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.Users_DeletedBy]
GO
ALTER TABLE [dbo].[Settings]
DROP CONSTRAINT [FK_dbo.Settings_dbo.Users_ModifiedBy]
GO
EXECUTE [sp_rename]
	@objname  = N'[dbo].[PK_dbo.Settings]',
	@newname  = N'tmp_07acad12283546e7b3e8da033c803e4f',
	@objtype  = 'OBJECT'
GO
EXECUTE [sp_rename]
	@objname  = N'[dbo].[Settings]',
	@newname  = N'tmp_4bf3a1b6ecfb4491808fb50374051256',
	@objtype  = 'OBJECT'
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings] (
	[SettingsId] bigint NOT NULL,
	[CompanyId] bigint NULL,
	[BankInterest] decimal(18, 2) NULL,
	[BankCharges] decimal(18, 2) NULL,
	[AssetScheduleId] bigint NULL,
	[RoundOffAccountId] bigint NULL,
	[SundryDebtorAccountId] bigint NULL,
	[SundryCreditorAccountId] bigint NULL,
	[PLExpenseScheduleId] bigint NULL,
	[PLIncomeScheduleId] bigint NULL,
	[TradingExpenseScheduleId] bigint NULL,
	[TradingIncomeScheduleId] bigint NULL,
	[CashBookId] bigint NULL,
	[BankBookId] bigint NULL,
	[InterestAccountId] bigint NULL,
	[CreatedOn] datetime NULL,
	[ModifiedOn] datetime NULL,
	[CreatedBy] bigint NULL,
	[ModifiedBy] bigint NULL,
	[IsDeleted] bit NOT NULL,
	[DeletedBy] bigint NULL,
	[DeletedOn] datetime NULL,
	CONSTRAINT [PK_dbo.Settings] PRIMARY KEY([SettingsId]) WITH (FILLFACTOR=100,
		DATA_COMPRESSION = NONE) ON [PRIMARY]
)
GO
CREATE INDEX [IX_AssetScheduleId]
 ON [dbo].[Settings] ([AssetScheduleId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_BankBookId]
 ON [dbo].[Settings] ([BankBookId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_CashBookId]
 ON [dbo].[Settings] ([CashBookId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_CompanyId]
 ON [dbo].[Settings] ([CompanyId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_InterestAccountId]
 ON [dbo].[Settings] ([InterestAccountId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_PLExpenseScheduleId]
 ON [dbo].[Settings] ([PLExpenseScheduleId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_PLIncomeScheduleId]
 ON [dbo].[Settings] ([PLIncomeScheduleId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_RoundOffAccountId]
 ON [dbo].[Settings] ([RoundOffAccountId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_SundryCreditorAccountId]
 ON [dbo].[Settings] ([SundryCreditorAccountId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_SundryDebtorAccountId]
 ON [dbo].[Settings] ([SundryDebtorAccountId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_TradingExpenseScheduleId]
 ON [dbo].[Settings] ([TradingExpenseScheduleId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
CREATE INDEX [IX_TradingIncomeScheduleId]
 ON [dbo].[Settings] ([TradingIncomeScheduleId])
WITH (FILLFACTOR=100,
	DATA_COMPRESSION = NONE)
ON [PRIMARY]
GO
ALTER INDEX [IX_AssetScheduleId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_BankBookId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_CashBookId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_CompanyId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_InterestAccountId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_PLExpenseScheduleId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_PLIncomeScheduleId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_RoundOffAccountId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_SundryCreditorAccountId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_SundryDebtorAccountId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_TradingExpenseScheduleId]
ON [dbo].[Settings]
DISABLE
GO
ALTER INDEX [IX_TradingIncomeScheduleId]
ON [dbo].[Settings]
DISABLE
GO
INSERT INTO [dbo].[Settings] (
	[SettingsId],
	[CompanyId],
	[BankInterest],
	[BankCharges],
	[AssetScheduleId],
	[RoundOffAccountId],
	[SundryDebtorAccountId],
	[SundryCreditorAccountId],
	[PLExpenseScheduleId],
	[PLIncomeScheduleId],
	[TradingExpenseScheduleId],
	[TradingIncomeScheduleId],
	[CashBookId],
	[BankBookId],
	[InterestAccountId],
	[CreatedOn],
	[ModifiedOn],
	[CreatedBy],
	[ModifiedBy],
	[IsDeleted],
	[DeletedBy],
	[DeletedOn])
SELECT
	[SettingsId],
	[CompanyId],
	[BankInterest],
	[BankCharges],
	[AssetScheduleId],
	[RoundOffAccountId],
	[SundryDebtorAccountId],
	[SundryCreditorAccountId],
	[PLExpenseScheduleId],
	[PLIncomeScheduleId],
	[TradingExpenseScheduleId],
	[TradingIncomeScheduleId],
	[CashBookId],
	[BankBookId],
	[InterestAccountId],
	[CreatedOn],
	[ModifiedOn],
	[CreatedBy],
	[ModifiedBy],
	[IsDeleted],
	[DeletedBy],
	[DeletedOn]
FROM [dbo].[tmp_4bf3a1b6ecfb4491808fb50374051256]
GO
ALTER INDEX ALL
ON [dbo].[Settings]
REBUILD
GO
DROP TABLE [dbo].[tmp_4bf3a1b6ecfb4491808fb50374051256]
GO
