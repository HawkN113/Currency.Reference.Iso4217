using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Extensions;
using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Models;
using Currency.Reference.Iso4217.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Currency.Reference.Iso4217.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCurrencyService_Should_Register_ICurrencyService_AsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCurrencyService();
        var provider = services.BuildServiceProvider();
        var service1 = provider.GetRequiredService<ICurrencyService>();
        var service2 = provider.GetRequiredService<ICurrencyService>();

        // Assert
        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.IsType<CurrencyService>(service1);
        Assert.Same(service1, service2);
    }

    [Fact]
    public void AddCurrencyService_Should_Not_Override_Existing_Service()
    {
        // Arrange
        var services = new ServiceCollection();
        var customService = new CustomCurrencyService();
        services.AddSingleton<ICurrencyService>(customService);

        // Act
        services.AddCurrencyService(); // TryAddSingleton не должен перезаписать
        var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<ICurrencyService>();

        // Assert
        Assert.Same(customService, service);
    }

    [Fact]
    public void AddCurrencyService_Should_Return_ServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var returned = services.AddCurrencyService();

        // Assert
        Assert.Same(services, returned);
    }

    private class CustomCurrencyService : ICurrencyService
    {
        public bool TryValidate(string value, out ValidationResult result)
        {
            result = new ValidationResult();
            return true;
        }

        public bool TryValidate(CurrencyCode code, out ValidationResult result)
        {
            result = new ValidationResult();
            return true;
        }

        public bool Exists(string value) => true;

        public bool Exists(CurrencyCode code) => true;

        public Models.Currency? Get(string value) => null;

        public Models.Currency? Get(CurrencyCode code) => null;

        public Models.Currency? GetHistorical(string value) => null;

        public Models.Currency[] GetAllHistorical() => [];

        public ICurrencyQueryStart Query() => null!;
    }
}