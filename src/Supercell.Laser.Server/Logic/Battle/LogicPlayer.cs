namespace Supercell.Laser.Server.Logic.Battle
{
    using Supercell.Laser.Server.Logic.Avatar;
    using Supercell.Laser.Server.Logic.Battle.Objects;
    using Supercell.Laser.Titan.Files.CsvData;

    internal class LogicPlayer
    {
        internal LogicData SelectedCharacter;
        internal int XpPoints;

        internal LogicCharacterServer Character;
        internal int UltiCharge;

        internal bool UsePin;
        internal int PinIndex;
        internal int PinMaxTick;

        internal long SessionId;

        internal int LastInputOk;

        internal LogicClientAvatar Avatar;
        internal bool IsAlive = true;

        internal LogicPlayer(long sessionid)
        {
            SessionId = sessionid;
            SelectedCharacter = new LogicData(16, 0); // avoid NRE for bots
        }
    }
}
