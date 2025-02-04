using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;
    public TextMeshProUGUI coinCountText, deadCounText;

    public void TapToStart()
    {
        startPanel.SetActive(false);

    }
}
