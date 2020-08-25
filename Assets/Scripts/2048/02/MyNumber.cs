using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyNumber : MonoBehaviour {
    private MyCell inCell;
    private Text number;
    private int value;
    private ConstVariable.NumberStatus status;
    private bool isCreateDone = false;
    private bool isMerge = false;
    private bool isMove = false;
    private bool isDestroy = false;

    private void Awake() {
        this.status = ConstVariable.NumberStatus.Normal;
        this.number = this.GetComponentInChildren<Text>();
        this.value = GenRandomNum();
        number.text = value.ToString();
        number.fontSize = GamePanel.fontSize;
        // 播放创建动画
        PlayCreateAni();
    }


    public void SetStatus(ConstVariable.NumberStatus status) {
        this.status = status;
    }

    public ConstVariable.NumberStatus GetStatus() {
        return this.status;
    }

    public int GetValue() {
        return this.value;
    }

    public void SetValue(int value) {
        this.value = value;
        this.number.text = value.ToString();
    }

    public MyCell GetCell() {
        return this.inCell;
    }

    public void SetCell(MyCell cell) {
        this.inCell = cell;
        cell.SetNumber(this);
    }

    public void MoveToCell(MyCell cell) {
        this.transform.SetParent(cell.transform);
        this.GetCell().SetNumber(null);
        cell.SetNumber(this);
        this.SetCell(cell);
        PlayMoveAni();
    }

    public void MergeCell() {
        this.SetValue(this.GetValue() * 2);
        this.SetStatus(ConstVariable.NumberStatus.Merged);
        PlayMergeAni();
    }

    public bool IsMerge(MyNumber num) {
        return num != null && num.GetStatus() != ConstVariable.NumberStatus.Merged && num.GetValue() == this.GetValue();
    }

    /// <summary>
    /// 以10%的概率生成4，90%的概率生成2
    /// </summary>
    /// <returns></returns>
    private static int GenRandomNum() {
        int value = Random.Range(0, 10);
        return value == 0 ? 4 : 2;
    }

    private void Update() {
        // 如果刚创建且没有在合并
        if (isCreateDone) {
            if (!isMerge) {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, 5 * Time.deltaTime);
            }
        } else {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * 1.5f, 8 * Time.deltaTime);
            if (transform.localScale == Vector3.one * 1.5f) {
                isCreateDone = true;
            }
        }

        // 合并动画
        if (isMerge) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * 1.5f, 4 * Time.deltaTime);
            if (transform.localScale == Vector3.one * 1.5f) {
                isMerge = false;
            }
        }

        // 移动动画
        if (isMove) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, 700 * Time.deltaTime);
            if (transform.localPosition == Vector3.zero) {
                isMove = false;
                if (isDestroy) {
                    Destroy(this.gameObject);
                }
            }
        }

    }

    private void PlayCreateAni() {
        // 设置为0以实现动画效果
        transform.localScale = Vector3.zero;
        isCreateDone = false;
    }

    private void PlayMergeAni() {
        isMerge = true;
    }

    private void PlayMoveAni() {
        isMove = true;
    }

    public void DestroyAfterMove() {
        this.GetCell().SetNumber(null);
        this.isMove = true;
        this.isDestroy = true;
    }

}