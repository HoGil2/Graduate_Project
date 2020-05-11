using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public GameObject que;
    public Transform target;
    private Vector3 CurrentPosition;
    private Vector3 MovePosition;


    public float sensitivityX = 0.0001F;


    /*void Awake()
     * 
    {
        initialZPos = followingCube.transform.position.y;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        que = GameObject.Find("GeoSphere004");
        target = GameObject.Find("GeoSphere001").transform;
     //   que.transform.position = new Vector3(target.transform.position.x+0.1f, target.transform.position.y+0.1f, target.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.A)) // x축 회전
            transform.RotateAround(target.transform.position,Vector3.left , sensitivityX);

        else if (Input.GetKey(KeyCode.W)) // x축 회전
            transform.RotateAround(target.transform.position, Vector3.up, sensitivityX);

        else if (Input.GetKey(KeyCode.D)) // y축 회전
            transform.RotateAround(target.transform.position, Vector3.right, sensitivityX);

        else if (Input.GetKey(KeyCode.S)) // y축 회전
            transform.RotateAround(target.transform.position, Vector3.down, sensitivityX);

        else if (Input.GetMouseButton(0))
        {
            
            if (CurrentPosition != null)
            {
                MovePosition = CurrentPosition - Input.mousePosition;
                if (Mathf.Abs(MovePosition.x) > 500 || Mathf.Abs(MovePosition.y) > 500)
                    return;
                Debug.Log("CurrentPosition: " + CurrentPosition + ", Input.mousePosition: " + Input.mousePosition);
                Debug.Log(MovePosition);

                Vector3 heading = que.transform.position - target.position;
                //var distance = heading.magnitude;
                //Vector3 direction = heading / distance;
                //Quaternion dir = Quaternion.Euler(heading / distance);

                if (MovePosition.y < 0)
                    que.transform.Translate( heading.normalized * Time.deltaTime * 1f);
                else if(MovePosition.y > 0)
                    que.transform.Translate( -heading.normalized * Time.deltaTime * 1f);
            }
            CurrentPosition = Input.mousePosition;
           
        }
            
        
        que.transform.LookAt(target);
        /*
        if (CurrentPosition != null)
        {
            MovePosition = CurrentPosition - Input.mousePosition;
            if (Mathf.Abs(MovePosition.x) > 500 || Mathf.Abs(MovePosition.y) > 500)
                return;
            Debug.Log("CurrentPosition: "+ CurrentPosition+ ", Input.mousePosition: "+ Input.mousePosition);
            Debug.Log(MovePosition);
            que.transform.Translate(0, MovePosition.y * 0.001f, MovePosition.z * 0.001f);
        }
        CurrentPosition = Input.mousePosition;
        */
    }
}
