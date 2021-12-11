namespace Supercell.Laser.Server.Protocol.Messages
{
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Server.Protocol.Enums;
    using Supercell.Laser.Server.Protocol.Messages.Client.Account;
    using Supercell.Laser.Server.Protocol.Messages.Client.Alliance;
    using Supercell.Laser.Server.Protocol.Messages.Client.Avatar;
    using Supercell.Laser.Server.Protocol.Messages.Client.Battle;
    using Supercell.Laser.Server.Protocol.Messages.Client.Home;
    using Supercell.Laser.Server.Protocol.Messages.Client.Security;
    using Supercell.Laser.Titan.DataStream;

    internal static class LogicLaserMessageFactory
    {
        internal const string RC4Key = "ggfdhjgcoletteigurh978rhu93t578b9fd89r";

        /// <summary>
        /// Creates a <see cref="PiranhaMessage"/> using the specified identifier.
        /// </summary>
        internal static PiranhaMessage CreateMessageByType(int id, Connection connection, ByteStream stream)
        {
            switch ((Message)id)
            {
                case Message.ClientHello:
                    return new ClientHelloMessage(connection, stream);
                case Message.Login:
                    return new LoginMessage(connection, stream);
                case Message.KeepAlive:
                    return new KeepAliveMessage(connection, stream);
                case Message.AnalyticEvent:
                    return new AnalyticEventMessage(connection, stream);
                case Message.ClientInfo:
                    return new ClientInfoMessage(connection, stream);
                case Message.ChangeName:
                    return new ChangeNameMessage(connection, stream);
                case Message.ClientInput:
                    return new ClientInputMessage(connection, stream);
                case Message.GoHome:
                    return new GoHomeMessage(connection, stream);
                case Message.EndClientTurn:
                    return new EndClientTurnMessage(connection, stream);
                case Message.MatchmakeRequest:
                    return new MatchmakeRequestMessage(connection, stream);
                case Message.CancelMatchmaking:
                    return new CancelMatchmakingMessage(connection, stream);
                case Message.GoHomeFromOfflinePractise:
                    return new GoHomeFromOfflinePractiseMessage(connection, stream);
                case Message.GetPlayerProfile:
                    return new GetPlayerProfileMessage(connection, stream);
                case Message.AskForAllianceData:
                    return new AskForAllianceDataMessage(connection, stream);
                case Message.ChatToAllianceStream:
                    return new ChatToAllianceStreamMessage(connection, stream);
                case Message.GetLeaderboardMessage:
                    return new GetLeaderboardMessage(connection, stream);
                case Message.AvatarNameCheckRequest:
                    return new AvatarNameCheckRequestMessage(connection, stream);
                default:
                {
                    Debugger.Warning($"Failed to handle a message with an ID of {id}!");
                    return null;
                }
            }
        }

        /// <summary>
        /// Sends the specified <see cref="PiranhaMessage"/>.
        /// </summary>
        internal static void Send(this PiranhaMessage message) => message.Connection.Messaging.Send(message);
    }
}
