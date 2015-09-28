
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
			AND NAME = 'usp_RptSummary'
		)
	DROP PROCEDURE usp_RptSummary
GO

CREATE PROCEDURE [dbo].[usp_RptSummary] @RptParameters dbo.RptParameter readonly	  
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
  
Select distinct lf.LoanId, lf.UserId, lf.LoanAmount, lf.TotalInstallments, lf.InteresRate, lf.InstallmentAmount, lf.RepaymentStartDate
,sum(lr.RepaymentAmount) over (partition by lr.LoanId) as TotalPaidAmountPerLoan
,sum(lr.RepaymentAmount) over (partition by lf.UserId) as TotalPaidAmountPerUser
--,sum(lr.RepaymentAmount) over (partition by lr.RepaymentDate) as TotalPaidAmountPerDate
,Min(isnull(lr.PendingPrincipalAmount, lf.LoanAmount)) over (partition by lr.LoanId) as BalanceLoanAmount
,Min(isnull(lr.PendingInstallments, lf.TotalInstallments)) over (partition by lr.LoanId) as BalanceLoanInstallments
,Sum(lr.InterestAmount) over (partition by lr.LoanId) as TotalInterestPaidAmountPerLoan
,Sum(lr.InterestAmount) over (partition by lf.UserId) as TotalInterestPaidAmountPerUser
,lf.TotalInstallments
,u.FirstName
,u.LastName
,u.UserType
,u.FatherName
,u.GuaranterName
, isnull(uaddr.Address1, '') + '<br />' + isnull(uaddr.Address2, '') + '<br />' + isnull(uaddr.Place, '') + '<br />' + isnull(uaddr.Post, '') + '<br />' + isnull(uaddrDist.DistrictName, '') + '<br />' + isnull(uaddrTaluk.TalukName, '') + '<br />' + isnull(uaddrVillage.VillageName, '') + '<br />' as UserAddress
, uc.Telephone1 as UserPhone
, uc.Mobile1 as UserMobile
, uc.Email1 as UserEmail
, isnull(upAddr.Address1, '') + '<br />' + isnull(upAddr.Address2, '') + '<br />' + isnull(upAddr.Place, '') + '<br />' + isnull(upAddr.Post, '') + '<br />' + isnull(upAddrDist.DistrictName, '') + '<br />' + isnull(upAddrTaluk.TalukName, '') + '<br />' + isnull(upAddrVillage.VillageName, '') + '<br />' as UserPermanentAddress
, upc.Telephone1 as PermanentPhone
, upc.Mobile1 as PermanentMobile
, upc.Email1 as PermanentEmail
from LoanInfo lf
left join LoanRepayment lr on lr.LoanId = lf.LoanId and lr.Status <> 4 and isnull(lr.IsDeleted, 0) = 0
left join Users u on u.UserID = lf.UserId and u.UserType = 3 -- Members only
left join Address uaddr on uaddr.AddressId = u.UserAddressId
left join District uaddrDist on uaddrDist.DistrictId = uaddr.DistrictId
left join Taluk uaddrTaluk on uaddrTaluk.TalukId = uaddr.TalukId
left join Village uaddrVillage on uaddrVillage.VillageId = uaddr.VillageId
left join Address upAddr on upAddr.AddressId = u.PermanentAddressId
left join District upAddrDist on upAddrDist.DistrictId = upAddr.DistrictId
left join Taluk upAddrTaluk on upAddrTaluk.TalukId = upAddr.TalukId
left join Village upAddrVillage on upAddrVillage.VillageId = upAddr.VillageId
left join Contact uc on uc.ContactId = u.UserContactId
left join contact upc on upc.ContactId = u.PermanentContactId
where isnull(lf.IsDeleted, 0) = 0
AND (@vCompanyId = 0 or u.CompanyId = @vCompanyId)
AND (
		@vCheckByLoanId = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE pm.EntityId = lf.LoanId
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
			WHERE dateadd(dd, 0, datediff(dd, 0 , lr.RepaymentDate)) between pm.EntityStartDate and pm.EntityEndDate
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
        OR (len(isnull(uaddrDist.DistrictName, '')) > 0 AND uaddrDist.DistrictName like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uaddrTaluk.TalukName, '')) > 0 AND uaddrTaluk.TalukName like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uaddrVillage.VillageName, '')) > 0 AND uaddrVillage.VillageName like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddr.Address1, '')) > 0 AND upAddr.Address1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddr.Address2, '')) > 0 AND upAddr.Address2 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddrDist.DistrictName, '')) > 0 AND upAddrDist.DistrictName like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddrTaluk.TalukName, '')) > 0 AND upAddrTaluk.TalukName like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddrVillage.VillageName, '')) > 0 AND upAddrVillage.VillageName like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upAddr.Place , '')) > 0 AND upAddr.Place like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uc.Telephone1 , '')) > 0 AND uc.Telephone1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uc.Mobile1 , '')) > 0 AND uc.Mobile1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(uc.Email1 , '')) > 0 AND uc.Email1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upc.Telephone1 , '')) > 0 AND upc.Telephone1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upc.Mobile1 , '')) > 0 AND upc.Mobile1 like '%' + pm.EntityStrVal + '%')
        OR (len(isnull(upc.Email1 , '')) > 0 AND upc.Email1 like '%' + pm.EntityStrVal + '%')
        OR (cast(lf.LoanId as varchar) like '%' + pm.EntityStrVal + '%')
        )
				AND pm.EntityType = 'M'
			)
    )
GO

PRINT 'usp_RptSummary is created successfully..'
GO
