using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int coinCount, deathCount;
    UIManager _uiManager;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        coinCount = 0;
        deathCount = 0;
    }
    public void UpdateCoinScore()
    {
        _uiManager.coinCountText.text = coinCount.ToString();
    }

    public void UpdateDeadScore()
    {
        deathCount++;
        _uiManager.deadCounText.text = "Death: " + deathCount.ToString();
    }
}
