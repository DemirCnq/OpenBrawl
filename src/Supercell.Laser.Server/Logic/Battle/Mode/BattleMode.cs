namespace Supercell.Laser.Server.Logic.Battle.Mode
{
    using Supercell.Laser.Server.DataStream;
    using Supercell.Laser.Server.Logic.Battle.Input;
    using Supercell.Laser.Server.Network.Udp;
    using Supercell.Laser.Server.Protocol.Messages.Server.Battle;
    using Supercell.Laser.Titan.Logic;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using static Supercell.Laser.Server.Logic.Battle.Input.ClientInput;

    internal class BattleMode
    {
        internal const int TickRate = 20;

        internal List<UdpLaserSocket> Sockets;
        internal Dictionary<long, LogicPlayer> Players;
        internal bool Running;

        internal LogicBattleModeServer BattleModeServer;
        internal LogicGameObjectManagerServer GameObjectManagerServer;

        internal Thread Thread;

        internal Queue<ClientInput> InputQueue;

        internal void AddInput(ClientInput input)
        {
            InputQueue.Enqueue(input);
        }

        internal BattleMode()
        {
            Sockets = new List<UdpLaserSocket>();
            Players = new Dictionary<long, LogicPlayer>();

            InputQueue = new Queue<ClientInput>();

            BattleModeServer = new LogicBattleModeServer();
            GameObjectManagerServer = new LogicGameObjectManagerServer();

            BattleModeServer.GameObjectManagerServer = GameObjectManagerServer;
            GameObjectManagerServer.BattleModeServer = BattleModeServer;
        }

        internal void InitMultiPlayer()
        {
            BattleModeServer.GameMode = Enums.GameModeVariation.Showdown;

            var players = new LogicArrayList<LogicPlayer>();
            players.AddRange(Players.Values.ToArray());

            /*while (players.Size < 4)
            {
                players.Add(new LogicPlayer(-1));
            }*/

            BattleModeServer.AddGameObjects(players, "FFFFFWWWFFFFFFFMWWWWWWWWWFFFFFFFFFFWWWWWWWWWMFFFFFFFFFTWWWWWFFFF..WWF.....FMWWWFFF................FFFWWWMF...D......WWWWFF4....T...C..FMWWFF.......M.D..M.......FFWWMF.....DD.D..4WWFF..D.....YY..FMWWF..1.D...M4..4M..D.212.FWWMFD.CYYYC.....WWF......D..CC..............FM....MF................CC...D...WW.D................D....D.FM.D..MF.D.......................TWW...D.........D..T........C..D.C........T....D...D........FWWT.....WW..D........MM.......D......MM................T..FFMM.....WWW..D........MM..D........D..MM.....MMMMMM..D....FFFFF.....WW......WWD.......D...........FF..D..M4FFFM........FFF..D..........4WW..........MMMMMM..D............FM..T.D....FF.............FWW..FFMM....MFFFFM........D....D.FM.........FF.1...D.....4FFWW..FFMM..............T..........4M.......1.FF.....D....WWWWWW............D.......WWWWT..D...MM...D.....FF..........WWWWWW........D...44......WWWW.............T....FWW..........................FMMF.....WWFF.....D...........WWWWW....MM....DD....MMM....FFFMMFFF.........M....D........WWWWWWW...MF.................MMMMMMMM.........MFF.....FM...WWWWWWWW...MF.........FFF..................D...MMMM....FM...WWWWMMMM...M.....MMM..FWWWW............................MM...MMMMFFFF.........FFM..FWWWW..............MMM................FFFFF..2.2...D.....M...WW4.....T....T.....4M..D.FFFF...........FF.YC...........M...WW......WWFFWW......M....WWWW..D..D..CY.FF.YC.......T...............WWWWWW...........WWWW........YC.FF............D..D.......T..WWWWWW..T...........T..D........FF.....MM..D...........D.......D........D............MM..D..FM.....4M.......MMM........FFFFFFFF.D......MMM.......MF.....MM..DD..N.......FFM...TWWW.FFFFFFFF.WWWT.D.MFF...D...N......MM......N....C...4M....WWW.FF.44.FF.WWW....M4...C....N....2.MM..........CY....M.DD.FWW.FF4MM4FF.WWF....M....YC........1.MM.1........CY....M....FWW.FF4MM4FF.WWF..D.M....YC........2.MM......N....C...4M....WWW.FF.44.FF.WWW....M4...C....N......MM......N.......FFM.D.TWWW.FFFFFFFF.WWWT...MFF.......N...D..MM.....FM.......MMM........FFFFFFFF........MMM.......M4.....MF..D..MM.....................D...................D..MM.....FF........D..T...........T..WWWWWW..T.......................FF.CY........WWWW...........WWWWWW....D..........T.......CY.FF.YC..D.....WWWW....M......WWFFWW......WW...M...........CY.FF......D....FFFF.DD.M4..D..T....T...D.4WW...M.....D........FFFFF................MMM..............WWWWF..MFF.........FFFFMMMM...MM............................WWWWF..MMM.....M...MMMMWWWW...MF....MMMM....D.................FFF.........FM...WWWWWWWW...MF.....FFM.........MMMMMMMM.................FM...WWWWWWW.2.2.........M.........FFFMMFFF....MMM..........MM....WWWWW...........D.....FFWW.....FMMF..........................WWF....T.D...........WWWW......44...D........WWWWWW..........FF.....D...MM...D..TWWWW..D....D............WWWWWW....D.....FF.1.......M4..........T..............MMFF..WWFF4.........1.FF....D....MF.D.............MFFFFM....MMFF..WWF.............FF....D.T..MF............DD.MMMMMM..........WW4..........D..FFF........MFFF4M..DD.FF...........D.......DWW......WW.....FFFFF.......MMMMMM.....MM..D...........MM...........WWW.....MMFF..T...D............MM......D.......MM........D..WW.....TWWF........D...D....T........C...DC........T...D........D...WWT.......................D.FM..D.MF......D..............D.D.WW...D...CC................FM....MF..............CC..D......FWW.....CYYYC.DFMWWF..1..D..M4..4M...D212.FWWMF..YY.....D..FFWW4..D.DD.....FMWWFF.......M..D.M.......FFWWMF..C...T....4FFWWWW......D...FMWWWFFF................FFFWWWMF.....FWW..FFFFWWWWWTFFFFFFFFFMWWWWWWWWWFFFFFFFFFFWWWWWWWWWMFFFFFFFWWWFFFFF");

            Running = true;

            Thread = new Thread(Update);
            Thread.Start();
        }

        internal void InitSinglePlayer()
        {
            BattleModeServer.GameMode = Enums.GameModeVariation.Showdown;

            var players = new LogicArrayList<LogicPlayer>();
            players.AddRange(Players.Values.ToArray());

            // 3 bots
            players.Add(new LogicPlayer(-1));
            players.Add(new LogicPlayer(-1));
            players.Add(new LogicPlayer(-1));

            BattleModeServer.AddGameObjects(players, "FFFFFWWWFFFFFFFMWWWWWWWWWFFFFFFFFFFWWWWWWWWWMFFFFFFFFFTWWWWWFFFF..WWF.....FMWWWFFF................FFFWWWMF...D......WWWWFF4....T...C..FMWWFF.......M.D..M.......FFWWMF.....DD.D..4WWFF..D.....YY..FMWWF..1.D...M4..4M..D.212.FWWMFD.CYYYC.....WWF......D..CC..............FM....MF................CC...D...WW.D................D....D.FM.D..MF.D.......................TWW...D.........D..T........C..D.C........T....D...D........FWWT.....WW..D........MM.......D......MM................T..FFMM.....WWW..D........MM..D........D..MM.....MMMMMM..D....FFFFF.....WW......WWD.......D...........FF..D..M4FFFM........FFF..D..........4WW..........MMMMMM..D............FM..T.D....FF.............FWW..FFMM....MFFFFM........D....D.FM.........FF.1...D.....4FFWW..FFMM..............T..........4M.......1.FF.....D....WWWWWW............D.......WWWWT..D...MM...D.....FF..........WWWWWW........D...44......WWWW.............T....FWW..........................FMMF.....WWFF.....D...........WWWWW....MM....DD....MMM....FFFMMFFF.........M....D........WWWWWWW...MF.................MMMMMMMM.........MFF.....FM...WWWWWWWW...MF.........FFF..................D...MMMM....FM...WWWWMMMM...M.....MMM..FWWWW............................MM...MMMMFFFF.........FFM..FWWWW..............MMM................FFFFF..2.2...D.....M...WW4.....T....T.....4M..D.FFFF...........FF.YC...........M...WW......WWFFWW......M....WWWW..D..D..CY.FF.YC.......T...............WWWWWW...........WWWW........YC.FF............D..D.......T..WWWWWW..T...........T..D........FF.....MM..D...........D.......D........D............MM..D..FM.....4M.......MMM........FFFFFFFF.D......MMM.......MF.....MM..DD..N.......FFM...TWWW.FFFFFFFF.WWWT.D.MFF...D...N......MM......N....C...4M....WWW.FF.44.FF.WWW....M4...C....N....2.MM..........CY....M.DD.FWW.FF4MM4FF.WWF....M....YC........1.MM.1........CY....M....FWW.FF4MM4FF.WWF..D.M....YC........2.MM......N....C...4M....WWW.FF.44.FF.WWW....M4...C....N......MM......N.......FFM.D.TWWW.FFFFFFFF.WWWT...MFF.......N...D..MM.....FM.......MMM........FFFFFFFF........MMM.......M4.....MF..D..MM.....................D...................D..MM.....FF........D..T...........T..WWWWWW..T.......................FF.CY........WWWW...........WWWWWW....D..........T.......CY.FF.YC..D.....WWWW....M......WWFFWW......WW...M...........CY.FF......D....FFFF.DD.M4..D..T....T...D.4WW...M.....D........FFFFF................MMM..............WWWWF..MFF.........FFFFMMMM...MM............................WWWWF..MMM.....M...MMMMWWWW...MF....MMMM....D.................FFF.........FM...WWWWWWWW...MF.....FFM.........MMMMMMMM.................FM...WWWWWWW.2.2.........M.........FFFMMFFF....MMM..........MM....WWWWW...........D.....FFWW.....FMMF..........................WWF....T.D...........WWWW......44...D........WWWWWW..........FF.....D...MM...D..TWWWW..D....D............WWWWWW....D.....FF.1.......M4..........T..............MMFF..WWFF4.........1.FF....D....MF.D.............MFFFFM....MMFF..WWF.............FF....D.T..MF............DD.MMMMMM..........WW4..........D..FFF........MFFF4M..DD.FF...........D.......DWW......WW.....FFFFF.......MMMMMM.....MM..D...........MM...........WWW.....MMFF..T...D............MM......D.......MM........D..WW.....TWWF........D...D....T........C...DC........T...D........D...WWT.......................D.FM..D.MF......D..............D.D.WW...D...CC................FM....MF..............CC..D......FWW.....CYYYC.DFMWWF..1..D..M4..4M...D212.FWWMF..YY.....D..FFWW4..D.DD.....FMWWFF.......M..D.M.......FFWWMF..C...T....4FFWWWW......D...FMWWWFFF................FFFWWWMF.....FWW..FFFFWWWWWTFFFFFFFFFMWWWWWWWWWFFFFFFFFFFWWWWWWWWWMFFFFFFFWWWFFFFF");

            Running = true;

            Thread = new Thread(Update);
            Thread.Start();
        }

        internal void Update()
        {
            try
            {
                while (Running)
                {
                    if (BattleModeServer.TicksGone >= 16000)
                    {
                        Running = false;
                        break;
                    }

                    while (InputQueue.Count > 0)
                    {
                        ClientInput input = InputQueue.Dequeue();
                        if (input != null)
                        {
                            ExecuteInput(input);
                        }
                    }

                    ExecuteOneTick();

                    for (int i = 0; i < Sockets.Count; i++)
                    {
                        if (Sockets[i].Alive && Sockets[i].EndPoint != null)
                        {
                            BitStream stream = new BitStream(new MemoryStream());
                            GameObjectManagerServer.Encode(stream, i);

                            VisionUpdateMessage vision = new VisionUpdateMessage();
                            vision.TicksGone = BattleModeServer.TicksGone;
                            vision.ViewersCount = BattleModeServer.TicksGone;
                            vision.VisionBitStream = stream;
                            vision.HandledInput = Players[Sockets[i].SessionId].LastInputOk;

                            UdpSingleMessagePacket packet = new UdpSingleMessagePacket();
                            packet.message = vision;
                            packet.Socket = Sockets[i];

                            Sockets[i].Send(packet);
                        }
                    }

                    Thread.Sleep(1000 / TickRate);
                }
            }
            catch (Exception) { }
        }

        private void ExecuteInput(ClientInput input)
        {
            LogicPlayer player = BattleModeServer.GetPlayerBySessionId(input.SessionId);

            if (player != null)
            {
                player.LastInputOk = input.Counter;
                if (input.Identifier == InputType.Move)
                {
                    player.Character.MoveTo(input.X, input.Y);
                }
                else if (input.Identifier == InputType.UsePin)
                {
                    if (!player.UsePin)
                    {
                        player.PinIndex = input.PinId;
                        player.UsePin = true;
                        player.PinMaxTick = BattleModeServer.TicksGone + 20;
                    }
                }
                else if (input.Identifier == InputType.Attack)
                {
                    player.Character.Attack(input.X, input.Y, input.AutoAim);
                }
            }
        }

        internal void ExecuteOneTick()
        {
            BattleModeServer.Tick();
        }
    }
}
