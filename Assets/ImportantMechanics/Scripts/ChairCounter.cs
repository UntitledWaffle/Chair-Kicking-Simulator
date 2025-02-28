using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCounter : MonoBehaviour
{
    public static ObjectCounter instance;
    private int objectCount;
    public string nextSceneName; // Set this in the Inspector

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
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }
}
