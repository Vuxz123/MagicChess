namespace com.ethnicthv.Inner.Object.Player
{
    public class Player: IPrivatePlayerFactory, IPlayer
    {
        private int _factionID;
        
        public int FactionID => _factionID;
        
        public void SetFaction(int factionID)
        {
            _factionID = factionID;
        }
    }

    public interface IPlayer
    {
        public int FactionID { get; }
    }

    public interface IPrivatePlayerFactory
    {
        public void SetFaction(int factionID);
    }
}