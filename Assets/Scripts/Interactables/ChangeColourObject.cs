using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourObject : Interactable
{

    MeshRenderer mesh;
    public Color[] colors;
    private int colourIndex;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.red;
    }

    protected override void Interact()
    {
        colourIndex++;
        if(colourIndex > colors.Length - 1)
        {
            colourIndex = 0;
        }
        mesh.material.color = colors[colourIndex];
    }
}
