using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaffleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            Destroy(other.gameObject);
        } else if (other.CompareTag("Destructible"))
        {
            Destroy(other.gameObject);
        }
    }
}
