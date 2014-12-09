using System.Collections.Generic;
using System;
using Chess.Game;
using System.Diagnostics;
using System.Linq;


namespace Chess.Model
{
    public enum PlayerColor
    {
        White,
        Black
    }


    public delegate void MakeMoveCallback(Square s);
    public delegate void SelectCallback(Square s);

    abstract class Player
    {

        protected MakeMoveCallback _makeMoveCb;
        protected SelectCallback _selectCb ;
        protected GameBoard _board;
        protected PlayerColor _color;

        public Player(PlayerColor c, GameBoard board, MakeMoveCallback ecb, SelectCallback scb)
        {
            _color = c;
            _board = board;
            _makeMoveCb = ecb;
            _selectCb = scb;
        }

        public abstract void StartTurn();

        public virtual void SquarePressed(Square s)
        {
        }
    }

    class HumanPlayer : Player
    {
        private bool _isMyTurn;
        private Square _selectedSquare;


        public HumanPlayer(PlayerColor c, GameBoard board, MakeMoveCallback ecb, SelectCallback scb)
            : base(c, board, ecb, scb)
        {
            _isMyTurn = false;
        }

        public override void StartTurn()
        {
            _isMyTurn = true;
        }

        public override void SquarePressed(Square s)
        {
            if (!_isMyTurn)
                return;

            Piece piece = s.Piece;

            if (piece != null && piece.Color == _color)
            {
                _selectedSquare = s;
                _selectCb(s);

            }
            else if (s.Background != s.OriginalBackground) // if one of the moves was pressed
            {
                _isMyTurn = false;
                _makeMoveCb(s);
                _selectedSquare = null;
                
            }
        }
    }

    class AIPlayer : Player
    {
        private AIStrategy _strgy;

        public AIPlayer(PlayerColor c, GameBoard board, MakeMoveCallback cb, SelectCallback scb, AIStrategy s)
            : base(c, board, cb, scb)
        {
            _strgy = s;
        }

        public override void StartTurn()
        {

            System.Threading.Thread.Sleep(100);

            List<Piece> pieces = _color == PlayerColor.Black ?_board.BlackPieces : _board.WhitePieces;

            List<Piece> piecesCopy = new List<Piece>();

            foreach (var piece in pieces)
            {
                piecesCopy.Add(piece.Clone());
            }
            
            Tuple<Move, Piece> ret = _strgy.ChooseMove(piecesCopy, _board);

            Piece p = ret.Item2;
            Move m = ret.Item1;

            _selectCb(_board[p.Position.X, p.Position.Y]);
            _makeMoveCb(_board[m.Position.X, m.Position.Y]);

        }

    }

    public abstract class AIStrategy
    {

        public abstract Tuple<Move, Piece> ChooseMove(List<Piece> pieces, GameBoard board);
    }

    public class RandomStrategy : AIStrategy
    {
        public override Tuple<Move, Piece> ChooseMove(List<Piece> pieces, GameBoard board)
        {
            Random rand = new Random();
           

            while (pieces.Count > 0)
            {
                Piece randPiece = pieces[rand.Next(0, pieces.Count - 1)];
                List<Move> moves = RuleEngine.GetAvailableMoves(randPiece, board);

                if (moves.Count == 0)
                {
                    pieces.Remove(randPiece);
                    continue;
                }


                Move randMove = moves[rand.Next(0, moves.Count - 1)];
                return new Tuple<Move, Piece>(randMove, randPiece);
            }

            Debug.Assert(false);
            return new Tuple<Move, Piece>(null, null);
        }
    }

    public class Offensive : AIStrategy
    {
        public override Tuple<Move, Piece> ChooseMove(List<Piece> pieces, GameBoard board)
        {
            List<Tuple<Move, Piece, Piece>> attacks = new List<Tuple<Move, Piece, Piece>>();
                      
            foreach (var piece in pieces)
            {
                List<Move> moves = RuleEngine.GetAvailableMoves(piece, board);
                moves = moves.FindAll(x => x.Type == MoveType.Attack);

                foreach (var move in moves)
                {
                    Piece attackedPiece = board[move.Position.X, move.Position.Y].Piece;
                    attacks.Add(new Tuple<Move, Piece, Piece>(move, attackedPiece, piece));
                }
            }

            if(attacks.Count == 0)
            {
                RandomStrategy random = new RandomStrategy();
                return random.ChooseMove(pieces, board);
            }

            attacks.OrderBy(x => x.Item2.Value);
            Move attack = attacks.Last().Item1;
            Piece selectedPiece = attacks.Last().Item3;

            return new Tuple<Move, Piece>(attack, selectedPiece);
        }
    }
}