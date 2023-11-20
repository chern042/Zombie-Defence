using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrierAnimation : MonoBehaviour
{

    public Rigidbody[] barrierLeft;
    public Rigidbody[] barrierRight;

    public float throwStrength = 20f;


    private Vector3[] originalPosLeft;
    private Vector3[] originalPosRight;

    private Quaternion[] originalRotLeft;
    private Quaternion[] originalRotRight;


    private Collider[] barrierLeftCollider;
    private Collider[] barrierRightCollider;

    private int barrierCount;
    private int logToThrow;

    // Start is called before the first frame update
    void Awake()
    {
        logToThrow = 0;
        barrierCount = (barrierLeft.Length == barrierRight.Length) ? barrierLeft.Length : 0;
        if (barrierCount == 0) return;
        originalPosLeft = new Vector3[barrierCount];
        originalPosRight = new Vector3[barrierCount];

        originalRotLeft = new Quaternion[barrierCount];
        originalRotRight = new Quaternion[barrierCount];

        barrierLeftCollider = new Collider[barrierCount];
        barrierRightCollider = new Collider[barrierCount];

    }

    private void Start()
    {
        for (int i = 0; i < barrierCount; i++)
        {
            originalPosLeft[i] = barrierLeft[i].transform.position;
            originalPosRight[i] = barrierRight[i].transform.position;


            originalRotLeft[i] = barrierLeft[i].transform.rotation;
            originalRotRight[i] = barrierLeft[i].transform.rotation;

            barrierLeftCollider[i] = barrierLeft[i].gameObject.GetComponent<Collider>();
            barrierRightCollider[i] = barrierRight[i].gameObject.GetComponent<Collider>();
        }
    }



    public void ThrowLogs()
    {
        barrierLeft[logToThrow].velocity = Vector3.right * throwStrength;
        barrierRight[logToThrow].velocity = Vector3.right * throwStrength;
        logToThrow++;
        if(logToThrow+1 == barrierCount)
        {
            logToThrow = 0;
        }
    }

    public void ReturnLogs()
    {
        if (logToThrow != 0)
        {


            barrierLeftCollider[logToThrow].isTrigger = true;
            barrierRightCollider[logToThrow].isTrigger = true;
            barrierLeft[logToThrow].isKinematic = true;
            barrierRight[logToThrow].isKinematic = true;
            barrierLeft[logToThrow].velocity = Vector3.zero;
            barrierRight[logToThrow].velocity = Vector3.zero;

            barrierLeft[logToThrow].transform.position = originalPosLeft[logToThrow];
            barrierRight[logToThrow].transform.position = originalPosRight[logToThrow];


            barrierRight[logToThrow].transform.rotation = originalRotLeft[logToThrow];
            barrierRight[logToThrow].transform.rotation = originalRotRight[logToThrow];

            while (barrierLeft[logToThrow].transform.rotation != originalRotLeft[logToThrow])
            {
                barrierLeft[logToThrow].transform.rotation = originalRotLeft[logToThrow];
            }


            while (barrierRight[logToThrow].transform.rotation != originalRotRight[logToThrow])
            {
                barrierRight[logToThrow].transform.rotation = originalRotRight[logToThrow];
            }

            barrierLeftCollider[logToThrow].isTrigger = false;
            barrierRightCollider[logToThrow].isTrigger = false;
            logToThrow--;
        }

    }

}
