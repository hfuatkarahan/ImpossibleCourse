using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel, gameSounds;
    private Opponents[] opponentsArray;
    public TextMeshProUGUI coinCountText, deadCounText;


    private void Start()
    {
        gameSounds = GameObject.Find("Game Sounds");
        opponentsArray = FindObjectsOfType<Opponents>();
    }

    public void TapToStart()
    {
        startPanel.SetActive(false);
        gameSounds.transform.Find("Background Music").GetComponent<AudioSource>().Play();

        foreach (var opponent in opponentsArray)
        {
            opponent.AgentStart();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
