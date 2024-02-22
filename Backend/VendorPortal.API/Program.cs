using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Management.Services.Models;
using User.Management.Services.Services;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailServices, EmailServices>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VendorPortalDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("VendorPortalConnectionString"),
    new MySqlServerVersion(new Version(8, 0, 32)))
);

builder.Services.AddDbContext<VendorPortalAuthDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("VendorPortalAuthConnectionString"),
    new MySqlServerVersion(new Version(8,0,32)))
) ;

builder.Services.AddIdentityCore<UserProfile>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<VendorPortalAuthDbContext>();

builder.Services.AddCors(p =>
    p.AddPolicy("corspolicy", build =>
    {
        build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
