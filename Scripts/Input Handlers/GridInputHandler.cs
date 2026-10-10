using Unity.Mathematics;
using UnityEngine;

public class GridInputHandler : InputReciever
{
    [SerializeField] private GameObject currentTile;

    [SerializeField] private GameObject unitSelectionManager;

    [SerializeField] private GameObject pauseMenu;

    private GameObject placedObject;

    public void HighlightTile()
    {
        currentTile.GetComponent<Tile>().OnSelected();
        UnitSelectorHandler handler = unitSelectionManager.GetComponent<UnitSelectorHandler>();
        UnitSelector selector = handler.GetCurrentSelector().GetComponent<UnitSelector>();
        if (placedObject == null && currentTile.GetComponent<Tile>().GetPlacedObject() == null)
        {
            placedObject = Instantiate(selector.GetUnitPrefab(), currentTile.transform.position, quaternion.identity);
        }
        else if(placedObject != null && currentTile.GetComponent<Tile>().GetPlacedObject() != null && !placedObject.TryGetComponent<Boarder>(out Boarder a))
        {
            Destroy(placedObject);
        }
    }

    private void ClearTileHighlight(bool isExitingMenu)
    {
        currentTile.GetComponent<Tile>().OnNotSelected();
        if (isExitingMenu)
        {
            Destroy(placedObject);
        }
    }

    public override void RightInput()
    {
        Tile tile = currentTile.GetComponent<Tile>();
        ChangeFocusedTile(tile.GetRightTile(), tile);
    }

    public override void LeftInput()
    {
        Tile tile = currentTile.GetComponent<Tile>();
        ChangeFocusedTile(tile.GetLeftTile(), tile);
    }

    public override void UpInput()
    {
        Tile tile = currentTile.GetComponent<Tile>();
        ChangeFocusedTile(tile.GetTopTile(), tile);
    }

    public override void DownInput()
    {
        Tile tile = currentTile.GetComponent<Tile>();
        ChangeFocusedTile(tile.GetBottomTile(), tile);
    }

    public override void BInput()
    {
        UniversalInputHandler.instance.ChangeInputReciever(unitSelectionManager);
        ClearTileHighlight(true);
    }

    public override void StartInput()
    {
        UniversalInputHandler.instance.ChangeInputReciever(pauseMenu);
        pauseMenu.GetComponent<Menu>().ChangeInputReceiver(gameObject);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public override void SelectInput()
    {
        Tile tile = currentTile.GetComponent<Tile>();
        UnitSelectorHandler handler = unitSelectionManager.GetComponent<UnitSelectorHandler>();
        UnitSelector selector = handler.GetCurrentSelector().GetComponent<UnitSelector>();
        if (placedObject == null && currentTile.GetComponent<Tile>().GetPlacedObject() == null)
        {
            placedObject = Instantiate(selector.GetUnitPrefab(), currentTile.transform.position, quaternion.identity);
        }
        else if (placedObject != null && currentTile.GetComponent<Tile>().GetPlacedObject() != null && !placedObject.TryGetComponent<Boarder>(out Boarder b))
        {
            Destroy(placedObject);
        }
        if (placedObject != null)
        {
             if (placedObject.TryGetComponent<Boarder>(out Boarder a) && selector.GetCooldown() == 0 && handler.GetGoldBank().GetGold() >= selector.GetGoldCost())
            {
                placedObject.GetComponent<PlacableObject>().PlaceTower();
                selector.ResetCooldown();
                handler.GetGoldBank().DecreasGold(selector.GetGoldCost());
                placedObject = null;
            }
            if (tile.GetPlacedObject() == null && selector.GetCooldown() == 0 && handler.GetGoldBank().GetGold() >= selector.GetGoldCost())
            {
                placedObject.GetComponent<PlacableObject>().PlaceTower();
                selector.ResetCooldown();
                handler.GetGoldBank().DecreasGold(selector.GetGoldCost());
                tile.SetPlacedObject(placedObject);
                placedObject = null;
            }
        }
       
        HighlightTile();
    }

    private void ChangeFocusedTile(GameObject newTile, Tile tile)
    {
        ClearTileHighlight(false);

        if (CheckTileIsValid(newTile) == true)
        {
            currentTile = newTile;
            HighlightTile();
            if (placedObject != null)
            {
                placedObject.transform.position = currentTile.transform.position;
            }
        }
        else
        {
            HighlightTile();
        }
    }

    private bool CheckTileIsValid(GameObject tileObject)
    {
        if (tileObject != null)
        {
            Tile tile = tileObject.GetComponent<Tile>();
            if (tile.GetTileType() == Tile.TileType.Player)
            {
                return true;
            }  
        }

        return false;  
    }
}
