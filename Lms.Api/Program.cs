using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lms.Data.Data;
//using Lms.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LmsApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LmsApiContext") ?? throw new InvalidOperationException("Connection string 'LmsApiContext' not found.")));

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//SeedData
using (var scope = app.Services.CreateScope())
{
    var db= scope.ServiceProvider.GetRequiredService<LmsApiContext>();
    db.Database.EnsureDeleted();
    db.Database.Migrate();
    try
    {
        await SeedData.InitAsync(db);
    }
    catch (Exception e)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(string.Join(" ", e.Message));
    }
}


    //await app.SeedDataAsync();

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
