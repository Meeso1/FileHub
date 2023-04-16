using Microsoft.EntityFrameworkCore;

namespace FileHub;

public sealed class FilesDbContext : DbContext
{
    public FilesDbContext(DbContextOptions options) : base(options)
    {
    }
}