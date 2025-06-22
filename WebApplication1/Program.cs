using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1.Common;
using WebApplication1.Common.Contracts;
using WebApplication1.Common.Extensions;
using WebApplication1.Repository.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger= new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Services.AddSingleton(Log.Logger);

//Adding configuration options
builder.Services.ConfigureOptions(configuration);
//Injecting services
builder.Services.ConfigureServices();


var sp = builder.Services.BuildServiceProvider();

//Adding DbContext  
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var secretManager = sp.GetRequiredService<ISecretManager>();
    var dbUsername = secretManager.GetDbUsername();
    var dbPassword = secretManager.GetDbPassword();
    
    var append= $";User Id={dbUsername};Password={dbPassword};";
    var connectionString = configuration.GetConnectionString("DefaultConnection") + append;

    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//not recommended for prod use, script inside cd should be executed for this operation
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();
}
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
