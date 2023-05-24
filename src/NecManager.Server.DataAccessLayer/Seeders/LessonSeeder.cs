namespace NecManager.Server.DataAccessLayer.Seeders;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

internal class LessonSeeder
{
    private ILogger<LessonSeeder> logger;
    private readonly ILessonAccessLayer lessonAccessLayer;

    public LessonSeeder(ILogger<LessonSeeder> logger, ILessonAccessLayer lessonAccessLayer)
    {
        this.logger = logger;
        this.lessonAccessLayer = lessonAccessLayer;
    }

    public async Task EnsureSeedDataAsync()
    {
        await this.SeedLessonAsync().ConfigureAwait(false);
    }

    private async Task SeedLessonAsync()
    {
        var lessonCount = await this.lessonAccessLayer.CountCollectionAsync(new(10, 1)).ConfigureAwait(false);

        if (lessonCount > 0)
            return;

        var lesson = new Lesson
        {
            Title = "A définir",
            Content = "...",
            Description = "...",
            Difficulty = DifficultyType.None,
            Weapon = WeaponType.None,
        };

        _ = await this.lessonAccessLayer.AddAsync(lesson).ConfigureAwait(false);
        this.logger.LogInformation("First lesson created.");
    }

}
