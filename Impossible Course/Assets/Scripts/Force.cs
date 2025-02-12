using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Force : MonoBehaviour
{
    [SerializeField] float force;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerTagTwo>(out PlayerTagTwo playerTagTwo))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * force);
        }
    }
}
