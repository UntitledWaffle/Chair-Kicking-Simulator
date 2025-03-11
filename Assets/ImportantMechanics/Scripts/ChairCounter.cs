using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCounter : MonoBehaviour
{
    private int chairCount;
    public string nextSceneName; // Set this in the Inspector

    private void Start()
    {
        CountObjects();
    }

    public void CountObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Chair");
        chairCount = objects.Length;
        Debug.Log("Total chairs in this level: " + chairCount);
    }

    public void RemoveObject()
    {
        chairCount--;

        Debug.Log("Chairs remaining: " + chairCount);

        if (chairCount <= 0)
        {
            Debug.Log("All chairs eliminated! Loading next level...");
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
