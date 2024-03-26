namespace RTransX.Modules;

using Microsoft.Playwright;
using System;

public class PlaywrightHelper {
    private static readonly Lazy<Task<IPlaywright>> PlaywrightTask = new(InitPlaywright);

    private static async Task<IPlaywright> InitPlaywright() {
        Program.Main(["install"]);
        return await Playwright.CreateAsync();
    }

    public static async Task<IBrowser> CreateBrowserAsync() {
        var playwright = await PlaywrightTask.Value;
        return await playwright.Chromium.LaunchAsync(new() {
            Headless = true,
        });
    }

}
