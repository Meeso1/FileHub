namespace FileHub;

public sealed class FileData
{
    public required Guid Id { get; init; }

    public required string FileName { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }

    public static FileData FromRequest(UploadRequest request)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            FileName = request.FileName,
            Username = request.Username,
            Password = request.Password
        };
    }

    public FileEntity ToEntity()
    {
        return new()
        {
            Id = Id,
            FileName = FileName,
            Password = Password,
            Username = Username
        };
    }

    public static FileData FromEntity(FileEntity entity)
    {
        return new()
        {
            Id = entity.Id,
            FileName = entity.FileName,
            Username = entity.Username,
            Password = entity.Password
        };
    }
}