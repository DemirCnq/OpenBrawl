namespace Supercell.Laser.Server.Logic.Battle
{
    using Supercell.Laser.Server.Logic.Battle.Enums;
    using Supercell.Laser.Server.Logic.Battle.Objects;
    using Supercell.Laser.Server.Logic.Battle.Tiles;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Network.Udp;
    using Supercell.Laser.Server.Protocol.Messages;
    using Supercell.Laser.Server.Protocol.Messages.Server.Battle;
    using Supercell.Laser.Titan.Files.CsvData;
    using Supercell.Laser.Titan.Logic;
    using Supercell.Laser.Titan.Logic.Math;

    internal class LogicBattleModeServer
    {
        internal int TicksGone;
        internal GameModeVariation GameMode;

        internal LogicGameObjectManagerServer GameObjectManagerServer;
        internal LogicTileMap TileMap;
        internal LogicArrayList<LogicPlayer> Players;

        internal bool UsePin = false;

        internal int PlayersAlive;

        internal int RoundState = -1;

        internal LogicPlayer GetPlayerBySessionId(long sessionId)
        {
            LogicPlayer player = null;

            foreach (LogicPlayer p in Players)
            {
                if (p.SessionId == sessionId)
                {
                    player = p;
                }
            }

            return player;
        }

        // Tick when poison starts - 500
        // Updates every 100 ticks
        internal bool IsTileOnPoisonArea(int xTile, int yTile)
        {
            if (GameMode != GameModeVariation.Showdown) return false;
            
            int tick = TicksGone;

            if (tick > 500)
            {
                int poisons = 0;
                poisons += (tick - 500) / 100;

                if (xTile <= poisons || xTile >= 59 - poisons || yTile <= poisons || yTile >= 59 - poisons)
                {
                    return true;
                }
            }
            return false;
        }

        internal void PlayerDied(LogicCharacterServer character, LogicCharacterServer killer)
        {
            if (GameMode == GameModeVariation.Showdown)
            {
                PlayersAlive--;

                foreach (LogicPlayer player in Players)
                {
                    if (player.Character == character)
                    {
                        player.IsAlive = false;
                        SendBattleEnd(player, PlayersAlive + 1);
                    }
                }

                if (PlayersAlive == 1)
                {
                    RoundState = 1;
                }
            }
        }

        internal async void SendBattleEnd(LogicPlayer player, int result)
        {
            BattleEndMessage message = new BattleEndMessage(UdpLaserSocketListener.GetSocketById(player.SessionId).TcpConnection);

            await System.Threading.Tasks.Task.Delay(1200);

            UdpLaserSocketListener.GetSocketById(player.SessionId).Alive = false;

            int exp = result == 1 ? 10 : 5;
            int trophies = LogicGamePlayUtil.GetTrophiesRewardBattleRoyale(result, -1);

            message.Result = result;
            message.PlusTokens = 200;
            message.PlusTrophies = trophies;
            message.Player = player;
            message.PlusExp = exp;

            player.Avatar.TrophiesReward += trophies > 0 ? trophies : 0;
            player.Avatar.TokensReward += 200;
            player.Avatar.Tokens += 200;
            player.Avatar.Brawlers[player.SelectedCharacter.InstanceId].Trophies = LogicMath.Max(0, player.Avatar.Brawlers[player.SelectedCharacter.InstanceId].Trophies + trophies);
            player.Avatar.Experience += exp;

            message.Send();
        }


        // I know its bad method!
        int[] xSpawns = new int[] {228, 5000, 5000, 228 };
        int[] ySpawns = new int[] {1337, 7000, 1337, 7000 };

        internal void AddGameObjects(LogicArrayList<LogicPlayer> players, string map)
        {
            Players = players;
            PlayersAlive = players.Count;

            TileMap = LogicTileMap.GenerateTileMap(map);

            for (int i = 0; i < players.Count; i++)
            {
                LogicCharacterServer chara = new LogicCharacterServer(players[i].SelectedCharacter); // 33 - homer disabler
                chara.SetPosition(xSpawns[i], ySpawns[i], 0);
                chara.ObjectIdx = i;
                GameObjectManagerServer.AddGameObject(chara);
                players[i].Character = chara;
            }

            // Gem Grab

            if (GameMode == GameModeVariation.GemGrab)
            {
                LogicItemServer mine = new LogicItemServer(new LogicData(18, 5));
                mine.SetPosition(3150, 4950, 0);
                GameObjectManagerServer.AddGameObject(mine);
            }

            if (GameMode == GameModeVariation.Showdown)
            {
                for (int i = 0; i < LogicTileMap.HEIGHT; i++)
                {
                    for (int j = 0; j < LogicTileMap.WIDTH; j++)
                    {
                        if (TileMap.Tiles[i, j].IsBox())
                        {
                            LogicCharacterServer box = new LogicCharacterServer(new LogicData(16, 51));
                            box.SetPositionByTile(j, i);
                            GameObjectManagerServer.AddGameObject(box);
                        }
                    }
                }
            }
        }

        bool Finished = false;

        internal void Tick()
        {
            TicksGone++;

            if (!Finished)
            {
                if (GameMode == GameModeVariation.Showdown)
                {
                    if (PlayersAlive <= 1)
                    {
                        foreach (LogicPlayer player in Players)
                        {
                            if (player.IsAlive == true)
                            {
                                SendBattleEnd(player, 1);
                                Finished = true;
                                return;
                            }
                        }
                    }
                }

                foreach (LogicPlayer player in Players)
                {
                    if (player.UsePin)
                    {
                        if (TicksGone >= player.PinMaxTick) player.UsePin = false;
                    }
                }
                GameObjectManagerServer.Tick();
            }
        }
    }
}
