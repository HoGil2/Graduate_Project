using Mybilliards.Stolzenberg.Events;
using Mybilliards.Stolzenberg.Models;
using UnityEngine;

namespace Mybilliards.Stolzenberg.Controllers
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        internal BallModel BallModel { get { return ballModel; } }

        [Header("Models")]
        [SerializeField] private BallModel ballModel;
        [Header("Controllers")]
        [SerializeField] private AudioController audioController;
        [Header("Components")]
        [SerializeField] private SphereCollider sphereCol;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private Transform viewTransform;
        [Header("Events")]
        [SerializeField] private GameEvent ballHitBall;
        [SerializeField] private GameEvent ballHitWall;

        
        private Vector3 lastFrameVelocity; //최근 프레임에서 확인된 공의 속도

        private void Awake()
        {

            rigid.drag = BallModel.Drag; 
        }

        private void Update()
        {
         
            CheckIfMoving();
            lastFrameVelocity = rigid.velocity;
        
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 부딪힌 충돌체의 태그가 Wall 이라면
            if (collision.transform.CompareTag("Wall"))
            {
                ballHitWall.Raise();
                if (audioController != null)
                {
                    audioController.PlayHitWallClip(collision.relativeVelocity.magnitude);
                }
                Bounce(collision.contacts[0].normal);
              
            }
            if (collision.transform.CompareTag("Ball"))
            {
                ballHitBall.Raise();
                if (audioController != null)
                {
                    audioController.PlayHitBallClip(collision.relativeVelocity.magnitude);
                }
            }
        }

        internal void ApplyForceToBall(Vector3 direction, float velocity)
        {

            
            rigid.AddForce(direction * velocity, ForceMode.Impulse);
            
           
        }

        internal void Bounce(Vector3 collisionNormal)
        {

            var newSpeed = lastFrameVelocity.magnitude / 1;
            var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

            rigid.velocity = direction * newSpeed;
        }

        private void CheckIfMoving()
        {

         
            // magnitude: RigidBody의 속도(vector3인 velocity의 크기)  <==== 0.05 보다 작으면 멈춘것으로 인식
            if (rigid.velocity.magnitude <= BallModel.MoveThreshHold)
            {
                ballModel.IsMoving = false;
                if(rigid.velocity.magnitude<1)
                    rigid.drag = 0.6f;
                
            }
            else
            {
                ballModel.IsMoving = true;
                rigid.drag = 0.2f;
            }

            
        }        
        
    }
}