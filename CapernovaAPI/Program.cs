

using Capernova.Utility;
//using Google.Apis.Auth.AspNetCore3;
//using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository;
using User.Managment.Repository.Repository.IRepository;

//variable para que funcione CORS en la aplicacion y react
//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

//permite almacenar el proveedor y la configuracion par aplicarla al CORS
var proveedor = builder.Services.BuildServiceProvider();
var configuration = proveedor.GetRequiredService<IConfiguration>();

//Se agrega la configuracion del servcio de CORS
builder.Services.AddCors(options =>
{
    var frontEndURL = configuration.GetValue<string>("frontEnd:Url");

    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontEndURL).AllowAnyMethod().AllowAnyHeader().AllowCredentials(); 
    });
   
});

// Add services to the container.

//Se agregue el servicio de conexion a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//Se agrega el almacenamiento en cache de las peticiones a las API
builder.Services.AddResponseCaching();

//Se agrega los repositorios de las entidades que se van a ocupar en el proyecto
builder.Services.AddScoped<ICourseRepositoty, CourseRepository>();
builder.Services.AddScoped<IProductoRepositoy, ProductoRepositoy>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<IDeberRepository, DeberRepository>();


//Se agrega el servicio de Stripe para el pago con tarjeta
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//Se agrega el servicio para Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



//Se agrega la configuracion para el requerimiento de Email
builder.Services.Configure<IdentityOptions>(options => 
{
    options.SignIn.RequireConfirmedEmail = true;
    
});


//
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(10));

//Se agrega el servicio para la autenticacion
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

//Se agrega el cookie para el identity cuando se realice el 2FA para logearse
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//    {
//        options.SlidingExpiration = true;
//        options.ExpireTimeSpan = new TimeSpan(0, 1, 0);
//    });

//Se agrega la configuracion del Email que se agrego en la appSettings.json
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

//Se agrega la configuracion de Paypal que se agrego en la appSettings.json
var paypalConfig = builder.Configuration.GetSection("Paypal").Get<PaypalSettings>();
builder.Services.AddSingleton(paypalConfig);

//Se agrega la configuracion de GoogleDrive que se agrego en la appSettings.json
var googleDriveConfig = builder.Configuration.GetSection("GoogleDrive").Get<GoogleDriveSettings>();
builder.Services.AddSingleton(googleDriveConfig);

//Se agrega la configuracion de Whatsapp API  que se agrego en la appSettings.json
var WhatsappConfig = builder.Configuration.GetSection("WhatsappConfig").Get<WhatsappSettings>();
builder.Services.AddSingleton(WhatsappConfig);


//Se agrega el repositorio Email para utilizarlo en el proyecto
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

//builder.Services.AddControllers();
//Se agrega para que funcione la serializacion y deserializacion json
builder.Services.AddControllers( option =>
{
    option.CacheProfiles.Add("Default30",
        new CacheProfile
        {
            Duration = 30
        });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//se agrega el APIkey para generar las solicitudes para el pago con tarjeta
//StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

//se agrega la url del front-end para poder emplearlo con las api que la necesiten
var frontUrl = builder.Configuration.GetSection("frontEnd").Get<FrontSettings>();
builder.Services.AddSingleton(frontUrl);



//Servicio de autenticacion para utilizar google drive
//builder.Services.AddAuthentication(o =>
//{
//    o.DefaultAuthenticateScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
//    o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
//    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//}).AddCookie()
//.AddGoogleOpenIdConnect(options =>
//{
//    options.ClientId = "533765406103-0mt3gsdbckirrrk7920dsdn0552fmkoe.apps.googleusercontent.com";
//    options.ClientSecret = "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";
//});


var app = builder.Build();

// Configure the HTTP request pipeline.




if (app.Environment.IsDevelopment())
{
    //Descomentar cuando se quiera realizar pruebas solo en modo local
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//Descomentar esta seccion para poder probarlo en la red
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("../swagger/v1/swagger.json", "CapernovaAPI");
    options.RoutePrefix = String.Empty;
});

app.UseHttpsRedirection();



//Se agrega a la aplicacion el permiso de utilizar CORS
app.UseCors();

// Permite agregar el permiso de CORS
//app.UseCors(x => x
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//    .SetIsOriginAllowed(origin => true)
//    .AllowCredentials()
//);

//Permite realizar la autenticacion ya que se va a emplear la autenticacion de google
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
