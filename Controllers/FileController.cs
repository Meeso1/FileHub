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
    public async Task<Guid> UploadAsync(UploadRequest request, IFormFile file)
    {
        var fileData = FileData.FromRequest(request);
        await _fileService.CreateAsync(fileData, file);
        return fileData.Id;
    }

    [HttpGet]
    [Route("download/{id:guid}")]
    public async Task<ActionResult<FileResult>> DownloadAsync(Guid id, [FromQuery] FileRequest request)
    {
        var result = await _fileService.CheckPermissionsAsync(id, request.Username, request.Password);
        switch (result)
        {
            case null:
                return NotFound();
            case false:
                return Forbid();
        }

        var streamWithData = await _fileService.GetStreamAsync(id);
        return File(streamWithData.Stream, "text/plain", streamWithData.Data.FileName);
    }
}