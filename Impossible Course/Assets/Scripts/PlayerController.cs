using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;

    public float speed = 10;

    public void AnimPlay(string animName)
    {
        _animator.SetBool("run", false);
        _animator.SetBool("idle", false);
        _animator.SetBool(animName, true);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * speed, _rigidbody.velocity.y, _joystick.Vertical * speed);

        Vector3 direction = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        if (direction != Vector3.zero)
        {
            // Y�nlendirme i�lemi sadece direction s�f�r de�ilse yap�l�r
            transform.rotation = Quaternion.LookRotation(direction);
            AnimPlay("run");
        }
        else
        {
            AnimPlay("idle");
        }
    }
}
