using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityIcon : MonoBehaviour
{
    public float currentColor_strengt = 1f;
    public SpriteRenderer sprite;
    public TextMesh textMesh;


    private void Update()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, currentColor_strengt);
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, currentColor_strengt);
        textMesh.gameObject.GetComponent<Renderer>().sortingLayerName = "UI";
    }

    public void SetIcon(string name, Sprite sprite)
    {
        textMesh.text = name;
        this.sprite.sprite = sprite;
    }


}
