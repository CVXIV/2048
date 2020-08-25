using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ConstVariable;

public class GamePanel : MonoBehaviour {
    public Text score;
    public Text bestScore;
    public Button restart;
    public Button quit;
    public Image grid;
    public GameObject prefabCell;
    public GameObject prefabNumber;
    private MyCell[,] cells;
    private List<MyCell> remainLocation;
    private int nums;
    private int cols, rows;
    public static int fontSize;
    private Vector3 pointerDownPos, pointerUpPos;

    private delegate bool Move();

    private Dictionary<Direction, Move> dirToMove;

    public void OnRestartClick() {
        InitGrid();
    }

    public void OnQuitClick() {
        SceneManager.LoadSceneAsync(ConstVariable.StartScene);
    }

    private void Awake() {
        dirToMove = new Dictionary<Direction, Move> {
            { Direction.Up, new Move(MoveUp) },
            { Direction.Down, new Move(MoveDown) },
            { Direction.Left, new Move(MoveLeft) },
            { Direction.Right, new Move(MoveRight) }
        };
        nums = PlayerPrefs.GetInt(ConstVariable.GameMode, 4);
        rows = cols = nums;
        InitGrid();
    }

    /// <summary>
    /// 初始化棋盘
    /// </summary>
    public void InitGrid() {
        // 初始化剩余格子
        InitRemainLocation();
        // 清空网格
        ClearGrid();
        // 设置展示的大小
        ResizeGrid();
        //创建格子填满网格
        FillGrid();
        // 新建两个随机数字
        CreateNumber();
        CreateNumber();
    }

    /// <summary>
    /// 初始化剩余格子
    /// </summary>
    /// <param name="nums"></param>
    private void InitRemainLocation() {
        this.remainLocation = new List<MyCell>(nums * nums);
    }

