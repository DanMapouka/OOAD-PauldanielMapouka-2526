# Documentatie – OOAD praktijkopgave Dierenarts

## Architectuur

De solution bevat een WPF-project en een class library. Alle communicatie met data gebeurt via de class library. Er staat geen SQL-code en geen kennis van SQLite in `MainWindow.xaml.cs`.

## Class library `CLDierenarts`

### `Dier`

Abstracte basisklasse voor alle dieren. Bevat de gemeenschappelijke properties en:

- `GeefInfo()`: virtuele methode voor de detailweergave;
- `ToString()`: tekst voor de ListBox en vermeldt `OPGENOMEN` wanneer nodig;
- `GetAll()` en `GetById()`;
- `InsertInDb()`, `UpdateInDb()` en `DeleteFromDb()`;
- `NeemOp()` om de opnamestatus en datum aan te passen.

### `Hond`

Erft van `Dier`, voegt `Ras` toe en overschrijft `GeefInfo()` met `base.GeefInfo()`.

### `Kat`

Erft van `Dier`, voegt `IsGevaccineerd` toe en overschrijft `GeefInfo()` met `base.GeefInfo()`.

### `DierValidator`

- property `MinimaalAantalTekensRas` met standaardwaarde 3;
- `IsGeldigeNaam()`: alleen letters, spaties en koppeltekens;
- `IsGeldigRas()`: minstens het ingestelde aantal tekens.

### Extra klasse en enumeratie

- extra klasse: `Eigenaar`;
- extra enum: `Urgentie` met `Laag`, `Normaal` en `Spoed`.

## WPF-hoofdscherm

- linksboven: filters op urgentie, eigenaar en alleen opgenomen dieren;
- linksonder: ListBox met het overzicht;
- rechtsboven: details via `GeefInfo()`, een echte `Image` voor hond of kat en de knop `Dier opnemen`;
- rechtsonder: formulier voor een nieuw ticket.

Bij toevoegen wordt de invoer gecontroleerd, het juiste object (`Hond` of `Kat`) gemaakt, via `InsertInDb()` opgeslagen en het overzicht opnieuw geladen.
