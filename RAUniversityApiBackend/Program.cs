// Version 0.0.1
// 1. Using para trabajar con entity framework
using Microsoft.EntityFrameworkCore;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Services;
using RAUniversityApiBackend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// 2. Connect with the SQL Server Express
const string CONNECTION_NAME = "DBUniversity";

var connectionString = builder.Configuration.GetConnectionString(CONNECTION_NAME);

// 3. Add Context
builder.Services.AddDbContext<DBUniversityContext>(
	options => options
		.UseSqlServer(connectionString)
);

// 7. Add Service of JWT Authorization
// TODO: builder.Services.AddJwtTokenServices(builder.Configuration);


// Add services to the container.

builder.Services.AddControllers();

// 4. Add Custom Services (Folder Services)
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IChaptersService, ChaptersService>();
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<IStudentsService, StudentsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
// TODO: Add all services

// 5. CORDS Configuration
builder.Services.AddCors(options => {
	options.AddPolicy(name: "CordsPolicy", builder =>
	{
		builder.AllowAnyOrigin();
		builder.AllowAnyMethod();
		builder.AllowAnyHeader();
	});
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 8. TODO: configure Swagger to care of Authorization
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

// 6. Tell app to use cords
app.UseCors("CordsPolicy");

app.Run();
