using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCell : MonoBehaviour {
    private MyNumber number;

    public bool CheckIsEmpty() {
        return number == null;
    }

    public void SetNumber(MyNumber number) {
        this.number = number;
    }

    public MyNumber GetNumber() {
        return this.number;
    }
}