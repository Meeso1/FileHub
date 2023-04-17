using FileHub.Configuration;
using FileHub.Database;
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

    public async Task CreateAsync(FileDetails fileDetails, IFormFile file)
    {
        _context.Files.Add(fileDetails.ToEntity());
        await _context.SaveChangesAsync();

        if (!Directory.Exists(_config.Value.StorageLocation)) Directory.CreateDirectory(_config.Value.StorageLocation);

        var path = Path.Combine(_config.Value.StorageLocation, fileDetails.Id.ToString());
        var storageFile = File.Open(path, FileMode.CreateNew);
        await file.CopyToAsync(storageFile);
    }

    public async Task<bool?> CheckPermissionsAsync(Guid id, string username, string? password)
    {
        var entity = await _context.Files.FirstOrDefaultAsync(e => e.Id == id);
        if (entity is null) return null;

        return entity.Username == username && (entity.Password is null || entity.Password == password);
    }

    public async Task<StreamWithData> GetStreamAsync(Guid id)
    {
        var fileData = FileDetails.FromEntity(await _context.Files.FirstAsync(e => e.Id == id));
        var path = Path.Combine(_config.Value.StorageLocation, id.ToString());
        var stream = new FileStream(path, FileMode.Open);
        return new StreamWithData(stream, fileData);
    }

    public async Task<IReadOnlyList<FileDetails>> GetFilesForUserAsync(string username)
    {
        return await _context.Files.Where(e => e.Username == username).Select(e => FileDetails.FromEntity(e))
            .ToListAsync();
    }
}

public sealed record StreamWithData(FileStream Stream, FileDetails Details);
