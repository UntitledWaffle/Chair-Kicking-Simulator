using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject FirstPersonController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            GetComponent<FirstPersonController>().enabled = false;
        }
    }
}
