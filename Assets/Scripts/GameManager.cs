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

        SceneManager.sceneLoaded += OnSceneLoaded;

        for (int i = 0; i < enemySpawners.Length; i++)
        {
            if (enemySpawners[i] == null) { Debug.LogError("EnemySpawner is null"); }
            enemySpawners[i].RespawnEnemy(enemySpawners[i].transform.position, i);
            if (enemySpawners[i].GetEnemy() == null) { Debug.LogError("Enemy didn't spawn"); }
        }

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
            GameStats.Instance.AddCoins(50);
            GameStats.Instance.AddXpToGrodnar(500f);
            GameStats.Instance.AddXpToLanstar(500f);
            GameStats.Instance.AddXpToSigfrid(500f);

            for (int i = 0; i < enemySpawners.Length; i++)
            {
                if (enemySpawners[i].GetSpawnerID() == currentEnemyID)
                {
                    enemySpawners[i].GetEnemy().GetComponent<PatrolAI>().SetAlive(false);
                }
            }

            List<Stat> grodnarStats = GameStats.Instance.GetGrodnarStats();
            List<Stat> lanstarStats = GameStats.Instance.GetLanstarStats();
            List<Stat> sigfridStats = GameStats.Instance.GetSigfridStats();
            for (int i = 0; i < grodnarStats.Count; i++)
            {
                if (grodnarStats[i].attribute == Attributes.CURR_HEALTH) {
                    if (grodnarStats[i].value <= 0) {
                        InventoryManager.Instance.InteractableGrodnarButton(false);
                    }
                }
            }
            for (int i = 0; i < lanstarStats.Count; i++)
            {
                if (lanstarStats[i].attribute == Attributes.CURR_HEALTH)
                {
                    if (lanstarStats[i].value <= 0)
                    {
                        InventoryManager.Instance.InteractableLanstarButton(false);
                    }
                }
            }
            for (int i = 0; i < sigfridStats.Count; i++)
            {
                if (sigfridStats[i].attribute == Attributes.CURR_HEALTH)
                {
                    if (sigfridStats[i].value <= 0)
                    {
                        InventoryManager.Instance.InteractableSigfridButton(false);
                    }
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

        AudioManager.Instance.StopMusic();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.Instance.combatMusic);
        AudioManager.Instance.PlayMusic();
    }

    public void LoadBossScene()
    {
        GameObject[] gameObjectsOnScene;
        gameObjectsOnScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in gameObjectsOnScene)
        {
            obj.SetActive(false);
        }
        SceneManager.LoadSceneAsync("BossCombat", LoadSceneMode.Additive);

        AudioManager.Instance.StopMusic();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.Instance.combatMusic);
        AudioManager.Instance.PlayMusic();
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
        CanvasManager.Instance.m_canvasToCombat.SetActive(false);
        CanvasManager.Instance.m_canvasMenu.SetActive(false);
        CanvasManager.Instance.m_canvasPause.SetActive(false);

        AudioManager.Instance.StopMusic();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.Instance.mapMusic);
        AudioManager.Instance.PlayMusic();

    }

    public void SetActiveScene(string scene) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //Debug.Log("OnSceneLoad: "+scene.name);
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
        Time.timeScale = 1;
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

    public void MainMenu() {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        Destroy(GameManager.Instance.gameObject);
        GameStats.Instance.DestroyInstance();
    }

    public void DestroyInstance() { Destroy(this.gameObject); }

    public void RestartButton() {
        party.transform.position = newGamePosition;
        StopMovement();
        EnablePartyMovement();
        Time.timeScale = 1;
        GameStats.Instance.SetCoins(0);
        InventoryManager.Instance.ClearInventories();
        InventoryManager.Instance.InitTavernLoot();
        CanvasManager.Instance.ContinueButton();
        //reset stats
    }

}
