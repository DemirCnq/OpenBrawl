namespace Supercell.Laser.Server.Logic.Avatar
{
    using Newtonsoft.Json;
    using Supercell.Laser.Server.Logic.Avatar.Slots;
    using Supercell.Laser.Server.Logic.Home;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic.Enums;
    using Supercell.Laser.Titan.Logic.Math;
    using System;

    internal class LogicClientAvatar
    {
        public const int CommodityCount = 8;

        [JsonIgnore] internal Connection Connection;

        [JsonProperty] internal LogicClientHome Home;

        [JsonProperty] internal int HighID;
        [JsonProperty] internal int LowID;

        [JsonProperty] internal string Token;

        [JsonProperty] internal Rank Rank;

        internal LogicLong Identifier => new LogicLong(HighID, LowID);

        [JsonProperty] internal LogicData Thumbnail;
        [JsonProperty] internal LogicData NameColor;

        [JsonProperty] internal int TrophyRoadReward;

        [JsonProperty] internal int TokenDoublerLeft;

        [JsonProperty] internal string Name;
        [JsonProperty] internal bool NameSet;

        [JsonProperty] internal int Diamonds;

        [JsonProperty] internal int TutorialsCompletedCount;

        [JsonProperty] internal LogicResources Resources;
        [JsonProperty] internal int Experience;

        [JsonProperty] internal Brawlers Brawlers;
        [JsonProperty] internal LogicData SelectedBrawler;
        [JsonProperty] internal int Tokens;

        [JsonProperty] internal int BoxesFromLastGoodDrop;

        internal readonly LogicTime Time;

        internal int TokensReward;
        internal int TrophiesReward;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicClientAvatar"/> class.
        /// </summary>
        internal LogicClientAvatar(Connection connection, LogicLong id) : this()
        {
            this.Connection = connection;
            this.HighID = id.High;
            this.LowID = id.Low;
        }

        internal LogicClientAvatar()
        {
            Thumbnail = new LogicData(28, 0);
            NameColor = new LogicData(43, 0);

            Home = new LogicClientHome(this);
            Resources = new LogicResources();

            Rank = Rank.Player;

            Time = new LogicTime();

            if (Experience == 0)
            {
                Experience = 800;
            }

            Brawlers = new Brawlers();
        }

        internal int Score
        {
            get
            {
                return Brawlers.GetTrophies();
            }
        }

        internal void Encode(ByteStream stream)
        {
            stream.EncodeLogicLong(Identifier);
            stream.EncodeLogicLong(Identifier);
            stream.EncodeLogicLong(Identifier);

            stream.WriteString(Name);
            stream.WriteBoolean(NameSet);
            stream.WriteInt(0);

            stream.WriteVInt(CommodityCount);

            stream.WriteVInt(Brawlers.Count);
            foreach (var brawler in Brawlers)
            {
                brawler.EncodeUnlockDataSlot(stream);
            }
            stream.WriteVInt(Brawlers.Count);
            foreach (var brawler in Brawlers)
            {
                brawler.EncodeTrophiesDataSlot(stream);
            }
            stream.WriteVInt(Brawlers.Count);
            foreach (var brawler in Brawlers)
            {
                brawler.EncodeTrophiesDataSlot(stream);
            }
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteVInt(Diamonds); // Diamonds
            stream.WriteVInt(0); // CumulativePurchasedDiamonds
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(2);
            stream.WriteVInt(TutorialsCompletedCount);
        }
    }
}
