using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public List<GameObject> barrierPieces = new List<GameObject>();


    [SerializeField]
    private float barrierTotalHealth = 100f;

    private float healthPerPiece;

    private float pieceHealth;

    private int piecesRemoved;

    // Start is called before the first frame update
    void Start()
    {
        healthPerPiece = barrierTotalHealth / barrierPieces.Count;
        pieceHealth = barrierTotalHealth / barrierPieces.Count;
        piecesRemoved = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        pieceHealth -= damage;
        if(pieceHealth < 0f && barrierPieces.Count != 0)
        {
            DestroyPiece(barrierPieces[piecesRemoved]);
            pieceHealth = healthPerPiece - (-1f * pieceHealth);
            piecesRemoved++;

        }
        else if(pieceHealth == 0f && barrierPieces.Count != 0)
        {
            DestroyPiece(barrierPieces[piecesRemoved]);
            pieceHealth = healthPerPiece;
            piecesRemoved++;
        }

        if(barrierPieces.Count == 0)
        {
            //ZOMBIES GET THROUGH
        }

    }


    public void RepairDamage(float repairAmount)
    {
       
    } 



    private void DestroyPiece(GameObject piece)
    {

        piece.GetComponent<BoxCollider>().enabled = false;
        piece.GetComponent<MeshRenderer>().enabled = false;
    }

    private void RepairPiece(GameObject piece)
    {

        piece.GetComponent<BoxCollider>().enabled = true;
        piece.GetComponent<MeshRenderer>().enabled = true;
    }
}
