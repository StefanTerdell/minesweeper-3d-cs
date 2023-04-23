using System;
using System.Collections.Generic;
using UnityEngine;
using Cells = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<Cell>>>;

public static class Utils
{
    public static void ApplyExplosiveForce(Cells cells, Cell origin)
    {
        {
            for (int x = 0; x < cells.Count; x++)
            {
                for (int y = 0; y < cells[x].Count; y++)
                {
                    for (int z = 0; z < cells[x][y].Count; z++)
                    {
                        var next = cells[x][y][z];

                        if (next == origin)
                        {
                            continue;
                        }
                        
                        next.ApplyExplosiveForce(origin.transform.position);
                    }
                }
            }
        }
    }

    public static bool CheckWin(Cells cells)
    {
        for (int x = 0; x < cells.Count; x++)
        {
            for (int y = 0; y < cells[x].Count; y++)
            {
                for (int z = 0; z < cells[x][y].Count; z++)
                {
                    var curr = cells[x][y][z];

                    if (curr.Mined != curr.Flagged)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /*
        Returns true if the first cell revealed has a mine.
    */
    public static bool CascadeReveal(Cell curr, Cells cells)
    {
        curr.Reveal();

        if (curr.Close == 0 && !curr.Mined)
        {
            var x = curr.Id.x;
            var y = curr.Id.y;
            var z = curr.Id.z;

            for (int rx = x > 0 ? -1 : 0; rx < 2 && rx + x < cells.Count; rx++)
            {
                for (int ry = y > 0 ? -1 : 0; ry < 2 && ry + y < cells[x].Count; ry++)
                {
                    for (int rz = z > 0 ? -1 : 0; rz < 2 && rz + z < cells[x][y].Count; rz++)
                    {
                        var next = cells[x + rx][y + ry][z + rz];

                        if (!next.Revealed)
                        {
                            CascadeReveal(next, cells);
                        }
                    }
                }
            }
        }

        return curr.Mined;
    }

    public static void CountMines(Cells cells)
    {
        for (int x = 0; x < cells.Count; x++)
        {
            for (int y = 0; y < cells[x].Count; y++)
            {
                for (int z = 0; z < cells[x][y].Count; z++)
                {
                    var curr = cells[x][y][z];

                    if (curr.Mined)
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
                                var neighbor = cells[x + rx][y + ry][z + rz];

                                if (neighbor == curr)
                                {
                                    continue;
                                }

                                if (neighbor.Mined)
                                {
                                    curr.Close += 1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static Cells NewCells(Vector3Int size, Func<Vector3Int, Cell> instantiateCell)
    {
        var cells = new List<List<List<Cell>>>();
        for (int x = 0; x < size.x; x++)
        {
            cells.Add(new List<List<Cell>>());
            for (int y = 0; y < size.y; y++)
            {
                cells[x].Add(new List<Cell>());

                for (int z = 0; z < size.z; z++)
                {
                    var cell = instantiateCell(new Vector3Int(x, y, z));

                    cells[x][y].Add(cell);
                }
            }
        }

        return cells;
    }
}
