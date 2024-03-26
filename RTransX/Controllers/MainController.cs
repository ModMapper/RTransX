namespace RTransX.Controllers;

using Microsoft.AspNetCore.Mvc;

using RTransX.Models;
using RTransX.Modules;
using System.Net.Mime;

[Route("/")]
[ApiController]
public class MainController(TokenChecker checker, DeepLPool pool) : ControllerBase {
    private long idSeq = 0;

    public TokenChecker TokenChecker => checker;

    public DeepLPool Pool => pool;

    [HttpGet("/")]
    public IActionResult Index() {
        return Content(Resources.Index, MediaTypeNames.Text.Html);
    }

    [HttpPost("translate")]
    public async Task<Response> Translate(Request req, [FromQuery(Name = "token")]string? token) {
        if(TokenChecker.Check(token)) {
            var result = await Pool.TranslateAsync(req.Text, req.SourceLang, req.TargetLang);
            return new() {
                Alternatives = result.Alternatives,
                Code = 200,
                Data = result.Text,
                Id = Interlocked.Increment(ref idSeq),
                Method = "Free",
                SourceLang = req.SourceLang,
                TargetLang = req.TargetLang,
            };
        } else {
            return new() {
                Alternatives = [],
                Code = 401,
                Data = string.Empty,
                Id = 0,
                Method = string.Empty,
                SourceLang = req.SourceLang,
                TargetLang = req.TargetLang,
            };
        }
    }
}
