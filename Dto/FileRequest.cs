using System.ComponentModel.DataAnnotations;

namespace FileHub.Dto;

public sealed class FileRequest
{
    [Required]
    public required string Username { get; init; }

    public string? Password { get; init; }
}
