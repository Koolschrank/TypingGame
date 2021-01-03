using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void set_active()
    {
        transform.localScale = originalScale;
    }

    public void Initiate_Abilities()
    {
        FindObjectOfType<BattleSystem>().Initiate_Abilities();
        
    }
}
