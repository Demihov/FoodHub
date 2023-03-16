using FoodHub;
using FoodHub.AutoMapper.Profiles;
using FoodHub.Data;
using FoodHub.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
	.RequireAuthenticatedUser()
	.Build();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
	    options.TokenValidationParameters = new TokenValidationParameters
	    {
		    // Вказує, чи валідуватиметься видавець при валідації токена
		    ValidateIssuer = true,
		    // Рядок, що представляє видавця
		    ValidIssuer = AuthOptions.ISSUER,

		    // чи валідуватиметься споживач токена
		    ValidateAudience = true,
		    // Установка споживача токена
		    ValidAudience = AuthOptions.AUDIENCE,
		    // чи валідуватиметься час існування
		    ValidateLifetime = true,

		    // Встановлення ключа безпеки
		    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
		    // валідація ключа безпеки
		    ValidateIssuerSigningKey = true,
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
