﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueCollision : MonoBehaviour
{
    bool touchTimeTrigger;
    bool calTrigger;
    float touchTime;
    float FORCECONSTONE;
    float FORCECONSTTWO;
    Rigidbody rb;
    float mass;
    Vector3 heading;
    Vector3 contactPos;
    public GameObject que;
    // Start is called before the first frame update
    void Start()
    {
        touchTimeTrigger = false;
        calTrigger = false;
        touchTime = 0.0f;
        FORCECONSTONE = 0.2f;
        FORCECONSTTWO = 0.3f;
        rb = GameObject.FindGameObjectWithTag("ball1").GetComponent<Rigidbody>();
        mass = rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(calTrigger == true)
        {
            Debug.Log("Calculate Velocity and Rotation with TochTime: " 
                + touchTime);
            CalculateVelRot();
            calTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("cue"))
        {
            Debug.Log("Que Entered");
        }
        touchTimeTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("cue"))
        {
            if (touchTimeTrigger == true)
            {
                Debug.Log("Que Stayed");
                // touchTime 확인필요
                touchTime += 0.05f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("cue"))
        {
            Debug.Log("Que Exited");
            touchTimeTrigger = false;
        }
        calTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "cue")
        {
            contactPos = collision.contacts[0].point;
            //Debug.Log("Que Entered: " + contactPos);
            
            touchTimeTrigger = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "cue")
        {
            if (touchTimeTrigger == true)
            {
                //Debug.Log("Que Stayed");
                // touchTime 확인필요
                touchTime += 0.05f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cue")
        {
            //Debug.Log("Que Exited");
            touchTimeTrigger = false;
        }

        calTrigger = true;
    }

    private void CalculateVelRot()
    {
        // 큐로부터 힘의 크기 0~3으로 정규화해서 가져오기
        // 임의값으로 지정
        float force = 300.0f;
        float impulse = force * touchTime;
        float power = FORCECONSTONE * force + FORCECONSTTWO * force * Mathf.Pow(touchTime, 2.0f);
        //float Xvelocity = power / mass;
        heading = que.transform.position - transform.position;
        rb.AddForce(heading.normalized * power * Time.deltaTime);
        //float Yrotation = power * 중심으로부터 당점 거리 / impulse;
        //float Xrotation = power * 중심으로부터 당점 거리 / impulse;
    }
}