    private void ClearGrid() {
        Transform gridTransform = grid.transform;
        for (int i = 0; i < gridTransform.childCount; ++i) {
            Destroy(gridTransform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 根据选择的难度动态更新格子大小和对应的字体大小（cellSize）
    /// </summary>
    /// <param name="nums"></param>
    private void ResizeGrid() {
        GridLayoutGroup gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        float totalXSpace = (nums - 1) * gridLayoutGroup.spacing.x + gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float totalYSpace = (nums - 1) * gridLayoutGroup.spacing.y + gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
        Vector2 cellSize = gridLayoutGroup.cellSize;
        cellSize.x = (grid.rectTransform.rect.width - totalXSpace) / nums;
        cellSize.y = (grid.rectTransform.rect.height - totalYSpace) / nums;
        gridLayoutGroup.cellSize = cellSize;
        fontSize = Mathf.FloorToInt(cellSize.x / 1.25f);
    }

    /// <summary>
    /// 填充棋盘
    /// </summary>
    private void FillGrid() {
        cells = new MyCell[nums, nums];
        for (int row = 0; row < nums; ++row) {
            for (int col = 0; col < nums; ++col) {
                cells[row, col] = CreateCell();
            }
        }
    }


    public MyCell CreateCell() {
        GameObject obj = Instantiate(prefabCell, grid.gameObject.transform);
        return obj.GetComponent<MyCell>();
    }

    /// <summary>
    /// 在棋盘上随机可用位置随机生成2或者4
    /// </summary>
    public void CreateNumber() {
        UpdateAvailCell();
        int length = this.remainLocation.Count;
        if (length == 0) {
            return;
        }
        int index = Random.Range(0, length);
        MyCell toGenCell = this.remainLocation[index];
        GameObject obj = Instantiate(prefabNumber, toGenCell.transform);
        obj.GetComponent<MyNumber>().SetCell(toGenCell);
    }


    /// <summary>
    /// 更新空格子
    /// </summary>
    private void UpdateAvailCell() {
        this.remainLocation.Clear();
        foreach (var cell in cells) {
            if (cell.CheckIsEmpty()) {
                this.remainLocation.Add(cell);
            }
        }
    }

    public void OnPointerDown() {
        this.pointerDownPos = Input.mousePosition;
    }


    /// <summary>
    /// 在鼠标抬起的时候计算
    /// </summary>
    public void OnPointerUp() {
        this.pointerUpPos = Input.mousePosition;
        if (Vector3.Distance(this.pointerDownPos, this.pointerUpPos) < 100) {
            return;
        }
        Direction dir = Calculate();
        ResortMerge(dir);
    }

    private Direction Calculate() {
        if (Mathf.Abs(this.pointerDownPos.x - this.pointerUpPos.x) > Mathf.Abs(this.pointerDownPos.y - this.pointerUpPos.y)) {
            if (this.pointerDownPos.x > this.pointerUpPos.x) {
                return Direction.Left;
            } else {
                return Direction.Right;
            }
        } else {
            if (this.pointerDownPos.y > this.pointerUpPos.y) {
                return Direction.Down;
            } else {
                return Direction.Up;
            }
        }
    }

    /// <summary>
    /// 比较两个相邻的数字，并判断要不要合并；并返回是否继续循环
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool HandleTwoNumber(MyNumber source, MyCell target, ref bool isCreateNum) {
        if (!target.CheckIsEmpty()) {
            MyNumber next = target.GetNumber();
            if (source.IsMerge(next)) {
                source.DestroyAfterMove();
                next.MergeCell();
                isCreateNum = true;
            }
            return true;
        } else {
            source.MoveToCell(target);
            isCreateNum = true;
            return false;
        }
    }

    private bool MoveUp() {
        bool isCreateNum = false;
        for (int i = 0; i < cols; ++i) {
            // 从1开始，因为第一个格子不管有没有数字都没有必要判断
            for (int j = 1; j < rows; ++j) {
                // 如果存在数字，则继续处理
                if (!cells[j, i].CheckIsEmpty()) {
                    MyNumber number = cells[j, i].GetNumber();
                    for (int k = j - 1; k >= 0; --k) {
                        if (HandleTwoNumber(number, cells[k, i], ref isCreateNum)) {
                            break;
                        }
                    }
                }
            }
        }
        return isCreateNum;
    }


    private bool MoveDown() {
        bool isCreateNum = false;
        for (int i = 0; i < cols; ++i) {
            for (int j = rows - 2; j >= 0; --j) {
                if (!cells[j, i].CheckIsEmpty()) {
                    MyNumber number = cells[j, i].GetNumber();
                    for (int k = j + 1; k < rows; ++k) {
                        if (HandleTwoNumber(number, cells[k, i], ref isCreateNum)) {
                            break;
                        }
                    }
                }
            }
        }
        return isCreateNum;
    }

    private bool MoveLeft() {
        bool isCreateNum = false;
        for (int i = 0; i < rows; ++i) {
            for (int j = 1; j < cols; ++j) {
                if (!cells[i, j].CheckIsEmpty()) {
                    MyNumber number = cells[i, j].GetNumber();
                    for (int k = j - 1; k >= 0; --k) {
                        if (HandleTwoNumber(number, cells[i, k], ref isCreateNum)) {
                            break;
                        }
                    }
                }
            }
        }
        return isCreateNum;
    }

    private bool MoveRight() {
        bool isCreateNum = false;
        for (int i = 0; i < rows; ++i) {
            for (int j = cols - 2; j >= 0; --j) {
                if (!cells[i, j].CheckIsEmpty()) {
                    MyNumber number = cells[i, j].GetNumber();
                    for (int k = j + 1; k < cols; ++k) {
                        if (HandleTwoNumber(number, cells[i, k], ref isCreateNum)) {
                            break;
                        }
                    }
                }
            }
        }
        return isCreateNum;
    }

    /// <summary>
    /// 上下左右移动格子
    /// </summary>
    public void ResortMerge(Direction dir) {
        bool isMove = this.dirToMove[dir].Invoke();
        // 如果发生了移动，则进行数据拷贝以及产生新数值
        if (isMove) {
            ResetNumberStatus();
            this.CreateNumber();
        }
/*        if (!CheckContinue()) {
            Debug.Log("游戏结束");
        }*/
    }

    private void ResetNumberStatus() {
        foreach(var cell in cells) {
            if (!cell.CheckIsEmpty()) {
                cell.GetNumber().SetStatus(NumberStatus.Normal);
            }
        }
    }

    /*    /// <summary>
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
        }*/
}