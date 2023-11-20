using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrierAnimation : MonoBehaviour
{

    public Rigidbody barrierLeft;
    public Rigidbody barrierRight;


    private Transform originalPosLeft;
    private Transform originalPosRight;


    // Start is called before the first frame update
    void Start()
    {
        originalPosLeft = barrierLeft.transform;
        originalPosRight = barrierRight.transform;
    }



    public void ThrowLogs()
    {

        barrierLeft.velocity = Vector3.right * 20f;
        barrierRight.velocity = Vector3.right * 20f;
    }

    public void ReturnLogs()
    {
        barrierLeft.transform.position = originalPosLeft.position;
        barrierLeft.transform.rotation = originalPosLeft.rotation;
        barrierRight.transform.position = originalPosRight.position;
        barrierRight.transform.rotation = originalPosRight.rotation;
    }

}
