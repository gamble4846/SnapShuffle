using Microsoft.Extensions.Configuration;
using SnapShuffle.DataAccess.Implementation;
using SnapShuffle.DataAccess.Interface;
using SnapShuffle.Managers.Implementation;
using SnapShuffle.Managers.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ITbUserManager, TbUserManager>();
builder.Services.AddTransient<ITbUserDataAccess, TbUserDataAccess>();
builder.Services.AddTransient<IScreenshotManager, ScreenshotManager>();
#endregion

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
