IF NOT EXISTS (
		SELECT *
		FROM sys.types st
		INNER JOIN sys.schemas ss ON st.schema_id = ss.schema_id
		WHERE st.NAME = N'RptParameter'
			AND ss.NAME = N'dbo'
		)
BEGIN
	
CREATE TYPE dbo.[RptParameter] AS TABLE (
	EntityId BIGINT NULL
  ,EntityStartDate date NULL
  ,EntityEndDate date NULL
  ,EntityIntVal int NULL
  ,EntityStrVal varchar(100) NULL
	,EntityType VARCHAR(10) NULL
	)

PRINT 'User type RptParameter is created successfully.'
END
ELSE
BEGIN
PRINT 'User type RptParameter already exists.'
END
GO


