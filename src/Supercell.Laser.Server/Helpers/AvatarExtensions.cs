namespace Supercell.Laser.Server.Helpers
{
    using Supercell.Laser.Server.Core;
    using Supercell.Laser.Server.Logic.Avatar;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic.Enums;
    using Supercell.Laser.Titan.Logic.Math;

    internal static class AvatarExtensions
    {
        /// <summary>
        /// Saves the specified avatar.
        /// </summary>
        internal static void Save(this LogicClientAvatar avatar, DBMS database = Settings.Database)
        {
            Avatars.Save(avatar, database);
        }

        internal static void Reset(this LogicClientAvatar avatar)
        {
            avatar.Thumbnail = new LogicData(28, 0);
            avatar.NameColor = new LogicData(43, 0);
            avatar.TutorialsCompletedCount = 2;
            avatar.TrophyRoadReward = 200;
            avatar.Home.Theme = new LogicData(41, 0);
            avatar.Home.SelectedBrawler = new LogicData(16, 0);
            avatar.SelectedBrawler = new LogicData(16, 0);
            avatar.Home.Region = "RU";
            avatar.Home.PlayedGameModes.AddRange(new int[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10});

            avatar.Home.BrawlPass = new Logic.Slots.Items.BrawlPassSeasonData()
            {
                Season = 3
            };

            avatar.Home.BattleTokensTimer = new LogicTimer();
            avatar.Home.BattleTokensTimer.StartTimer(avatar.Time, 77777);

            avatar.Home.BrawlPassTimer = new LogicTimer();
            avatar.Home.BrawlPassTimer.StartTimer(avatar.Time, 77777);

            avatar.Home.PowerPlayTimer = new LogicTimer();
            avatar.Home.PowerPlayTimer.StartTimer(avatar.Time, 77777);

            avatar.Home.TrophySeasonTimer = new LogicTimer();
            avatar.Home.TrophySeasonTimer.StartTimer(avatar.Time, 77777);

            avatar.Brawlers.Reset();
        }
    }
}
