using Microsoft.EntityFrameworkCore;

namespace FileHub.Database;

public sealed class FilesDbContext : DbContext
{
    public FilesDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<FileEntity> Files => Set<FileEntity>();
}
