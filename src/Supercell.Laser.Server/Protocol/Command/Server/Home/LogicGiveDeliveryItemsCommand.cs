namespace Supercell.Laser.Server.Protocol.Command.Server.Home
{
    using Supercell.Laser.Server.Core;
    using Supercell.Laser.Server.Files;
    using Supercell.Laser.Server.Files.Tables;
    using Supercell.Laser.Server.Logic.Gatcha;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.Logic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LogicGiveDeliveryItemsCommand : LogicCommand
    {
        internal LogicGiveDeliveryItemsCommand(Connection connection) : base(connection)
        {
            this.Type = Enums.Command.GiveDeliveryItems;
        }

        /*
         * 10 - Big Box
         */

        internal int BoxId;
        internal int Gold;
        internal LogicArrayList<GatchaDrop> Drops;

        internal void RandomizeRewards(int boxId)
        {
            Drops = new LogicArrayList<GatchaDrop>();
            BoxId = boxId;

            if (boxId == 10)
            {
                int rewards = 3;

                Gold = Loader.Random.Rand(30, 160);

                int seed = Loader.Random.Rand(0, 100);

                if (seed < 60)
                {
                    int character = GenerateRandomCharacter();

                    if (character == 1) return;

                    GatchaDrop drop = new GatchaDrop()
                    {
                        Count = 1,
                        Data = new Titan.Files.CsvData.LogicData(16, character),
                        DropId = 1
                    };

                    Drops.Add(drop);
                }
            }
        }

        internal int GenerateRandomCharacter()
        {
            int attempts = 0;
            Start:
            attempts++;
            if (attempts > 5) return -1;
            int character = Loader.Random.Next(0, Connection.Avatar.Brawlers.Count);

            if (Connection.Avatar.Brawlers[character].Unlocked || Connection.Avatar.Brawlers[character].Disabled) goto Start;

            int cardid = Connection.Avatar.Brawlers[character].CardData.InstanceId;

            LogicCardData card = LogicDataTables.Tables.Get(LogicDataTables.Files.Cards).GetDataWithInstanceId<LogicCardData>(cardid);

            string rarity = card.Rarity;

            if (card.DynamicRarityStartSeason == 1) rarity = "epic";
            if (card.DynamicRarityStartSeason == 2) rarity = "mega_epic";
            if (card.DynamicRarityStartSeason == 3) rarity = "legendary";

            if (rarity == "epic")
            {
                if (Connection.Avatar.BoxesFromLastGoodDrop > 7)
                {
                    Connection.Avatar.BoxesFromLastGoodDrop = 0;
                    return character;
                }
                else
                {
                    goto Start;
                }
            }
            else if (rarity == "mega_epic")
            {
                if (Connection.Avatar.BoxesFromLastGoodDrop > 15)
                {
                    Connection.Avatar.BoxesFromLastGoodDrop = 0;
                    return character;
                }
                else
                {
                    goto Start;
                }
            }
            else if (rarity == "legendary")
            {
                if (Connection.Avatar.BoxesFromLastGoodDrop > 80)
                {
                    Connection.Avatar.BoxesFromLastGoodDrop = 0;
                    return character;
                }
                else
                {
                    goto Start;
                }
            }

            Connection.Avatar.BoxesFromLastGoodDrop++;

            return character;
        }

        internal override void Encode()
        {
            Stream.WriteVInt(0);
            Stream.WriteVInt(1);
            Stream.WriteVInt(BoxId);

            Stream.WriteVInt(Drops.Count + 1);
            {
                Stream.WriteVInt(Gold);
                Stream.WriteVInt(0);
                Stream.WriteVInt(7);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
            }
            foreach (var drop in Drops)
            {
                drop.Encode(Stream);
            }

            for (int i = 0; i < 13; i++)
            {
                Stream.WriteVInt(0);
            }
        }

        internal override void Execute()
        {
            base.Execute();
            foreach (var drop in Drops)
            {
                drop.Apply(Connection.Avatar);
            }
        }
    }
}
