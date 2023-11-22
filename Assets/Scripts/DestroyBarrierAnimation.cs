using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrierAnimation : MonoBehaviour
{

    public Rigidbody[] barrierLeft;
    public Rigidbody[] barrierRight;

    public float throwStrength = 2f;


    private Vector3[] originalPosLeft;
    private Vector3[] originalPosRight;

    private Quaternion[] originalRotLeft;
    private Quaternion[] originalRotRight;


    private Collider[] barrierLeftCollider;
    private Collider[] barrierRightCollider;

    private int barrierCount;

    // Start is called before the first frame update
    void Awake()
    {
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



    public void ThrowLogs(int logToThrowIndex)
    {
        if (logToThrowIndex > 1)
        {
            barrierLeftCollider[logToThrowIndex].isTrigger = true;
            barrierRightCollider[logToThrowIndex].isTrigger = true;
            barrierLeft[logToThrowIndex].isKinematic = false;
            barrierRight[logToThrowIndex].isKinematic = false;
            barrierLeft[logToThrowIndex].velocity = (Vector3.right * throwStrength);
            barrierRight[logToThrowIndex].velocity = (Vector3.right * throwStrength);
            barrierLeft[logToThrowIndex].AddForceAtPosition((Vector3.right * (throwStrength/16)),transform.position- (Vector3.forward * 3f),ForceMode.Force);
            barrierRight[logToThrowIndex].AddForceAtPosition((Vector3.left * (throwStrength/16)), transform.position - (Vector3.forward * 3f), ForceMode.Force);

        }else if(logToThrowIndex == 1)
        {
            barrierLeftCollider[logToThrowIndex].isTrigger = true;
            barrierRightCollider[logToThrowIndex].isTrigger = true;
            barrierLeft[logToThrowIndex].isKinematic = false;
            barrierRight[logToThrowIndex].isKinematic = false;
            barrierLeft[logToThrowIndex].velocity = (Vector3.right * throwStrength);
            barrierRight[logToThrowIndex].velocity = (Vector3.right * throwStrength);
            barrierLeft[logToThrowIndex].AddForceAtPosition((Vector3.right * (throwStrength / 8)), transform.position - (Vector3.forward * 3f), ForceMode.Force);
            barrierRight[logToThrowIndex].AddForceAtPosition((Vector3.left * (throwStrength / 8)), transform.position + (Vector3.forward * 3f), ForceMode.Force);
        }
        else if(logToThrowIndex == 0)
        {
            barrierLeftCollider[logToThrowIndex].isTrigger = true;
            barrierRightCollider[logToThrowIndex].isTrigger = true;
            barrierLeft[logToThrowIndex].isKinematic = false;
            barrierRight[logToThrowIndex].isKinematic = false;
        }

        
        //Invoke("MakeLogsInvisible", 5f);
        StartCoroutine(MakeLogsInvisible(logToThrowIndex));

    }






    IEnumerator MakeLogsInvisible(int index)
{
        yield return new WaitForSeconds(5f);

        barrierLeft[index].gameObject.GetComponent<MeshRenderer>().enabled = false;
        barrierRight[index].gameObject.GetComponent<MeshRenderer>().enabled = false;
        barrierLeftCollider[index].enabled = false;
        barrierRightCollider[index].enabled = false;
        barrierLeft[index].isKinematic = true;
        barrierRight[index].isKinematic = true;
    }

    public void ReturnLogs(int logToReturn)
    {

            barrierLeft[logToReturn].gameObject.GetComponent<MeshRenderer>().enabled = true;
            barrierRight[logToReturn].gameObject.GetComponent<MeshRenderer>().enabled = true;
            barrierLeftCollider[logToReturn].enabled = true;
            barrierRightCollider[logToReturn].enabled = true;

            barrierLeftCollider[logToReturn].isTrigger = true;
            barrierRightCollider[logToReturn].isTrigger = true;

            barrierLeft[logToReturn].isKinematic = true;
            barrierRight[logToReturn].isKinematic = true;
            barrierLeft[logToReturn].velocity = Vector3.zero;
            barrierRight[logToReturn].velocity = Vector3.zero;

            barrierLeft[logToReturn].transform.position = originalPosLeft[logToReturn];
            barrierRight[logToReturn].transform.position = originalPosRight[logToReturn];


            barrierRight[logToReturn].transform.rotation = originalRotLeft[logToReturn];
            barrierRight[logToReturn].transform.rotation = originalRotRight[logToReturn];

            while (barrierLeft[logToReturn].transform.rotation != originalRotLeft[logToReturn])
            {
                barrierLeft[logToReturn].transform.rotation = originalRotLeft[logToReturn];
            }


            while (barrierRight[logToReturn].transform.rotation != originalRotRight[logToReturn])
            {
                barrierRight[logToReturn].transform.rotation = originalRotRight[logToReturn];
            }

            barrierLeftCollider[logToReturn].isTrigger = false;
            barrierRightCollider[logToReturn].isTrigger = false;


    }

}
