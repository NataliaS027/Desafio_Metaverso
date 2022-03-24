using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance { get; protected set; }
    public GameObject Camera, QRCode;
    public GameObject cameraObject, imageObject;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Instance.Camera.active == true && cameraObject.transform.localScale.x < 5)
        {
            cameraObject.transform.localScale = new Vector2(cameraObject.transform.localScale.x + 0.08f, cameraObject.transform.localScale.x + 0.08f);
        }
    }
}
