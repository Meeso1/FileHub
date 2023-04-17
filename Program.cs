using FileHub;
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();