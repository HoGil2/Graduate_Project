using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SpeedCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;
    private Rigidbody rigid;

    private bool check;
    private StreamWriter sw;

    // Start is called before the first frame update
    void Start()
    {
        check = false;
        rigid = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
          Speedcheck();
    }


    private void Speedcheck()
    {

        //FileStream fs = File.Create("speed.txt");
        if(check == false) { 
            sw = new StreamWriter("speed.txt", false);
            check = true;
        }
        //   string text =Time.time + " " + rigid.velocity.magnitude.ToString("F6");
        string text = Time.time.ToString("F1") + " " + (rigid.velocity.magnitude).ToString("F3");
       // if (rigid.velocity.magnitude!=0.0f)
               sw.WriteLine(text);
        Debug.Log((rigid.velocity.magnitude * 100).ToString("F1"));

    }

}
