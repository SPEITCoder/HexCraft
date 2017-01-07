using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : UnitAdv
{
    public override void Initialize()
    {
        base.Initialize();
        Transform archer = null;
        foreach (Transform child in transform)
        {
            //Debug.Log(child.gameObject.name);
            if (child.gameObject.name.Equals("WK_archer"))
                archer = transform.Find("WK_archer");
        }
        if (archer == null)
        {
            Debug.Log("No Archer");
            return;
        }
        var unitTexture = Resources.Load(KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
        Debug.Log(KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
        if (unitTexture == null)
        {
            Debug.Log("No Texture");
            return;
        }
        archer.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = unitTexture as Texture;
    }
    protected override void Defend(Unit other, int damage)
    {
        var realDamage = damage;
        if (other is Paladin)
            realDamage *= 2;//Paladin deals double damage to archer.

        base.Defend(other, realDamage);
    }
}
