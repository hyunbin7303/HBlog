using Blazored.LocalStorage;
using KevBlog.WebClient;
using KevBlog.WebClient.Providers;
using KevBlog.WebClient.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IPostService, PostClientService>();
builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<UserClientService>();
builder.Services.AddScoped<ApiAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
