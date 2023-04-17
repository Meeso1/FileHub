using FileHub.Configuration;
using FileHub.Database;
using Microsoft.AspNetCore.Identity;
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
        await using var storageFile = File.Open(path, FileMode.CreateNew);
        await file.CopyToAsync(storageFile);
    }

    public async Task<bool?> CheckPermissionsAsync(Guid id, string username, string? password)
    {
        var entity = await _context.Files.FirstOrDefaultAsync(e => e.Id == id);
        if (entity is null) return null;

        return entity.Username == username && CheckPassword(password, entity.PasswordHash, username);
    }

    public async Task<StreamWithData> GetStreamAsync(Guid id)
    {
        var fileData = FileDetails.FromEntity(await _context.Files.FirstAsync(e => e.Id == id));
        var path = Path.Combine(_config.Value.StorageLocation, id.ToString());
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return new StreamWithData(stream, fileData);
    }

    public async Task<IReadOnlyList<FileDetails>> GetFilesForUserAsync(string username)
    {
        return await _context.Files.Where(e => e.Username == username).Select(e => FileDetails.FromEntity(e))
            .ToListAsync();
    }

    public static string? HashPassword(string? password, string username)
    {
        return password is null ? null : new PasswordHasher<string>().HashPassword(username, password);
    }

    private static bool CheckPassword(string? password, string? hash, string username)
    {
        if (hash is null) return true;
        if (password is null) return false;

        return new PasswordHasher<string>().VerifyHashedPassword(username, hash, password) is
            PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}

public sealed record StreamWithData(FileStream Stream, FileDetails Details);
