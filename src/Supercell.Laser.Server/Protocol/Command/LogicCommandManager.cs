namespace Supercell.Laser.Server.Protocol.Command
{
    using Supercell.Laser.Server.Logic;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.Logic.Enums;
    using Supercell.Laser.Server.Protocol.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Supercell.Laser.Titan.DataStream;

    internal class LogicCommandManager
    {
        /// <summary>
        /// Creates a <see cref="LogicCommand"/> using the specified type.
        /// </summary>
        internal static LogicCommand CreateCommand(int type, Connection connection, ByteStream stream)
        {
            switch ((Command)type)
            {
                default:
                {
                    Debugger.Warning($"Command {type} does not exist.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="LogicCommand"/> is allowed in the current state.
        /// </summary>
        internal static bool IsCommandAllowedInCurrentState(LogicCommand command)
        {
            Connection connection = command.Connection;
            int type = (int)command.Type;

            if (connection.IsConnected)
            {
                if (LogicVersion.IsProd || connection.Avatar.Rank != Rank.Administrator)
                {
                    if (type >= 1000)
                    {
                        Debugger.Error("Execute command failed! Debug commands are not allowed when debug is off.");
                        return false;
                    }
                }

                if (type >= 600 && type < 700)
                {
                    if (connection.State != State.Battle)
                    {
                        Debugger.Error($"Execute command failed! Command {type} is only allowed in battle state. Avatar {connection.Avatar}");
                        return false;
                    }
                }
                if (type >= 500 && type <= 598)
                {
                    if (connection.State != State.Home)
                    {
                        Debugger.Error($"Execute command failed! Command {type} is only allowed in home state. Avatar {connection.Avatar}");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
