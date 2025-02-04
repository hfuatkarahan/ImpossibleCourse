using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    Vector3 startPosition;
    PlayerController _playerController;


    private void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        FallReturn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            //_scoreManager.coinCount += 5;
            //_uiManager.coinCountText.text = _scoreManager.coinCount.ToString();

        }

        if (other.CompareTag("Finish"))
        {
            _playerController.speed = 0;
            //GameManager.Instance.isGameOver = true;
            //_canvas.SetActive(false);
            //_drawingCanvas.SetActive(true);
            //_mainCamera.gameObject.SetActive(false);
            //_dummyCam.gameObject.SetActive(true);
            //_drawingCamera.gameObject.SetActive(true);
            //_paintBoy.SetActive(true);
        }
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

    public void FallReturn()
    {
        if (transform.position.y < -7f)
        {
            transform.position = startPosition;
            //_scoreManager.UpdateDeadScore();
        }
    }
}
