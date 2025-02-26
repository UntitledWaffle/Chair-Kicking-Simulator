using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCounter : MonoBehaviour
{
    public static ObjectCounter instance;
    private int objectCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CountObjects();
    }

    public void CountObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Chair");
        objectCount = objects.Length;
    }

    public void RemoveObject()
    {
        objectCount--;

        if (objectCount <= 0)
        {
            Debug.Log("You Win!");
        }
    }
}