using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaffleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);

        if (other.CompareTag("Chair"))
        {
            Destroy(other.gameObject);
        }
    }
}
