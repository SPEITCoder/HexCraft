using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public abstract class KindomInfo{


	//protected string _kindomName;
	protected string _soilderTexturePath;

	public string TextureFolderPostion{get; private set;}

	public virtual int KindomNumber{ get; protected set;}
	public virtual string KindomName { get; protected set;}
	public virtual string SoilderTexturePath
	{
		get
		{
			//return Application.dataPath + TextureFolderPostion + _soilderTexturePath;
			return _soilderTexturePath;
		}
		set
		{
			_soilderTexturePath = value;
		}
	}

	public KindomInfo()
	{
		TextureFolderPostion = "TBS Framework/HexAssets/";
	}
}

public class BlueKindomInfo : KindomInfo
{
	public BlueKindomInfo()
	{
		KindomNumber = 0;
		KindomName = "BlueKindom";
		SoilderTexturePath = "WK_StandardUnits_Blue";		
	}
}

public class GreenKindomInfo : KindomInfo
{
	public GreenKindomInfo()
	{
		KindomNumber = 1;
		KindomName = "GreenKindom";
		SoilderTexturePath = "WK_StandardUnits_Green";		
	}
}

public class RedKindomInfo : KindomInfo
{
	public RedKindomInfo()
	{
		KindomNumber = 2;
		KindomName = "RedKindom";
		SoilderTexturePath = "WK_StandardUnits_Red";		
	}
}

public class WhiteKindomInfo : KindomInfo
{
	public WhiteKindomInfo()
	{
		KindomNumber = 3;
		KindomName = "WhiteKindom";
		SoilderTexturePath = "WK_StandardUnits_White";		
	}
}

public class KindomInfoHelper
{
	public static KindomInfo getKindomInfobyNumber(int k)
	{
		switch (k)
		{
		case 0:
			return new BlueKindomInfo ();
			break;
		case 1:
			return new GreenKindomInfo ();
			break;
		case 2:
			return new RedKindomInfo ();
			break;
		case 3:
			return new WhiteKindomInfo ();
			break;
		default:
			Debug.LogError ("Wrong Number.");
			return null;
			break;
		}
	}
}
