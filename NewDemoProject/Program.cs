using ApplicationLayer.Interface;
using ApplicationLayer.Services;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

// Dependancy Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITrackableRepository<User>, TrackableRepository<User>>();
builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyDemoDBContext>().AddDefaultTokenProviders();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
