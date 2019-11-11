-- ================================================
-- Template generated from Template Explorer using:
-- Create Inline Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
create or alter function showFinalServicePointIncome(@name varchar)
returns int

begin
	declare @totalSalary int;
	declare @income int;
	select @totalSalary=sum(salary) from
	ServicePointWorkers w join ServicePointsServicePointWorkers sw on w.id=sw.workerID join ServicePoint s on s.name=sw.servicePointName
	where s.name=@name group by s.name;
	select @income=income from ServicePoint where name=@name;
	return @income-@totalSalary;
end
