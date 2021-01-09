using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCam : MonoBehaviour
{
    Vector3 startPosition;
    public Transform followObject;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followObject.position.x + startPosition.x, followObject.position.y + startPosition.y, followObject.position.z + startPosition.z);
    }
}
