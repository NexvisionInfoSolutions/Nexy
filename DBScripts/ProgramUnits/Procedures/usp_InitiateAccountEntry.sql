

----------------------------------------------------------
-- Author: Digeesh
-- Date: 11/Nov/14
-- Purpose: procedure usp_RptEmployeeShifts to get all the employee shifts
-- Returns the list of all users
----------------------------------------------------------
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE type = 'P'
			AND NAME = 'usp_RptDepositSummary'
		)
	DROP PROCEDURE usp_RptDepositSummary
GO

CREATE PROCEDURE [dbo].[usp_RptDepositSummary] @RptParameters dbo.RptParameter readonly	  
  ,@vCompanyId bigint  
AS

Declare @vCheckByLoanId bit = 0;
Declare @vCheckByLoanMember bit = 0;
Declare @vCheckByLoanDate bit = 0;
Declare @vCheckByLoanStatus bit = 0;
Declare @vCheckByCharacter bit = 0;


IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityType = 'L' -- Loan Ids
		)
	SET @vCheckByLoanId = 1;

IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityType = 'U' -- Member Ids
		)
	SET @vCheckByLoanMember = 1;

IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityType = 'D' -- Date range for repayment dates
		)
	SET @vCheckByLoanDate = 1;

IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityType = 'S' -- Loan Status
		)
	SET @vCheckByLoanStatus = 1;
  
IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityType = 'M' -- Mixed mode
		)
	SET @vCheckByCharacter = 1;  
  
Select distinct lf.DepositId, lf.UserId, lf.DepositAmount, lf.TotalInstallments , lf.InteresRate, lf.InstallmentAmount, lf.CreatedOn as DepositDate, lf.MaturityDate as MaturityDate, lf.MatureAmount
,sum(lr.WithdrawalAmount ) over (partition by lr.DepositId) as TotalWithdrawnAmountPerDeposit
,sum(lr.WithdrawalAmount) over (partition by lf.UserId) as TotalPaidAmountPerUser
--,sum(lr.RepaymentAmount) over (partition by lr.RepaymentDate) as TotalPaidAmountPerDate
,Min(isnull(lr.BalanceDepositAmount, lf.DepositAmount )) over (partition by lr.DepositId) as BalanceDepositAmount
--,Min(isnull(lr. , lf.TotalInstallments)) over (partition by lr.LoanId) as BalanceLoanInstallments
,Sum(lr.InterestAmount) over (partition by lr.DepositId) as TotalInterestPaidAmountPerDeposit
,Sum(lr.InterestAmount) over (partition by lf.UserId) as TotalInterestPaidAmountPerUser
,lf.TotalInstallments
,u.FirstName
,u.LastName
,u.UserType
,u.FatherName
,u.GuaranterName
, isnull(uaddr.Address1, '') + '<br />' + isnull(uaddr.Address2, '') + '<br />' + isnull(uaddr.Place, '') + '<br />' + isnull(uaddr.Post, '') + '<br />' + isnull(uaddr.District, '') + '<br />' as UserAddress
, uc.Telephone1 as UserPhone
, uc.Mobile1 as UserMobile
, uc.Email1 as UserEmail
, isnull(upAddr.Address1, '') + '<br />' + isnull(upAddr.Address2, '') + '<br />' + isnull(upAddr.Place, '') + '<br />' + isnull(upAddr.Post, '') + '<br />' + isnull(upAddr.District, '') + '<br />' as UserPermanentAddress
, upc.Telephone1 as PermanentPhone
, upc.Mobile1 as PermanentMobile
, upc.Email1 as PermanentEmail
from DepositInfo lf
left join WithdrawalInfo lr on lr.DepositId = lf.DepositId and lr.Status <> 4 and isnull(lr.IsDeleted, 0) = 0
left join Users u on u.UserID = lf.UserId and u.UserType = 3 -- Members only
left join Address uaddr on uaddr.AddressId = u.UserAddressId
left join Address upAddr on upAddr.AddressId = u.PermanentAddressId
left join Contact uc on uc.ContactId = u.UserContactId
left join contact upc on upc.ContactId = u.PermanentContactId
where isnull(lf.IsDeleted, 0) = 0
AND (@vCompanyId = 0 or u.CompanyId = @vCompanyId)
AND (
		@vCheckByLoanId = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE pm.EntityId = lf.DepositId
				AND pm.EntityType = 'L'
			)
		)
	AND (
		@vCheckByLoanMember = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE pm.EntityId = u.UserID
				AND pm.EntityType = 'U'
			)
		)
	AND (
		@vCheckByLoanDate = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE dateadd(dd, 0, datediff(dd, 0 , lr.WithdrawalDate )) between pm.EntityStartDate and pm.EntityEndDate
				AND pm.EntityType = 'D'
			)
		)
  AND (
		@vCheckByLoanStatus = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE pm.EntityIntVal = lf.Status
				AND pm.EntityType = 'S'
			)
		)
  AND (@vCheckByCharacter = 0
  OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE ((len(isnull(u.FirstName, '')) > 0 AND u.FirstName like pm.EntityStrVal+ '%')
        OR (len(isnull(u.LastName, '')) > 0 AND u.LastName like pm.EntityStrVal+ '%')
        OR (len(isnull(u.UserName, '')) > 0 AND u.UserName like pm.EntityStrVal+ '%')
        OR (len(isnull(uaddr.Address1, '')) > 0 AND uaddr.Address1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uaddr.Address2, '')) > 0 AND uaddr.Address2 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uaddr.Place , '')) > 0 AND uaddr.Place like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddr.Address1, '')) > 0 AND upAddr.Address1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddr.Address2, '')) > 0 AND upAddr.Address2 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddr.Place , '')) > 0 AND upAddr.Place like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uc.Telephone1 , '')) > 0 AND uc.Telephone1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uc.Mobile1 , '')) > 0 AND uc.Mobile1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uc.Email1 , '')) > 0 AND uc.Email1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upc.Telephone1 , '')) > 0 AND upc.Telephone1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upc.Mobile1 , '')) > 0 AND upc.Mobile1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upc.Email1 , '')) > 0 AND upc.Email1 like '%' + pm.EntityStrVal + '%')
        OR (cast(lf.DepositId as varchar) like '%' + pm.EntityStrVal + '%')
        )
				AND pm.EntityType = 'M'
			)
    )
GO

PRINT 'usp_RptDepositSummary is created successfully..'
GO
