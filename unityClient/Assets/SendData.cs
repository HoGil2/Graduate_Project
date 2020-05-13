using System.Collections;
using UnityEngine;
using SocketIO;
using System.Collections.Generic;


public class SendData : MonoBehaviour
{
    private SocketIOComponent socket;
   // Dictionary<string, string> data = new Dictionary<string, string>();
    JSONObject jdata;

    public GameObject ball1;
    public GameObject ball2;
    public GameObject ball3;
    public GameObject que;

    private Transform ball1trans;
    private Transform ball2trans;
    private Transform ball3trans;
    private Transform quetrans;

    private float pos_x;
    private float pos_y;
    private float pos_z;


    private float rot_x;
    private float rot_y;
    private float rot_z;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        ball1trans = ball1.transform;
        ball2trans = ball2.transform;
        ball3trans = ball3.transform;
        quetrans = que.transform;
        init();

        

        
    }


    // Update is called once per frame
    void Update()
    {

        GetFrame();
    }


    public void GetFrame()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("ball1pos_x", ball1trans.position.x.ToString());
        data.Add("ball1pos_y", ball1trans.position.y.ToString());
        data.Add("ball1pos_z", ball1trans.position.z.ToString());

        data.Add("ball1rot_x", ball1trans.rotation.x.ToString());
        data.Add("ball1rot_y", ball1trans.rotation.y.ToString());
        data.Add("ball1rot_z", ball1trans.rotation.z.ToString());

        data.Add("ball2pos_x", ball2trans.position.x.ToString());
        data.Add("ball2pos_y", ball2trans.position.y.ToString());
        data.Add("ball2pos_z", ball2trans.position.z.ToString());

        data.Add("ball2rot_x", ball2trans.rotation.x.ToString());
        data.Add("ball2rot_y", ball2trans.rotation.y.ToString());
        data.Add("ball2rot_z", ball2trans.rotation.z.ToString());

        data.Add("ball3pos_x", ball3trans.position.x.ToString());
        data.Add("ball3pos_y", ball3trans.position.y.ToString());
        data.Add("ball3pos_z", ball3trans.position.z.ToString());

        data.Add("ball3rot_x", ball3trans.rotation.x.ToString());
        data.Add("ball3rot_y", ball3trans.rotation.y.ToString());
        data.Add("ball3rot_z", ball3trans.rotation.z.ToString());

        data.Add("quepos_x", quetrans.position.x.ToString());
        data.Add("quepos_y", quetrans.position.y.ToString());
        data.Add("quepos_z", quetrans.position.z.ToString());

        data.Add("querot_x", quetrans.rotation.x.ToString());
        data.Add("querot_y", quetrans.rotation.y.ToString());
        data.Add("querot_z", quetrans.rotation.z.ToString());

        

        jdata = new JSONObject(data);

        socket.Emit("SendDatabyUnity", jdata);
        
        


    }

    public void init()
    {
     pos_x = 0f; pos_y = 0f; pos_z = 0f;
     rot_x = 0f;  rot_y = 0f; rot_z = 0f;

}



}
