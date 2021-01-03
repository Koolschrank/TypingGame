using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    public float currentColor_strengt =1f;
    public string _text;
    Animator _animator;
    TextMesh _textMesh;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _textMesh = GetComponentInChildren<TextMesh>();
        _textMesh.text = _text;
    }

    public void Update()
    {
        _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.g, _textMesh.color.b, currentColor_strengt);
        _textMesh.gameObject.transform.localScale = transform.parent.transform.localScale;

    }

    public void Set_active()
    {

    }

    public void Set_Off()
    {
        _animator.Play("GoOut");
        Destroy(gameObject, 2f);
    }
}
