using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PayAppApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PayAppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("PayAppConnectionString")));

//builder.Services.AddOpenApiDocument(configure =>
//{
//    configure.Title = "Admin Portal Api";
//    configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
//    {
//        Type = OpenApiSecuritySchemeType.ApiKey,
//        Name = "Authorization",
//        In = OpenApiSecurityApiKeyLocation.Header,
//        Description = "Type into the textbox: Bearer {your JWT token}."
//    });
//    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
//});
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = false;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenDefination:JwtKey"])),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["TokenDefination:JwtIssuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["TokenDefination:JwtAudience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(cors =>
{
    cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
