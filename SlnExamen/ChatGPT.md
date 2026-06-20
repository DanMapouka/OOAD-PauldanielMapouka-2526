# ChatGPT-logboek

## Project
OOAD-praktijkopgave Dierenarts

## Afspraken

- WPF App: `WpfDierenarts` op `.NET 10`.
- Class Library: `CLDierenarts` op `.NET 10`.
- Alle data- en SQL-code zit in de class library.
- Code-behind weet niet of de data uit CSV, JSON, SQL of een andere bron komt.
- De implementatie gebruikt de cursusonderwerpen WPF, layout, classes, enum, overerving en databanken.

## Uitwerking

- `Dier`, `Hond`, `Kat`, `DierValidator`, `Eigenaar` en `Urgentie`.
- `GeefInfo()` met `virtual`, `override` en `base.GeefInfo()`.
- `ToString()` voor de ListBox, inclusief `OPGENOMEN`.
- CRUD-methodes in de klassen die met de tabellen overeenkomen.
- Connectiestring in `App.config`.
- Vierdelige WPF-layout, filters, details, afbeeldingen, opnemen en toevoegen.
