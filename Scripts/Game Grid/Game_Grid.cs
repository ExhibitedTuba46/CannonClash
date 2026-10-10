using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Game_Grid : MonoBehaviour
{
    [SerializeField] private List<GameObject> grid_tiles;
    [SerializeField] private List<GameObject> grid_rows;

    public void LocateTileNeighbours()
    {
        foreach (GameObject tile in grid_tiles)
        {
            tile.GetComponent<Tile>().LocateNeighbours();
        }
    }

    public GameObject GetGridRow(int row)
    {
        return grid_rows[row - 1];
    }

    public List<GameObject> GetGridTiles()
    {
        return grid_tiles;
    }
}
