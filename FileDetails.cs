using FileHub.Database;
using FileHub.Dto;

namespace FileHub;

public sealed class FileDetails
{
    public required Guid Id { get; init; }

    public required string FileName { get; init; }

    public required string Username { get; init; }

    public required string? PasswordHash { get; init; }

    public required long Size { get; init; }

    public FileEntity ToEntity()
    {
        return new FileEntity
        {
            Id = Id,
            FileName = FileName,
            PasswordHash = PasswordHash,
            Username = Username,
            Size = Size
        };
    }

    public static FileDetails FromEntity(FileEntity entity)
    {
        return new FileDetails
        {
            Id = entity.Id,
            FileName = entity.FileName,
            Username = entity.Username,
            PasswordHash = entity.PasswordHash,
            Size = entity.Size
        };
    }

    public FileDetailsResponse ToResponse()
    {
        return new FileDetailsResponse
        {
            Id = Id,
            FileName = FileName,
            Size = Size
        };
    }
}
