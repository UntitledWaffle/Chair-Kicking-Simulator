using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject) // Check if something entered the trigger
        {
            print("Hit Barrier");

            if (other.CompareTag("Chair")) 
            {
                ObjectCounter.instance.RemoveObject(); 
            }

            Destroy(other.gameObject); 
        }
    }
}
