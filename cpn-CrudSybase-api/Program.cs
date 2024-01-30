using cpn_CrudSybase_api.Services;
using cpn_CrudSybase_api.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(ISybaseAdoService), typeof(SybaseAdoService));
var configuration = builder.Configuration;
builder.Services.Configure<Settings>(
          options =>
          {
              //GeneradorRSA rsa = new GeneradorRSA();
              //options.SybaseConnectionString = rsa.decryptedRSA(configuration.GetValue<string>("ConnectionStrings:SybaseConnectionRSA"));
              options.SybaseConnectionString = configuration.GetValue<string>("ConnectionStrings:SybaseConnectionRSA");
          }
);


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
