using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {
    private delegate bool Move();

    private const int cols = 4;
    private const int rows = 4;
    private readonly int[,] board;
    private readonly int[,] tempBoard;
    private readonly List<Location> remainLocation;
    private readonly Dictionary<Direction, Move> dirToMove;


    public Board() {
        board = new int[rows, cols];
        tempBoard = new int[rows, cols];
        remainLocation = new List<Location>(cols * rows);

        dirToMove = new Dictionary<Direction, Move> {
            { Direction.Up, new Move(this.MoveUp) },
            { Direction.Down, new Move(this.MoveDown) },
            { Direction.Left, new Move(this.MoveLeft) },
            { Direction.Right, new Move(this.MoveRight) }
        };
    }

    /// <summary>
    /// 以10%的概率生成4，90%的概率生成2
    /// </summary>
    /// <returns></returns>
    private static int GenRandomNum() {
        int value = Random.Range(0, 10);
        return value == 0 ? 4 : 2;
    }

    private void UpdateAvailCell() {
        this.remainLocation.Clear();
        for (int i = 0; i < cols * rows; ++i) {
            if (board[i / cols, i % cols] == 0) {
                this.remainLocation.Add(new Location(i / cols, i % cols));
            }
        }
    }

    private bool MoveUp() {
        System.Array.Clear(tempBoard, 0, tempBoard.Length);
        for (int i = 0; i < cols; ++i) {
            int index = 0;
            for (int j = 0; j < rows; ++j) {
                if (board[j, i] != 0) {
                    tempBoard[index++, i] = board[j, i];
                }
            }

            for (int mergeIndex = 0; mergeIndex < rows - 1; ++mergeIndex) {
                if (tempBoard[mergeIndex, i] == 0) {
                    break;
                }
                if (tempBoard[mergeIndex, i] == tempBoard[mergeIndex + 1, i]) {
                    tempBoard[mergeIndex, i] += tempBoard[mergeIndex + 1, i];
                    tempBoard[mergeIndex + 1, i] = 0;
                    for (int j = mergeIndex + 1; j < rows - 1; ++j) {
                        tempBoard[j, i] = tempBoard[j + 1, i];
                        tempBoard[j + 1, i] = 0;
                    }
                    break;
                }
            }
        }
        return this.CheckBoard();
    }

    private bool MoveDown() {
        System.Array.Clear(tempBoard, 0, tempBoard.Length);
        for (int i = 0; i < cols; ++i) {
            int index = rows - 1;
            for (int j = rows - 1; j >= 0; --j) {
                if (board[j, i] != 0) {
                    tempBoard[index--, i] = board[j, i];
                }
            }

            for (int mergeIndex = rows - 1; mergeIndex > 0; --mergeIndex) {
                if (tempBoard[mergeIndex, i] == 0) {
                    break;
                }
                if (tempBoard[mergeIndex, i] == tempBoard[mergeIndex - 1, i]) {
                    tempBoard[mergeIndex, i] += tempBoard[mergeIndex - 1, i];
                    tempBoard[mergeIndex - 1, i] = 0;
                    for (int j = mergeIndex - 1; j > 0; --j) {
                        tempBoard[j, i] = tempBoard[j - 1, i];
                        tempBoard[j - 1, i] = 0;
                    }
                    break;
                }
            }
        }
        return this.CheckBoard();
    }

    private bool MoveLeft() {
        System.Array.Clear(tempBoard, 0, tempBoard.Length);
        for (int i = 0; i < rows; ++i) {
            int index = 0;
            for (int j = 0; j < cols; ++j) {
                if (board[i, j] != 0) {
                    tempBoard[i, index++] = board[i, j];
                }
            }

            for (int mergeIndex = 0; mergeIndex < cols - 1; ++mergeIndex) {
                if (tempBoard[i, mergeIndex] == 0) {
                    break;
                }
                if (tempBoard[i, mergeIndex] == tempBoard[i, mergeIndex + 1]) {
                    tempBoard[i, mergeIndex] += tempBoard[i, mergeIndex + 1];
                    tempBoard[i, mergeIndex + 1] = 0;
                    for (int j = mergeIndex + 1; j < cols - 1; ++j) {
                        tempBoard[i, j] = tempBoard[i, j + 1];
                        tempBoard[i, j + 1] = 0;
                    }
                    break;
                }
            }
        }
        return this.CheckBoard();
    }

    private bool MoveRight() {
        System.Array.Clear(tempBoard, 0, tempBoard.Length);
        for (int i = 0; i < rows; ++i) {
            int index = cols - 1;
            for (int j = cols - 1; j >= 0; --j) {
                if (board[i, j] != 0) {
                    tempBoard[i, index--] = board[i, j];
                }
            }

            for (int mergeIndex = cols - 1; mergeIndex > 0; --mergeIndex) {
                if (tempBoard[i, mergeIndex] == 0) {
                    break;
                }
                if (tempBoard[i, mergeIndex] == tempBoard[i, mergeIndex - 1]) {
                    tempBoard[i, mergeIndex] += tempBoard[i, mergeIndex - 1];
                    tempBoard[i, mergeIndex - 1] = 0;
                    for (int j = mergeIndex - 1; j > 0; --j) {
                        tempBoard[i, j] = tempBoard[i, j - 1];
                        tempBoard[i, j - 1] = 0;
                    }
                    break;
                }
            }
        }
        return this.CheckBoard();
    }

    /// <summary>
    /// 上下左右移动格子
    /// </summary>
    public void ResortMerge(Direction dir) {
        bool isMove = this.dirToMove[dir].Invoke();
        // 如果发生了移动，则进行数据拷贝以及产生新数值
        if (isMove) {
            System.Array.Copy(tempBoard, board, board.Length);
            this.BoardGenNum();
        }
        if (!CheckContinue()) {
            Debug.Log("游戏结束");
        }
    }

    /// <summary>
    /// 检测游戏是否可以继续
    /// </summary>
    /// <returns></returns>
    private bool CheckContinue() {
        // 如果棋盘中还存在空的格子，则可以继续
        for (int i = 0; i < cols * rows; ++i) {
            if (board[i / cols, i % cols] == 0) {
                return true;
            }
        }
        // 如果棋盘已经满了，则需要判断是否可以移动
        bool result = false;
        foreach (Direction dir in System.Enum.GetValues(typeof(Direction))) {
            result |= dirToMove[dir].Invoke();
            if (result) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测是否进行了有效的移动
    /// </summary>
    /// <returns></returns>
    private bool CheckBoard() {
        for (int i = 0; i < tempBoard.Length; ++i) {
            if (tempBoard[i / cols, i % cols] != this.board[i / cols, i % cols]) {
                return true;
            }
        }
        return false;
    }


    public void Display() {
        MonoBehaviour.print(
            board[0, 0].ToString() + board[0, 1].ToString() + board[0, 2].ToString() + board[0, 3].ToString() + "\n" +
            board[1, 0].ToString() + board[1, 1].ToString() + board[1, 2].ToString() + board[1, 3].ToString() + "\n" +
            board[2, 0].ToString() + board[2, 1].ToString() + board[2, 2].ToString() + board[2, 3].ToString() + "\n" +
            board[3, 0].ToString() + board[3, 1].ToString() + board[3, 2].ToString() + board[3, 3].ToString()
            );
    }

    /// <summary>
    /// 在棋盘上随机可用位置随机生成2或者4
    /// </summary>
    public void BoardGenNum() {
        UpdateAvailCell();
        int length = this.remainLocation.Count;
        if (length == 0) {
            return;
        }
        int index = Random.Range(0, length);
        Location loc = this.remainLocation[index];
        // 更新棋盘信息
        this.board[loc.Row, loc.Col] = GenRandomNum();
    }

    private struct Location {
        readonly int row;
        readonly int col;

        public Location(int row, int col) : this() {
            this.row = row;
            this.col = col;
        }

        public int Row { get => row; }
        public int Col { get => col; }
    }

    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }

}