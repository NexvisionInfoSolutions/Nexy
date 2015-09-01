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
		,EntityType VARCHAR(10) NULL
		)

	PRINT 'User type RptParameter is created successfully.'
END
ELSE
	PRINT 'User type RptParameter already exists'
GO


