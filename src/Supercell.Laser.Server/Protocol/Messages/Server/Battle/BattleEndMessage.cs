namespace Supercell.Laser.Server.Protocol.Messages.Server.Battle
{
    using Supercell.Laser.Server.Logic.Battle;
    using Supercell.Laser.Server.Network;
    using Supercell.Laser.Titan.DataStream;

    internal class BattleEndMessage : PiranhaMessage
    {
        internal BattleEndMessage(Connection connection) : base(connection)
        {
            this.Type = Enums.Message.BattleEnd;
        }

        internal int Result;
        internal int PlusTrophies;
        internal int PlusTokens;
        internal int PlusExp;
        internal LogicPlayer Player;

        internal override void Encode()
        {
            Stream.WriteVInt(2);
            Stream.WriteVInt(Result);
            Stream.WriteVInt(PlusTokens);
            Stream.WriteVInt(PlusTrophies);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0); // coin shower
            Stream.WriteVInt(0);

            Stream.WriteByte(16); // Result Info

            Stream.WriteVInt(-1); // Championship challenge type
            Stream.WriteVInt(1); // unk

            Stream.WriteVInt(1);
            {
                Stream.WriteVInt(1);
                Stream.WriteDataReference(Player.SelectedCharacter);

                Stream.WriteVInt(0); // Skin DataRef

                Stream.WriteVInt(0); // brawler trophies
                Stream.WriteVInt(0); // power play related
                Stream.WriteVInt(1); // power level brawler

                Stream.WriteBoolean(true);
                Stream.WriteInt(Player.Avatar.HighID);
                Stream.WriteInt(Player.Avatar.LowID);

                Stream.WriteString(Player.Avatar.Name);
                Stream.WriteVInt(100); // exp
                Stream.WriteVInt(28000000);
                Stream.WriteVInt(43000000);
                Stream.WriteVInt(-1); // unk
            }

            Stream.WriteVInt(2); // exp array
            {
                Stream.WriteVInt(0);
                Stream.WriteVInt(PlusExp);
                Stream.WriteVInt(8);
                Stream.WriteVInt(0);
            }

            Stream.WriteVInt(0); // bonuska array

            Stream.WriteVInt(2); // bars array
            {
                Stream.WriteVInt(1);
                Stream.WriteVInt(this.Connection.Avatar.Brawlers[Player.SelectedCharacter.InstanceId].Trophies - PlusTrophies);
                Stream.WriteVInt(this.Connection.Avatar.Brawlers[Player.SelectedCharacter.InstanceId].Trophies - PlusTrophies);

                Stream.WriteVInt(5);
                Stream.WriteVInt(this.Connection.Avatar.Experience - PlusExp);
                Stream.WriteVInt(this.Connection.Avatar.Experience - PlusExp);
            }

            Stream.WriteVInt(28);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false); // Play Again

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
        }
    }
}
