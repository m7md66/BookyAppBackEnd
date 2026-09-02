using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookyApp.Controllers
{
    /// <summary>
    /// Public, unauthenticated landing page for a shared quotation.
    /// GET /q/{id} renders a small HTML page that:
    ///   1. shows the quote (with Open Graph tags so WhatsApp/Twitter render a preview),
    ///   2. tries to open the mobile app via the "bookyapp://quotation/{id}" deep link,
    ///   3. falls back to the platform's app store if the app is not installed.
    /// Store URLs and the public base URL come from the "Sharing" section of appsettings.json.
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("q")]
    public class ShareController : ControllerBase
    {
        private readonly IQuotationService _quotationService;
        private readonly IConfiguration _configuration;

        public ShareController(IQuotationService quotationService, IConfiguration configuration)
        {
            _quotationService = quotationService;
            _configuration = configuration;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Quotation(Guid id)
        {
            var sharing = _configuration.GetSection("Sharing");
            var scheme = sharing["AppScheme"] ?? "bookyapp";
            var androidUrl = sharing["AndroidStoreUrl"] ?? "";
            var iosUrl = sharing["IosStoreUrl"] ?? "";

            var result = await _quotationService.GetQuotationById(id);
            var quote = result?.Data;

            var content = quote?.Content ?? "";
            var bookTitle = quote?.BookTitle ?? "Booky";
            var author = quote?.BookAuther ?? "";

            var pageTitle = string.IsNullOrWhiteSpace(bookTitle) ? "Booky" : bookTitle;
            var description = string.IsNullOrWhiteSpace(content)
                ? "Read and share quotes on Booky."
                : $"“{content}”" + (string.IsNullOrWhiteSpace(author) ? "" : $" — {author}");

            var appLink = $"{scheme}://quotation/{id}";

            var html = BuildHtml(
                HtmlEncoder.Default.Encode(pageTitle),
                HtmlEncoder.Default.Encode(description),
                HtmlEncoder.Default.Encode(content),
                HtmlEncoder.Default.Encode(bookTitle),
                HtmlEncoder.Default.Encode(author),
                JsonSerializer.Serialize(appLink),
                JsonSerializer.Serialize(androidUrl),
                JsonSerializer.Serialize(iosUrl));

            return Content(html, "text/html; charset=utf-8");
        }

        private static string BuildHtml(
            string pageTitle, string description, string content, string bookTitle, string author,
            string appLinkJs, string androidUrlJs, string iosUrlJs)
        {
            var sb = new StringBuilder();
            sb.Append("<!doctype html><html lang=\"ar\" dir=\"rtl\"><head>");
            sb.Append("<meta charset=\"utf-8\">");
            sb.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            sb.Append($"<title>{pageTitle} · Booky</title>");
            sb.Append($"<meta property=\"og:title\" content=\"{pageTitle}\">");
            sb.Append($"<meta property=\"og:description\" content=\"{description}\">");
            sb.Append("<meta property=\"og:type\" content=\"article\">");
            sb.Append($"<meta name=\"twitter:title\" content=\"{pageTitle}\">");
            sb.Append($"<meta name=\"twitter:description\" content=\"{description}\">");
            sb.Append("<meta name=\"twitter:card\" content=\"summary\">");
            sb.Append("<style>");
            sb.Append("*{box-sizing:border-box}body{margin:0;font-family:-apple-system,Segoe UI,Roboto,sans-serif;");
            sb.Append("background:#F7F3EA;color:#242424;display:flex;min-height:100vh;align-items:center;justify-content:center;padding:24px}");
            sb.Append(".card{background:#fff;border-radius:14px;padding:28px;max-width:420px;width:100%;box-shadow:0 2px 12px rgba(0,0,0,.08)}");
            sb.Append(".quote{font-size:19px;line-height:1.7;font-style:italic;margin:0 0 16px}");
            sb.Append(".book{font-size:14px;font-weight:600;color:#2F5D50}.author{font-size:14px;color:#5F5B54}");
            sb.Append(".btn{display:block;text-align:center;margin-top:24px;background:#2F5D50;color:#fff;text-decoration:none;");
            sb.Append("padding:14px;border-radius:10px;font-size:16px;font-weight:600}");
            sb.Append(".hint{margin-top:12px;font-size:13px;color:#5F5B54;text-align:center}");
            sb.Append("</style></head><body><div class=\"card\">");
            if (!string.IsNullOrEmpty(content))
                sb.Append($"<p class=\"quote\">“{content}”</p>");
            sb.Append($"<div class=\"book\">{bookTitle}</div>");
            if (!string.IsNullOrEmpty(author))
                sb.Append($"<div class=\"author\">{author}</div>");
            sb.Append("<a class=\"btn\" id=\"open\" href=\"#\">افتح في تطبيق Booky</a>");
            sb.Append("<p class=\"hint\">لم يفتح التطبيق؟ ستُنقل تلقائياً إلى المتجر.</p>");
            sb.Append("</div><script>");
            sb.Append($"var appLink={appLinkJs},androidUrl={androidUrlJs},iosUrl={iosUrlJs};");
            sb.Append("var ua=navigator.userAgent||'';");
            sb.Append("var isIOS=/iPad|iPhone|iPod/.test(ua);var isAndroid=/Android/.test(ua);");
            sb.Append("var store=isIOS?iosUrl:(isAndroid?androidUrl:'');");
            sb.Append("function go(){window.location=appLink;if(store){setTimeout(function(){window.location=store;},1400);}}");
            sb.Append("document.getElementById('open').addEventListener('click',function(e){e.preventDefault();go();});");
            sb.Append("if(isIOS||isAndroid){go();}");
            sb.Append("</script></body></html>");
            return sb.ToString();
        }
    }
}
