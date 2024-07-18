namespace com.ethnicthv.Inner.Object.Player
{
    public class PlayerManager
    {
        private Player _player;
        private Player _opponent;
        
        public IPlayer Player => _player;
        public IPlayer Opponent => _opponent;

        public PlayerManager()
        {
            _player = LoadPlayer();
        }
        
        public void SetOpponent(Player opponent)
        {
            _opponent = opponent;
        }
        
        private static Player LoadPlayer()
        {
            return new Player();
        }
        
        public void SetPlayerFaction(int factionID)
        {
            _player.SetFaction(factionID);
        }
        
        public void SetOpponentFaction(int factionID)
        {
            _opponent.SetFaction(factionID);
        }
        
        public void DummyOpponent()
        {
            _opponent = new Player();
        }
    }
}