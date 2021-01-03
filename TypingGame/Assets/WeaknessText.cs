using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessText : MonoBehaviour
{
    public float currentColor_strengt = 1f;
    Animator _animator;
    public TextMesh _textMesh;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.g, _textMesh.color.b, currentColor_strengt);

    }

    public void Set_String(string _string)
    {
        
        _textMesh.text = _string;
    }
}
