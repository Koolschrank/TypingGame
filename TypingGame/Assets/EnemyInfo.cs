using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    public float new_visibility;
    float _visibility;
    public Text fire, ice, thunder;
    bool _active;
    Animator a;
    Image image;


    public float visibility
    {
        get { return _visibility; }
        set
        {
            _visibility = value;
            image.color = new Color(image.color.r, image.color.g, image.color.b, _visibility);
            fire.color = new Color(fire.color.r, fire.color.g, fire.color.b, _visibility);
            ice.color = new Color(ice.color.r, ice.color.g, ice.color.b, _visibility);
            thunder.color = new Color(thunder.color.r, thunder.color.g, thunder.color.b, _visibility);
        }
    }

    public void Start()
    {
        
        a = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if(new_visibility!= _visibility)
        {
            visibility = new_visibility;
        }
    }

    public void SetInfo(EnemyStats enemy)
    {
        if (_active) return;
        _active = true;

        a.Play("in");
        fire.text = "";
        ice.text = "";
        thunder.text = "";
        foreach(ElementalEffectiveness element in enemy.elements)
        {
            if(element.element == Element.Fire)
            {
                SetText(fire, element);
            }
            else if (element.element == Element.Ice)
            {
                SetText(ice, element);
            }
            else if (element.element == Element.Thunder)
            {
                SetText(thunder, element);
            }
        }
    }

    public void GoBack()
    {
        if (!_active) return;
        _active = false;

        a.Play("out");
    }

    public void SetText(Text _text, ElementalEffectiveness element)
    {
        switch (element.effect)
        {
            case Effectiveness.normal:
                _text.text = "";
                break;
            case Effectiveness.resistent:
                _text.text = "Resistent";
                _text.color = Color.black;
                break;
            case Effectiveness.immun:
                _text.text = "Immun";
                _text.color = Color.black;
                break;
            case Effectiveness.absorb:
                _text.text = "Absorb";
                _text.color = Color.green;
                break;
            case Effectiveness.weak:
                _text.text = "Weak";
                _text.color = Color.red;
                break;
        }
    }
}
