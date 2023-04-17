using System.ComponentModel.DataAnnotations;

namespace FileHub.Dto;

public sealed class FileDetailsResponse
{
    [Required] 
    public required Guid Id { get; init; }

    [Required] 
    public required string FileName { get; init; }

    [Required] 
    public required long Size { get; init; }
}
