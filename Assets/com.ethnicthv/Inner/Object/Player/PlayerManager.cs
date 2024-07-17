namespace com.ethnicthv.Inner.Object.Player
{
    public class PlayerManager
    {
        private Player _player;
        private Player _opponent;

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
    }
}