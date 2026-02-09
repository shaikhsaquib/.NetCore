using StudentManagement.Repositories;
using StudentManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

app.MapControllers();

app.Run();
