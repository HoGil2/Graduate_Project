using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    //https://answers.unity.com/questions/246709/rotate-around-the-center-of-the-world-with-mouse.html
    private Transform target;
    private float distance = 0.3f;
    private float DISTMIN = 0.0f;
    private float DISTMAX = 1.0f;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private float startingDistance = 0.0f;
    private float desiredDistance = 0.0f;
    private float XMOUSESENSITIVITY = 5.0f;
    private float YMOUSESENSITIVITY = 5.0f;
    private float MOUSEWHEELSENSITIVITY = 5.0f;
    private float YMINLIMIT = 0.0f;
    private float YMAXLIMIT = 80.0f;
    private float DISTANCESMOOTH = 0.05f;
    private float velocityDistance = 0.0f;
    private Vector3 desiredPosition = Vector3.zero;

    private float XSMOOTH = 0.05f;
    private float YSMOOTH = 0.1f;
    private float velX = 0.0f;
    private float velY = 0.0f;
    private float velZ = 0.0f;
    private Vector3 position = Vector3.zero;

    private float moveSpeed = 0.1f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("ball1").transform;

        distance = Mathf.Clamp(distance, DISTMIN, DISTMAX);
        startingDistance = distance;
        Reset();
    }

    void Update()
    {

    }

    void MoveCtrl()
    {
        //Vector3 heading = this.transform.position - target.position;

        //키보드 W,S,A,D Player 이동 함수
        //if (Input.GetKey(KeyCode.W))
        //{
        //    this.transform.Translate(heading.normalized * moveSpeed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    this.transform.Translate(-heading.normalized * moveSpeed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        //}

    }

    void LateUpdate()
    {
        if (!target)
            return;

        HandlePlayerInput();

        CalculateDesiredPosition();

        UpdatePosition();
    }

    void HandlePlayerInput()
    {
        float deadZone = 0.01f;
        Vector3 heading = this.transform.position - target.transform.position;

        if (Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X") * XMOUSESENSITIVITY;
            mouseY -= Input.GetAxis("Mouse Y") * YMOUSESENSITIVITY;
        }

        // this is where the mouseY is limited - Helper script
        mouseY = ClampAngle(mouseY, YMINLIMIT, YMAXLIMIT);

        // get Mouse Wheel Input
        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(distance - (Input.GetAxis("Mouse ScrollWheel") * MOUSEWHEELSENSITIVITY),
                                                                                DISTMIN, DISTMAX);
        }
    }

    void CalculateDesiredPosition()
    {
        // Evaluate distance
        distance = Mathf.SmoothDamp(distance, desiredDistance, ref velocityDistance, DISTANCESMOOTH);

        // Calculate desired position -> Note : mouse inputs reversed to align to WorldSpace Axis
        desiredPosition = CalculatePosition(mouseY, mouseX, distance);
    }

    Vector3 CalculatePosition(float rotationX, float rotationY, float distnace)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        return target.position + (rotation * direction);
    }

    void UpdatePosition()
    {
        var posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, XSMOOTH);
        var posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, YSMOOTH);
        var posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, XSMOOTH);
        position = new Vector3(posX, posY, posZ);

        transform.position = position;

        transform.LookAt(target);
    }

    void Reset()
    {
        mouseX = 0;
        mouseY = 10;
        distance = startingDistance;
        desiredDistance = distance;
    }

    float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360 || angle > 360)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }

    public void ShowOverheadView()
    {
        //firstPersonCamera.enabled = false;
        //overheadCamera.enabled = true;
    }

    public void ShowFirstPersonView()
    {
        //firstPersonCamera.enabled = true;
        //overheadCamera.enabled = false;
    }
}
