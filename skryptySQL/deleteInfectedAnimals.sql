create or alter procedure deleteInfectedAnimals(@diseaseName varchar)
as
begin
	declare @animalId int;
	declare @endDate date;
	declare diseaseCursor cursor LOCAL STATIC READ_ONLY FORWARD_ONLY for
	select animalID,endDate from DiseaseHistory;
	open diseaseCursor;
	fetch next from diseaseCursor into @animalId, @endDate;
	while @@FETCH_STATUS=0
	begin
		if @endDate is null 
			delete from animals where id=@animalId;
		FETCH NEXT FROM diseaseCursor INTO @animalId, @endDate;
	end
	close diseaseCursor;
	deallocate diseaseCursor
end
