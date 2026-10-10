using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : InputReciever
{
    [SerializeField] private int levelIndex = 0;

    [SerializeField] private float startDelay = 1;
    [SerializeField] private float resultScreenDuration = 5;
    private AIOpponentBrain AIControl;

    [SerializeField] private GameObject startingSequencePrefab;
    [SerializeField] private GameObject winScreenPrefab;
    [SerializeField] private GameObject soyjakWinScreenPrefab;
    [SerializeField] private GameObject lossScreenPrefab;

    [SerializeField] private GameObject[] compendiumPages;
    private int currentCompendiumPageIndex = 0;
    private GameObject currentCompendiumPage;

    [SerializeField] private Vector3 startingSequenceSpawnPos;

    [SerializeField] private GameObject gridInputObject;

    [SerializeField] private GameObject canvas;

    [SerializeField] private int enemyUnits = 0;
    [SerializeField] private int playerUnits = 0;

    [SerializeField] private int playerComebackThreshold = 2;
    [SerializeField] private int aiComebackThreshold = 4;

    private bool gameStarted = false;

    private void Start()
    {
        AIControl = GameObject.FindGameObjectWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>();
        Time.timeScale = 1;
        if (compendiumPages.Length == 0)
        {
            StartCoroutine(GameStartDelay());
        }
        else
        {
            canvas.SetActive(false);
            ShowNextCompendiumPage();
        }
    }

    public override void SelectInput()
    {
        if (gameStarted == false)
        {
            ShowNextCompendiumPage();
        }
    }

    private void ShowNextCompendiumPage()
    {
        if (currentCompendiumPage != null)
        {
            Destroy(currentCompendiumPage);
        }
        if (currentCompendiumPageIndex < compendiumPages.Length)
        {
            GameObject newPage = Instantiate(compendiumPages[currentCompendiumPageIndex], Vector2.zero, quaternion.identity);
            currentCompendiumPage = newPage;
            currentCompendiumPageIndex++;
        }
        else
        {
            StartCoroutine(GameStartDelay());
        }
    }

    private IEnumerator GameStartDelay()
    {
        gameStarted = true;
        canvas.SetActive(true);
        yield return new WaitForSeconds(startDelay);
        
        GameObject start = Instantiate(startingSequencePrefab, startingSequenceSpawnPos, quaternion.identity);
        StartSequence startScript = start.GetComponent<StartSequence>();

        float nextDelay = startScript.GetFrameInterval() * startScript.GetFrameCount();
        yield return new WaitForSeconds(nextDelay);

        UniversalInputHandler.instance.ChangeInputReciever(gridInputObject);
        gridInputObject.GetComponent<GridInputHandler>().HighlightTile();
        
        UnitSelectorHandler selectorHandler = GameObject.FindGameObjectWithTag("PlayerUnitControl").GetComponent<UnitSelectorHandler>();
        AIOpponentBrain aiBrain = GameObject.FindGameObjectWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>();

        selectorHandler.StartPassiveGold();
        aiBrain.ActivateBrain();
    }   

    public void EndGame(bool isPlayer)
    {
        Debug.Log("Game Over");

        UnitSelectorHandler selectorHandler = GameObject.FindGameObjectWithTag("PlayerUnitControl").GetComponent<UnitSelectorHandler>();
        AIOpponentBrain aiBrain = GameObject.FindGameObjectWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>();

        selectorHandler.StopPassiveGold();
        aiBrain.DeactivateBrain();
        UniversalInputHandler.instance.ChangeInputReciever(null);

        canvas.SetActive(false);

        if (isPlayer == true)
        {
            Debug.Log("Player Loss");

            Instantiate(lossScreenPrefab, Vector2.zero, quaternion.identity);
        }
        else
        {
            Debug.Log("Player Win");

            int randomChance = UnityEngine.Random.Range(0, 70);

            if (randomChance != 69)
            {
                Instantiate(winScreenPrefab, Vector2.zero, quaternion.identity);
            }
            else
            {
                Instantiate(soyjakWinScreenPrefab, Vector2.zero, quaternion.identity);
            }

            SaveDataManager saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveDataManager>();

            int currentLevelIndex = saveManager.GetHighestLvl();
            
            if (levelIndex > currentLevelIndex)
            {
               
                saveManager.SetHighestLvl(currentLevelIndex + 1);
            }
        }

        StartCoroutine(ResultScreenSequence());
    }

    private void FixedUpdate()
    {
        if (enemyUnits > playerUnits)
        {
            int unitDif = enemyUnits - playerUnits;
            if (unitDif > playerComebackThreshold)
            {
                GameObject.FindGameObjectWithTag("PlayerUnitControl").GetComponent<UnitSelectorHandler>().SetPasiveAd(unitDif);
                Debug.Log("Unit dif:" + unitDif);
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("PlayerUnitControl").GetComponent<UnitSelectorHandler>().SetPasiveAd(0);
        }

        if (enemyUnits < playerUnits)
        {
            int unitDif = playerUnits - enemyUnits;
            if (unitDif > aiComebackThreshold)
            {
                GameObject.FindGameObjectWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>().SetPasiveAd(unitDif);
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("EnemyUnitControl").GetComponent<AIOpponentBrain>().SetPasiveAd(0);
        }
    }

    private IEnumerator ResultScreenSequence()
    {
        yield return new WaitForSeconds(resultScreenDuration);

        SceneManager.LoadScene("LevelSelect");
    }

    public void PlayerUnitAdded()
    {
        playerUnits++;
    }

    public void EnemyUnitAdded()
    {
        enemyUnits++;
    }

    public void RemovePlayerUnit()
    {
        playerUnits--;
    }

    public void RemoveEnemyUnit()
    {
        enemyUnits--;
    }
}
