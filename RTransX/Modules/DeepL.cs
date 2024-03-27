namespace RTransX.Modules;

using Microsoft.Playwright;

using RTransX.Models;

using System.Threading.Tasks;

public class DeepL {
    public static async Task<DeepL> CreateAsync(IBrowser browser) {
        var page = await browser.NewPageAsync();
        await page.GotoAsync(URL);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await page.EvaluateAsync(Resources.DeepL);
        return new DeepL(page);
    }

    private DeepL(IPage page) {
        Page = page;
    }

    private const string URL = @"https://www.deepl.com/translator";

    private IPage Page { get; }

    public async Task SetLanguageAsync(string source, string target) {
        await Page.EvaluateAsync("setLanguage", new { source, target });
    }

    public async Task<DeepLResult> TranslateAsync(string text) {
        return await Page.EvaluateAsync<DeepLResult>("translateAsync", text);
    }
}
