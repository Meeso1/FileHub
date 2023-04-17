using System.ComponentModel.DataAnnotations;

namespace FileHub;

public sealed class FileRequest
{
    [Required] 
    public required string Username { get; init; }

    [Required] 
    public required string Password { get; init; }
}