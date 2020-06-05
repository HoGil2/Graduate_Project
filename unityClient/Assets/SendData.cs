using System.Collections;
using UnityEngine;
using SocketIO;
using System.Collections.Generic;
using ScrollUI;
using UnityEngine.UI;

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

    private bool setting = false;

    // int count = 0;
    private float fDestroyTime = 2f;
    private float fTickTime;

    public Button button;
    public RectTransform Canvas;

    

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        ball1trans = ball1.transform;
        ball2trans = ball2.transform;
        ball3trans = ball3.transform;
        quetrans = que.transform;


        socket.On("SetReplayBoard", ReplayBoard);

        

        // 사용 변수 초기화
        Init();

        





    }


    // Update is called once per frame
    void Update()
    {
        // 서버 통신하는데 시간 딜레이가 있음
        fTickTime += Time.deltaTime;
        if (fTickTime >= fDestroyTime)
        {
            // 리플레이 화면 요청
            if (setting != true)
                RequestReplayByUnity();
            CreateReplay();
        }
        // 프레임 DB 전송
        // SendFrameByUnity();


    }


    public void SendFrameByUnity()
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

        socket.Emit("SendFrameByUnity", jdata);
        
        


    }
    public void CreateReplay()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("user_id", "junoung");

        jdata = new JSONObject(data);
        socket.Emit("CreateTable", jdata);


    }


    public void Init()
    {
     pos_x = 0f; pos_y = 0f; pos_z = 0f;
     rot_x = 0f;  rot_y = 0f; rot_z = 0f;

    }

    // 리플레이 화면 요청
    public void RequestReplayByUnity()
    {
        //SetReplayBoard 
       
        Debug.Log("리플레이 화면 요청");
        socket.Emit("RequestReplayByUnity");
        setting = true;
    }

    public void ReplayBoard(SocketIOEvent e)
    {

        InitForReplayBoard(e.data.list.Count, e.data.keys, e.data);

    }

    public void InitForReplayBoard(int len, List<string> keys, JSONObject values)
    {
        int yValue = 0;
        for (int i = 0; i < len; i++)
        {
            //item 초기화
            Button newItem = button;
            newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(Canvas.rect.width,30);

            Text title = newItem.GetComponentInChildren<Text>();
            title.text = keys[i] + "\t\t\t" + "저장날짜" + values[i].ToString();
            

            var index = Instantiate(newItem, new Vector3(0, yValue, 0), Quaternion.identity);
            index.transform.SetParent(GameObject.Find("Content").transform);
            yValue -= 200;
        }

    }




}
