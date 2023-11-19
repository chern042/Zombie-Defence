using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayWall : MonoBehaviour
{



    [Header("Selective Wall Collider For Invisible Player Walls")]

    [Tooltip("String that represents the tag for the game object that CAN go through this wall.")]
    [SerializeField]
    public string canGoThroughWall = "Zombie";


    [Tooltip("String that represents the tag for the game object that CAN NOT go through this wall.")]
    [SerializeField]
    public string cantGoThroughWall = "GameController";

    private BoxCollider collider;

    private void Awake()
    {
        collider = gameObject.GetComponent<BoxCollider>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(canGoThroughWall != null)
        {
            if (other.gameObject.CompareTag(canGoThroughWall)){
                collider.isTrigger = true;
            }
            if (other.gameObject.CompareTag(cantGoThroughWall))
            {
                collider.isTrigger = false;
            }
        }
    }
}
