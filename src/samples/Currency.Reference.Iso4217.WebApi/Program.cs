using Currency.Reference.Iso4217.Extensions;
using Currency.Reference.Iso4217.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ---- Register Currency service ----
builder.Services.AddCurrencyService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ---- Endpoints ----
app.MapCurrencyEndpoints();

await app.RunAsync();