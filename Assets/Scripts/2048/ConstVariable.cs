using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstVariable {
    public const string GameMode = "GameMode";
    public const string StartScene = "01";
    public const string GameScene = "02";
    public const string Sound = "Sound"; // 音效
    public const string Volume = "Volume";// 音量

    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    public enum NumberStatus {
        Normal,
        Merged
    }
}