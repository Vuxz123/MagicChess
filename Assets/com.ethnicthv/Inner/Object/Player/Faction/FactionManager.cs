using System.Collections.Generic;

namespace com.ethnicthv.Inner.Object.Player.Faction
{
    public class FactionManager
    {
        private int _availableFactionID;
        private Dictionary<int, FactionScheme> _factions = new();
        
        public FactionManager()
        {
            
        }
        
        public void AddFaction(FactionScheme faction)
        {
            faction.ID = _availableFactionID;
            _factions.Add(_availableFactionID, faction);
            _availableFactionID++;
        }
    }
}