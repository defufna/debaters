using Debaters.WebAPI;
using Microsoft.Extensions.FileProviders;
using VeloxDB.AspNet.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions((options)=>
{
    options.JsonSerializerOptions.Converters.Add(new LongToStringConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddVeloxDBConnectionProvider("address=localhost:7568;");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine(Path.GetFullPath("../www"));
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseFileServer(new FileServerOptions()
    {
        FileProvider = new PhysicalFileProvider(Path.GetFullPath("../www")),
    });
    app.UseRouting();
    app.MapGet("/c/{*rest}", async (context) => await context.Response.SendFileAsync("../www/index.html"));
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
