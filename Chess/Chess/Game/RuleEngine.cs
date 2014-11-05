using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class RuleEngine
    {
        private Square[][] board;

        public RuleEngine(Square[][] board)
        {
            this.board = board;
        }
    }
}
