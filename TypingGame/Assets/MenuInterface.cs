using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInterface : MonoBehaviour
{
    public Vector3 close_position, open_position;
    public float transitionTime =1f;
    float startTime;
    MainMenuOptiones status = MainMenuOptiones.closed;
    public CurrentMenu after_close, after_open;
    MainMenuAnimation MMA;

    private void Start()
    {
        MMA = FindObjectOfType<MainMenuAnimation>();
    }

    private void Update()
    {
        switch(status)
        {
            case MainMenuOptiones.opened:
                Opened();
                break;
            case MainMenuOptiones.closed:
                Closed();
                break;
            case MainMenuOptiones.in_closing:
                In_Closing();
                break;
            case MainMenuOptiones.in_opening:
                In_Opening();
                break;
        }

    }

    public void Opened()
    {

    }

    public void Closed()
    {

    }

    public void In_Closing()
    {
        if (Time.timeSinceLevelLoad - startTime / transitionTime >= 1)
        {
            ReachedClose();
            return;
        }
        transform.localPosition = Vector3.Lerp(open_position, close_position, easingFunction(Time.timeSinceLevelLoad - startTime / transitionTime));
    }

    public void In_Opening()
    {
        if (Time.timeSinceLevelLoad - startTime / transitionTime >= 1)
        {
            ReachedOpen();
            return;
        }
        transform.localPosition = Vector3.Lerp(close_position, open_position, easingFunction(Time.timeSinceLevelLoad - startTime / transitionTime));
    }

    public void SetClosing()
    {
        status = MainMenuOptiones.in_closing;
        startTime = Time.timeSinceLevelLoad;
    }

    public void SetOpening()
    {
        status = MainMenuOptiones.in_opening;
        startTime = Time.timeSinceLevelLoad;
    }

    public void ReachedClose()
    {
        status = MainMenuOptiones.closed;
        MMA.Signal(after_close);
    }

    public void ReachedOpen()
    {
        status = MainMenuOptiones.opened;
        MMA.Signal(after_open);
    }

    private float easingFunction(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }


}
