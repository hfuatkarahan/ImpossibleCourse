using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowEffect : MonoBehaviour
{
    [SerializeField] Gradient _gradient;

    private void Update()
    {
        GetComponent<Renderer>().material.color = _gradient.Evaluate(Time.time % 1);
    }
}
