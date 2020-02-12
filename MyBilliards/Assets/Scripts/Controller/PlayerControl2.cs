using System.Collections;
using System.Collections.Generic;
using Mybilliards.Stolzenberg.Models;
using Mybilliards.Stolzenberg.Events;
using Mybilliards.Stolzenberg.Variables;

using UnityEngine;

namespace Mybilliards.Stolzenberg.Controllers
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerControl2 : MonoBehaviour
    {
        [Header("Models")]
        [SerializeField] private PlayerModel playerModel;
        [Header("Controllers")]
        [SerializeField] private CameraController camController;
        [SerializeField] private BallController ballController;
        [SerializeField] private AudioController audioController;
        [SerializeField] private RewindController rewindController;
        [SerializeField] private BallController[] ballsOnField;
        [Header("Components")]
        [SerializeField] private SphereCollider sphereCol;
        [Header("Events")]
        [SerializeField] private GameEvent playerSwingQueue;
        [Header("References")]
        [SerializeField] private FloatReference pressedTimeReference;
        [SerializeField] private IntReference shotsReference;



        private void Awake()
        {
            pressedTimeReference.Value = 0;
            shotsReference.Value = 0;
        }

        private void Update()
        {
            GetPressedTime();
            ApplyForceToBall();
            RotateBallWithCam();
            UpdateMouseState();
        }

        private void GetPressedTime()
        {
            if (Input.GetKey(KeyCode.Space))
            {
               
                if (IsAnyBallMoving() || rewindController.IsRewinding) return;

                if (pressedTimeReference.Value <= 1)
                {
                    pressedTimeReference.Value += playerModel.TimeToFullPower * Time.deltaTime;
                }
            }
            else
            {
                if (pressedTimeReference.Value >= 0)
                {
                    pressedTimeReference.Value -= playerModel.TimeToFullPower * Time.deltaTime;
                }
            }

            pressedTimeReference.Value = Mathf.Clamp(pressedTimeReference.Value, 0, 1);
        }

        private void ApplyForceToBall()
        {
            
            Vector3 force =new Vector3(transform.forward.x,transform.forward.y,transform.forward.z);

            if (Input.GetKeyUp(KeyCode.Space))
            {
                
                if (IsAnyBallMoving() || rewindController.IsRewinding) return;

                
                ballController.ApplyForceToBall(camController.transform.forward, playerModel.Force * Mathf.Clamp(pressedTimeReference.Value, 0, playerModel.TimeToFullPower));

                if (audioController != null)
                {
                    audioController.PlaySwingQueueClip(pressedTimeReference.Value);
                }

                shotsReference.Value++;
                pressedTimeReference.Value = 0;

                playerSwingQueue.Raise();
            }
        }

        private void RotateBallWithCam()
        {
            if (!ballController.BallModel.IsMoving)
            {
                Vector3 rotation = camController.transform.eulerAngles;
               // transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotation.y, transform.eulerAngles.z);
                //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rotation.y, transform.eulerAngles.z);

            }
            
        }

        private void UpdateMouseState()
        {
            Cursor.visible = !ballController.BallModel.IsMoving;
        }

        private bool IsAnyBallMoving()
        {
            for (int i = 0; i < ballsOnField.Length; ++i)
            {
                if (ballsOnField[i].BallModel.IsMoving)
                {
                    return true;
                }
            }

            return false;
        }

        
    }
}
