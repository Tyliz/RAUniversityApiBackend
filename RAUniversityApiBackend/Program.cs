// Version 0.0.1
// 1. Using para trabajar con entity framework
using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;

var builder = WebApplication.CreateBuilder(args);


// 2. Connect with the SQL Server Express
const string CONNECTION_NAME = "DBUniversity";

var connectionString = builder.Configuration.GetConnectionString(CONNECTION_NAME);

// 3. Add Context
builder.Services.AddDbContext<DBUniversityContext>(
	options => options
		.UseSqlServer(connectionString)
);

// Add services to the container.

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
