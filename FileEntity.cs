using System.ComponentModel.DataAnnotations;

namespace FileHub;

public sealed class FileEntity
{
    [Key] 
    public required Guid Id { get; init; }

    public required string FileName { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }
}