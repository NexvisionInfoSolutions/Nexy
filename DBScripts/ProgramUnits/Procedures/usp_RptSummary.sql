

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


IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityIdType = 'U'
		)
	SET @vCheckUserId = 1;

IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityIdType = 'S'
		)
	SET @vCheckShiftTypeId = 1;

IF EXISTS (
		SELECT TOP 1 1
		FROM @RptParameters rp
		WHERE rp.EntityIdType = 'T'
		)
	SET @vCheckScheduleTypeId = 1;
  
/*if isnull(@vManagerId,0) = 0
  select @vManagerId = ap.UserId FROM AppSession ap
  inner join ShiftsterUsers u on u.UserId = ap.UserId
  inner join UserDetails ud on ud.UserId = u.UserId
  where ap.SessionKey = @vSessionKey and ud.Status <> 2 and ud.UserMode = 1;*/

DECLARE @vLoggedInUserId BIGINT, @vModuleName varchar(100);

Select @vCompanyId = u.CompanyId, @vLoggedInUserId = u.UserId, @vModuleName = ap.ModuleName FROM AppSession ap left join ShiftsterUsers u on u.UserId = ap.UserId
where ap.SessionKey = @vSessionKey;

SELECT distinct u.UserId
  ,isnull(u.FirstName,space(0))+space(1)+isnull(u.LastName,space(0)) as EmpName
	,isnull(u.Rate,0) as Rate
  ,sh.ShiftId
  ,Sh.ShiftStartDate
  ,case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end as StdShiftWeekHours
  ,sh.ShiftHours AS ShiftHours		
  ,sum(isnull(u.Rate,0) * sh.ShiftHours) OVER (
		PARTITION BY 1
		) AS GrandTotalScheduledCosts
	,sum(sh.ShiftHours) OVER (
		PARTITION BY u.UserId
		--,sh.WeekStartDate
		) AS TotalScheduledHours
    ,sum(isnull(u.Rate,0) * sh.ShiftHours) OVER (
		PARTITION BY u.UserId
		--,sh.WeekStartDate
		) AS TotalScheduledCosts
	,CASE 
		WHEN st.AllowHoursExceed = 0
			OR sum(sh.ShiftHours) OVER (
				PARTITION BY u.UserId
				--,sh.WeekStartDate
				) < case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
			THEN sum(sh.ShiftHours) OVER (
					PARTITION BY u.UserId
					--,sh.WeekStartDate
					)
		ELSE case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
		END AS TotalRegularHours
  ,isnull(u.Rate,0.0) * CASE 
		WHEN st.AllowHoursExceed = 0
			OR sum(sh.ShiftHours) OVER (
				PARTITION BY u.UserId
				--,sh.WeekStartDate
				) < case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
			THEN sum(sh.ShiftHours) OVER (
					PARTITION BY u.UserId
					--,sh.WeekStartDate
					)
		ELSE case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
		END AS TotalRegularCost
	,CASE 
		WHEN st.AllowHoursExceed = 1
			AND sum(sh.ShiftHours) OVER (
				PARTITION BY u.UserId
				--,sh.WeekStartDate
				) > case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
			THEN sum(sh.ShiftHours) OVER (
					PARTITION BY u.UserId
					--,sh.WeekStartDate
					) - case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
		ELSE 0
		END AS TotalExceededHours
  ,isnull(u.Rate,0.0) * CASE 
		WHEN st.AllowHoursExceed = 1
			AND sum(sh.ShiftHours) OVER (
				PARTITION BY u.UserId
				--,sh.WeekStartDate
				) > case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
			THEN sum(sh.ShiftHours) OVER (
					PARTITION BY u.UserId
					--,sh.WeekStartDate
					) - case when st.ExceededHoursType = 1 then st.OvertimeThreshholdHours else st.ExceededThresholdHours end
		ELSE 0
		END AS TotalExceededCosts
  ,sum(case when sh.ShiftMode = @vCompTimeMode then sh.ShiftHours else 0 end) OVER (PARTITION BY u.UserId
  --, sh.WeekStartDate
  ) AS TotalCompHours  
  ,sum(case when sh.ShiftMode = @vOverTimeMode then sh.ShiftHours else 0 end) OVER (PARTITION BY u.UserId --, sh.WeekStartDate
  ) AS TotalOverTimeHours  
  ,sum(isnull(u.Rate,0.0) * case when sh.ShiftMode = @vCompTimeMode then sh.ShiftHours else 0 end) OVER (PARTITION BY u.UserId --, sh.WeekStartDate
  ) AS TotalCompCost  
  ,sum(isnull(u.Rate,0.0) * case when sh.ShiftMode = @vOverTimeMode then sh.ShiftHours else 0 end) OVER (PARTITION BY u.UserId --, sh.WeekStartDate
  ) AS TotalOverTimeCost  
FROM vwShiftInfo sh
INNER JOIN Activity ac ON ac.ActivityShiftId = sh.ShiftId
LEFT JOIN Schedule sch ON sch.ScheduleId = sh.ScheduleId
LEFT JOIN ScheduleType scht ON scht.ScheduleTypeId = sch.ScheduleTypeId
LEFT JOIN ShiftType sht ON sht.ShiftTypeId = sh.ShiftTypeId
INNER JOIN ShiftsterUsers u ON u.UserId = sh.ShUserId
LEFT JOIN Settings st on st.CompanyId = u.CompanyId
WHERE isnull(sh.IsDeleted, 'N') = 'N'
	--AND sht.ShiftTypeStatus = 1
	--AND u.STATUS = 1
  AND (@vModuleName = 'MyShiftster' or (@vModuleName <> 'MyShiftster' and isnull(sh.IsPersonal, 0) = 0))
	AND (
		@vCheckUserId = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE pm.EntityId = sh.ShUserId
				AND pm.EntityIdType = 'U'
			)
		)
	AND (
		@vCheckShiftTypeId = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE pm.EntityId = sh.ShiftTypeId
				AND pm.EntityIdType = 'S'
			)
		)
	AND (
		@vCheckScheduleTypeId = 0
		OR EXISTS (
			SELECT TOP 1 1
			FROM @RptParameters pm
			WHERE pm.EntityId = scht.ScheduleTypeId
				AND pm.EntityIdType = 'T'
			)
		)
	  AND (@vWeekStartDate IS NULL OR (sh.ShiftStartDate between @vWeekStartDate and dateadd(dd,6,@vWeekStartDate)))
    --and (isnull(@vManagerId,0) = 0 or sh.ShManagerId = @vManagerId)
    and (ISNULL(@vCompanyId,0) = 0 or u.CompanyId = @vCompanyId)
GO

PRINT 'usp_RptSummary is created successfully..'
GO
