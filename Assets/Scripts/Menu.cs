using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject difficultyMenu;

    void Start()
    {
        ShowMenu();
    }

    public void ShowDifficulty()
    {
        mainMenu.SetActive(false);
        difficultyMenu.SetActive(true);
    }

    public void ShowMenu()
    {
        mainMenu.SetActive(true);
        difficultyMenu.SetActive(false);
    }

    public void SelectDifficulty(string difficulty)
    {
        DifficultyManager.instance.SelectDifficulty(difficulty);
        Play();
    }

    public void Play()
    {
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
