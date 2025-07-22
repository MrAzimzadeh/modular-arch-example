using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add WebApi Module with all dependencies
builder.Services.AddWebApiModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Initialize all modules
await app.Services.InitializeWebApiModuleAsync();

app.Run();
