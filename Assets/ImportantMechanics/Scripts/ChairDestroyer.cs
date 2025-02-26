using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject)
        {
            Destroy(other.gameObject);
            print("Hit Barrier");
        }
    }
}