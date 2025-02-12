using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI[] rankTexts;

    private GameObject[] players;
    private List<RankSystem> sortArray = new List<RankSystem>();

    public bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            sortArray.Add(players[i].GetComponent<RankSystem>());
        }
    }

    void CalculateRank()
    {
        sortArray = sortArray.OrderBy(x => x.distance).ToList();

        for (int i = 0; i < rankTexts.Length && i < sortArray.Count; i++)
        {
            rankTexts[i].text = sortArray[i].name;
        }
    }

    void Update()
    {
        CalculateRank();
    }
}
