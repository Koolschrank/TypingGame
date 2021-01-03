using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractText : MonoBehaviour
{
    public GameObject interactTextPrefab;
    GameObject sceneText;
    public string text;
    public bool showSpawnText;

    // Start is called before the first frame update
    void Start()
    {
        if (showSpawnText) SpawnText(Vector3.zero, text);
    }

    public void SpawnText(Vector3 posistion, string name)
    {
        GameObject interactText = Instantiate(interactTextPrefab, posistion, Quaternion.identity) as GameObject;
        interactText.transform.SetParent(gameObject.transform);
        interactText.GetComponent<TextPopUp>().SetText(text, gameObject);
        sceneText = interactText;
    }

    // Update is called once per frame
    void Update()
    {
        //var pos = gameObject.ge
    }
}
