

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository;
using User.Managment.Repository.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Se agregue el servicio de conexion a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"))
);

//Se agrega el servicio para Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


//Se agrega la configuracion para el requerimiento de Email
builder.Services.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail = true);

//
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(10));

//Se agrega el servicio para la autenticacion
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

//Se agrega la configuracion del Email que se agrego en la appSettings.json
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);


//Se agrega el repositorio Email para utilizarlo en el proyecto
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
