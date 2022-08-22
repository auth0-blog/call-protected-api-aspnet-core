using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
     {
         options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
         options.TokenValidationParameters = 
           new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidAudience = builder.Configuration["Auth0:Audience"],
             ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}",
             ValidateLifetime = true
         };
     });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var responseMessages = new[]
{
    "Success", "Insufficient points"
};

app.MapGet("/redeem", () =>
{
    var messageIndex = Random.Shared.Next(responseMessages.Length);

    var response = new RedeemResponse
        (
            DateTime.Now,
            messageIndex,
            responseMessages[messageIndex]
        );

    return response;
})
.WithName("Redeem")
.RequireAuthorization();;

app.Run();

record RedeemResponse(DateTime Date, int Code, string? Message);
