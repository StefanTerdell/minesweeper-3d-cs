using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    Empty,
    Flagged,
    Mine
}

public class Cell : MonoBehaviour
{
    public CellState state;
    private int close;
    public Vector3Int id;
    public Action onClick;
    public Action onRightClick;
    private bool revealed;
    public Number number;

    public bool Revealed
    {
        get => revealed;
        set
        {
            GetComponent<MeshRenderer>().enabled = !value;
            number.gameObject.SetActive(value);
            revealed = value;
        }
    }

    public int Close
    {
        get => close;
        set
        {
            number.Set(value);
            close = value;
        }
    }

    void OnGUI()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (state == CellState.Mine)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(pos.x, Screen.height - pos.y, 100, 30), "X");
        }
        else if (state == CellState.Flagged) { }
        else if (Close != 0)
        {
            // GUI.Label(new Rect(pos.x, Screen.height - pos.y, 100, 30), Close.ToString());
        }
    }
}
