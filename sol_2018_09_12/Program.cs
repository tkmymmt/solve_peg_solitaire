using System;

namespace sol_2018_09_12
{
    enum Direction
    {
        up = 0,
        right = 1,
        down = 2,
        left = 3
    }

    class MainClass
    {
        static BoardData _baseBoard;
        static bool _isSolved;

        public static void Main(string[] args)
        {
            _baseBoard = BoardData.MakeFirstBoard();

            Solve(_baseBoard);

            Console.ReadLine();
        }

        static void Solve(BoardData board)
        {
            if (_isSolved) return;

            if(board.Count == 31 && board.Board[3, 3] == 1)
            {
                for (int i = 0; i < board.Position.GetLength(0);i++)
                {
                    Console.WriteLine(string.Format("({0}.{1}) => ({2}.{3})"
                                                    , board.Position[i, 0].ToString()
                                                    , board.Position[i, 1].ToString()
                                                    , board.Position[i, 2].ToString()
                                                    , board.Position[i, 3].ToString()));
                }
                _isSolved = true;
                return;
            }

            for (int i = 0; i < board.Board.GetLength(0); i++)
            {
                for (int j = 0; j < board.Board.GetLength(1); j++)
                {
                    if(board.IsShiftable(j, i, Direction.up))
                    {
                        Solve(board.ShiftedBoard(j, i, Direction.up));
                    }

                    if(board.IsShiftable(j, i, Direction.right))
                    {
                        Solve(board.ShiftedBoard(j, i, Direction.right));
                    }

                    if(board.IsShiftable(j, i, Direction.down))
                    {
                        Solve(board.ShiftedBoard(j, i, Direction.down));
                    }

                    if(board.IsShiftable(j, i, Direction.left))
                    {
                        Solve(board.ShiftedBoard(j, i, Direction.left));
                    }
                }
            }
        }

        class BoardData
        {
            public int[,] Board { get; set; }

            public int Count { get; set; }

            public int[,] Position { get; set; }

            public BoardData(int[,] board, int count, int[,] position)
            {
                Board = board;
                Count = count;
                Position = position;
            }

            public BoardData(int[,] board)
            {
                Board = board;
                Count = 0;
                Position = new int[31, 4];
            }

            public bool IsShiftable(int x, int y, Direction direction)
            {
                var lim = Board.GetLength(0);

                switch(direction)
                {
                    case Direction.up:
                        return 0 <= y - 2 && (Board[x, y] == 1 && Board[x, y - 1] == 1 && Board[x, y - 2] == 0);
                    case Direction.right:
                        return x + 2 < lim && (Board[x, y] == 1 && Board[x + 1, y] == 1 && Board[x + 2, y] == 0);
                    case Direction.down:
                        return y + 2 < lim && (Board[x, y] == 1 && Board[x, y + 1] == 1 && Board[x, y + 2] == 0);
                    case Direction.left:
                        return 0 <= x - 2 && (Board[x, y] == 1 && Board[x - 1, y] == 1 && Board[x - 2, y] == 0);
                    default:
                        return false;
                }
            }

            public BoardData ShiftedBoard(int x, int y, Direction direction)
            {
                int[,] oldBoard = Board;
                int[,] newBoard = ArrayDeepCopy(oldBoard);
                int[,] newPosition = ArrayDeepCopy(Position);
                int count = Count;

                switch(direction)
                {
                    case Direction.up:
                        newBoard[x, y] = oldBoard[x, y] == 0 ? 1 : 0;
                        newBoard[x, y - 1] = oldBoard[x, y - 1] == 0 ? 1 : 0;
                        newBoard[x, y - 2] = oldBoard[x, y - 2] == 0 ? 1 : 0;
                        newPosition[count, 0] = x;
                        newPosition[count, 1] = y;
                        newPosition[count, 2] = x;
                        newPosition[count, 3] = y - 2;

                        return new BoardData(newBoard, count + 1, newPosition);
                    case Direction.right:
                        newBoard[x, y] = oldBoard[x, y] == 0 ? 1 : 0;
                        newBoard[x + 1, y] = oldBoard[x + 1, y] == 0 ? 1 : 0;
                        newBoard[x + 2, y] = oldBoard[x + 2, y] == 0 ? 1 : 0;
                        newPosition[count, 0] = x;
                        newPosition[count, 1] = y;
                        newPosition[count, 2] = x + 2;
                        newPosition[count, 3] = y;

                        return new BoardData(newBoard, count + 1, newPosition);
                    case Direction.down:
                        newBoard[x, y] = oldBoard[x, y] == 0 ? 1 : 0;
                        newBoard[x, y + 1] = oldBoard[x, y + 1] == 0 ? 1 : 0;
                        newBoard[x, y + 2] = oldBoard[x, y + 2] == 0 ? 1 : 0;
                        newPosition[count, 0] = x;
                        newPosition[count, 1] = y;
                        newPosition[count, 2] = x;
                        newPosition[count, 3] = y + 2;

                        return new BoardData(newBoard, count + 1, newPosition);
                    case Direction.left:
                        newBoard[x, y] = oldBoard[x, y] == 0 ? 1 : 0;
                        newBoard[x - 1, y] = oldBoard[x - 1, y] == 0 ? 1 : 0;
                        newBoard[x - 2, y] = oldBoard[x - 2, y] == 0 ? 1 : 0;
                        newPosition[count, 0] = x;
                        newPosition[count, 1] = y;
                        newPosition[count, 2] = x - 2;
                        newPosition[count, 3] = y;

                        return new BoardData(newBoard, count + 1, newPosition);
                    default:
                        return MakeFirstBoard();
                }
            }

            public static BoardData MakeFirstBoard()
            {
                int[,] newBoard = new int[7, 7];

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if((i < 2 || 4 < i) && (j < 2 || 4 < j))
                        {
                            newBoard[i, j] = -1;
                        }
                        else
                        {
                            newBoard[i, j] = i == 3 && j == 3 ? 0 : 1;
                        }
                    }
                }

                return new BoardData(newBoard, 0, new int[31, 4]);
            }

            public void ShowBoard()
            {
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    string line = string.Empty;
                    for (int j = 0; j < Board.GetLength(1); j++)
                    {
                        string s = string.Empty;
                        switch(Board[i, j])
                        {
                            case -1:
                                s = " ";
                                break;
                            case 0:
                                s = "○";
                                break;
                            case 1:
                                s = "●";
                                break;
                            default:
                                break;
                        }
                        line += s;
                        if (j == Board.GetLength(1) - 1) line += Environment.NewLine;
                    }

                    Console.WriteLine(line);
                }
            }

            int[,] ArrayDeepCopy(int[,] oldArray)
            {
                var newArray = new int[oldArray.GetLength(0), oldArray.GetLength(1)];
                for (int i = 0; i < oldArray.GetLength(0); i++)
                {
                    for (int j = 0; j < oldArray.GetLength(1); j++)
                    {
                        newArray[i, j] = oldArray[i, j];
                    }
                }
                return newArray;
            }
        }
    }
}
