using Blazored.LocalStorage;
using Blazored.Toast;
using HBlog.WebClient;
using HBlog.WebClient.Extensions;
using HBlog.WebClient.Handlers;
using HBlog.WebClient.Providers;
using HBlog.WebClient.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast(); 
builder.Services.RegisterClientServices();
builder.Services.AddTransient<TokenHandler>();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("https://localhost:6001/api/") });


builder.Services.AddScoped<ApiAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthStateProvider>());
builder.Services.AddScoped<PostDashboardState>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
