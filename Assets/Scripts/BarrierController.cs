using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public List<GameObject> barrierPiecesLeft = new List<GameObject>();
    public List<GameObject> barrierPiecesRight = new List<GameObject>();


    [SerializeField]
    public float barrierTotalHealth = 100f;

    private float healthPerPiece;

    private float pieceHealth;

    [HideInInspector]
    public int piecesRemoved;

    // Start is called before the first frame update
    void Start()
    {
        healthPerPiece = barrierTotalHealth / barrierPiecesLeft.Count;
        pieceHealth = barrierTotalHealth / barrierPiecesLeft.Count;
        piecesRemoved = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        pieceHealth -= damage;
        if(pieceHealth < 0f && piecesRemoved != barrierPiecesLeft.Count && piecesRemoved != barrierPiecesRight.Count)
        {
            DestroyPiece(barrierPiecesLeft[piecesRemoved]);
            DestroyPiece(barrierPiecesRight[piecesRemoved]);

            pieceHealth = healthPerPiece - (-1f * pieceHealth);
            piecesRemoved++;

        }
        else if(pieceHealth == 0f && piecesRemoved != barrierPiecesLeft.Count && piecesRemoved != barrierPiecesRight.Count)
        {
            DestroyPiece(barrierPiecesLeft[piecesRemoved]);
            DestroyPiece(barrierPiecesRight[piecesRemoved]);
            pieceHealth = healthPerPiece;
            piecesRemoved++;
        }

        else if(pieceHealth == 0 || (piecesRemoved == barrierPiecesLeft.Count && piecesRemoved == barrierPiecesRight.Count))
        {
            //ZOMBIES GET THROUGH
            Debug.Log("Zombies GOT THRU*********");
        }

    }


    public void RepairDamage(float repairAmount)
    {
       
    } 



    private void DestroyPiece(GameObject piece)
    {

        piece.GetComponent<Collider>().enabled = false;
        piece.GetComponent<MeshRenderer>().enabled = false;
    }

    private void RepairPiece(GameObject piece)
    {

        piece.GetComponent<Collider>().enabled = true;
        piece.GetComponent<MeshRenderer>().enabled = true;
    }
}
