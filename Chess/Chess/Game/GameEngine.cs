using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Chess.Model;
using System.Diagnostics;

namespace Chess.Game
{

    public class InvalidMoveTypeException : Exception
    {
        public InvalidMoveTypeException()
        {
        }

        public InvalidMoveTypeException(string message)
            : base(message)
        {
        }

        public InvalidMoveTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class GameEngine
    {
        //public Square[][] Board { get; private set; }

        private readonly XmlExport _xmlExport;
        private const String XmlFilename = "Game.xml";

        private readonly GameBoard _board;
        private List<Move> _moves;
        private Square _selectedSquare;

        private Queue<Player> _players;
        private Player currentPlayer;

        private FileSystemWatcher _fileSystemWatcher;

        public GameEngine()
        {
            Turn = PlayerColor.White;
            _board = new GameBoard();
            _xmlExport = new XmlExport(this);
            _players = new Queue<Player>();

            _players.Enqueue(new HumanPlayer(PlayerColor.White, _board, this.MoveTo, this.HumanSelectSquare));
            _players.Enqueue(new AIPlayer(PlayerColor.Black, _board, this.MoveTo, this.AISelectSquare, new RandomStrategy()));

            currentPlayer = _players.Dequeue();

            if (File.Exists(XmlFilename))
            {
                _xmlExport.UpdateBoard(XmlFilename, this); // load last session
            }
            else
            {
                _xmlExport.Export(XmlFilename); // create a new xml document
            }

            fileSystemWatcherinit(); // watch for changes in xml file
            currentPlayer.StartTurn();
        }

        public void NewGame()
        {
            Turn = PlayerColor.White;

            _players = new Queue<Player>();
            _players.Enqueue(new HumanPlayer(PlayerColor.White, _board, this.MoveTo, this.HumanSelectSquare));
            _players.Enqueue(new AIPlayer(PlayerColor.Black, _board, this.MoveTo, this.AISelectSquare, new RandomStrategy()));

            currentPlayer = _players.Dequeue();
            currentPlayer.StartTurn();

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

        public PlayerColor Turn { get; set; }

        public void HumanSelectSquare(Square square)
        {
            _selectedSquare = square;
            Piece piece = square.Piece;

            if (_moves != null) // if there already are moves, reset them
            {
                resetBackgrounds(_moves);
            }

            _moves = RuleEngine.GetAvailableMoves(piece, _board); // gets the new moves for the current piece
            foreach (Move move in _moves)
            {
                SquareBackground bg = GetBackground(move.Type);
                Board.SetBackgroundAt(move.Position.X, move.Position.Y, bg);
            }
        }

        private SquareBackground GetBackground(MoveType mt)
        {
            switch (mt)
            {
                case MoveType.PromoteQueen :
                    return SquareBackground.Promote;
                case MoveType.Attack :
                    return SquareBackground.Attacked;
                case MoveType.Move :
                    return SquareBackground.Move;
                default :
                    throw new InvalidMoveTypeException("lol fail");
            }
        }

        public void AISelectSquare(Square square)
        {
            _selectedSquare = square;
            Piece piece = square.Piece;

            if (_moves != null) // if there already are moves, reset them
            {
                resetBackgrounds(_moves);
            }

            //this is bad the moves has already been computed by the AIplayer
            //TODO: FIX
            _moves = RuleEngine.GetAvailableMoves(piece, _board); // gets the new moves for the current piece
        }



        public void MoveTo(Square square)
        {

            Move mMove = null;

            foreach (Move move in _moves)
            {
                if (Board[move.Position.X, move.Position.Y] == square) // find the move
                {
                    mMove = move;
                }
            }

            if (mMove == null)
                Debug.Assert(false);

            _board.MovePiece(_selectedSquare.Piece, mMove );

            PlayerColor p = Turn == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
            CheckState cState = RuleEngine.GetCheckState(p, _board);

            switch (cState)
            {
                case CheckState.MATE:
                    MessageBox.Show("lol checkmate!", "lol");
                    _board.ClearBoard();
                    NewGame();
                    break;
                case CheckState.CHECK:
                    MessageBox.Show("lol check!", "lol");
                    goto default;
                case CheckState.STALE :
                    MessageBox.Show("lol stalemate!", "lol");
                    _board.ClearBoard();
                    NewGame();
                    break;
                default:
                    _selectedSquare = null;
                    resetBackgrounds(_moves);
                    _moves = null;
                    swapTurn();
            

                    _fileSystemWatcher.EnableRaisingEvents = false;
                    _xmlExport.Export(XmlFilename);
                    _fileSystemWatcher.EnableRaisingEvents = true;
                    
                    break;
            }
        }

        public void HandleInput(Square square)
        {
            currentPlayer.SquarePressed(square);
        }

        private void swapTurn()
        {
            Turn = Turn == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
            _players.Enqueue(currentPlayer);
            currentPlayer = _players.Dequeue();
            currentPlayer.StartTurn();
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
