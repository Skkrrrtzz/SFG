CREATE procedure [dbo].[sp_AddConformityToAuditPlan]
	@PlanId int,
	@Description nvarchar(max),
	@AreaSection nvarchar(max)
as 
begin
--	declare @LatestConformity int; 
--	declare @seq nvarchar(max);

--	select @LatestConformity = MAX(ConformityId) from [dbo].[Conformities] where PlanId = @PlanId;
--	set @seq = N'create sequence dbo.seq_PlanConformities start with' + cast(@LatestConformity as nvarchar(10)) + 'increment by 1;';
--	EXEC sp_executesql @seq;

	drop table if exists #temp_PlanConformities
	create table #temp_PlanConformities (
		ConformityId int identity(1,1) primary key,
--		PlanId int
		ConformityDescription varchar(max),
		ConformityAreaSection varchar(max)
	)

	-- seed with values
	insert into #temp_PlanConformities
	select ConformityId, ConformityDescription, ConformityAreaSection
	from [dbo].[Conformities] conformities
	where PlanId = @PlanId;

	-- add conformity
--	insert into #temp_PlanConformities (
--		PlanId, ConformityDescription, ConformityAreaSection
--	) values (
--		@PlanId, @Description, @AreaSection
--	);

	-- insert into main table
--	insert into [dbo].[Conformities]
--	select * from #temp_PlanConformities where ConformityId = (select max(ConformityId) from #temp_PlanConformities); 

	select * from #temp_PlanConformities;

--	drop sequence [dbo].[seq_PlanConformities];
end;

--AddConformityToAuditPlan()

GO

