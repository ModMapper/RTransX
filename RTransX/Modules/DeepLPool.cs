namespace RTransX.Modules;

using Microsoft.Playwright;

using RTransX.Models;

using System.Collections.Concurrent;

public class DeepLPool {
    public static async Task<DeepLPool> CreatePoolAsync(int count) {
        return new(await PlaywrightHelper.CreateBrowserAsync(), count);
    }

    private DeepLPool(IBrowser browser, int count) {
        Count = count;
        Browser = browser;
        Lock = new(count, count);
        Pool = [];
        Cache = new((key) => FetchTranslateAsync(key.text, key.source, key.target));
    }

    public int Count { get; }

    private IBrowser Browser { get; }

    private SemaphoreSlim Lock { get; }

    private ConcurrentStack<DeepL> Pool { get; }

    private Cache<(string text, string source, string target), DeepLResult> Cache { get; }

    public async Task<DeepLResult> TranslateAsync(string text, string source, string target) {
        return await Cache.GetValueAsync((text, source, target));
    }

    private async Task<DeepLResult> FetchTranslateAsync(string text, string source, string target) {
        if (text.Length == 0) return new() { Text = "", Alternatives = [] };
        await Lock.WaitAsync();
        try {
            if (!Pool.TryPop(out var deepl))
                deepl = await DeepL.CreateAsync(Browser);
            try {
                return await deepl.TranslateAsync(text, source, target);
            } finally {
                Pool.Push(deepl);
            }
        } finally {
            Lock.Release();
        }
    }
}
