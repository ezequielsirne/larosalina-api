using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using LaRosalinaAPI.Repository;
using Microsoft.EntityFrameworkCore;
using LaRosalinaAPI.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

//SQL Conection
builder.Services.AddDbContext<LaRosalinaDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
//Para genererar en DbContext: Scaffold-DbContext -Connection "Data Source=SQL5105.site4now.net;Initial Catalog=db_a67707_larosalina;User ID=db_a67707_larosalina_admin;Password=29deabril;" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context LaRosalinaDbContext -UseDatabaseNames
//Para actualizar los modelos: Scaffold-DbContext -Connection name=ConnectionStrings:SqlConnection -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context LaRosalinaDbContext -UseDatabaseNames -Force 

//**Dependencias**//
//Microsoft.entityframeworkcore.sqlserver
//Microsoft.entityframeworkcore.tools
//Microsoft.EntityFrameworkCore.Proxies => Para poder usar .UseLazyLoadingProxies(); de ser necesario

//Secret Key para el JWT
var key = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
builder.Services.AddSingleton(provider => key);

//Aquí se configura la Autenciación
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Repository
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

builder.Services.AddScoped<IComprobanteRepository, ComprobanteRepository>();
builder.Services.AddScoped<IGastoRepository, GastoRepository>();
builder.Services.AddScoped<IHuespedRepository, HuespedRepository>();
builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//AutoMapper para mapear los Modelos con los DTOs
builder.Services.AddAutoMapper(typeof(LaRosalinaMapper));
//**Dependencias**//
//AutoMapper
//Automapper.extensions.microsoft.dependencyinjection

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Soporte para CORS
//Se pueden habilitar: 1-Un dominio, 2-multiples dominios,
//3-cualquier dominio (Tener en cuenta seguridad)
//Usamos de ejemplo el dominio: http://localhost:3223, se debe cambiar por el correcto
//Se usa (*) para todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PolicyCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
