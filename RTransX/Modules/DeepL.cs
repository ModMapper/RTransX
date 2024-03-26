namespace RTransX.Modules;

using Microsoft.Playwright;

using RTransX.Models;

using System.Threading.Tasks;

public class DeepL {
    public static async Task<DeepL> CreateAsync(IBrowser browser) {
        var page = await browser.NewPageAsync();
        await page.GotoAsync(URL);
        await page.WaitForSelectorAsync("#textareasContainer");
        await Task.Delay(2000);
        await page.EvaluateAsync(Resources.DeepL);
        return new DeepL(page);
    }

    private DeepL(IPage page) {
        Page = page;
    }

    private const string URL = @"https://www.deepl.com/translator";

    private IPage Page { get; }

    public async Task SetSourceAsync(string lang) {
        await Page.EvaluateAsync("setSourceLang", lang);
    }

    public async Task SetTargetAsync(string lang) {
        await Page.EvaluateAsync("setTargetLang", lang);
    }

    public async Task<DeepLResult> TranslateAsync(string text) {
        return await Page.EvaluateAsync<DeepLResult>("translate", text);
    }
}
