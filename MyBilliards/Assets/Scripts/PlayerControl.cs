using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public Camera player;
    private Vector3 currentMousePosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Camera>();

    }

    // Update is called once per frame

    void Update() // Using Keyboard for Camera Moving
    {
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(Vector3.left * 3 * Time.deltaTime);
         //   Debug.Log("A KEY PRESS");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(Vector3.right * 3 * Time.deltaTime);
         //   Debug.Log("D KEY PRESS");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.up * 3 * Time.deltaTime);
        //    Debug.Log("D KEY PRESS");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(Vector3.down * 3 * Time.deltaTime);
        //    Debug.Log("D KEY PRESS");
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            player.transform.Translate(Vector3.forward * 3 * Time.deltaTime);
         //   Debug.Log("Q KEY PRESS");
        }
        else if (Input.GetKey(KeyCode.E))
        {
            player.transform.Translate(Vector3.back * 3 * Time.deltaTime);
        //    Debug.Log("E KEY PRESS");
        }
        else if (Input.GetMouseButton(0)) // mouse left click
        {
         //   Debug.Log("MOUSE 0");
        }
        else if (Input.GetMouseButton(1)) // mouse right click
        {
        //    Debug.Log("MOUSE 1");
        }
        else if (Input.GetMouseButton(2)) // mouse Wheel click
        {
            /*
            Debug.Log("MOUSE 2");
            if (currentMousePosition != null) {
                currentMousePosition -= Input.mousePosition;
               
                    player.transform.eulerAngles = new Vector3(currentMousePosition.x, currentMousePosition.y, currentMousePosition.z);
                    Debug.Log("Rotation!!");
                 //   Debug.Log("MOUSE POSITION: "+Input.mousePosition.x+", "+Input.mousePosition.y+", "+Input.mousePosition.z);
                
            }
            currentMousePosition = Input.mousePosition;
            */
        }




    }


}