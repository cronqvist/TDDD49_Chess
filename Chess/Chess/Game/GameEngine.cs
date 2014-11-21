using System.Collections.Generic;
using System.Windows;
using Chess.Model;

namespace Chess.Game
{
    public class GameEngine
    {
        //public Square[][] Board { get; private set; }


        private readonly GameBoard _board;
        private readonly RuleEngine _ruleEngine;
        private List<Move> _moves;
        private Square _selectedSquare;

        public GameEngine()
        {
            Turn = Player.White;
            _board = new GameBoard();
            _ruleEngine = new RuleEngine(Board);
        }

        public GameBoard Board
        {
            get { return _board; }
        }

        public Player Turn { get; private set; }


        public void HandleInput(Square square)
        {
            Piece piece = square.Piece;

            if (piece != null && piece.Color == Turn) // a piece of correct turn was pressed
            {
                _selectedSquare = square;

                if (_moves != null) // if there already are moves, reset them
                {
                    ResetBackgrounds(_moves);
                }

                _moves = _ruleEngine.GetAvailableMoves(piece); // gets the new moves for the current piece
                foreach (Move move in _moves)
                {
                    SquareBackground bg = move.Type == MoveType.Move ? SquareBackground.Move : SquareBackground.Attacked;
                    Board.SetBackgroundAt(move.Position.X, move.Position.Y, bg);
                }
            }
            else if (square.Background != square.OriginalBackground) // if one of the moves was pressed
            {
                // valid square was pressed for move
                foreach (Move move in _moves)
                {
                    if (Board[move.Position.X, move.Position.Y] == square) // find the move
                    {
                        _selectedSquare.Piece.Position = new PiecePosition(move.Position.X, move.Position.Y);
                        Board[move.Position.X, move.Position.Y].Piece = _selectedSquare.Piece;
                            //TODO: Play som fancy animation
                        _selectedSquare.Piece = null;

                        Player p = Turn == Player.White ? Player.Black : Player.White;
                        if (_ruleEngine.IsCheck(p))
                        {
                            MessageBox.Show("lol check!", "lol");
                        }

                        break;
                    }
                }

                SwapTurn();
                ResetBackgrounds(_moves);
                _selectedSquare = null;
                _moves = null;
            }
        }


        private void SwapTurn()
        {
            Turn = Turn == Player.White ? Player.Black : Player.White;
        }

        private void ResetBackgrounds(IEnumerable<Move> moves)
        {
            foreach (var move in moves)
            {
                Board[move.Position.X, move.Position.Y].ResetBackground();
            }
        }
    }
}