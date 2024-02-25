using CoreAudio;
using Ebenezer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("getpowerpointdisplaymonitor", () =>
    {
        return PowerPointDisplayMonitorModule.GetConfig();
    })
    .WithName("GetPowerPointDisplayMonitor")
    .WithOpenApi();

app.MapPost("setpowerpointdisplaymonitor", (string id) =>
    {
        return PowerPointDisplayMonitorModule.SetConfig(id);
    })
    .WithName("SetPowerPointDisplayMonitor")
    .WithOpenApi();

app.MapGet("enumeratedisplaydevices", () =>
    {
        return PowerPointDisplayMonitorModule.GetAllDisplayDevices();
    })
    .WithName("EnumerateDisplayDevices")
    .WithOpenApi();

app.MapGet("defaultaudiodevice", () =>
    {
        return AudioDeviceModule.GetDefaultAudioDevice();
    })
    .WithName("GetDefaultAudioDevice")
    .WithOpenApi();

app.MapGet("enumerateaudiodevices", () =>
    {
        return AudioDeviceModule.GetAllAudioDevices();
    })
    .WithName("GetAllAudioDevices")
    .WithOpenApi();

app.MapPost("setdefaultaudiodevice", (string id) =>
    {
        AudioDeviceModule.SetDefaultAudioDevice(id);
    })
    .WithName("SetDefaultAudioDevice")
    .WithOpenApi();

app.MapPost("launchprogram", (string command, bool killExistingApp) =>
    {
        RunCommandModule.LaunchProgram(command, killExistingApp);
    })
    .WithName("LaunchProgram")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

