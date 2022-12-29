using Ciel.API.Core;
using Ciel.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Ciel.API.Controllers;

public class FileUploadController : APIController
{
    private readonly AppConfig _appConfig;

    public FileUploadController(IOptions<AppConfig> config)
    {
        _appConfig = config.Value;
    }

    [HttpPost("/api/upload")]
    [Authorize]
    public async Task<IActionResult> FileUploadAsync(IFormFileCollection files)
    {
        var listFile = new List<string>();
        foreach (var file in files)
        {
            listFile.Add(await UploadAsync(file));
        }
        return Ok(ApiResult<List<string>>.Success(listFile));
    }

    private async Task<string> UploadAsync(IFormFile file)
    {
        var uploadPath = Path.Combine(_appConfig.ConfigurationsPath, _appConfig.UploadFolder);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var fileName = GetRandomFileNameWithExtension(Path.GetExtension(file.FileName));
        var tempFile = Path.Combine(uploadPath, fileName);

        using var stream = System.IO.File.OpenWrite(tempFile);
        await file.CopyToAsync(stream);

        return Path.Combine(_appConfig.UploadFolder, fileName);
    }

    private static string GetRandomFileNameWithExtension(string? extension)
    {
        return Path.ChangeExtension(Path.GetRandomFileName(), extension);
    }
}
