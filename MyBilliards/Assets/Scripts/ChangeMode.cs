using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMode : MonoBehaviour
{
    enum State { PLAY, CAMERA };
    GameObject PlayButton;
    GameObject CameraButton;
   

    // Start is called before the first frame update
    void Start()
    {
        PlayButton = GameObject.Find("PlayButton");
        CameraButton = GameObject.Find("CameraButton");
    }

    // Update is called once per frame
    void Update()

    {
        
    }

    public void SelcetedMode(string mode)
    {
        switch(mode) {
            case "Play":
                Debug.Log("Play button");
                break;
            case "Camera":
                Debug.Log("Camera button");
                break;
            default:
                Debug.Log("default");
                break;
        }

    }


}
