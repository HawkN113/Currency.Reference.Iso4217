# Currency.Reference.Iso4217

[![NuGet](https://img.shields.io/nuget/v/Currency.Reference.Iso4217?label=Currency.Reference.Iso4217)](https://www.nuget.org/packages/Currency.Reference.Iso4217/)
[![NuGet](https://img.shields.io/nuget/v/Currency.Reference.Iso4217.Generators?label=Currency.Reference.Iso4217.Generators)](https://www.nuget.org/packages/Currency.Reference.Iso4217.Generators/)
[![GitHub license](https://img.shields.io/github/license/HawkN113/Flash.Configuration)](https://github.com/HawkN113/Flash.Configuration/blob/main/LICENSE)

| ![Currency.Reference.Iso4217](docs/img/Currency.Reference.Iso4217.png) | **Currency.Reference.Iso4217** provides ISO 4217 currency codes, historical currency data, and replacement mappings. |
|--------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
---

## Features
- **Actual currency list**
- **Strongly-Typed currency codes** – `CurrencyCode` enum is generated at compile-time.
- **Historical currency support** – Access withdrawn currencies.
- **Lightweight & Dependency-Free** – Minimal overhead, compatible with .NET 8 and above.
- **Integration ready** – Use in libraries, console apps, or web applications.

---

## Packages

| Package | Description |
|---------|-------------|
| [Currency.Reference.Iso4217](https://www.nuget.org/packages/Currency.Reference.Iso4217) | Main library with domain models, currency utilities, and generated `CurrencyCode` type. |
---

## Getting Started

### Install via NuGet

```bash
dotnet add package Currency.Reference.Iso4217 --version 8.0.0
```
---

### Required Namespaces
```csharp
using Currency.Reference.Iso4217;
using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Domain.Models;
using Currency.Reference.Iso4217.Extensions;
```

---

### Usage Example

#### Registration
Use extension method `.AddCurrencyService();`
```csharp
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCurrencyService();
    })
    .Build();
```
To get service instance:
```csharp
var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
```
or inject
```csharp
app.MapGet("/weatherforecast", ([FromServices] ICurrencyService currencyService) => ...
````

#### Get all currencies
```csharp
var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
var currencies = currencyService?.Query()
    .Includes
    .Type(CurrencyType.Fiat)
    .Type(CurrencyType.SpecialUnit)
    .Type(CurrencyType.SpecialReserve)
    .Type(CurrencyType.PreciousMetal)
   .Build();
```

#### Get fiat currencies
```csharp
var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
var currencies = currencyService?.Query()
   .Includes
   .Type(CurrencyType.Fiat)
   .Build();
```

#### Get currencies by query
Excludes `EUR` and `USD` from the list: 
```csharp
var currencies = currencyService?.Query()
   .Includes
   .Type(CurrencyType.Fiat)
   .Without(w => w.Codes(CurrencyCode.EUR, CurrencyCode.USD))
   .Build();
```

Includes only `EUR` and `USD` in the list:
```csharp
var currencies = currencyService?.Query()
   .Includes
   .Type(CurrencyType.Fiat)
   .With(w => w.Codes(CurrencyCode.EUR, CurrencyCode.USD))
   .Build();
```

#### Get historical currencies
```csharp
var historical = currencyService.GetAllHistorical();
foreach (var currency in historical)
{
    Console.WriteLine($"{currency.Code} - {currency.Name} (Withdrawn: {currency.WithdrawnOn})");
}
```

#### Lookup currency
By string code
```csharp
var afnWithString = currencyService.Get("AFN");
```
By currency code
```csharp
var afnWithCode = currencyService.Get(CurrencyCode.AFN);
```

#### Validate currency
By string code
```csharp
var validResult = currencyService.TryValidate("AFN", out var validateResult);
```
By currency code
```csharp
var validResult = currencyService.TryValidate(CurrencyCode.AFN, out var validateResult);
```

---

### Generated Types
- `CurrencyCode` – strongly-typed enum with all ISO 4217 codes.
- `Currency` – domain model representing a currency (code, name, numeric code, withdrawn date).

---

### License
This project is licensed under the MIT License.

---

### References
- [ISO 4217 Standard](https://www.iso.org/iso-4217-currency-codes.html)
- [GitHub Repository](https://github.com/HawkN113/Currency.Reference.Iso4217)

