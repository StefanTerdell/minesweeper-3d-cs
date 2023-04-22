using System;
using System.Collections;
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
        // Debug.Log(cell.id);
        // GameObject.Destroy(cell.gameObject);
        Cascade(cell);
    }

    bool Cascade(Cell cell)
    {
        cell.Revealed = true;

        if (cell.Close == 0 && cell.state == CellState.Empty)
        {
            var x = cell.id.x;
            var y = cell.id.y;
            var z = cell.id.z;

            for (int rx = x > 0 ? -1 : 0; rx < 2 && rx + x < cells.Count; rx++)
            {
                for (int ry = y > 0 ? -1 : 0; ry < 2 && ry + y < cells[x].Count; ry++)
                {
                    for (int rz = z > 0 ? -1 : 0; rz < 2 && rz + z < cells[x][y].Count; rz++)
                    {
                        var next = cells[x + rx][y + ry][z + rz];

                        if (!next.Revealed)
                        {
                            Cascade(next);
                        }
                    }
                }
            }
        }

        return cell.state == CellState.Mine;
    }

    void OnRightClick(Cell cell)
    {
        Debug.Log(cell.id);
    }

    void CountMines()
    {
        for (int x = 0; x < cells.Count; x++)
        {
            for (int y = 0; y < cells[x].Count; y++)
            {
                for (int z = 0; z < cells[x][y].Count; z++)
                {
                    if (cells[x][y][z].state == CellState.Mine)
                    {
                        continue;
                    }

                    for (int rx = x > 0 ? -1 : 0; rx < 2 && rx + x < cells.Count; rx++)
                    {
                        for (int ry = y > 0 ? -1 : 0; ry < 2 && ry + y < cells[x].Count; ry++)
                        {
                            for (
                                int rz = z > 0 ? -1 : 0;
                                rz < 2 && rz + z < cells[x][y].Count;
                                rz++
                            )
                            {
                                if (x == 1 && y == 1 && z == 1)
                                {
                                    Debug.Log(new Vector3(rx, ry, rz));
                                }

                                if (rx == 0 && ry == 0 && rz == 0)
                                {
                                    continue;
                                }

                                if (cells[x + rx][y + ry][z + rz].state == CellState.Mine)
                                {
                                    cells[x][y][z].Close += 1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void Initialize()
    {
        cells = new List<List<List<Cell>>>();
        for (int x = 0; x < size.x; x++)
        {
            cells.Add(new List<List<Cell>>());
            for (int y = 0; y < size.y; y++)
            {
                cells[x].Add(new List<Cell>());

                for (int z = 0; z < size.z; z++)
                {
                    var position =
                        transform.position
                        + new Vector3(x - (size.x / 2), y - (size.y / 2), z - (size.z / 2))
                            * spacing;

                    var obj = Instantiate(cubePrefab, position, Quaternion.identity);

                    obj.transform.parent = transform;

                    var cell = obj.GetComponent<Cell>();

                    if (cell == null)
                    {
                        throw new Exception("No Cell script found in cubePrefab");
                    }

                    cell.id = new Vector3Int(x, y, z);
                    cell.onClick = () => OnClick(cell);
                    cell.onRightClick = () => OnRightClick(cell);
                    cell.state =
                        new System.Random().Next(100) < incidence
                            ? CellState.Mine
                            : CellState.Empty;

                    cells[x][y].Add(cell);
                }
            }
        }

        CountMines();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (cubePrefab == null)
        {
            throw new Exception("Need cubePrefab");
        }

        Initialize();
    }

    // Update is called once per frame
    void Update() { }
}
