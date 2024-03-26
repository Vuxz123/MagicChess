using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Outer.Behaviour.Chess.Square;
using com.ethnicthv.Outer.Behaviour.Piece;
using com.ethnicthv.Outer.Util;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Outer.Behaviour.Chess
{
    public class ChessInitBehaviour : MonoBehaviour
    {
        //the black and white squares that contain the chess squares
        public GameObject blackSquares;
        public GameObject whiteSquares;
        
        public Material blackMaterial;
        public Material whiteMaterial;

        public new string tag = "ChessSquare";

        private UnityEngine.Camera _main;

        public readonly (GameObject, ISquare)[,] Squares = new (GameObject, ISquare)[8,8]; //8x8 array of game objects
        public readonly (GameObject, IPiece)[] Pieces = new (GameObject, IPiece)[32]; //32 array of game objects
        
        private void Start()
        {
            //get the size of the black square cell mesh
            var size = CalcSize();
            Debug.Log(size.ToString());
            //divide the blackSquares into 8x8 squares
            GenGrid(size);
            InitPieces(GameManagerInner.Instance.CreateChessBoard());
        }

        private (Vector3, Vector3) CalcSize()
        {
            var local = blackSquares.GetComponent<MeshFilter>().mesh.bounds.size;
            var lx = local.x / 8;
            var ly = local.y / 8;
            var lz = local.z;
            var origin = blackSquares.transform.TransformVector(local);
            var x = origin.x / 8;
            var y = -origin.y;
            var z = origin.z / 8;
            return (new Vector3(x, y, z), new Vector3(lx, lz, ly));
        }

        private void GenGrid((Vector3, Vector3) size)
        {
            var ori = blackSquares.transform.TransformPoint(blackSquares.GetComponent<MeshFilter>().mesh.bounds.center);

            Debug.Log(ori.ToString());
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    //create a new game object
                    var temp = new GameObject
                    {
                        transform =
                        {
                            parent = i % 2 == 0 ? j % 2 == 0 ? whiteSquares.transform : blackSquares.transform :
                                j % 2 == 0 ? blackSquares.transform : whiteSquares.transform,
                            position = (new Vector3(size.Item1.x / 2, 0, size.Item1.z / 2)) +
                                       new Vector3(ori.x + (i - 4) * size.Item1.x, ori.y,
                                           ori.z + (j - 4) * size.Item1.z)
                        }
                    };
                    //add a collider to the game object
                    var c = temp.AddComponent<BoxCollider>();
                    //set the size of the collider
                    c.size = size.Item1;
                    
                    var temp2 = temp.AddComponent<SquareBehaviour>();
                    temp2.SetPos(i, j);
                    //set the tag of the game object
                    temp.tag = tag;

                    Squares[i, j] = (temp, temp2);
                }
            }
        }

        private void InitPieces(ChessBoard board)
        {
            board.Outer = GetComponent<ChestBoardBehavior>();
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    var piece = board[GameManagerInner.ConvertOuterToInnerPos(x,y)];
                    if (piece == null) continue;
                    var square = Squares[x, y].Item2;
                    var prefab = PiecePrefabProvider.GetPiecePrefab(piece.GetPieceType());
                    if (prefab == null) continue;
                    var go = Instantiate(prefab, transform);
                    go.transform.position = square.transform.position;
                    go.GetComponentInChildren<MeshRenderer>().material = piece.GetPieceSide() == Inner.Object.Piece.Piece.Side.White ? whiteMaterial : blackMaterial;
                    var pieceBehaviour = go.AddComponent<PieceBehaviour>();
                    pieceBehaviour.SetPiece(piece);
                    piece.Outer = pieceBehaviour;
                    Pieces[piece.GetID()] = (go, pieceBehaviour);
                }
            }
        }

        public void TestMethod()
        {
            Debug.Log("Test Success!");
        }
    }
}