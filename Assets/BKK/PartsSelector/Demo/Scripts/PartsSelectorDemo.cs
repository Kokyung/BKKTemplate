using System;
using System.Collections;
using System.Collections.Generic;
using BKK.Avatar;
using UnityEngine;

public class PartsSelectorDemo : MonoBehaviour
{
    public BasePartsSelector partsSelector;

    public Transform target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            partsSelector.ChangeModel("Man_Jacket_Mesh", target);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            partsSelector.ClearAllSkins(false);
        }
    }
}
