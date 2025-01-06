
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PRN231_Assignment2_eBookStoreAPI.Interface;
using PRN231_Assignment2_eBookStoreAPI.Repositories.Common;
using PRN231_Assignment2_eBookStoreAPI.UnitOfWork;
using PRN231_FinalProject.DTO;
using PRN231_FinalProject.Interface.Repositories.Common;
using PRN231_FinalProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

});
//builder.Services.AddControllers().AddNewtonsoftJson(x =>
//    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddDbContext<PRN231_FinalProjectContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
