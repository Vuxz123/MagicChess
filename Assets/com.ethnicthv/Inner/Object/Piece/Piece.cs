using com.ethnicthv.Inner.Object.Piece.Action;
using com.ethnicthv.Inner.Object.Piece.Properties;
using com.ethnicthv.Outer.Behaviour.Piece;
using UnityEngine.Rendering.Universal.Internal;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Inner.Object.Piece
{
    public class Piece: PieceEntity
    {
        private static int _idCounter;
        
        private int _id;
        
        private readonly Type _type;
        private readonly Side _side;

        public Side side => _side;

        private bool _isMovable = true;
        private bool _isDefendable = true;
        private bool _isAttackable = true;
        private bool _isDead = false;
        
        private PieceProperties _properties;

        public Piece(Type type, Side side)
        {
            _type = type;
            _side = side;
            _id = _idCounter;
            _idCounter++;
            _properties = PieceProperties.Provider.GetProperties(type);
        }
        
        public IPiece Outer { get; protected internal set; }
        
        public PieceProperties PieceProperties => _properties;

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
        
        public void DealDamage(DamageSource damage)
        {
            _properties.Health.Damage(this, damage);
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

        public override string ToString()
        {
            return $"{nameof(_id)}: {_id}, {nameof(_type)}: {_type}, {nameof(_side)}: {_side}, {nameof(_isMovable)}: {_isMovable}, {nameof(_isDefendable)}: {_isDefendable}, {nameof(_isAttackable)}: {_isAttackable}, {nameof(_isDead)}: {_isDead}";
        }
        
        public string ToName()
        {
            return $"{_side} {_type}";
        }
    }
    
    public class  ActionType
    {
        /// <summary>
        /// Action type for move, accepts 3 parameters, Piece and (int, int) location
        /// </summary>
        public static readonly ActionType Move = new ActionType("Move", 2);
        public static readonly ActionType Attack = new ActionType("Attack", 2);
        public static readonly ActionType Defend = new ActionType("Defend", 2);
        public static readonly ActionType Dead = new ActionType("Dead", 2);

        /// number of parameters
        public readonly byte Np;

        public readonly string Name;
        
        public ActionType(string name, int np)
        {
            Np = (byte)np;
            Name = name;
        }
        
        public override string ToString()
        {
            return Name;
        }
            
    }
}