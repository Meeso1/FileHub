using System.ComponentModel.DataAnnotations;

namespace FileHub.Configuration;

public class StorageConfiguration
{
    public const string SectionName = "Storage";

    [Required]
    public required string StorageLocation { get; init; }
}
