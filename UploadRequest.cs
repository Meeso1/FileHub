using System.ComponentModel.DataAnnotations;

namespace FileHub;

public sealed class UploadRequest
{
    [Required] 
    public string FileName { get; init; }

    [Required] 
    public string Username { get; init; }

    [Required] 
    public string Password { get; init; }
}