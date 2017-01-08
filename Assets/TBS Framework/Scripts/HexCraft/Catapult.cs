using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catapult : UnitAdv
{
    public override bool IsUnitAttackable(Unit other, Cell sourceCell)
    {
        if (sourceCell.GetDistance(other.Cell) <= AttackRange && sourceCell.GetDistance(other.Cell) > 1)
            return true;

        return false;
    }

    public override void Initialize()
    {
        base.Initialize();
        Transform catapult = null;
        foreach (Transform child in transform)
        {
            //Debug.Log(child.gameObject.name);
            if (child.gameObject.name.Equals("WK_catapult"))
                catapult = transform.Find("WK_catapult");
        }
        if (catapult == null)
        {
            Debug.Log("No Catapult");
            return;
        }
        var unitTexture = Resources.Load(KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
        Debug.Log(KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
        if (unitTexture == null)
        {
            Debug.Log("No Texture");
            return;
        }
        catapult.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = unitTexture as Texture;
    }
}
