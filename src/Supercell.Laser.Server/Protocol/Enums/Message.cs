namespace Supercell.Laser.Server.Protocol.Enums
{
    internal enum Message
    {
        ClientHello = 10100,
        Login = 10101,
        KeepAlive = 10108,
        AnalyticEvent = 10110,
        ClientInfo = 10177,
        ChangeName = 10212,
        ClientInput = 10555,

        GoHome = 14101,
        EndClientTurn = 14102,
        MatchmakeRequest = 14103,
        CancelMatchmaking = 14106,
        GoHomeFromOfflinePractise = 14109,
        GetPlayerProfile = 14113,

        AskForAllianceData = 14302,
        ChatToAllianceStream = 14315,

        GetLeaderboardMessage = 14403,

        AvatarNameCheckRequest = 14600,

        SetEncryption = 20000,
        ServerHello = 20100,
        LoginFailed = 20103,
        LoginOk = 20104,
        KeepAliveServer = 20108,

        AvatarNameCheckResponse = 20300,

        MatchMakingStatus = 20405,
        MatchMakingCancelled = 20406,

        StartLoading = 20559,

        BattleEnd = 23456,

        OwnHomeData = 24101,
        VisionUpdate = 24109,
        AvailableServerCommand = 24111,
        PlayerProfile = 24113,

        UdpConnectionInfo = 24112,

        AllianceData = 24301,
        AllianceStreamEntry = 24312,
        MyAlliance = 24399,

        LeaderboardMessage = 24403
    }
}
