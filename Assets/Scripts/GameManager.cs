using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager mInstance;

    static public GameManager Instance
    {
        get { return mInstance; }
        private set { }
    }

    private EnemyType currentEnemyType;
    private int currentEnemyID;
    public EnemySpawner[] enemySpawners;

    private bool enemyEngaged;
    private bool combatIsOver;

    public TargetPosition pointToGo;
    public GameObject party;
    private Vector3 newGamePosition;

    public GameObject arrowPrefab;

    private void Awake()
    {
        //Singleton
        if (mInstance == null)
        {
            mInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        combatIsOver = false;
        enemyEngaged = false;
        currentEnemyID = -1;
        currentEnemyType = EnemyType.NONE;
        newGamePosition = new Vector3(-720, 271, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(this == null) { return; }

        for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawners[i].RespawnEnemy(enemySpawners[i].transform.position, i);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void Update()
    {

        if (enemyEngaged)
        {
            enemyEngaged = false;

            //Save stats

            //Enemy info
            Debug.Log(currentEnemyID);
            CanvasManager.Instance.m_canvasToCombat.SetActive(true);
        }

        if (combatIsOver)
        {
            combatIsOver = false;

            LoadMapScene();

            InventoryManager.Instance.GenerateRandomLoot((int)Random.Range(1, 6));
            GameStats.Instance.AddCoins(500);
            GameStats.Instance.AddXpToGrodnar(800f);
            GameStats.Instance.AddXpToLanstar(800f);
            GameStats.Instance.AddXpToSigfrid(800f);

            for (int i = 0; i < enemySpawners.Length; i++)
            {
                if (enemySpawners[i].GetSpawnerID() == currentEnemyID)
                {
                    enemySpawners[i].GetEnemy().GetComponent<PatrolAI>().SetAlive(false);
                }
            }
            CanvasManager.Instance.NormalSpeed();

            InventoryManager.Instance.UpdateCoinsAmount();
            InventoryManager.Instance.DisplayLoot();

        }
    }

    public void LoadCombatScene() {
        GameObject[] gameObjectsOnScene;
        gameObjectsOnScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in gameObjectsOnScene)
        {
            obj.SetActive(false);
        }
        SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
    }

    public void LoadMapScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("CombatScene"));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapScene"));
        GameObject[] gameObjectsOnScene;
        gameObjectsOnScene = SceneManager.GetSceneByName("MapScene").GetRootGameObjects();
        foreach (GameObject obj in gameObjectsOnScene)
        {
            obj.SetActive(true);
        }

        InventoryManager.Instance.HideInventories();
        GameObject canvasHostal = GameObject.FindGameObjectWithTag("CanvasHostal");
        canvasHostal.SetActive(false);
        GameObject canvasPause = GameObject.FindGameObjectWithTag("CanvasPause");
        canvasPause.SetActive(false);
        CanvasManager.Instance.DisableCombatCanvas();
        CanvasManager.Instance.canvasMenu.SetActive(false);

    }

    public void SetActiveScene(string scene) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoad: "+scene.name);
        SetActiveScene(scene.name);
    }

    public void ContinueGame() {
        LoadMapScene();
        party.transform.position = newGamePosition;
        StopMovement();
        EnablePartyMovement();
        GameStats.Instance.SetCoins(GameStats.Instance.GetCoins()/2);
        //Eliminar objetos de inventario también?
    }

    public void RestartGame() {
        LoadMapScene();
        party.transform.position = newGamePosition;
        StopMovement();
        EnablePartyMovement();
        GameStats.Instance.SetCoins(0);
        InventoryManager.Instance.ClearInventories();
        InventoryManager.Instance.InitTavernLoot();
        //reset stats
    }

    /*private void OnDestroy()
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            if (spawner != null)
            {
                Destroy(spawner.GetEnemy());
                Destroy(spawner.gameObject);
            }
        }
    }*/


    public bool GetCombatIsOver() { return combatIsOver; }

    public void SetCombatIsOver(bool isCombatOver) { combatIsOver = isCombatOver; }

    public void SetCurrentEnemyID(int ID) { currentEnemyID = ID; }

    public void SetCurrentEnemyType(EnemyType type) { currentEnemyType = type; }

    public EnemyType GetCurrentEnemyType() { return currentEnemyType; }

    public void SetEnemyEngaged (bool engaged) { enemyEngaged = engaged; }

    public void EnablePartyMovement() { pointToGo.SetMovement(true); }

    public void DisablePartyMovement() { pointToGo.SetMovement(false); }

    public void StopMovement() { pointToGo.gameObject.transform.position = party.transform.position; }

    public void QuitGame() { Application.Quit(); }

    public void DestroyInstance() { Destroy(this.gameObject); }

}
