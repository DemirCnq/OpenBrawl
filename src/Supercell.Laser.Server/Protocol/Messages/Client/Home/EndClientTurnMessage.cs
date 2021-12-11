namespace Supercell.Laser.Server.Protocol.Messages.Client.Home
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Command.Server.Home;
    using Supercell.Laser.Server.Protocol.Messages.Server.Home;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class EndClientTurnMessage : PiranhaMessage
    {
        internal EndClientTurnMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {

        }

        internal int CommandId;

        internal override void Decode()
        {
            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
            int count = Stream.ReadVInt();
            if (count > 0)
            {
                CommandId = Stream.ReadVInt();
            }
        }

        internal override void Handle()
        {
            if (CommandId > 0)
            {
                Console.WriteLine("ECT: " + CommandId);

                if (CommandId == 535)
                {
                    if (Connection.Avatar.Tokens >= 500)
                    {
                        Connection.Avatar.Tokens -= 500;

                        Stream.ReadVInt();
                        Stream.ReadVInt();
                        Stream.ReadVInt();
                        Stream.ReadVInt();

                        int boxId = Stream.ReadVInt();
                        //Console.WriteLine(boxId);

                        LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand(Connection);
                        command.RandomizeRewards(boxId);

                        new AvailableServerCommandMessage(Connection, command).Send();
                    }
                }
                else if (CommandId == 505)
                {
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Connection.Avatar.Thumbnail.InstanceId = Stream.ReadVInt();

                    Console.WriteLine("Thumbnail changed: " + Connection.Avatar.Thumbnail.InstanceId);
                }
                else if (CommandId == 527)
                {
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Connection.Avatar.NameColor.InstanceId = Stream.ReadVInt();

                    Console.WriteLine("NameColor changed: " + Connection.Avatar.NameColor.InstanceId);
                }
            }
        }
    }
}
