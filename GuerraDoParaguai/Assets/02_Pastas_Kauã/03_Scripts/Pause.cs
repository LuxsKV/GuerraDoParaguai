using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pause, options, controls;

    int intPause = 0; // 0 = desable;  1 = pause; 2 = options; 3 = controls
    // Start is called before the first frame update
    void Start()
    {
        intPause = 0;
        pause.SetActive(false);
    }
    private void Update()
    {
        InPause();
        
        Debug.Log(intPause);
    }
    public void InPause()
    {
        if (Input.GetButtonDown("Cancel") && intPause == 0)
        {
            pause.SetActive(true);
            MouseLook.instance.UnLockCursor();
            intPause = 1;
        }
        else if (Input.GetButtonDown("Cancel") && intPause == 1)
        {
            intPause = 0;
            pause.SetActive(false);
            MouseLook.instance.LockCursor();
        }
        else if (Input.GetButtonDown("Cancel") && intPause == 2)
        {
            intPause = 1;
            options.SetActive(false);
            pause.SetActive(true);
        }
        else if (Input.GetButtonDown("Cancel") && intPause == 3)
        {
            intPause = 2;
            controls.SetActive(false);
            options.SetActive(true);
        }
    }
    public void OutPause()
    {
        if (intPause == 1)
        {
            intPause = 0;
            pause.SetActive(false);
            MouseLook.instance.LockCursor();
        }
    }
    public void InOptions()
    {
        if (intPause == 1)
        {
            options.SetActive(true);
            pause.SetActive(false);
            intPause = 2;
        }
    }
    public void OutOptions()
    {  
        if (intPause == 2)
        {
            intPause = 1;
            options.SetActive(false);
            pause.SetActive(true);
        }
    }
    public void InControls()
    {
        if (intPause == 2)
        {
            intPause = 3;
            options.SetActive(false);
            controls.SetActive(true);
        }
    }
    public void OutControls()
    {
        if (intPause == 3)
        {
            intPause = 2;
            controls.SetActive(false);
            options.SetActive(true);
        }
    }
}
