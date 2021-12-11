namespace Supercell.Laser.Server.Logic.Home
{
    using Newtonsoft.Json;
    using Supercell.Laser.Server.Logic.Avatar;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Logic.Slots.Items;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic;
    using Supercell.Laser.Titan.Logic.Math;
    using System;

    internal class LogicClientHome
    {
        [JsonIgnore] internal LogicClientAvatar Avatar;

        [JsonProperty] internal LogicArrayList<int> PlayedGameModes;
        [JsonProperty] internal LogicArrayList<LogicData> UnlockedSkins;
        [JsonProperty] internal LogicArrayList<LogicData> SelectedSkins;

        [JsonProperty] internal LogicTimer TrophySeasonTimer;
        [JsonProperty] internal LogicTimer PowerPlayTimer;
        [JsonProperty] internal LogicTimer BrawlPassTimer;

        [JsonProperty] internal int BattleTokens;
        [JsonProperty] internal LogicTimer BattleTokensTimer;

        [JsonProperty] internal LogicData SelectedBrawler;
        [JsonProperty] internal string Region;
        [JsonProperty] internal string ContentCreator;

        [JsonProperty] internal BrawlPassSeasonData BrawlPass;

        [JsonProperty] internal LogicData Theme;

        internal LogicClientHome(LogicClientAvatar avatar)
        {
            Avatar = avatar;

            PlayedGameModes = new LogicArrayList<int>();
            UnlockedSkins = new LogicArrayList<LogicData>();
            SelectedSkins = new LogicArrayList<LogicData>();
        }

        internal LogicLong Identifier => Avatar.Identifier;

        internal DateTime Timestamp => DateTime.Now;

        internal void Encode(ByteStream stream)
        {
            stream.WriteVInt(Timestamp.Year * 1000 + Timestamp.DayOfYear);
            stream.WriteVInt(75555);

            stream.WriteVInt(Avatar.Score);
            stream.WriteVInt(Avatar.Score);
            stream.WriteVInt(Avatar.Score);

            stream.WriteVInt(122);
            stream.WriteVInt(Avatar.Experience);

            stream.WriteDataReference(Avatar.Thumbnail);
            stream.WriteDataReference(Avatar.NameColor);

            stream.WriteVInt(PlayedGameModes.Count);
            for (int i = 0; i< PlayedGameModes.Count; i++)
            {
                stream.WriteVInt(PlayedGameModes[i]);
            }
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteVInt(0);

            stream.WriteVInt(0); // Token Doublers!
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteVInt(0);
            stream.WriteVInt(1);

            stream.WriteVInt(1);
            stream.WriteVInt(0);

            stream.WriteVInt(4);
            stream.WriteVInt(2);
            stream.WriteVInt(2);
            stream.WriteVInt(2);

            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteVInt(0); // Shop array

            stream.WriteVInt(1);
            {
                stream.WriteVInt(0);
                stream.WriteVInt(0);
                stream.WriteVInt(10);
            }

            stream.WriteVInt(200); // battle tokens
            stream.WriteVInt(0);

            stream.WriteVInt(0);

            stream.WriteVInt(0); // тикетики
            stream.WriteVInt(0);

            stream.WriteDataReference(Avatar.SelectedBrawler);

            stream.WriteString(Region);
            stream.WriteString(ContentCreator);

            stream.WriteVInt(2); // rewards
            {
                stream.WriteInt(3);
                stream.WriteInt(Avatar.TokensReward);

                stream.WriteInt(4);
                stream.WriteInt(Avatar.TrophiesReward);
            }

            stream.WriteVInt(0);

            //BrawlPass.Encode(stream);

            stream.WriteVInt(1);
            {
                stream.WriteVInt(2);
                stream.WriteVInt(34500 +  Avatar.Tokens);
                stream.WriteVInt(1);
                stream.WriteVInt(0);

                stream.WriteVInt(1);
                {
                    for (int i = 0; i < 4; i++)
                    {
                        stream.WriteByte(0xFF);
                        stream.WriteByte(0xFF);
                        stream.WriteByte(0xFF);
                        stream.WriteByte(0xFF);
                    }
                }
                stream.WriteVInt(1);
                {
                    for (int i = 0; i < 4; i++)
                    {
                        stream.WriteByte(0xFF);
                        stream.WriteByte(0xFF);
                        stream.WriteByte(0xFF);
                        stream.WriteByte(0xFF);
                    }
                }
            }

            stream.WriteVInt(0); // PowerPlay Season Data

            stream.WriteBoolean(true);
            stream.WriteVInt(0); // LogicQuests

            stream.WriteBoolean(true);
            stream.WriteVInt(0); // Emotes

            stream.WriteVInt(2021228);
            stream.WriteVInt(100);
            stream.WriteVInt(10);
            stream.WriteVInt(30);
            stream.WriteVInt(3);
            stream.WriteVInt(80);
            stream.WriteVInt(10);
            stream.WriteVInt(40);
            stream.WriteVInt(1000);
            stream.WriteVInt(500);
            stream.WriteVInt(50);
            stream.WriteVInt(999900);

            stream.WriteVInt(0);

            Events.Encode(stream);

            stream.WriteVInt(8);
            {
                stream.WriteVInt(20);
                stream.WriteVInt(35);
                stream.WriteVInt(75);
                stream.WriteVInt(140);
                stream.WriteVInt(290);
                stream.WriteVInt(480);
                stream.WriteVInt(800);
                stream.WriteVInt(1250);
            }

            stream.WriteVInt(8);
            {
                stream.WriteVInt(1);
                stream.WriteVInt(2);
                stream.WriteVInt(3);
                stream.WriteVInt(4);
                stream.WriteVInt(5);
                stream.WriteVInt(10);
                stream.WriteVInt(15);
                stream.WriteVInt(20);
            }

            stream.WriteVInt(3);
            {
                stream.WriteVInt(1);
                stream.WriteVInt(1);
                stream.WriteVInt(1);
            }

            stream.WriteVInt(3);
            {
                stream.WriteVInt(1);
                stream.WriteVInt(1);
                stream.WriteVInt(1);
            }

            stream.WriteVInt(4);
            {
                stream.WriteVInt(20);
                stream.WriteVInt(50);
                stream.WriteVInt(140);
                stream.WriteVInt(200);
            }

            stream.WriteVInt(4);
            {
                stream.WriteVInt(150);
                stream.WriteVInt(400);
                stream.WriteVInt(1200);
                stream.WriteVInt(2600);
            }

            stream.WriteVInt(0);
            stream.WriteVInt(200);
            stream.WriteVInt(20);
            stream.WriteVInt(8640);
            stream.WriteVInt(10);
            stream.WriteVInt(5);
            stream.WriteVInt(6);
            stream.WriteVInt(50);
            stream.WriteVInt(604800);
            stream.WriteVInt(1);

            stream.WriteVInt(0); // ReleaseEntry

            stream.WriteVInt(1); // IntEntry
            {
                stream.WriteInt(1);
                stream.WriteInt(41000000 + 15); // Theme
            }

            stream.WriteVInt(0);

            stream.WriteVInt(0);

            stream.WriteLogicLong(Avatar.Identifier);

            stream.WriteVInt(0);

            stream.WriteVInt(0);

            stream.WriteVInt(0);

            stream.WriteVInt(0);

            stream.WriteVInt(0);
        }
    }
}
