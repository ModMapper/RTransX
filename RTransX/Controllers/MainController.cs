namespace RTransX.Controllers;

using Microsoft.AspNetCore.Mvc;

using RTransX.Models;
using RTransX.Modules;
using System.Net.Mime;

[Route("/")]
[ApiController]
public class MainController(ILogger<MainController> logger, TokenChecker checker, DeepLPool pool) : ControllerBase {
    private long idSeq = 0;

    public ILogger<MainController> Logger => logger;

    public TokenChecker TokenChecker => checker;

    public DeepLPool Pool => pool;

    [HttpGet("/")]
    public IActionResult Index() {
        return Content(Resources.Index, MediaTypeNames.Text.Html);
    }

    [HttpPost("translate")]
    public async Task<Response> Translate(Request req, [FromQuery(Name = "token")]string? token) {
        if(TokenChecker.Check(token)) {
            long id = Interlocked.Increment(ref idSeq);
            Logger.LogInformation("[{id}] 번역 요청 [{source,2} -> {target,2}][{length}자]", id, req.SourceLang, req.TargetLang, req.Text.Length);
            var result = await Pool.TranslateAsync(req.Text, req.SourceLang, req.TargetLang);
            Logger.LogInformation("[{id}] 번역 완성 [{source,2} -> {target,2}][{length}자]", id, req.SourceLang, req.TargetLang, result.Text.Length);
            return new() {
                Alternatives = result.Alternatives,
                Code = 200,
                Data = result.Text,
                Id = id,
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
