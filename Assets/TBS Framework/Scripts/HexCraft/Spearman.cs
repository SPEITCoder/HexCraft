using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spearman : UnitAdv
{

	public override void Initialize()
	{
		base.Initialize();
		Transform spearman = null;
		foreach (Transform child in transform) 
		{
			//Debug.Log(child.gameObject.name);
			if(child.gameObject.name.Equals("WK_heavy_infantry"))
				spearman = transform.Find("WK_heavy_infantry");
		}
		if (spearman == null)
		{
			Debug.Log ("No HeavyInfantry");
			return;
		}
		var unitTexture = Resources.Load(KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
		Debug.Log (KindomInfoHelper.getKindomInfobyNumber(PlayerNumber).SoilderTexturePath);
		if (unitTexture == null)
		{
			Debug.Log ("No Texture");
			return;
		}
		spearman.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = unitTexture as Texture;
	}

    protected override void Defend(Unit other, int damage)
    {
        var realDamage = damage;
        if (other is Archer)
            realDamage *= 2;//Archer deals double damage to spearman.

        base.Defend(other, realDamage);
    }
}
