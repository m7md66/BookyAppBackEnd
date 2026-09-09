using Application;
using Application.Contracts.Services;
using Application.DTOs;
using Application.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BookyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public FilesController(IEmailService emailService, IStringLocalizer<SharedResource> localizer) {
        _emailService = emailService;
        _localizer = localizer;
        }


        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            //var ss = _userId;
            if (file == null || file.Length == 0)
            {
                return BadRequest(_localizer[MessageKeys.NoFileSelected].Value);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Books", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/Books/{fileName}";
            string? coverImageUrl = TryGenerateCoverFromFirstPage(filePath, fileName);

            return Ok(new { fileUrl, coverImageUrl });
        }

        // Renders the first page of an uploaded PDF as the book's default cover image.
        private string? TryGenerateCoverFromFirstPage(string filePath, string fileName)
        {
            if (!string.Equals(Path.GetExtension(fileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            try
            {
                var coverFileName = Path.GetFileNameWithoutExtension(fileName) + "_cover.png";
                var coverPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Books", coverFileName);

                using (var pdfStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    PDFtoImage.Conversion.SavePng(coverPath, pdfStream, page: 0);
                }

                return $"{Request.Scheme}://{Request.Host}/Books/{coverFileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to generate cover image from PDF: {ex}");
                return null;
            }
        }
        [HttpPost("SendMail")]
        public Task<ApiResponse<bool>> SendMail()
        {
           

            return _emailService.sendMailAsync();
        }

    }
}
