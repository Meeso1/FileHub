using System.ComponentModel.DataAnnotations;

namespace FileHub;

public sealed class FileDataResponse
{
    // TODO: Continue
    [Required] 
    public required Guid Id { get; init; }
}