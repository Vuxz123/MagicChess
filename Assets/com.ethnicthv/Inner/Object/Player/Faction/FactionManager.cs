using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Player.Faction.DefaultImpl;
using Unity.VisualScripting;

namespace com.ethnicthv.Inner.Object.Player.Faction
{
    public class FactionManager
    {
        private int _availableFactionID;
        private Dictionary<int, FactionScheme> _factions = new();
        
        public FactionManager()
        {
            AddFaction(new NormalChess());
        }
        
        public void AddFaction(FactionScheme faction)
        {
            faction.ID = _availableFactionID;
            _factions.Add(_availableFactionID, faction);
            _availableFactionID++;
        }
        
        public FactionScheme GetFaction(int factionID)
        {
            return _factions[factionID];
        }
    }
}