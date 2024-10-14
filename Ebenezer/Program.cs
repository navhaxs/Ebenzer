using System.Net;
using Ebenezer;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5053, listenOptions =>
    {
        listenOptions.UseHttps(httpsOptions =>
        {
            httpsOptions.ServerCertificate = SelfSignedCertificate.GenerateSelfSignedCertificate();
        });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWindowsService();

var app = builder.Build();

// Add this block to redirect HTTP to HTTPS
app.Use(async (context, next) =>
{
    if (context.Request.Scheme != "https")
    {
        var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        context.Response.Redirect(httpsUrl, permanent: true);
        return;
    }

    await next();
});

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
        app.UseSwagger();
        app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.MapGet("getpowerpointdisplaymonitor", PowerPointDisplayMonitorModule.GetConfig)
    .WithName("GetPowerPointDisplayMonitor")
    .WithTags("PowerPoint")
    .WithOpenApi();

app.MapPost("setpowerpointdisplaymonitor", PowerPointDisplayMonitorModule.SetConfig)
    .WithName("SetPowerPointDisplayMonitor")
    .WithTags("PowerPoint")
    .WithOpenApi();

app.MapGet("enumeratedisplaydevices", PowerPointDisplayMonitorModule.GetAllDisplayDevices)
    .WithName("EnumerateDisplayDevices")
    .WithTags("Display")
    .WithOpenApi();

app.MapGet("defaultaudiodevice", AudioDeviceModule.GetDefaultAudioDevice)
    .WithName("GetDefaultAudioDevice")
    .WithTags("Audio")
    .WithOpenApi();

app.MapGet("enumerateaudiodevices", AudioDeviceModule.GetAllAudioDevices)
    .WithName("GetAllAudioDevices")
    .WithTags("Audio")
    .WithOpenApi();

app.MapPost("setdefaultaudiodevice", AudioDeviceModule.SetDefaultAudioDevice)
    .WithName("SetDefaultAudioDevice")
    .WithTags("Audio")
    .WithOpenApi();
//
// app.MapPost("launchprogram", (string command, bool killExistingApp = false, string? parameters = null) =>
//     {
//         RunCommandModule.LaunchProgram(command, killExistingApp, parameters);
//     })
//     .WithName("LaunchProgram")
//     .WithOpenApi();
app.MapPost("restartscanconverterhx", async () =>
    {
        await RunCommandModule.KillProgram("Application.Network.ScanConverterHX.x64.exe");
        await RunCommandModule.KillProgram("Application.Network.ScanConverter2.x64.exe");
        RunCommandModule.LaunchProgram(
            "C:\\Program Files\\NDI\\NDI 6 Tools\\Screen Capture\\Application.Network.ScanConverterHX.x64.exe",
            true);
    })
    .WithName("Restart NDI Scan Converter HX")
    .WithTags("NDI Scan Converter")
    .WithOpenApi();

app.MapPost("restartscanconverter", async () =>
    {
        await RunCommandModule.KillProgram("Application.Network.ScanConverterHX.x64.exe");
        await RunCommandModule.KillProgram("Application.Network.ScanConverter2.x64.exe");
        RunCommandModule.LaunchProgram(
            "C:\\Program Files\\NDI\\NDI 6 Tools\\Screen Capture\\Application.Network.ScanConverter2.x64.exe",
            true);
    })
    .WithName("Restart NDI Scan Converter")
    .WithTags("NDI Scan Converter")
    .WithOpenApi();

app.MapGet("enumeratenetworkinginterfaces",
        NetworkingInterfacesModule.ListAllNetworkingInterfaces)
    .WithName("GetNetworkingInterfaces")
    .WithTags("Network")
    .WithOpenApi();

app.MapPost("requestshutdown", PowerModule.RequestShutdown)
    .WithName("RequestShutdown")
    .WithTags("Power")
    .WithOpenApi();

app.MapDelete("cancelshutdown", PowerModule.CancelShutdown)
    .WithName("CancelShutdown")
    .WithTags("Power")
    .WithOpenApi();

app.Run();
