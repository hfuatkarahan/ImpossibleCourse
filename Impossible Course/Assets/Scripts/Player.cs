using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    Vector3 startPosition, startRotation;
    PlayerController _playerController;


    private void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //startRotation = new Vector3(0, 0, 0);
        _playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(DeadRoutine());
        }
    }
    private IEnumerator DeadRoutine()
    {
        _playerController.AnimPlay("dead");
        _playerController.speed = 0;

        yield return new WaitForSeconds(2f);

        transform.position = startPosition;
        _playerController.speed = 10;
    }
}
