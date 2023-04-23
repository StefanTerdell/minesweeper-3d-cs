using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Vector3Int size;

    [Range(1, 100)]
    public int incidence = 20;
    public float spacing;

    public List<List<List<Cell>>> cells;

    void OnClick(Cell cell)
    {
        if (Utils.CascadeReveal(cell, cells))
        {
            Utils.ApplyExplosiveForce(cells, cell);

            GameController.Lose();
        }
        else if (Utils.CheckWin(cells))
        {
            GameController.Win();
        }
    }

    void OnRightClick(Cell cell)
    {
        cell.Flagged = !cell.Flagged;

        if (Utils.CheckWin(cells))
        {
            GameController.Win();
        }
    }

    void Start()
    {
        if (cubePrefab == null)
        {
            throw new Exception("Need cubePrefab");
        }

        Initialize();
    }

    void Initialize()
    {
        cells = Utils.NewCells(
            size,
            (id) =>
            {
                var position =
                    transform.position
                    + new Vector3(id.x - (size.x / 2), id.y - (size.y / 2), id.z - (size.z / 2))
                        * spacing;

                var obj = Instantiate(cubePrefab, position, Quaternion.identity);

                obj.transform.parent = transform;

                var cell = obj.GetComponent<Cell>();

                if (cell == null)
                {
                    throw new Exception("No Cell script found in cubePrefab");
                }

                cell.Id = id;
                cell.OnClick = () => OnClick(cell);
                cell.OnRightClick = () => OnRightClick(cell);
                cell.Mined = new System.Random().Next(100) < incidence;

                return cell;
            }
        );

        Utils.CountMines(cells);
    }
}
