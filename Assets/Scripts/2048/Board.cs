using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {
    private const int cols = 4;
    private const int rows = 4;
    private readonly int[,] board;
    private readonly List<Location> remainLocation;

    public Board() {
        board = new int[rows, cols];
        remainLocation = new List<Location>(cols * rows);
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

    /// <summary>
    /// 上下左右移动格子
    /// </summary>
    public void ResortMerge(Direction dir) {
        int[,] temp = new int[rows, cols];
        switch (dir) {
            case Direction.Up:
                for (int i = 0; i < cols; ++i) {
                    int index = 0;
                    for (int j = 0; j < rows; ++j) {
                        if (board[j, i] != 0) {
                            temp[index++, i] = board[j, i];
                        }
                    }

                    for (int mergeIndex = 0; mergeIndex < rows - 1; ++mergeIndex) {
                        if (temp[mergeIndex, i] == 0) {
                            break;
                        }
                        if (temp[mergeIndex, i] == temp[mergeIndex + 1, i]) {
                            temp[mergeIndex, i] += temp[mergeIndex + 1, i];
                            temp[mergeIndex + 1, i] = 0;
                            for (int j = mergeIndex + 1; j < rows - 1; ++j) {
                                temp[j, i] = temp[j + 1, i];
                                temp[j + 1, i] = 0;
                            }
                            break;
                        }
                    }
                }
                break;
            case Direction.Down:
                for (int i = 0; i < cols; ++i) {
                    int index = rows - 1;
                    for (int j = rows - 1; j >= 0; --j) {
                        if (board[j, i] != 0) {
                            temp[index--, i] = board[j, i];
                        }
                    }

                    for (int mergeIndex = rows - 1; mergeIndex > 0; --mergeIndex) {
                        if (temp[mergeIndex, i] == 0) {
                            break;
                        }
                        if (temp[mergeIndex, i] == temp[mergeIndex - 1, i]) {
                            temp[mergeIndex, i] += temp[mergeIndex - 1, i];
                            temp[mergeIndex - 1, i] = 0;
                            for (int j = mergeIndex - 1; j > 0; --j) {
                                temp[j, i] = temp[j - 1, i];
                                temp[j - 1, i] = 0;
                            }
                            break;
                        }
                    }
                }
                break;
            case Direction.Left:
                for (int i = 0; i < rows; ++i) {
                    int index = 0;
                    for (int j = 0; j < cols; ++j) {
                        if (board[i, j] != 0) {
                            temp[i, index++] = board[i, j];
                        }
                    }

                    for (int mergeIndex = 0; mergeIndex < cols - 1; ++mergeIndex) {
                        if (temp[i, mergeIndex] == 0) {
                            break;
                        }
                        if (temp[i, mergeIndex] == temp[i, mergeIndex + 1]) {
                            temp[i, mergeIndex] += temp[i, mergeIndex + 1];
                            temp[i, mergeIndex + 1] = 0;
                            for (int j = mergeIndex + 1; j < cols - 1; ++j) {
                                temp[i, j] = temp[i, j + 1];
                                temp[i, j + 1] = 0;
                            }
                            break;
                        }
                    }
                }
                break;
            case Direction.Right:
                for (int i = 0; i < rows; ++i) {
                    int index = cols - 1;
                    for (int j = cols - 1; j >= 0; --j) {
                        if (board[i, j] != 0) {
                            temp[i, index--] = board[i, j];
                        }
                    }

                    for (int mergeIndex = cols - 1; mergeIndex > 0; --mergeIndex) {
                        if (temp[i, mergeIndex] == 0) {
                            break;
                        }
                        if (temp[i, mergeIndex] == temp[i, mergeIndex - 1]) {
                            temp[i, mergeIndex] += temp[i, mergeIndex - 1];
                            temp[i, mergeIndex - 1] = 0;
                            for (int j = mergeIndex - 1; j > 0; --j) {
                                temp[i, j] = temp[i, j - 1];
                                temp[i, j - 1] = 0;
                            }
                            break;
                        }
                    }
                }
                break;
        }
        System.Array.Copy(temp, this.board, this.board.Length);
        /// todo如果棋盘没有变动，则不用生成新数字
        this.BoardGenNum();
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