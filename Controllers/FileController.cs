using FileHub.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FileHub.Controllers;

[ApiController]
public sealed class FileController : ControllerBase
{
    private readonly FileService _fileService;

    public FileController(FileService service)
    {
        _fileService = service;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<Guid> UploadAsync(IFormFile file, [FromQuery] UploadRequest request)
    {
        var fileData = new FileDetails
        {
            Id = Guid.NewGuid(),
            FileName = request.FileName ?? file.FileName,
            Username = request.Username,
            Password = request.Password,
            Size = file.Length
        };

        await _fileService.CreateAsync(fileData, file);
        return fileData.Id;
    }

    [HttpGet]
    [Route("download/{id:guid}")]
    public async Task<ActionResult> DownloadAsync(Guid id, [FromQuery] FileRequest request)
    {
        var result = await _fileService.CheckPermissionsAsync(id, request.Username, request.Password);
        switch (result)
        {
            case null:
                return NotFound();
            case false:
                return Unauthorized();
        }

        var streamWithData = await _fileService.GetStreamAsync(id);
        return File(streamWithData.Stream, "text/plain", streamWithData.Details.FileName);
    }

    [HttpGet]
    [Route("files")]
    public async Task<ActionResult<IReadOnlyList<FileDetailsResponse>>> GetForUserAsync(
        [FromQuery] string? username = null)
    {
        if (username is null) return BadRequest();

        return (await _fileService.GetFilesForUserAsync(username)).Select(f => f.ToResponse()).ToList();
    }
}
