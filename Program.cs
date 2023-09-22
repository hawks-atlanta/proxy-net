using proxy_net.Infrastructure.DataSources;
using proxy_net.Infrastructure.Repositories;
using proxy_net.Models.Auth.DataSources;
using proxy_net.Models.Auth.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IAuthDatasource, AuthDatasourcesImpl>(); //injection
builder.Services.AddScoped<IAuthRepository, AuthRepositoryImpl>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "proxy_net v1");
    });
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
