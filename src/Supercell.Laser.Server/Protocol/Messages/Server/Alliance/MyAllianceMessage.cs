namespace Supercell.Laser.Server.Protocol.Messages.Server.Alliance
{
    using Supercell.Laser.Server.Network;

    internal class MyAllianceMessage : PiranhaMessage
    {
        internal MyAllianceMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.MyAlliance;
        }

        internal override void Encode()
        {
            Stream.WriteVInt(1);

            Stream.WriteVInt(1);

            Stream.WriteVInt(25);
            Stream.WriteVInt(0);

            Stream.WriteLong(1337);
            Stream.WriteString("PW Brawl");

            Stream.WriteVInt(8);
            Stream.WriteVInt(0);

            Stream.WriteVInt(0);
            Stream.WriteVInt(1);

            Stream.WriteVInt(7777);
            Stream.WriteVInt(1337);

            Stream.WriteVInt(0);
            Stream.WriteString("RU");

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
        }
    }
}
