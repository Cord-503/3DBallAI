using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfPlayerInSight : MonoBehaviour
{
    [SerializeField] private AiMoving _enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _enemy.DetectPlayer(other.transform);

        }
    }
}
