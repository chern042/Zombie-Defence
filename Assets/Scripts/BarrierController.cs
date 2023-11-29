using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private DestroyBarrierAnimation destroyBarrierAnimation;

    public bool BarrierDestroyed { get => barrierDestroyed; }

    private bool barrierDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        healthPerPiece = barrierTotalHealth / barrierPiecesLeft.Count;
        pieceHealth = barrierTotalHealth / barrierPiecesLeft.Count;
        piecesRemoved = 0;
        destroyBarrierAnimation = GetComponent<DestroyBarrierAnimation>();
        barrierDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (!barrierDestroyed)
        {
            pieceHealth -= damage;
            if (pieceHealth < 0f && (piecesRemoved != barrierPiecesLeft.Count && piecesRemoved != barrierPiecesRight.Count))
            {

                destroyBarrierAnimation.ThrowLogs(piecesRemoved);
                pieceHealth = healthPerPiece - (-1f * pieceHealth);
                piecesRemoved++;

            }
            else if (pieceHealth == 0f && (piecesRemoved != barrierPiecesLeft.Count && piecesRemoved != barrierPiecesRight.Count))
            {

                destroyBarrierAnimation.ThrowLogs(piecesRemoved);
                pieceHealth = healthPerPiece;
                piecesRemoved++;
            }


            if (pieceHealth == 0 || (piecesRemoved == barrierPiecesLeft.Count && piecesRemoved == barrierPiecesRight.Count))
            {
                barrierDestroyed = true;
                //ZOMBIES GET THROUGH
                Debug.Log("Zombies GOT THRU*********");
            }
        }

    }


    public void RepairDamage(float repairAmount)
    {
        if (piecesRemoved != 0)
        {
            pieceHealth += repairAmount;
            Debug.Log("Repaired piece: " + (pieceHealth - repairAmount) + " to: " + pieceHealth);
            if (pieceHealth > healthPerPiece)
            {
                destroyBarrierAnimation.ReturnLogs(piecesRemoved);
                pieceHealth = pieceHealth - healthPerPiece;
                piecesRemoved--;
                barrierDestroyed = false;
            }else if(pieceHealth == healthPerPiece)
            {
                destroyBarrierAnimation.ReturnLogs(piecesRemoved);
                pieceHealth = 0;
                piecesRemoved--;
                barrierDestroyed = false;
            }
        }
    } 

}
