using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour
{
    public DialogSetUp[] dialogs;
}

[Serializable]
public class DialogSetUp
{
    public string[] sentences;
    public Sprite[] characters;
    public Sprite background;
    public Sprite focus;
}
