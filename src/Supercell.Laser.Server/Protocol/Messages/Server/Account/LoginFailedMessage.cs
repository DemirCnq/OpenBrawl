namespace Supercell.Laser.Server.Protocol.Messages.Server.Account
{
    using Supercell.Laser.Server.Network;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class LoginFailedMessage : PiranhaMessage
    {
        internal LoginFailedMessage(Connection connection) : base(connection)
        {
            Type = Enums.Message.LoginFailed;
        }

        internal ErrorCode Code;
        internal string FingerprintSHA;
        internal string RedirectHost;
        internal string ContentUrl;
        internal string UpdateUrl;
        internal string Reason;
        internal int EndMaintenanceTime;

        internal override void Encode()
        {
            Stream.WriteInt((int)Code);
            Stream.WriteString(FingerprintSHA);
            Stream.WriteString(RedirectHost);
            Stream.WriteString(ContentUrl);
            Stream.WriteString(UpdateUrl);
            Stream.WriteString(Reason);
            Stream.WriteInt(EndMaintenanceTime);
        }

        internal override void Encrypt()
        {
            
        }
    }

    public enum ErrorCode : int
    {
        CUSTOM_MESSAGE = 0,
        ACCOUNT_NOT_EXISTS = 1,
        DATA_VERSION = 7,
        CLIENT_VERSION = 8,
        REDIRECTION = 9,
        SERVER_MAINTENANCE = 10,
        BANNED = 11,
        PERSONAL_BREAK = 12,
        ACCOUNT_LOCKED = 13,
        WRONG_STORE = 15,
        VERSION_NOT_UP_TO_DATE_STORE_NOT_READY = 16,
        CHINESE_APP_STORE_CONFLICT_MESSAGE = 18,
    }
}
