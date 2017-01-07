using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paladin : UnitAdv
{
    public override void Initialize()
    {
        base.Initialize();
        Transform cavalry = null;
        foreach (Transform child in transform)
        {
            //Debug.Log(child.gameObject.name);
            if (child.gameObject.name.Equals("WK_cavalry"))
                cavalry = transform.Find("WK_cavalry");
        }
        if (cavalry == null)
        {
            Debug.Log("No Cavalry");
            return;
        }
        var unitTexture = Resources.Load(KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
        Debug.Log(KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
        if (unitTexture == null)
        {
            Debug.Log("No Texture");
            return;
        }
        cavalry.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = unitTexture as Texture;
    }
    protected override void Defend(Unit other, int damage)
    {
        var realDamage = damage;
        if (other is Spearman)
            realDamage *= 2;//Spearman deals double damage to paladin.

        base.Defend(other, realDamage);
    }
}
