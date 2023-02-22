using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Row[] rows { get; private set; }
    public Cell[] cells { get; private set; }
    public int size => cells.Length;
    public int height => rows.Length;

    public int width => size / height;
    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        cells = GetComponentsInChildren<Cell>();
    }
    private void Start()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            for (int j = 0; j < rows[i].cells.Length; j++)
            {
                rows[i].cells[j].coordinates = new Vector2Int(j, i);
            }
        }
    }

    public Cell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].occupied)
        {
            index++;

            //avoid out of index bounds
            if (index >= cells.Length)
            {
                index = 0;
            }

            if (index == startingIndex)
            {
                return null; // cant find empty cell
            }
        }

        return cells[index];
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        else return null;
    }

    public Cell GetCell(Vector2Int coordinates){
        return GetCell(coordinates.x, coordinates.y);
    }

    public Cell GetAdjacentCell(Cell cell, Vector2Int direction){
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }
}
