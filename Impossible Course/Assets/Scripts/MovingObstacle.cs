using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float width = 6;


    void Update()
    {
        transform.position = new Vector3 (Mathf.Sin(Time.time * speed) * width, 
            transform.position.y, transform.position.z);
    }
}
