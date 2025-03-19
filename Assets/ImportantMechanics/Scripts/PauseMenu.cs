using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public PauseMenu pausemenu;

    private void Start()
    {
        {

        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pausemenu.gameObject.SetActive(!pausemenu.gameObject.SetActive);
        }
    }

}
