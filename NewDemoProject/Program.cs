using API_Controller_Demo;
using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using ApplicationLayer.Services;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using InfrastructureLayer.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

var builder = WebApplication.CreateBuilder(args);

//Add DBContext
builder.Services.AddDbContext<MyDemoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDemoDBContext") ?? throw new InvalidOperationException("Connection string 'MyDemoDBContext' not found.")));

// Inject DBContext class for UnitOfWork
builder.Services.AddScoped<DbContext, MyDemoDBContext>();

// Dependancy Injection for Unit Of Work and Repository.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//SP: Dot net core ITrackableRepository Service (Provided By Dotnet Service).
builder.Services.AddScoped<ITrackableRepository<User>, TrackableRepository<User>>();
builder.Services.AddScoped<IUserService, UserService>();

//SP: Extend with Custom IRepositoryX Service with ITrackableRepository service. (Extend with Custom Service + Provided By Dotnet Service).
builder.Services.AddScoped<IRepositoryX<Movie>, RepositoryX<Movie>>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IRepositoryX<Theater>, RepositoryX<Theater>>();
builder.Services.AddScoped<ITheaterService, TheaterService>();
builder.Services.AddScoped<IRepositoryX<ShowTime>, RepositoryX<ShowTime>>();
builder.Services.AddScoped<IShowTimeService, ShowTimeService>();
builder.Services.AddScoped<IRepositoryX<Seats>, RepositoryX<Seats>>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IRepositoryX<Booking>, RepositoryX<Booking>>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IRepositoryX<SeatBooking>, RepositoryX<SeatBooking>>();
builder.Services.AddScoped<ISeatBookingService, SeatBookingService>();

// Add AutoMapper Service
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Dependancy Injection for Email Service
builder.Services.AddSingleton<IEmailService, EmailService>(sp =>
{
    var ServiceProvider = builder.Services.BuildServiceProvider();
    var logger = ServiceProvider.GetRequiredService<ILogger<EmailService>>();
    var smtpServer = builder.Configuration["SmtpConfig:SmtpServer"];
    var smtpPort = int.Parse(builder.Configuration["SmtpConfig:SmtpPort"]);
    var smtpUsername = builder.Configuration["SmtpConfig:SmtpUsername"];
    var smtpPassword = builder.Configuration["SmtpConfig:SmtpPassword"];
    var senderEmail = builder.Configuration["SmtpConfig:SenderEmail"];
    return new EmailService(smtpServer, smtpPort, smtpUsername, smtpPassword, senderEmail, logger);
});

//Dependancy Injection For User and Role 
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
{
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedAccount = true;

}).AddEntityFrameworkStores<MyDemoDBContext>().AddDefaultTokenProviders();
// JWT Token Service
builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
    };
});

// For JWTTokenHelper Injection
builder.Services.AddSingleton(sp =>
{
    var secretKey = builder.Configuration["JWT:Secret"];
    var issuer = builder.Configuration["JWT:ValidIssuer"];
    var audience = builder.Configuration["JWT:ValidAudience"];
    var tokenExpirationMinutes = 60;

    return new JwtTokenHelper(secretKey, issuer, audience, tokenExpirationMinutes);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For Swagger Authorize button
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Configure the Swagger "Authorize" button to use the JWT token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[] { }
            }
        });
});

var app = builder.Build();
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
