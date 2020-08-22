using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour {

    private Board board;
    private void Awake() {
        board = new Board();
        // 生成两个随机数字给棋盘
        board.BoardGenNum();
        board.BoardGenNum();
    }

    private void Start() {
        board.Display();
    }

    private void Update() {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (vertical != 0) {
            board.ResortMerge(vertical == 1 ? Board.Direction.Up : Board.Direction.Down);
            board.Display();
        }
        if (horizontal != 0) {
            board.ResortMerge(horizontal == 1 ? Board.Direction.Right : Board.Direction.Left);
            board.Display();
        }
    }


}
