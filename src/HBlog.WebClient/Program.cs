using Blazored.LocalStorage;
using Blazored.Toast;
using HBlog.WebClient;
using HBlog.WebClient.Extensions;
using HBlog.WebClient.Handlers;
using HBlog.WebClient.Providers;
using HBlog.WebClient.Services;
using HBlog.WebClient.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast(); 

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8090/api/") }); // Replace builder.HostEnvironment.BaseAddress
builder.Services.AddHttpClient("Annoy", (configure) => configure.BaseAddress = new Uri("http://localhost:8090/api/"));
builder.Services.RegisterClientServices();


builder.Services.AddTransient<TokenHandler>();
//builder.Services.AddHttpClient<HttpServiceProvider>(
//    "Auth",
//    opt => opt.BaseAddress = new Uri("http://localhost:8090/api/"))
//    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped<ApiAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthStateProvider>());
builder.Services.AddScoped<PostDashboardState>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
