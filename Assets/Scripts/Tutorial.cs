using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private Text subtitles;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoPlay();
        }
    }

    public void GoPlay()
    {
        SceneManager.LoadScene("Play", LoadSceneMode.Single);
    }

    public void ChangeText(string text)
    {
        subtitles.text = text;
    }
}
