using Autofac;
using Autofac.Extensions.DependencyInjection;
using code;
using code.common;

var builder = WebApplication.CreateBuilder(args);

var connectionStrings = new ConnectionString(relationalDBConn: "");
    builder.Services.AddOptions();
    builder.Services.Configure<ConnectionString>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new DependencyInjection<int, int>()));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware(typeof(CustomMiddleware<int>));

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
