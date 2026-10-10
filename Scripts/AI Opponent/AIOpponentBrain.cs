using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class AIOpponentBrain : MonoBehaviour
{
    private GoldBank goldBank;

    [SerializeField] private GameObject gameGrid;

    [SerializeField] private GameObject[] availableUnits;
    //0 Cannon
    //1 Mortar
    //2 Grape
    //3 Boarder
    //4 Barrel

    [SerializeField] private LayerMask laneMask;

    [SerializeField] private float analysisInterval = 1;

    [SerializeField] private bool isAIActive = false;

    [SerializeField] private GameObject NextUnit;
    [SerializeField] private GameObject NextTile;

    [SerializeField] private bool canCannon = true;
    [SerializeField] private bool canMortar = true;
    [SerializeField] private bool canGrapeShot = true;
    [SerializeField] private bool canBoarder = true;
    [SerializeField] private bool canBarrel = true;

    private bool placedObject;

    void Start()
    {
        goldBank = GetComponent<GoldBank>();
        
        StartCoroutine(AnalysisRoutine());
    }

    public void ActivateBrain()
    {
        isAIActive = true;
        goldBank.StartPassiveGain();
        StartCoroutine(AnalysisRoutine());
    }

    public void DeactivateBrain()
    {
        isAIActive = false;
        goldBank.StopPassiveGain();
    }

    private void Update()
    {
        if (NextTile != null && NextUnit != null && placedObject == false)
        {
            Tile tile = NextTile.GetComponent<Tile>();
            var unit = NextUnit.GetComponent<PlacableObject>();

            if ((tile.GetPlacedObject() == null || NextUnit == availableUnits[3]) && goldBank.GetGold() >= unit.GetCost())
            {
                GameObject newUnit = Instantiate(NextUnit, NextTile.transform.position, quaternion.identity);
                newUnit.GetComponent<PlacableObject>().SetPlayerControlStatus(false);
                if (NextUnit != availableUnits[3])
                {
                    tile.SetPlacedObject(newUnit);
                }
                newUnit.GetComponent<PlacableObject>().PlaceTower();

                goldBank.DecreasGold(unit.GetCost());
                placedObject = true;
            }
        }
    }

    public GoldBank GetGoldBank()
    {
        return goldBank;
    }

    private IEnumerator AnalysisRoutine()
    {
        while (isAIActive == true)
        {
            yield return new WaitForSeconds(analysisInterval);
            
            int priorityLane = FindPriorityLane();

            AnalyseLane(priorityLane);
        }
    }

    private Vector2 FindLanePositions(int lane, bool isStart)
    {
        float yPos = FindLaneYPos(lane);
        
        if (isStart == true)
        {
            Vector2 laneStartPosition = new Vector2(11, yPos);
            return laneStartPosition;
        }
        else
        {
            Vector2 laneEndPosition = new Vector2(-10, yPos);
            return laneEndPosition;
        }
    }

    private float FindLaneYPos(int lane)
    {
        float yPos = 0;

            switch (lane)
            {
                case 1:
                    yPos = 6.41f;
                    break;
                case 2:
                    yPos = 4.71f;
                    break;
                case 3:
                    yPos = 3.12f;
                    break;
                 case 4:
                    yPos = 1.7f;
                    break;
                case 5:
                    yPos = -0.07f;
                    break;
                case 6:
                    yPos = -1.72f;
                    break;
            }
        return yPos;
    }

    private void AnalyseLane(int lane)
    {
        Vector2 laneStartPosition = FindLanePositions(lane, true);
        Vector2 laneEndPosition = FindLanePositions(lane, false);

        Debug.DrawRay(laneStartPosition, (laneEndPosition - laneStartPosition), Color.blue, analysisInterval);
        RaycastHit2D[] laneAnalysis = Physics2D.RaycastAll(laneStartPosition, (laneEndPosition - laneStartPosition), 30, laneMask);

        GameObject tileRow = gameGrid.GetComponent<Game_Grid>().GetGridRow(lane);
        List<GameObject> myTiles = new List<GameObject>();
        List<GameObject> enemyTiles = new List<GameObject>();

        for (int child = 0; child < tileRow.transform.childCount; child++)
        {
            GameObject tile = tileRow.transform.GetChild(child).gameObject;

            if (tile.GetComponent<Tile>().GetTileType() == Tile.TileType.Enemy)
            {
                myTiles.Add(tile);
            }
            else if (tile.GetComponent<Tile>().GetTileType() == Tile.TileType.Player)
            {
                enemyTiles.Add(tile);
            }
        }

        List<GameObject> strategyTiles = new List<GameObject>();

        foreach (GameObject tile in myTiles)
        {
            if (tile.GetComponent<Tile>().GetPlacedObject() == null)
            {
                strategyTiles.Add(tile);
            }
        }

        List <GameObject> enemybasicsTiles = new List<GameObject>();
        List <GameObject> enemymortarsTiles = new List<GameObject>();
        List <GameObject> enemygrapeShotsTiles = new List<GameObject>();
        List <GameObject> enemyBoarders = new List<GameObject>();
        List <GameObject> enemyBarrelsTiles = new List<GameObject>();

        List <GameObject> mybasicsTiles = new List<GameObject>();
        List <GameObject> mymortarsTiles = new List<GameObject>();
        List <GameObject> mygrapeShotsTiles = new List<GameObject>();
        List <GameObject> myBoarders = new List<GameObject>();
        List <GameObject> myBarrelsTiles = new List<GameObject>();

        placedObject = false;

        //Get the position and types of all towers in the lane
        foreach (RaycastHit2D hit in laneAnalysis)
        {
            GameObject towerObject = hit.collider.gameObject;
            var tower = towerObject.GetComponent<PlacableObject>();
            
            GameObject unitTile = null;
            
            if (tower.GetPlayerControlStatus() == true)
            {
                foreach (GameObject tile in enemyTiles)
                {
                    if (tile.GetComponent<Tile>().GetPlacedObject() == towerObject)
                    {
                        unitTile = tile;
                    }
                }
            }
            else
            {
                foreach (GameObject tile in myTiles)
                {
                    if (tile.GetComponent<Tile>().GetPlacedObject() == towerObject)
                    {
                        unitTile = tile;
                    }
                }
            }
            
            if (tower is BasicTower)
            {
                if (tower.GetPlayerControlStatus() == true)
                {
                    enemybasicsTiles.Add(unitTile);
                }
                else
                {
                    mybasicsTiles.Add(unitTile);
                }
            }
            else if (tower is MortarTower)
            {
                if (tower.GetPlayerControlStatus() == true)
                {
                    enemymortarsTiles.Add(unitTile);
                    if (unitTile == enemyTiles[0])
                    {
                        RemoveStrategyTiles(0, 1, myTiles, strategyTiles);
                    }
                    else if (unitTile == enemyTiles[1])
                    {
                        RemoveStrategyTiles(0, 1, myTiles, strategyTiles);
                    }
                    else if (unitTile == enemyTiles[2])
                    {
                        RemoveStrategyTiles(2, 3, myTiles, strategyTiles);
                    }
                    else if (unitTile == enemyTiles[3])
                    {
                        RemoveStrategyTiles(2, 3, myTiles, strategyTiles);
                    }
                }
                else
                {
                    mymortarsTiles.Add(unitTile);
                }
                
            }
            else if (tower is GrapeShot)
            {   
                if (tower.GetPlayerControlStatus() == true)
                {
                    enemygrapeShotsTiles.Add(unitTile);
                   
                    if (unitTile == enemyTiles[3])
                    {
                        RemoveStrategyTiles(0, 1, myTiles, strategyTiles);
                    }
                    else if(unitTile == enemyTiles[2])
                    {
                        RemoveStrategyTile(0, myTiles, strategyTiles);
                    }
                }
                else
                {
                    mygrapeShotsTiles.Add(unitTile);
                }
            }
            else if(tower is Boarder)
            {
                if (tower.GetPlayerControlStatus() == true)
                {
                    enemyBoarders.Add(towerObject);
                }
                else
                {
                    myBoarders.Add(towerObject);
                }
            }
            else if (tower is Barrel)
            {
                if (tower.GetPlayerControlStatus() == true)
                {
                    enemyBarrelsTiles.Add(unitTile);
                }
                else
                {
                    myBarrelsTiles.Add(unitTile);
                }
            }
        }

        //Determine the best reaction to the lane
        if (strategyTiles.Count > 0)
        {
            if (strategyTiles.Contains(myTiles[0]) && enemyTiles[3].GetComponent<Tile>().GetPlacedObject() != null)
            {
                if (enemyTiles[3].GetComponent<Tile>().GetPlacedObject().GetComponent<PlacableObject>() is not GrapeShot)
                {
                    if (canGrapeShot == true)
                    {
                        NextUnit = availableUnits[2];
                        NextTile = myTiles[0];
                        return;
                    }   
                }
                else if (myBarrelsTiles.Contains(myTiles[0]) && myTiles[1].GetComponent<Tile>().GetPlacedObject() != null)
                {
                    if (canGrapeShot == true)
                    {
                        NextUnit = availableUnits[2];
                        NextTile = myTiles[1];
                        return;
                    }
                    
                }
                else
                {   
                    if (canBarrel == true)
                    {
                        NextUnit = availableUnits[4];
                        NextTile = myTiles[0];
                        return;
                    }
                }
            }
            if (strategyTiles.Contains(myTiles[1]) && enemyTiles[2].GetComponent<Tile>().GetPlacedObject() != null)
            {
                if (enemyTiles[2].GetComponent<Tile>().GetPlacedObject().GetComponent<PlacableObject>() is not GrapeShot)
                {
                    
                    if (canGrapeShot == true)
                    {
                        NextUnit = availableUnits[2];
                        NextTile = myTiles[0];
                        return;
                    }
                }
                else if (myBarrelsTiles.Contains(myTiles[0]) && myTiles[1].GetComponent<Tile>().GetPlacedObject() == null)
                {
                    if (canGrapeShot == true)
                    {
                        NextUnit = availableUnits[2];
                        NextTile = myTiles[1];
                        return;
                    }
                }
                else
                {
                    if (canBarrel == true)
                    {
                        NextUnit = availableUnits[4];
                        NextTile = myTiles[0];
                        return;
                    }
                }
            }
            if (enemybasicsTiles.Count > 0)
            {
                if(strategyTiles.Count > 1 && myBarrelsTiles.Count == 0)
                {
                    float lowestXValue = Mathf.Infinity;
                    GameObject bestTile = null;

                    foreach (GameObject tile in strategyTiles)
                    {
                        if (tile.transform.position.x < lowestXValue)
                        {
                            lowestXValue = tile.transform.position.x;
                            bestTile = tile;
                        }
                    }
                    
                    if (canBarrel == true)
                    {
                        NextUnit = availableUnits[4];
                        NextTile = bestTile;
                        return;
                    }
                }
                else if(FindMortarRange(enemybasicsTiles, strategyTiles, enemyTiles, myTiles, mymortarsTiles) == true)
                {
                    return;
                }
                else if(enemygrapeShotsTiles.Count == 0)
                {
                    if (canBoarder == true)
                    {
                        NextUnit = availableUnits[3];
                        NextTile = myTiles[0];
                        return; 
                    }
                }
                
            }
            if (enemygrapeShotsTiles.Count > 0)
            {
                bool canPlaceMortar = FindMortarRange(enemygrapeShotsTiles, strategyTiles, enemyTiles, myTiles, mymortarsTiles);
                if (canPlaceMortar == true)
                {
                    return;
                }
            }
            if (enemymortarsTiles.Count > 0 && enemygrapeShotsTiles.Count == 0)
            {
                if (canBoarder == true)
                {
                    NextUnit = availableUnits[3];
                    NextTile = myTiles[0];
                    return; 
                }
            }
            if (enemyBoarders.Count > 0)
            {
                    float highestXValue = -10000;
                    GameObject closestBoarder = null;

                    foreach (GameObject boarder in enemyBoarders)
                    {
                        if (boarder.transform.position.x > highestXValue)
                        {
                            highestXValue = boarder.transform.position.x;
                            closestBoarder = boarder;
                        }
                    }

                    highestXValue = -100000;
                    GameObject bestTile = null;

                    foreach (GameObject tile in strategyTiles)
                    {
                        if (tile.transform.position.x > highestXValue)
                        {
                            highestXValue = tile.transform.position.x;
                            bestTile = tile;
                        }
                    }

                    if (bestTile != null && mygrapeShotsTiles.Count == 0)
                    {
                       if (canGrapeShot == true)
                    {
                        NextUnit = availableUnits[2];
                        NextTile = bestTile;
                        return;
                    }
                    }
                    else if (bestTile != null && myBarrelsTiles.Count == 0)
                    {
                        if (canBarrel == true)
                    {
                        NextUnit = availableUnits[4];
                        NextTile = bestTile;
                        return;
                    }
                    }
                    else
                    {
                        if (canBoarder == true)
                        {
                            NextUnit = availableUnits[3];
                            NextTile = myTiles[0];
                            return; 
                        }
                    }
                    
            }
            int randomSafeTile = UnityEngine.Random.Range(0, strategyTiles.Count);
            if (canCannon == true)
            {
                NextUnit = availableUnits[0];
                NextTile = strategyTiles[randomSafeTile];
                return;
            }
        }
        else
        {
            if (canBoarder == true)
            {
                NextUnit = availableUnits[3];
                NextTile = myTiles[3];
                return; 
            }
            else
            {   
                strategyTiles.Clear();

                foreach (GameObject tile in myTiles)
                {
                    if (tile.GetComponent<Tile>().GetPlacedObject() == null)
                    {
                        strategyTiles.Add(tile);
                    }
                }

                int randomSafeTile = UnityEngine.Random.Range(0, strategyTiles.Count - 1);
                if (canCannon == true && strategyTiles.Count != 0)
                {
                    NextUnit = availableUnits[0];
                    NextTile = strategyTiles[randomSafeTile];
                    return;
                }
            }
        }
    }

    private bool FindMortarRange(List<GameObject> enemyUnitTiles, 
    List<GameObject> strategyTiles, List<GameObject> enemyTiles, List<GameObject> myTiles, List<GameObject> myMortarsTiles)
    {
        if (canMortar == true)
        {
            if ((enemyUnitTiles.Contains(enemyTiles[0]) || enemyUnitTiles.Contains(enemyTiles[1])) && strategyTiles.Contains(myTiles[0]) && !myMortarsTiles.Contains(myTiles[1]))
            {   
                NextUnit = availableUnits[1];
                NextTile = myTiles[0];
                return true;
            }
            else if ((enemyUnitTiles.Contains(enemyTiles[0]) || enemyUnitTiles.Contains(enemyTiles[1])) && strategyTiles.Contains(myTiles[1]) && !myMortarsTiles.Contains(myTiles[0]))
            {
                NextUnit = availableUnits[1];
                NextTile = myTiles[1];
                return true;
            }
            else if ((enemyUnitTiles.Contains(enemyTiles[2]) || enemyUnitTiles.Contains(enemyTiles[3])) && strategyTiles.Contains(myTiles[2]) && !myMortarsTiles.Contains(myTiles[3]))
            {
                NextUnit = availableUnits[1];
                NextTile = myTiles[2];
                return true;
            }
            else if ((enemyUnitTiles.Contains(enemyTiles[2]) || enemyUnitTiles.Contains(enemyTiles[3])) && strategyTiles.Contains(myTiles[3]) && !myMortarsTiles.Contains(myTiles[2]))
            {
                NextUnit = availableUnits[1];
                NextTile = myTiles[3];
                return true;
            }
        }
       
        return false;
    }

    private void RemoveStrategyTiles(int tileIndex1, int tileIndex2, 
    List<GameObject> myTiles, List<GameObject> strategyTiles)
    {
        GameObject tile1 = myTiles[tileIndex1];
        GameObject tile2 = myTiles[tileIndex2];
        strategyTiles.Remove(tile1);
        strategyTiles.Remove(tile2);
    }

    public void SetPasiveAd(int newPassive)
    {
        goldBank.SetPassiveAddtional(newPassive);
    }

    private void RemoveStrategyTile(int tileIndex1, 
    List<GameObject> myTiles, List<GameObject> strategyTiles)
    {
        GameObject tile1 = myTiles[tileIndex1];
        strategyTiles.Remove(tile1);
    }

    private int FindPriorityLane()
    {
        int lowestPriorityValue = 1000000000;

        List<int> priorityLanes = new List<int>();

        for (int lane = 1; lane < 7; lane++)
        {
            Vector2 laneStartPosition = FindLanePositions(lane, true);
            Vector2 laneEndPosition = FindLanePositions(lane, false);

            Debug.DrawRay(laneStartPosition, (laneEndPosition - laneStartPosition), Color.pink, analysisInterval);
            RaycastHit2D[] laneAnalysis = Physics2D.RaycastAll(laneStartPosition, (laneEndPosition - laneStartPosition), 30, laneMask);

            int priorityValue = 0;

            foreach (RaycastHit2D hit in laneAnalysis)
            {
                var towerObject = hit.collider.gameObject.GetComponent<PlacableObject>();

                if (towerObject is BasicTower)
                {
                    if (towerObject.GetComponent<PlacableObject>().GetPlayerControlStatus() == true)
                    {
                        priorityValue -= 2;
                    }
                    else
                    {
                        priorityValue += 2;
                    }
                }
                else if (towerObject is MortarTower)
                {
                    if (towerObject.GetComponent<PlacableObject>().GetPlayerControlStatus() == true)
                    {
                        priorityValue -= 1;
                    }
                    else
                    {
                        priorityValue += 1;
                    }
                }
                else if (towerObject is GrapeShot)
                {
                    if (towerObject.GetComponent<PlacableObject>().GetPlayerControlStatus() == true)
                    {
                        priorityValue -= 1;
                    }
                    else
                    {
                        priorityValue += 1;
                    }
                }
                else if (towerObject is Boarder)
                {
                    if (towerObject.GetComponent<PlacableObject>().GetPlayerControlStatus() == true)
                    {
                        priorityValue -= 3;
                    }
                    else
                    {
                        priorityValue += 3;
                    }
                }
                else if (towerObject is Barrel)
                {
                    if (towerObject.GetComponent<PlacableObject>().GetPlayerControlStatus() == false)
                    {
                        priorityValue += 1;
                    }
                }
            }
            if (priorityValue < lowestPriorityValue)
            {
                lowestPriorityValue = priorityValue;
                priorityLanes.Clear();
                priorityLanes.Add(lane);
            }
            else if (priorityValue == lowestPriorityValue)
            {
                priorityLanes.Add(lane);
            }
        }
        
        int randomPrioirityLane = UnityEngine.Random.Range(0, priorityLanes.Count);
        return priorityLanes[randomPrioirityLane];
    }
}
