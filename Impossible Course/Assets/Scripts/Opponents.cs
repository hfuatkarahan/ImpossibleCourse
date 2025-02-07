using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponents : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Animator _animator;
    private NavMeshAgent agent;
    private Vector3 agentStartPosition;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agentStartPosition = transform.position;
    }

    void AnimPlay(string animName)
    {
        _animator.SetBool("walk", false);
        _animator.SetBool("idle", false);
        _animator.SetBool("death", false);
        _animator.SetBool(animName, true);
    }

    public void AgentStart()
    {
        agent.enabled = true;
        agent.SetDestination(target.transform.position);
        AnimPlay("walk");
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver && agent.enabled)
        {
            agent.isStopped = true;
            AnimPlay("idle");
            OpponentsFallReturn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            DeadRoutine();
        }

        if (collision.gameObject.CompareTag("Hammer"))
        {
            StartCoroutine(ObstacleHit());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            agent.speed = 0;
            AnimPlay("idle");
        }
    }

    private IEnumerator DeadRoutine()
    {
        AnimPlay("death");
        agent.speed = 0;

        yield return new WaitForSeconds(2f);

        transform.position = agentStartPosition;
        agent.speed = 10;
    }

    IEnumerator ObstacleHit()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(0.3f);
        agent.enabled = true;
        agent.SetDestination(target.transform.position);
    }

    public void OpponentsFallReturn()
    {
        if (transform.position.y < -7f)
        {
            transform.position = agentStartPosition;
            transform.eulerAngles = agentStartPosition;
        }
    }

}
