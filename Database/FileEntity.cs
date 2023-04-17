using System.ComponentModel.DataAnnotations;

namespace FileHub.Database;

public sealed class FileEntity
{
    [Key] 
    public required Guid Id { get; init; }

    public required string FileName { get; init; }

    public required string Username { get; init; }

    public string? Password { get; init; }

    public required long Size { get; init; }
}
