using System.ComponentModel.DataAnnotations;

namespace FileHub.Dto;

public sealed class UploadRequest
{
    public string? FileName { get; init; }

    [Required] 
    public required string Username { get; init; }

    public string? Password { get; init; }
}
