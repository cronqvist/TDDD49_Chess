﻿using System.Collections.Generic;
using Chess.Model;
using System.Linq;
using System.Diagnostics;

namespace Chess.Game
{
    public enum CheckState
    {
        NONE, CHECK, MATE, STALE
    };

    public class RuleEngine
    {

        public static List<Move> GetAvailableMoves(Piece piece, GameBoard _board)
        {
            List<Move> ret = new List<Move>();
            List<Move> potentialMoves = piece.GetAvailableMoves(_board);
            PiecePosition orgPos = piece.Position;

            //Check for "self check" 
            foreach (var move in potentialMoves)
            {
                GameBoard newBoard = new GameBoard(_board);
                King mKing = piece.Color == PlayerColor.White ? newBoard.WhiteKing : newBoard.BlackKing;
                Piece nPiece = newBoard.GetPieceAt(orgPos.X, orgPos.Y);

                if (nPiece.IsKing())
                    mKing = nPiece as King;

                newBoard.MovePiece(nPiece, move);

                if (!IsThreatened(mKing, newBoard))
                    ret.Add(move);
            }

            return ret;
        }

        public static bool IsPromotePos(PiecePosition p)
        {
            return p.Y == 0 || p.Y == 7;
        }


        public static CheckState GetCheckState(PlayerColor p, GameBoard _board)
        {
            bool canMove = false;
            King king = p == PlayerColor.White ? _board.WhiteKing : _board.BlackKing;

            List<Piece> pieces = p == PlayerColor.White ? _board.WhitePieces : _board.BlackPieces;
            foreach (var piece in pieces)
            {
                List<Move> moves = GetAvailableMoves(piece, _board);
                if (moves.Count > 0)
                {
                    canMove = true;
                    break;
                }
            }

            if (IsThreatened(king, _board))
            {
                return canMove ? CheckState.CHECK : CheckState.MATE;
            }

            return !canMove ? CheckState.STALE : CheckState.NONE;
        }

        private static bool IsThreatened(Piece p, GameBoard b)
        {
            bool dirThreat = ThreatInDir(p, b, 0, 1) ||
                             ThreatInDir(p, b, 0, -1) ||
                             ThreatInDir(p, b, 1, 0) ||
                             ThreatInDir(p, b, -1, 0) ||
                             ThreatInDir(p, b, 1, 1) ||
                             ThreatInDir(p, b, 1, -1) ||
                             ThreatInDir(p, b, -1, 1) ||
                             ThreatInDir(p, b, -1, -1);

            if (dirThreat)
                return dirThreat;

            return KnightThreat(p,b);
        }

        private static bool KnightThreat(Piece p, GameBoard b)
        {
            List<Knight> knights = p.Color == PlayerColor.White ? b.BlackKnights : b.WhiteKnights;

            foreach (var knight in knights)
            {
                List<Move> moves = knight.GetAvailableMoves(b);
                foreach (var move in moves)
                {
                    if (move.Type == MoveType.Attack && move.Position == p.Position)
                        return true;
                }
            }

            return false;
        }

        private static bool ThreatInDir(Piece p, GameBoard b, int xDir, int yDir)
        {
            if (p == null)
                return false; //theres something weird going on

            int nr_steps = 8;
            PiecePosition pos = p.Position;
            PlayerColor myColor = p.Color;
            IEnumerable<int> steps = Enumerable.Range(1,nr_steps);
            foreach (int step in steps)
            {
                PiecePosition nPos = new PiecePosition(pos.X + step * xDir, pos.Y + step * yDir);
                
                if(!GameBoard.IsInBoard(nPos.X, nPos.Y)) 
                    break;

                Piece otherPiece = b.GetPieceAt(nPos.X, nPos.Y);

                if (otherPiece != null) 
                {

                    if (otherPiece.Color == myColor)
                        return false;

                    List<Move> moves = otherPiece.GetAvailableMoves(b);
                    foreach (var move in moves)
                    {
                        if ((move.Type == MoveType.Attack || move.Type == MoveType.PromoteQueen) && move.Position == pos)
                            return true;
                    }

                    return false;
                }
            }

            return false;
        }


    }
}
