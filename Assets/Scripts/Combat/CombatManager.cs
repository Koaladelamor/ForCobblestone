using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject[] m_players_temp;
    private GameObject[] m_players;
    private GameObject[] m_enemies;

    public GameObject m_spiderPrefab;
    public GameObject m_wormPrefab;

    private GameObject m_boss;
    public GameObject m_bossPrefab;

    public bool bossCombat;

    public GameObject endGameAnim;
    public GameObject m_canvasToMap;
    public GameObject m_gameOverCanvas;

    Vector2 EnemySpawnTile1;
    Vector2 EnemySpawnTile2;
    Vector2 EnemySpawnTile3;

    private int turn;
    private bool startCombat;
    private bool turnSet;

    private void Awake()
    {
        EnemySpawnTile1 = new Vector2(3, Random.Range(0, 4));
        EnemySpawnTile2 = new Vector2(3, Random.Range(0, 4));
        if (EnemySpawnTile2.y == EnemySpawnTile1.y) {
            while (EnemySpawnTile2.y == EnemySpawnTile1.y)
            {
                EnemySpawnTile2 = new Vector2(3, Random.Range(0, 4));
            }
        }
        EnemySpawnTile3 = new Vector2(3, Random.Range(0, 4));
        if (EnemySpawnTile3.y == EnemySpawnTile1.y || EnemySpawnTile3.y == EnemySpawnTile2.y)
        {
            while (EnemySpawnTile3.y == EnemySpawnTile1.y || EnemySpawnTile3.y == EnemySpawnTile2.y)
            {
                EnemySpawnTile3 = new Vector2(3, Random.Range(0, 4));
            }
        }
        startCombat = false;
        turnSet = false;
        m_enemies = new GameObject[3];
        m_players = new GameObject[m_players_temp.Length];
    }


    // Start is called before the first frame update
    void Start()
    {

        m_canvasToMap.SetActive(false);

        if (bossCombat)
        {
            m_boss = Instantiate(m_bossPrefab, new Vector3(190, 38, 0), transform.rotation);
        }
        else {
            EnemyType enemyType = GameManager.Instance.GetCurrentEnemyType();
            switch (enemyType)
            {
                case EnemyType.NONE:
                    Debug.Log("EnemyType set to none");
                    break;
                case EnemyType.SPIDER:
                    m_enemies[0] = Instantiate(m_spiderPrefab, new Vector3(190, 38, 0), transform.rotation);
                    m_enemies[1] = Instantiate(m_spiderPrefab, new Vector3(190, -14, 0), transform.rotation);
                    m_enemies[2] = Instantiate(m_spiderPrefab, new Vector3(190, -60, 0), transform.rotation);
                    break;
                case EnemyType.WORM:
                    m_enemies[0] = Instantiate(m_wormPrefab, new Vector3(190, 38, 0), transform.rotation);
                    m_enemies[1] = Instantiate(m_wormPrefab, new Vector3(190, -14, 0), transform.rotation);
                    m_enemies[2] = Instantiate(m_wormPrefab, new Vector3(190, -60, 0), transform.rotation);
                    break;
                default:
                    break;
            }
        }





        Invoke("AssignEnemies", 0.5f);

        turn = 0;
        CalculatePlayersTurn();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O)) {
            Invoke("VictoryCanvas", 1f);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Invoke("GameOverCanvas", 1f);
        }



        if (Input.GetKeyDown(KeyCode.S))
        {
            startCombat = true;
        }

        if (bossCombat)
        {
            if (BossDefeated())
            {
                Invoke("VictoryCanvas", 2f);
                turnSet = true;
            }

            if (startCombat && !turnSet)
            {
                switch (turn)
                {
                    default:
                        break;
                    case 0:
                        m_boss.GetComponent<EnemyPawn>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 1:
                        m_players[0].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 2:
                        m_players[1].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 3:
                        m_players[2].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 4:
                        turn = 0;
                        break;
                    case 5:
                        turn = 0;
                        break;
                    case 6:
                        turn = 0;
                        break;
                    case 7:
                        turn = 0;
                        break;
                }
            }
        }
        else
        {
            if (EnemiesDefeated())
            {
                UpdateCurrentHP();
                Invoke("VictoryCanvas", 1f);
                turnSet = true;
            }

            if (startCombat && !turnSet)
            {

                switch (turn)
                {
                    default:
                        break;

                    case 0:
                        m_players[0].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 1:
                        m_enemies[0].GetComponent<EnemyPawn>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 2:
                        m_players[1].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 3:
                        m_enemies[1].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 4:
                        m_players[2].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 5:
                        m_enemies[2].GetComponent<PawnController>().SetTurnOn();
                        turnSet = true;
                        break;

                    case 6:
                        turn = 0;
                        break;
                    case 7:
                        turn = 0;
                        break;
                }
            }
        }
    }

    private void CalculatePlayersTurn() {

        int[] pawnsAgility = new int[m_players_temp.Length];
        PawnController pawnController;
        for (int i = 0; i < m_players_temp.Length; i++)
        {
            pawnController = m_players_temp[i].GetComponent<PawnController>();
            if (pawnController == null) {
                Debug.Log("ERROR PLAYER NULL");
            }
            pawnsAgility[i] = pawnController.GetAgility();
            Debug.Log(pawnsAgility[i]);
        }


        //1st Turn
        if (pawnsAgility[0] > pawnsAgility[1] && pawnsAgility[0] > pawnsAgility[1])
        {
            m_players[0] = m_players_temp[0];
        }
        else if (pawnsAgility[1] > pawnsAgility[0] && pawnsAgility[1] > pawnsAgility[2])
        {
            m_players[0] = m_players_temp[1];
        }
        else if (pawnsAgility[2] > pawnsAgility[0] && pawnsAgility[2] > pawnsAgility[1])
        {
            m_players[0] = m_players_temp[2];
        }

        //2nd Turn
        if (pawnsAgility[0] > pawnsAgility[1] && pawnsAgility[0] < pawnsAgility[2])
        {
            m_players[1] = m_players_temp[0];
        }
        else if (pawnsAgility[0] < pawnsAgility[1] && pawnsAgility[0] > pawnsAgility[2])
        {
            m_players[1] = m_players_temp[0];
        }
        else if (pawnsAgility[1] > pawnsAgility[0] && pawnsAgility[1] < pawnsAgility[2])
        {
            m_players[1] = m_players_temp[1];
        }
        else if (pawnsAgility[1] < pawnsAgility[0] && pawnsAgility[1] > pawnsAgility[2])
        {
            m_players[1] = m_players_temp[1];
        }
        else if (pawnsAgility[2] > pawnsAgility[0] && pawnsAgility[2] < pawnsAgility[1])
        {
            m_players[1] = m_players_temp[2];
        }
        else if (pawnsAgility[2] < pawnsAgility[0] && pawnsAgility[2] > pawnsAgility[1])
        {
            m_players[1] = m_players_temp[2];
        }


        //3rd Turn
        if (pawnsAgility[0] < pawnsAgility[1] && pawnsAgility[0] < pawnsAgility[2])
        {
            m_players[2] = m_players_temp[0];
        }
        else if (pawnsAgility[1] < pawnsAgility[0] && pawnsAgility[1] < pawnsAgility[2])
        {
            m_players[2] = m_players_temp[1];
        }
        else if (pawnsAgility[2] < pawnsAgility[0] && pawnsAgility[2] < pawnsAgility[1])
        {
            m_players[2] = m_players_temp[2];
        }

        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_players[i] == null) {
                Debug.Log("ERROR on setting players turn");
            }
        }
    }

    bool EnemiesDefeated() {

        int enemiesDead = 0;
        int enemies = m_enemies.Length;
        for (int i = 0; i < enemies; i++) {
            if (!m_enemies[i].GetComponent<PawnController>().IsAlive()) {
                enemiesDead++;
            }
        }

        if (enemiesDead == enemies) {
            return true;
        }

        return false;
    }

    bool BossDefeated()
    {

        if (!m_boss.GetComponent<PawnController>().IsAlive())
        {
            return true;
        }

        return false;
    }
    bool PlayersDefeated()
    {

        int playersDead = 0;
        int players = m_players.Length;
        for (int i = 0; i < players; i++)
        {
            if (!m_players[i].GetComponent<PawnController>().IsAlive())
            {
                playersDead++;
            }
        }

        if (playersDead == players)
        {
            return true;
        }

        return false;
    }

    public void AssignEnemies() {
        if (bossCombat)
        {
            GridManager.Instance.AssignBossToTile(m_boss, new Vector2(3, 2));
        }
        else {
            GridManager.Instance.AssignPawnToTile(m_enemies[0], EnemySpawnTile1);
            GridManager.Instance.AssignPawnToTile(m_enemies[1], EnemySpawnTile2);
            GridManager.Instance.AssignPawnToTile(m_enemies[2], EnemySpawnTile3);
        }
    }

    public void CombatIsOver() {
        GameManager.Instance.SetCombatIsOver(true);
    }

    public void VictoryCanvas() {
        m_canvasToMap.SetActive(true);
    }

    public void GameOverCanvas()
    {
        m_gameOverCanvas.SetActive(true);
    }

    public void SetBoolCombatToTrue() {
        startCombat = true;
    }

    public void SetSpeedx2() {
        Time.timeScale = 2;
    }

    public void SetSpeedx1()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void NextTurn() {
        turn++;
        turnSet = false;
    }

    public void StartCombat() {
        startCombat = true;
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].GetComponent<PawnController>().SetDraggable(false);
        }
    }

    public void UpdateCurrentHP() {
        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_players[i].GetComponent<PawnController>().GetCharacterType() == PawnController.CHARACTER.GRODNAR) {
                GameStats.Instance.SetCurrentHP(GameStats.Instance.GetGrodnarStats(), m_players[i].GetComponent<PawnController>().GetCurrentHP());
            }
            else if (m_players[i].GetComponent<PawnController>().GetCharacterType() == PawnController.CHARACTER.LANSTAR)
            {
                GameStats.Instance.SetCurrentHP(GameStats.Instance.GetLanstarStats(), m_players[i].GetComponent<PawnController>().GetCurrentHP());
            }
            else if (m_players[i].GetComponent<PawnController>().GetCharacterType() == PawnController.CHARACTER.SIGFRID)
            {
                GameStats.Instance.SetCurrentHP(GameStats.Instance.GetSigfridStats(), m_players[i].GetComponent<PawnController>().GetCurrentHP());
            }
        }
    }

    public void ContinueGame() {
        GameManager.Instance.ContinueGame();
    }

    public void RestartGame() {
        GameManager.Instance.RestartGame();
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.Instance.mapMusic);
        AudioManager.Instance.PlayMusic();
    }

    public void GoToMainMenu() {
        GameManager.Instance.MainMenu();
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.ChangeBackgroundMusic(AudioManager.Instance.menuMusic);
        AudioManager.Instance.PlayMusic();
    }

    public void ExitGame() {
        GameManager.Instance.QuitGame();
    }

    public GameObject[] GetPlayers() { return m_players; }

    public void EndGame() {
        endGameAnim.SetActive(true);
    }
}
