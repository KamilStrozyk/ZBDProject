Struktura projektu dla przypomnienia:
Będziesz miał jeszcze jedną encję zaimplementowaną przeze mnie, żebyś wiedział o co cho

Foldery:
	View- po prostu widoki, ale tutek musi być pełny lel xD 
		Scripts- domyslne skrypty
			Views- dla naszych widoków
				Shared- wspólne dla kilku widoków
	Model- jeśli będzie Ci potrzeba jakiś dodatkowych modeli to dodajesz tu, te z bazy są w EntityDataModel
	Controllers- tutaj piszesz obsługę akcji prosto z widoków, Controller musi być dla każdej encji z widokiem
				 Akcje controllera mają wywoływać metody będące w logice, 
	Logic- tu trzymasz logikę dla controllerów, tu znajduje się odniesienie do bazy i do repozytorium
		Interface- tak dla zasady, żeby był 
	Repository
		Interface- tutaj masz interfejsy dla customowych repozytoriów
		W samym folderze robisz po prostu repo:
			Schemat działania:
				Wystarczą Ci operacje z Generic Repository to w logice podpinasz tylko je, potrzebujesz czegoś więcej
				to robisz interfejs, nazywasz go INazwaEncjiRepository, dajesz tam prototypy funkcji, implementujesz w NazwaEncjiRepositoty
				które dziedziczy po GenericRepository<NazwaEncji> i po ww. interfejsie- tak przygotowane Repo podpinasz w logice zamiast Generic
	Po co nam repozytorium:
		1. Kod będzie przejrzysty w ciul
		2. Ten wzorzec architektoniczny jest faktycznie wykorzystywany w firmach, np. w mojej i będziesz miał prostszy start jako koder
		3. Fajny wpis w CV xD