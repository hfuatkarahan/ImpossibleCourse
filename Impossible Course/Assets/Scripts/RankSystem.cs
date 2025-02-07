using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankSystem : MonoBehaviour
{
    public int rank;
    public GameObject _target;
    public float distance;

    void DistanceMeter()
    {
        distance = Vector3.Distance(transform.position, _target.transform.position);
    }

    void Update()
    {
        DistanceMeter();
    }
}
