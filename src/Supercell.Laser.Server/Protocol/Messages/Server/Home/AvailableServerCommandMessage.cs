namespace Supercell.Laser.Server.Protocol.Messages.Server.Home
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Command;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AvailableServerCommandMessage : PiranhaMessage
    {
        internal AvailableServerCommandMessage(Connection connection, LogicCommand command) : base(connection)
        {
            Type = Enums.Message.AvailableServerCommand;
            Command = command;
        }

        internal LogicCommand Command;

        internal override void Encode()
        {
            Stream.WriteVInt((int)this.Command.Type);
            Command.Encode();

            this.Stream.Write(this.Command.Stream.ToArray());

            this.Command.Execute();
        }
    }
}
