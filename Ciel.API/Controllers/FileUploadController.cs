using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ciel.API.Controllers;

public class FileUploadController : APIController
{
    private readonly string _uploadPath;

    public FileUploadController(IConfiguration configuration)
    {
        _uploadPath = configuration["Settings:StoredFilesPath"] ?? Directory.GetCurrentDirectory();
    }

    [HttpPost("/api/upload")]
    [Authorize]
    public async Task<IActionResult> FileUploadAsync(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            await UploadAsync(file);
        }
        return Ok();
    }

    private async Task<string> UploadAsync(IFormFile file)
    {
        if (!Directory.Exists(_uploadPath))
            Directory.CreateDirectory(_uploadPath);

        var fileName = GetRandomFileName(Path.GetExtension(file.FileName));
        var tempFile = Path.Combine(_uploadPath, fileName);

        using var stream = System.IO.File.OpenWrite(tempFile);
        await file.CopyToAsync(stream);

        return tempFile;
    }

    private static string GetRandomFileName(string extension = null)
    {
        if (extension == null)
        {
            return Path.GetRandomFileName();
        }
        return Path.ChangeExtension(Path.GetRandomFileName(), extension);
    }
}
