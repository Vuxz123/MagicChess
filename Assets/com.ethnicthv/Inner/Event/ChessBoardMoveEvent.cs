using com.ethnicthv.Other;
using com.ethnicthv.Other.Network;
using com.ethnicthv.Other.Network.P;
using com.ethnicthv.Outer;

namespace com.ethnicthv.Inner.Event
{
    [Network(eventNetworkName: "chessboard-move")]
    public class ChessBoardMoveEvent : NetworkEvent, INeedConfirm
    {
        public (int,int) From { get; private set; }
        public (int,int) To { get; private set; }
        
        public ChessBoardMoveEvent()
        {
            From = (0,0);
            To = (0,0);
        }
        
        public ChessBoardMoveEvent((int,int) from, (int,int) to)
        {
            From = from;
            To = to;
        }
        
        public override void ToPacket(PacketWriter writer)
        {
            writer.Write(From.Item1);
            writer.Write(From.Item2);
            writer.Write(To.Item1);
            writer.Write(To.Item2);
        }

        public override Other.Ev.Event FromPacket(PacketReader packet)
        {
            var from = (packet.ReadInt(), packet.ReadInt());
            var to = (packet.ReadInt(), packet.ReadInt());
            return new ChessBoardMoveEvent(from, to);
        }
        
        public override string ToString()
        {
            return $"ChessBoardMoveEvent: {From} -> {To}";
        }

        public void Rollback()
        {
            // rollback the move behaviour
            
            // swap the pieces back in the inner board
            var board = GameManagerInner.Instance.Board;
            (board[From.Item1, From.Item2], board[To.Item1, To.Item2]) =
                (board[To.Item1, To.Item2], board[From.Item1, From.Item2]);
            
            // set the outer position back
            var outerFrom = GameManagerInner.Instance.Board[From].Outer;
            outerFrom.SetPosToSquare(GameManagerOuter.ConvertInnerToOuterPos(To), false);
        }
    }
}