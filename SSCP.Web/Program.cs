using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SSCP.Web;
using Microsoft.AspNetCore.Http;
using SSCP.Web.Services.SharedServices;
using SSCP.Web.Services.AuthServices;
using Blazored.SessionStorage;
using Plugin.DoubleEncryption;
using SSCP.Web.Services.ReportServices;
using MudBlazor.Services;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient().AddHttpContextAccessor().AddBlazoredSessionStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAPIHandler, APIHandler>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<AesBcCrypto>();
builder.Services.AddMudServices();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
{
    Converters =
    {
        new SeverityDictionaryConverter() // Add your custom converter here
    }
});


await builder.Build().RunAsync();


