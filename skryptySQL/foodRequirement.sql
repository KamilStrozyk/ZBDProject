
create or alter function countFoodRequirement(@food varchar)
returns int

begin
	declare @requirement int;
	select @requirement=sum(howMany*appetite) from food f join SpiecesFood sf on f.name=sf.foodName join Spieces s on s.name=sf.spieceName where f.name=@food group by f.name;
	return @requirement;
end



