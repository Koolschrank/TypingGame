using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackTransition : MonoBehaviour
{
    public float closePosition, openPosition, transitionTime;
    float currentPosition,startTime, endTime;
    state currentState = state.closed;

    // Start is called before the first frame update
    void Start()
    {
    }

    public state GetState()
    {
        return currentState;
    }

    private void SetXPosition()
    {
        if(currentState == state.open_backpack)
        {
            currentPosition = easingFunction(((Time.timeSinceLevelLoad - startTime) / (endTime - startTime)));
            var distance = Mathf.Abs(closePosition - openPosition);
            var current_distance = currentPosition * distance;
            //Debug.Log(distance);

            transform.localPosition = new Vector2(closePosition +current_distance, transform.localPosition.y);
            //if (current_distance >= distance)
            //{
            //    currentState = state.opend;
            //}
        }
        else if (currentState == state.close_backpack)
        {
            currentPosition = easingFunctionrevese(((Time.timeSinceLevelLoad - startTime) / (endTime - startTime)));
            var distance = Mathf.Abs(closePosition - openPosition);
            var current_distance = currentPosition * distance;
            transform.localPosition = new Vector2(openPosition - current_distance, transform.localPosition.y);
            //if(current_distance>= distance)
            //{
            //    currentState = state.closed;
            //}
        }
        //else if (currentState == state.closed)
        //{
        //    if (Input.GetKeyDown("r"))
        //    {
        //        Debug.Log("letsGo");
        //        StartTimer(state.open_backpack);
        //    }
        //}
        //else if (currentState == state.opend)
        //{
        //    if (Input.GetKeyDown("r"))
        //    {
        //        StartTimer(state.close_backpack);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        SetXPosition();
    }

    public void TransitionNewState(float transitionTime,state oldState, state newState)
    {
        StartCoroutine(TransitionTime( transitionTime, oldState, newState));
    }

    IEnumerator TransitionTime(float transitionTime, state oldState, state newState)
    {
        Debug.Log(newState);
        yield return new WaitForSeconds(transitionTime);
        if(currentState == oldState)
        currentState = newState;
    }

    private float easingFunction(float x)
    {
        return 1 - Mathf.Pow(1 - x, 4);
    }

    private float easingFunctionrevese(float x)
    {
        return x * x;
    }

    public void StartTimer(state new_state)
    {
        endTime = transitionTime + Time.timeSinceLevelLoad;
        startTime = Time.timeSinceLevelLoad;
        currentState = new_state;

        switch(new_state)
        {
            case state.close_backpack:
                TransitionNewState(transitionTime,new_state, state.closed);
                break;
            case state.open_backpack:
                TransitionNewState(transitionTime, new_state, state.opend);
                break;
        }
        
    }
}
