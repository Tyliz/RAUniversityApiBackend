// 1. Using para trabajar con entity framework
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RAUniversityApiBackend.DataAccess;
using RAUniversityApiBackend.Extensions;
using RAUniversityApiBackend.Services;
using RAUniversityApiBackend.Services.Interfaces;
// 13. Using serilog for events
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 14. Config Serilog
builder.Host.UseSerilog((hostBuilderCtx, loggerConf) =>
{
	loggerConf
		.WriteTo.Console()
		.WriteTo.Debug()
		.ReadFrom.Configuration(hostBuilderCtx.Configuration);
});

// 10. Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 2. Connect with the SQL Server Express
const string CONNECTION_NAME = "DBUniversity";

var connectionString = builder.Configuration.GetConnectionString(CONNECTION_NAME);

// 3. Add Context
builder.Services.AddDbContext<DBUniversityContext>(
	options => options
		.UseSqlServer(connectionString)
);

// 7. Add Service of JWT Authorization
builder.Services.AddJWTServices(builder.Configuration);


// Add services to the container.

builder.Services.AddControllers();

// 4. Add Custom Services (Folder Services)
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IChaptersService, ChaptersService>();
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<IStudentsService, StudentsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAccountsService, AccountsService>();


// 5. CORDS Configuration
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "CordsPolicy", builder =>
	{
		builder.AllowAnyOrigin();
		builder.AllowAnyMethod();
		builder.AllowAnyHeader();
	});
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 8. Add Authorization
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("UserOnlyPolicy", policy => policy.RequireClaim("UserOnly", "User1"));
});

// 9. configure Swagger to care of Authorization
builder.Services.AddSwaggerGen(options =>
	{
		// We define the Security for authorization
		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			In = ParameterLocation.Header,
			Description = "JWT Authorization Header using Bearer Scheme",
		});

		options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				new string[]{}
			}
		});
	}
);

var app = builder.Build();

// 11. SUPPORTED CULTURES
var suppertedCultures = new[] { "en-US", "es-ES", "fr-FR", "de-de", };
var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture(suppertedCultures[0]) // English by default
	.AddSupportedCultures(suppertedCultures) // Add all supported cultures for UI 
	.AddSupportedUICultures(suppertedCultures); // Add supported cultures for UI 

// 12. ADD LOCALIZATION to App
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// 15. Tell app to use Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 6. Tell app to use cords
app.UseCors("CordsPolicy");

app.Run();
