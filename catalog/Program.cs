using Auth0.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuth0WebAppAuthentication(options => {
      options.Domain = builder.Configuration["Auth0:Domain"];
      options.ClientId = builder.Configuration["Auth0:ClientId"];
      options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
      options.Scope = "openid profile email";
    })
    .WithAccessToken(options =>
    {
      options.Audience = builder.Configuration["Auth0:Audience"];
    });

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();

builder.Services.AddHttpClient("RedemptionAPI", 
      client => client.BaseAddress = new Uri(builder.Configuration["RedemptionApiBaseUrl"]))
    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("RedemptionAPI"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
