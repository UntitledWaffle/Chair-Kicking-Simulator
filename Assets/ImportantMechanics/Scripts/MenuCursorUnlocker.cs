using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursorUnlocker : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
