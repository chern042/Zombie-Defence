using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class PlayerPoints : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI pointsText;

    [SerializeField]
    private TextMeshProUGUI addedPointsAnimatedPrefab;

    [SerializeField]
    private Canvas canvas;

    [HideInInspector]
    public int points;
    private TextMeshProUGUI instantiatedObject;
    private void Start()
    {
        points = 0;

    }
    public void AddPoints(int addAmount)
    {
            points += addAmount;
            pointsText.text = "POINTS: " + points;
            instantiatedObject = Instantiate(addedPointsAnimatedPrefab);
            instantiatedObject.transform.SetParent(canvas.transform, false);
            instantiatedObject.text = "+" + addAmount;
            instantiatedObject.color = Color.green;
    }

    public void RemovePoints(int removeAmount)
    {
        if (points != 0 && (points - removeAmount) >= 0)
        {
            points -= removeAmount;
            pointsText.text = "POINTS: " + points;
            instantiatedObject = Instantiate(addedPointsAnimatedPrefab);
            instantiatedObject.transform.SetParent(canvas.transform, false);

            instantiatedObject.text = "-" + removeAmount;
            instantiatedObject.color = Color.red;

        }
        else if(points != 0 && (points - removeAmount) < 0)
        {
            instantiatedObject = Instantiate(addedPointsAnimatedPrefab);
            instantiatedObject.transform.SetParent(canvas.transform, false);

            instantiatedObject.text = "-" + points;
            instantiatedObject.color = Color.red;
            points = 0;
            pointsText.text = "POINTS: 0";
        }
    }



}
