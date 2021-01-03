using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PublicVaribles")]
public class PublicVaribles : ScriptableObject
{
    public Vector2[] buff_text_positions;
    public GameObject StatText, weakText;
    public Color downColor;
    public ParticleSystem bodySwapPartical;
}
