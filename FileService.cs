using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FileHub;

public sealed class FileService
{
    private readonly IOptions<StorageConfiguration> _config;
    private readonly FilesDbContext _context;

    public FileService(IOptions<StorageConfiguration> config, FilesDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task CreateAsync(FileData fileData, IFormFile file)
    {
        _context.Files.Add(fileData.ToEntity());
        await _context.SaveChangesAsync();

        var path = Path.Combine(_config.Value.StorageLocation, fileData.Id.ToString());
        var storageFile = File.Open(path, FileMode.CreateNew);
        await file.CopyToAsync(storageFile);
    }

    public async Task<bool?> CheckPermissionsAsync(Guid id, string username, string password)
    {
        var entity = await _context.Files.FirstOrDefaultAsync(e => e.Id == id);
        if (entity is null) return null;

        return entity.Username == username && entity.Password == password;
    }

    public async Task<StreamWithData> GetStreamAsync(Guid id)
    {
        var fileData = FileData.FromEntity(await _context.Files.FirstAsync(e => e.Id == id));
        var path = Path.Combine(_config.Value.StorageLocation, id.ToString());
        var stream = new FileStream(path, FileMode.Open);
        return new(stream, fileData);
    }
}

public sealed record StreamWithData(FileStream Stream, FileData Data);
