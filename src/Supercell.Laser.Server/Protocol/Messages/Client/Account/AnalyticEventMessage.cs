namespace Supercell.Laser.Server.Protocol.Messages.Client.Account
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.DataStream;
    using System;

    internal class AnalyticEventMessage : PiranhaMessage
    {
        internal AnalyticEventMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
        }

        internal string Event;
        internal string Info;

        internal override void Decode()
        {
            Event = Stream.ReadString();
            Info = Stream.ReadString();
        }

        internal override void Handle()
        {
            //Console.WriteLine($"Event: {Event} Info: {Info}");
        }
    }
}
