namespace Supercell.Laser.Server.Protocol.Messages.Server.Avatar
{
    using Supercell.Laser.Server.Logic.Avatar;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class PlayerProfileMessage : PiranhaMessage
    {
        internal PlayerProfileMessage(Connection connection, int highid, int lowId) : base(connection)
        {
            Avatar = Avatars.Get(new LogicLong(highid, lowId));
            Type = Enums.Message.PlayerProfile;
        }

        internal LogicClientAvatar Avatar;

        internal override void Encode()
        {
            Stream.EncodeLogicLong(Avatar.Identifier);
            Stream.WriteVInt(0);

            Stream.WriteVInt(Avatar.Brawlers.GetUnlockedBrawlersCount());

            foreach (var brawler in Avatar.Brawlers)
            {
                if (brawler.Unlocked)
                {
                    Stream.WriteDataReference(brawler.CharData);
                    Stream.WriteVInt(0);
                    Stream.WriteVInt(brawler.Trophies);
                    Stream.WriteVInt(brawler.Trophies);
                    Stream.WriteVInt(1);
                }
            }

            Stream.WriteVInt(14); // Player Stats

            {
                Stream.WriteVInt(1);
                Stream.WriteVInt(0); // 3v3 wins

                Stream.WriteVInt(2);
                Stream.WriteVInt(Avatar.Experience);

                Stream.WriteVInt(3);
                Stream.WriteVInt(Avatar.Score);

                Stream.WriteVInt(4);
                Stream.WriteVInt(Avatar.Score);

                Stream.WriteVInt(5);
                Stream.WriteVInt(Avatar.Brawlers.GetUnlockedBrawlersCount());

                Stream.WriteVInt(7);
                Stream.WriteVInt(28000000 + Avatar.Thumbnail.InstanceId); // thumbnail

                Stream.WriteVInt(8);
                Stream.WriteVInt(0); // solo wins

                Stream.WriteVInt(9);
                Stream.WriteVInt(0);

                Stream.WriteVInt(10);
                Stream.WriteVInt(0);

                Stream.WriteVInt(11);
                Stream.WriteVInt(0);

                Stream.WriteVInt(12);
                Stream.WriteVInt(0);

                Stream.WriteVInt(13);
                Stream.WriteVInt(0);

                Stream.WriteVInt(14);
                Stream.WriteVInt(0);

                Stream.WriteVInt(15);
                Stream.WriteVInt(0);
            }

            Stream.WriteString(Avatar.Name);
            Stream.WriteVInt(Avatar.Experience);
            Stream.WriteVInt(28000000 + Avatar.Thumbnail.InstanceId);
            Stream.WriteVInt(43000000 + Avatar.NameColor.InstanceId);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false); // Is in club

            Stream.WriteVInt(0);
        }
    }
}
