using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float currentColor_strengt;
    TextMesh text;

    void Start()
    {
        text = GetComponent<TextMesh>();
        Destroy(gameObject.transform.parent.gameObject, 2);
    }

    private void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, currentColor_strengt);
    }
}
