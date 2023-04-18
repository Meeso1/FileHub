using FileHub;
using FileHub.Configuration;
using FileHub.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<FileService>();
builder.Services.AddDbContext<FilesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Files")));
builder.Services.Configure<StorageConfiguration>(builder.Configuration.GetSection(StorageConfiguration.SectionName));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FilesDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(
    x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(_ => true).AllowCredentials()
);
app.MapControllers();

app.Run();
