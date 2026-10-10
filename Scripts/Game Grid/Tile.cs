using UnityEngine;

public class Tile : MonoBehaviour
{
    private BoxCollider2D col;
    [SerializeField] private GameObject left_tile;
    [SerializeField] private GameObject right_tile;
    [SerializeField] private GameObject top_tile;
    [SerializeField] private GameObject bottom_tile;

    public enum TileType
    {
        Player,
        Ocean,
        Enemy
    }

    [SerializeField] private TileType tileType;

    [SerializeField] private LayerMask tileMask;

    [SerializeField] private GameObject placedObject;
    
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void LocateNeighbours()
    {
        ClearNeighbours();

        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
        RaycastHit2D hit_left = Physics2D.Raycast(transform.position, Vector2.left, 0.9f, tileMask);
        RaycastHit2D hit_right = Physics2D.Raycast(transform.position, Vector2.right, 0.9f, tileMask);
        RaycastHit2D hit_up = Physics2D.Raycast(transform.position, Vector2.up, 0.9f, tileMask);
        RaycastHit2D hit_down = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, tileMask);
        if(hit_left)
        {
            left_tile = hit_left.transform.gameObject;
        }
        else
        {
            left_tile = null;
        }
        if (hit_right)
        {
            right_tile = hit_right.transform.gameObject;
        }
        else
        {
            right_tile = null;
        }
        if (hit_up)
        {
            top_tile = hit_up.transform.gameObject;
        }
        else
        {
            top_tile = null;
        }
        if (hit_down)
        {
            bottom_tile = hit_down.transform.gameObject;
        }
        else
        {
            bottom_tile = null;
        }
            col.enabled = true;
    }

    private void ClearNeighbours()
    {
        top_tile = null;
        right_tile = null;
        bottom_tile = null;
        left_tile = null;
    }

    public void OnSelected()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void OnNotSelected()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public GameObject GetRightTile()
    {
        return right_tile;
    }

    public GameObject GetTopTile()
    {
        return top_tile;
    }

    public GameObject GetBottomTile()
    {
        return bottom_tile;
    }

    public GameObject GetLeftTile()
    {
        return left_tile;
    }

    public TileType GetTileType()
    {
        return tileType;
    }

    public GameObject GetPlacedObject()
    {
        return placedObject;
    }

    public void SetPlacedObject(GameObject newObject)
    {
        placedObject = newObject;
    }
}
