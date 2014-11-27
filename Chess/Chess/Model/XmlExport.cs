using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Chess.Game;

namespace Chess.Model
{
    public class XmlExport
    {
        private readonly GameEngine _gameEngine;

        public XmlExport(GameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public void UpdateBoard(String filename, GameEngine gameEngine)
        {
            var pieceList = new List<Piece>();
            XDocument doc;

            try
            {
                 doc = XDocument.Load(filename);
            }
            catch (Exception e)
            {
                //something wrong with xml file lol fix it
                return;
            }

            var turn = doc.Descendants("Turn").First().Value;
            if (turn == "Black")
            {
                gameEngine.Turn = Player.Black;
            }
            else
            {
                gameEngine.Turn = Player.White;
            }

            var piecesList = (from pieces in doc.Descendants("Piece")
                                select pieces);

            foreach (var xElement in piecesList)
            {
                var color = xElement.Descendants("Player").First().Value;
                Player playerColor;
                if (color == "White")
                {
                    playerColor = Player.White;
                }
                else
                {
                    playerColor = Player.Black;
                }

                var type = xElement.Descendants("Type").First().Value;
                    
                var position = xElement.Descendants("Position").First();
                var posX = position.Descendants("X").First().Value;
                var posY = position.Descendants("Y").First().Value;

                Piece piece;
                switch (type)
                {
                    case "Rook":
                        piece = new Rook(playerColor, new PiecePosition(int.Parse(posX), int.Parse(posY)));
                        break;
                    case "Pawn":
                        piece = new Pawn(playerColor, new PiecePosition(int.Parse(posX), int.Parse(posY)));
                        break;
                    case "King":
                        piece = new King(playerColor, new PiecePosition(int.Parse(posX), int.Parse(posY)));
                        break;
                    case "Queen":
                        piece = new Queen(playerColor, new PiecePosition(int.Parse(posX), int.Parse(posY)));
                        break;
                    case "Knight":
                        piece = new Knight(playerColor, new PiecePosition(int.Parse(posX), int.Parse(posY)));
                        break;
                    default:
                        piece = new Bishop(playerColor, new PiecePosition(int.Parse(posX), int.Parse(posY)));
                        break;
                        
                }

                pieceList.Add(piece);
            }

            gameEngine.Board.SetBoard(pieceList);
        }

        public void Export(String filename)
        {
            var doc = new XDocument(
                new XElement("Board",
                    new XElement("Pieces"),
                    new XElement("Turn")));

            addPieces(doc, _gameEngine.Board.BlackPieces);
            addPieces(doc, _gameEngine.Board.WhitePieces);

            var xElement = doc.Descendants("Turn").FirstOrDefault();
            if (xElement != null)
                xElement.Add(new XElement("Player", _gameEngine.Turn.ToString()));

            doc.Save(filename);
        }

        private void addPieces(XDocument doc, List<Piece> pieces)
        {
            pieces.ForEach(piece =>
            {
                var element = doc.Descendants("Pieces").FirstOrDefault();
                if (element != null)
                    element.Add(new XElement("Piece",
                        new XElement("Player", piece.Color.ToString()),
                        new XElement("Type", piece.GetType().Name),
                        new XElement("Position",
                            new XElement("X", piece.Position.X.ToString(CultureInfo.InvariantCulture)),
                            new XElement("Y", piece.Position.Y.ToString(CultureInfo.InvariantCulture)))
                        ));
            });
        }
    }
}
