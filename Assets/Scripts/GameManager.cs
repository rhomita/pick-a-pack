using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private CameraController cam;

    [Header("Enemies")]
    [SerializeField] private List<EnemySpawner> spawners;
    [SerializeField] private float enemiesCooldownSeconds = 2f;
    [SerializeField] private int enemiesBatchAmount;
    [SerializeField] private int enemiesAmount;

    [Header("Shop")]
    [SerializeField] private int toiletPapersAmount;
    [SerializeField] private GameObject shelvesObject;
    [SerializeField] private Transform shopEntrance;

    [Header("UI")]
    [SerializeField] private InfoUI infoUI;

    private float enemiesCooldown = 0;
    private int enemiesOffset = 0;

    private float seconds = 180;

    private bool isFinished = false;
    private bool isPaused = false;

    #region Singleton
    public static GameManager instance { get; private set; }
    void Awake()
    {
        instance = this;
    }
    #endregion

    void Start()
    {
        if (DifficultyManager.instance != null)
        {
            enemiesCooldownSeconds = DifficultyManager.instance.GetEnemiesCooldownSeconds();
            enemiesBatchAmount = DifficultyManager.instance.GetEnemiesBatchAmount();
            enemiesAmount = DifficultyManager.instance.GetEnemiesAmount();
        }
        
        HideCursor();
        SpawnToiletPaperPacks();
    }

    void Update()
    {
        if (isFinished) return;
        infoUI.UpdateTimer(seconds);
        seconds -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                EnableCursor();
                infoUI.ShowPause();
            } else {
                HideCursor();
                infoUI.ShowPlay();
            }
        }

        if (seconds <= 0)
        {
            Finish();
            return;
        }

        SpawnWithCooldown();
    }

    void Finish()
    {
        isFinished = true;
        // Disable user.
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerCombat>().enabled = false;
        cam.enabled = false;

        infoUI.ShowFinish();
        EnableCursor();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Play", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void SpawnWithCooldown()
    {
        if (enemiesOffset >= enemiesAmount) return;

        if (enemiesCooldown > 0)
        {
            enemiesCooldown -= Time.deltaTime;
            return;
        }
        enemiesCooldown = enemiesCooldownSeconds;

        SpawnEnemies(enemiesOffset);
        enemiesOffset += enemiesBatchAmount;
    }

    void SpawnEnemies(int offset = 0)
    {
        for (int i = offset; i < Mathf.Clamp(enemiesBatchAmount + offset, 0, enemiesAmount); i++)
        {
            SpawnRandomSpawner();
        }
    }

    void SpawnRandomSpawner()
    {
        int randomSpawner = Random.Range(0, spawners.Count - 1);
        spawners[randomSpawner].Spawn();


    }

    void SpawnToiletPaperPacks()
    {
        Shelf[] shelves = shelvesObject.GetComponentsInChildren<Shelf>();

        for(int i = 0; i < toiletPapersAmount; i++)
        {
            bool added = SpawnRandomShelf(shelves);
            if (!added) i--;
        }
    }

    bool SpawnRandomShelf(Shelf[] shelves) {
        int randomShelf = Random.Range(0, shelves.Length - 1);
        return shelves[randomShelf].AddToiletPaperPack();
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Transform GetShopEntrance()
    {
        return shopEntrance;
    }

    public InfoUI GetInfoUI()
    {
        return infoUI;
    }

    public CameraController GetCamera()
    {
        return cam;
    }
}
