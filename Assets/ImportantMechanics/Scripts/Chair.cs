using UnityEngine;

public class Chair : MonoBehaviour
{
    private void OnDestroy()
    {
        if (FindObjectOfType<ObjectCounter>() != null)
        {
            FindObjectOfType<ObjectCounter>().RemoveObject();
        }
    }
}
