using com.ethnicthv.Inner.Object.Piece.Action;
using com.ethnicthv.Outer.Piece;

namespace com.ethnicthv.Inner.Object.Piece
{
    public class Piece: PieceEntity
    {
        private static int _idCounter;
        
        private int _id;
        
        private readonly Type _type;
        private readonly Side _side;

        private bool _isMovable = true;
        private bool _isDefendable = true;
        private bool _isAttackable = true;
        private bool _isDead = false;

        public Piece(Type type, Side side)
        {
            _type = type;
            _side = side;
            _id = _idCounter;
            _idCounter++;
        }
        
        public IPiece Outer { get; protected internal set; }

        public void SetMovable(bool movable)
        {
            _isMovable = movable;
        }

        public void SetAttackable(bool attackable)
        {
            _isAttackable = attackable;
        }

        public void SetDefendable(bool defendable)
        {
            _isDefendable = defendable;
        }

        public void SetDead(bool dead)
        {
            _isDead = dead;
        }
        
        public bool IsMovable()
        {
            return _isMovable;
        }

        public bool IsAttackable()
        {
            return _isAttackable;
        }

        public bool IsDefendable()
        {
            return _isDefendable;
        }

        public bool IsDead()
        {
            return _isDead;
        }
        
        public int GetID()
        {
            return _id;
        }

        public void DoAction(ActionType type, params object[] data)
        {
            PieceAction.GetPieceAction(_type).DoAction(type, this, data);
        }

        public Type GetPieceType()
        {
            return _type;
        }
        
        public Side GetPieceSide()
        {
            return _side;
        }
        
        public enum Side
        {
            Black = 1,White = -1
        }
        
        public enum Type
        {
            King = 1,Queen = 2,Rook = 3,Bishop = 4,Knight = 5,Pawn = 6
        }
    }
    
    public enum  ActionType
    {
        Move = 2,
        Attack = 1,
        Defend = 1,
        Dead = 1
    }
}