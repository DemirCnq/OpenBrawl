namespace Supercell.Laser.Server.Protocol.Messages.Server.Battle
{
    using Supercell.Laser.Server.Logic.Battle;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Files.CsvData;
    using System.Collections.Generic;

    internal class StartLoadingMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Battle;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartLoadingMessage"/> class.
        /// </summary>
        public StartLoadingMessage(Connection connection) : base(connection)
        {
            this.Type = Message.StartLoading;
        }

        internal LogicData Location;

        internal List<LogicPlayer> Players;

        internal override void Encode()
        {
            Stream.WriteInt(Players.Count);
            Stream.WriteInt(0);
            Stream.WriteInt(0); // Side
            Stream.WriteInt(Players.Count);

            int index = 0;
            foreach (LogicPlayer player in Players)
            {
                Stream.WriteLogicLong(player.Avatar.Identifier);
                Stream.WriteVInt(index);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteInt(0);

                Stream.WriteDataReference(player.SelectedCharacter);

                Stream.WriteVInt(0);

                Stream.WriteBoolean(false);

                {
                    Stream.WriteString(player.Avatar.Name);

                    Stream.WriteVInt(100);
                    Stream.WriteVInt(28000000);
                    Stream.WriteVInt(43000000);
                    Stream.WriteVInt(-1);
                }

                Stream.WriteBoolean(false);

                index += 1;
            }

            Stream.WriteInt(0);
            {
            }
            Stream.WriteInt(0);
            {
            }

            Stream.WriteInt(0);

            Stream.WriteVInt(0);
            Stream.WriteVInt(1); // DrawMap
            Stream.WriteVInt(1);

            Stream.WriteBoolean(true);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteDataReference(Location);

            Stream.WriteBoolean(false);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteVInt(0);
        }
    }
}
