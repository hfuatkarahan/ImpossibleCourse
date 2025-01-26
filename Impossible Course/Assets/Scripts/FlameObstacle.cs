using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameObstacle : MonoBehaviour
{
    [SerializeField] private GameObject shiningGameObject;
    private ParticleSystem ps;


    private void Start()
    {
        ps = shiningGameObject.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision other)
    {
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        var main = ps.main;
        main.startColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
}
