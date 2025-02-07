using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform uiCoinIcon;

    Vector3 startPosition;
    PlayerController _playerController;
    ScoreManager _scoreManager;
    UIManager _uiManager;
    GameObject canvas;

    private void Awake()
    {
        _uiManager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        _scoreManager = GameObject.Find("Game Manager").GetComponent<ScoreManager>();
    }

    private void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _playerController = GetComponent<PlayerController>();
        canvas = GameObject.Find("Canvas");
    }

    private void Update()
    {
        FallReturn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            _uiManager.coinCountText.text = _scoreManager.coinCount.ToString();
            _scoreManager.coinCount += 5;
        }

        if (other.CompareTag("Finish"))
        {
            _playerController.speed = 0;
            canvas.SetActive(false);
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
        _playerController.AnimPlay("death");
        _playerController.speed = 0;
        _scoreManager.UpdateDeadScore();

        yield return new WaitForSeconds(2f);

        transform.position = startPosition;
        _playerController.speed = 10;
    }

    public void FallReturn()
    {
        if (transform.position.y < -7f)
        {
            transform.position = startPosition;
            _scoreManager.UpdateDeadScore();
        }
    }
}
