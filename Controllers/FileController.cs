using Microsoft.AspNetCore.Mvc;

namespace FileHub.Controllers;

[ApiController]
public sealed class FileController : ControllerBase
{
    [HttpPost]
    public void UploadAsync(IFormFile file)
    {
        throw new NotImplementedException();
    }
}