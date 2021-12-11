namespace Supercell.Laser.Server.Protocol.Messages.Server.Home
{
    using Supercell.Laser.Server.Logic.Avatar;
    using Supercell.Laser.Server.Logic.Slots;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.DataStream;
    using System.Collections.Generic;

    internal class LeaderboardMessage : PiranhaMessage
    {
        internal LeaderboardMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.LeaderboardMessage;
            AvatarList = Avatars.OrderByDescending();
        }

        internal List<LogicClientAvatar> AvatarList;

        internal override void Encode()
        {
            int playerIndex = 0;

            Stream.WriteBoolean(true); // кто
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteString(null);

            Stream.WriteVInt(AvatarList.Count);
            foreach (var avatar in AvatarList)
            {
                if (avatar.LowID == Connection.Avatar.LowID)
                {
                    playerIndex = AvatarList.IndexOf(avatar) + 1;
                }

                Stream.EncodeLogicLong(avatar.Identifier);

                Stream.WriteVInt(1);
                Stream.WriteVInt(avatar.Score);

                Stream.WriteVInt(1);

                Stream.WriteString(null); // Club name

                Stream.WriteString(avatar.Name ?? "NoName");
                Stream.WriteVInt(avatar.Experience);
                Stream.WriteVInt(avatar.Thumbnail.GlobalId);
                Stream.WriteVInt(avatar.NameColor.GlobalId);
                Stream.WriteVInt(46000000 + avatar.NameColor.InstanceId);
                Stream.WriteVInt(0);
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(playerIndex);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteString("RU");
        }
    }
}
