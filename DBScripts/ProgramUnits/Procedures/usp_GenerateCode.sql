
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
			AND NAME = 'usp_GenerateCode'
		)
	DROP PROCEDURE usp_GenerateCode
GO

CREATE PROCEDURE [dbo].[usp_GenerateCode] @vTableName varchar(100)
AS

Declare @vPrimaryKey varchar(100);
/*SELECT --KU.table_name as tablename,
@vPrimaryKey = column_name as primarykeycolumn
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
INNER JOIN
INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
and ku.table_name=@vTableName
ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;*/

select @vPrimaryKey = IdField from SysCode where TableName = @vTableName;

Declare @vSQL NVarchar(MAX) =  N'select top 1 sc.Prefix+cast(max(' + @vPrimaryKey + ') over(partition by 1) + 1 as varchar)+sc.Suffix from ' + @vTableName + ' t left join SysCode sc on sc.TableName = ''' + @vTableName + '''';
EXECUTE sp_executesql @vSQL;
GO

PRINT 'usp_GenerateCode is created successfully..'
GO
