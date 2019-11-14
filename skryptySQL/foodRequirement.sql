-- ================================================
-- Template generated from Template Explorer using:
-- Create Inline Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
create or alter function countFoodRequirement(@food varchar)
returns int

begin
	declare @requirement int;
	select @requirement=sum(howMany*apetite) from food f join SpiecesFood sf on f.name=sf.foodName join Spieces s on s.name=sf.spieceName where f.name=@food group by f.name;
	return @requirement;
end



