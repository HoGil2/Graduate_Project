#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;
using System.Collections.Generic;


public class TestSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;
    Dictionary<string, string> data = new Dictionary<string, string>();
    JSONObject jdata;
    string note;
    public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("boop", TestBoop);
		socket.On("error", TestError);
		socket.On("close", TestClose);
        //        socket.On("SendMessageByNode", SendMessageByNode);
        data.Add("Name", "monkey");
        data.Add("id", "monkey5255");

        jdata = new JSONObject(data);

        note = "테스트입니다.";

        StartCoroutine("BeepBoop");
        
	}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            socket.Emit("test",JSONObject.CreateStringObject(note));
            
            
            Debug.Log("[SocketIO] Send Message to Server...");
        }
    }

    private IEnumerator BeepBoop()
	{
		// wait 1 seconds and continue
		yield return new WaitForSeconds(1);
		
		socket.Emit("beep");
		
		// wait 3 seconds and continue
		yield return new WaitForSeconds(3);
		
		socket.Emit("beep");
		
		// wait 2 seconds and continue
		yield return new WaitForSeconds(2);
		
		socket.Emit("beep");
		
		// wait ONE FRAME and continue
		yield return null;
		
		socket.Emit("beep");
		socket.Emit("beep");
	}


    private void SendMessageByNode(SocketIOEvent e)
    {
        //name : 이벤트 이름, data : JSON Data
        Debug.Log("[SocketIO] Received Data from Node: {" + e.name + " : " + e.data + "}");

        data = e.data.ToDictionary();

        string tmp;

        //딕셔너리로 변환한 값의 필드로 값을 찾아 출력한다.
        data.TryGetValue("name", out tmp);

        Debug.Log(tmp);

        if (e.data.ToDictionary().ContainsKey("name")) Debug.Log("name is here!!");
        else Debug.Log("name is not here!!");
    }
 




    public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	

	public void TestBoop(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		Debug.Log(
			"#####################################################" +
			"THIS: " + e.data.GetField("this").str +
			"#####################################################"
		);
	}
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
