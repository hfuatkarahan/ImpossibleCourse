using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    private Opponents[] opponentsArray;
    public TextMeshProUGUI coinCountText, deadCounText;

    private void Start()
    {
        opponentsArray = FindObjectsOfType<Opponents>();
    }

    public void TapToStart()
    {
        startPanel.SetActive(false);

        foreach (var opponent in opponentsArray)
        {
            opponent.AgentStart();
        }
    }
}
