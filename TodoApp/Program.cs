var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ??  "8080";

builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(int.Parse(port)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();


app.Lifetime.ApplicationStarted.Register(LogPortOnStartup);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/greet", (string name) => "Hello " + name)
    .WithName("Greet")
    .WithOpenApi();

app.MapHealthChecks("/health");

app.Run();
void LogPortOnStartup() => app.Logger.LogInformation("Server started in port {}", port);
