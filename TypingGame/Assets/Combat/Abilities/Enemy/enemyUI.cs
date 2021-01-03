using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyUI : MonoBehaviour
{
    public Slider slider;
    public Text _name, hp_text, defence, resistance;
    public bool showStats = true;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
    }

    
}
