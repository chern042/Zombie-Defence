using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{

    [SerializeField]
    private float playerHealth = 100f;

    private bool playerAlive;
    public bool PlayerAlive { get => playerAlive; }

    // Start is called before the first frame update
    void Start()
    {
        playerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(float damage)
    {
        playerHealth -= damage;
        if(playerHealth <= 0)
        {
            Debug.Log("PLAYER DEAD");
            playerAlive = false;
        }
    }
 
}
