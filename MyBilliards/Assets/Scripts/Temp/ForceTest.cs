using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mybilliards.Stolzenberg.Test {

    public class ForceTest : MonoBehaviour

    {
        Rigidbody rigid;


        // Start is called before the first frame update
        void Start()
        {
            rigid = this.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                rigid.AddForce(transform.forward * 1 * 1, ForceMode.Impulse);
        }


        void ApplyForceToBall(Vector3 direction, float velocity)
        {
            rigid.AddForce(direction * velocity, ForceMode.Impulse);

        }
    }

    

}
