namespace Currency.Reference.Iso4217.WebApi.Models;

record CurrencyInfo(string Code, string Name, DateOnly? WithdrawalDate);