using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Chess.Model;

namespace Chess.Game
{
    public class GameEngine
    {
        //public Square[][] Board { get; private set; }

        private readonly XmlExport _xmlExport;
        private const String XmlFilename = "Game.xml";

        private readonly GameBoard _board;
        private readonly RuleEngine _ruleEngine;
        private List<Move> _moves;
        private Square _selectedSquare;

        private FileSystemWatcher _fileSystemWatcher;

        public GameEngine()
        {
            Turn = Player.White;
            _board = new GameBoard();
            _ruleEngine = new RuleEngine(Board);

            _xmlExport = new XmlExport(this);

            if (File.Exists(XmlFilename))
            {
                _xmlExport.UpdateBoard(XmlFilename, this); // load last session
            }
            else
            {
                _xmlExport.Export(XmlFilename); // create a new xml document
            }

            fileSystemWatcherinit(); // watch for changes in xml file*/
        }

        public void NewGame()
        {
            Turn = Player.White;
            _board.BuildStartPieces();
            _board.ResetSquares();
            _xmlExport.Export(XmlFilename);
        }

        private void fileSystemWatcherinit()
        {
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Path = Directory.GetCurrentDirectory();
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileSystemWatcher.Filter = XmlFilename;
            _fileSystemWatcher.Changed += fileChanged;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void fileChanged(object sender, FileSystemEventArgs e)
        {
            _xmlExport.UpdateBoard(XmlFilename, this);  // load new file
        }

        public GameBoard Board
        {
            get { return _board; }
        }

        public Player Turn { get; set; }


        public void HandleInput(Square square)
        {
            Piece piece = square.Piece;

            if (piece != null && piece.Color == Turn) // a piece of correct turn was pressed
            {
                _selectedSquare = square;

                if (_moves != null) // if there already are moves, reset them
                {
                    resetBackgrounds(_moves);
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

               
                swapTurn();
                resetBackgrounds(_moves);

                _fileSystemWatcher.EnableRaisingEvents = false;
                _xmlExport.Export(XmlFilename);
                _fileSystemWatcher.EnableRaisingEvents = true;

                _selectedSquare = null;
                _moves = null;
            }
        }


        private void swapTurn()
        {
            Turn = Turn == Player.White ? Player.Black : Player.White;
        }

        private void resetBackgrounds(IEnumerable<Move> moves)
        {
            foreach (var move in moves)
            {
                Board[move.Position.X, move.Position.Y].ResetBackground();
            }
        }
    }
}
