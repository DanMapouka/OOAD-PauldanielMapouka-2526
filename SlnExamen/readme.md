# SlnExamen – Dierenarts (.NET 10)

## Openen en starten

1. Open `SlnExamen.slnx` in Visual Studio 2026.
2. Zorg dat de workload **.NET desktop development** en de .NET 10 SDK geïnstalleerd zijn.
3. Laat NuGet de packages herstellen.
4. Stel `WpfDierenarts` in als opstartproject.
5. Start met `F5`.

## Projecten

- `CLDierenarts`: alle klassen, validatie en databankcommunicatie.
- `WpfDierenarts`: uitsluitend de WPF-gebruikersinterface en events.

De WPF-code-behind bevat geen SQL, geen connectiestring, geen databankpad en geen verwijzing naar SQLite. Ze vraagt data alleen op via de publieke klassen `Dier` en `Eigenaar` uit `CLDierenarts`.

## Databank

De meegeleverde SQLite-databank staat in `WpfDierenarts/Database/Dierenartsen.db`. De connectiestring staat, zoals in de cursus, in `WpfDierenarts/App.config`. Het originele meegeleverde script staat ernaast als `DierenartsenDB.sql`.
