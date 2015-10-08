
declare @Periden int, @Tempiden int,@AccountId int,@AccScheduleId int,@VilllageId int
declare @sl_no int, @name varchar(200), @mno int, @badgeno varchar(200), @per_house varchar(200), @per_place varchar(200), @per_po varchar(200), 
@per_village varchar(200), @per_taluk varchar(200), @per_district varchar(200), @per_pin varchar(200), @per_phone varchar(200), @idno varchar(200), 
@temp_house varchar(200), 
@temp_place varchar(200), 
                      @temp_po varchar(200), @temp_Village varchar(200), @temp_taluk varchar(200), @temp_district varchar(200), @temp_pin varchar(200), 
                      @temp_phone varchar(200), @occupation varchar(200), @houseno varchar(200), @ward varchar(200), @sex varchar(200), @dob varchar(200), 
                      @religion varchar(200), 
                      @caste varchar(200), @sc_st varchar(200), @classification varchar(200), @guardian_name varchar(200), 
                      @guardiantype varchar(200), @nominee varchar(200), @adressofnominee varchar(200), @relationwithmember varchar(200),@dateofadmssn  varchar(200), 
                      @dateofapplication varchar(200), @admittedstate varchar(200), @resolutionno varchar(200), 
                      @dateofresolution varchar(200), @monthlyincome varchar(200), @age  varchar(200), 
                      @die varchar(200), @cd varchar(200), @branchcode varchar(200), @parttype varchar(200)
--------------------------------------------------------

(SELECT    @AccScheduleId= SundryDebtorAccountId 
			FROM         LoanManagement.dbo.Settings)
			
			
DECLARE @MyCursor CURSOR
SET @MyCursor = CURSOR FAST_FORWARD
FOR
				SELECT  sl_no, name, mno, badgeno, per_house, per_place, per_po, per_village, per_taluk, per_district, per_pin, per_phone, idno, temp_house, temp_place, 
                      temp_po, temp_Village, temp_taluk, temp_district, temp_pin, temp_phone, occupation, houseno, ward, sex, dob, religion, caste, sc_st, classification, guardian_name, 
                      guardiantype, nominee, adressofnominee, relationwithmember, dateofadmssn, dateofapplication, admittedstate, resolutionno, dateofresolution, monthlyincome, age, 
                      die, cd, branchcode, parttype
				FROM         personala
OPEN @MyCursor
FETCH NEXT FROM @MyCursor
INTO @sl_no, @name, @mno, @badgeno, @per_house, @per_place, @per_po, @per_village, @per_taluk, @per_district, @per_pin, @per_phone, @idno, 
@temp_house, @temp_place, 
                      @temp_po, @temp_Village, @temp_taluk, @temp_district, @temp_pin, @temp_phone, @occupation, @houseno, @ward, @sex, @dob, @religion, 
                      @caste, @sc_st, @classification, @guardian_name, 
                      @guardiantype, @nominee, @adressofnominee, @relationwithmember, @dateofadmssn, @dateofapplication, @admittedstate, @resolutionno, 
                      @dateofresolution, @monthlyincome, @age, 
                      @die, @cd, @branchcode, @parttype
WHILE @@FETCH_STATUS = 0
BEGIN
			print @name + ', ' + @name + ', ' +@occupation
			select @VilllageId=VillageId from LoanManagement.dbo.Village where VillageName=@per_village
			INSERT  
			INTO     LoanManagement.dbo.Address( Address1, Address2, City,		Zipcode,   Post,		CountryId, StateId, DistrictId, TalukId, VillageId,IsDeleted,CreatedOn)
			VALUES     (			@per_house,'',		@per_place, @per_pin,  @per_po,		1,1,1,1,@VilllageId,		 0 ,GETDATE())
			select @Periden=@@IDENTITY
			
			if(@temp_house != null)
			begin
				INSERT  
				INTO     LoanManagement.dbo.Address( Address1, Address2, City,		Zipcode,   Post,		CountryId, StateId, DistrictId, TalukId, VillageId,IsDeleted,CreatedOn)
				VALUES     (			@temp_house,'',		@temp_place, @temp_pin,  @temp_po,		1,1,1,1,@VilllageId,		 0 ,GETDATE())
				select @Tempiden=@@IDENTITY
			end
			INSERT INTO LoanManagement.dbo.AccountHead
                      (AccountCode, AccountName, ScheduleId, AccountTypeId,ContactId, AddressId, IsDeleted)
			VALUES     ((SELECT   'ACC_'+  { fn UCASE(REPLACE(LTRIM(RTRIM(@name)), ' ', '_') )}),(SELECT     { fn UCASE(REPLACE(LTRIM(RTRIM(@name)), ' ', '_')) }),
			(SELECT     SundryDebtorAccountId 
			FROM         LoanManagement.dbo.Settings),3,1,1,0)
			select @AccountId=@@IDENTITY
			
			
			
			 
				
			INSERT INTO LoanManagement.dbo.OpeningBalance
							  (AccountHeadId, ScheduleId, DebitOpeningBalance, CreditOpeningBalance, ClosingBalance, FinancialYearId, IsDeleted)
				VALUES     (@AccountId,@AccScheduleId,0,0,0,1,0)
				
			INSERT    
			INTO     LoanManagement.dbo.Users( Code, FirstName,  IsActive, Designation, UserType,   UserAddressId, 
								  PermanentAddressId ,GuaranterName, Occupation,  AccountHeadId,IsDeleted )
			VALUES     (@name,@name,1,@occupation,2,@Tempiden,@Periden,@guardian_name,@occupation,@AccountId,0 )


			
FETCH NEXT FROM @MyCursor
INTO @sl_no, @name, @mno, @badgeno, @per_house, @per_place, @per_po, @per_village, @per_taluk, @per_district, @per_pin, @per_phone, @idno, 
@temp_house, @temp_place, 
                      @temp_po, @temp_Village, @temp_taluk, @temp_district, @temp_pin, @temp_phone, @occupation, @houseno, @ward, @sex, @dob, @religion, 
                      @caste, @sc_st, @classification, @guardian_name, 
                      @guardiantype, @nominee, @adressofnominee, @relationwithmember, @dateofadmssn, @dateofapplication, @admittedstate, @resolutionno, 
                      @dateofresolution, @monthlyincome, @age, 
                      @die, @cd, @branchcode, @parttype
END
CLOSE @MyCursor
DEALLOCATE @MyCursor

