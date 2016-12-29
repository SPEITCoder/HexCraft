using UnityEngine;
using System.Collections;
using System.Linq;
using System;


public class OnGameUnitGenerator : MonoBehaviour 
{
	public GameObject FootmanPrefab;
	public GameObject CavalryPrefab;
	public GameObject ArcherPrefab;
	public GameObject CatapultPrefab;


	public Transform SpawnUnit(Cell cell, MilitaryBranch type, int playerNumner)
	{
		GameObject unit;
		switch (type)
		{
		case MilitaryBranch.Footman:
			unit = Instantiate(FootmanPrefab);
			break;
		case MilitaryBranch.Cavalry:
			unit = Instantiate(CavalryPrefab);
			break;
		case MilitaryBranch.Archer:
			unit = Instantiate(ArcherPrefab);
			break;
		case MilitaryBranch.Catapult:
			unit = Instantiate(CatapultPrefab);
			break;
		default:
			Debug.LogError("Invalid Miitary Branch");
			unit = Instantiate(FootmanPrefab);
			break;
		}

		if (!cell.IsTaken)
		{
			cell.IsTaken = true;
			unit.GetComponent<Unit>().Cell = cell;
			unit.transform.position = cell.transform.position;
			unit.GetComponent<Unit>().PlayerNumber = playerNumner;
			unit.GetComponent<Unit>().Initialize();

			Vector3 offset = new Vector3(0,0, cell.GetComponent<Cell>().GetCellDimensions().z);
			unit.transform.position = cell.transform.position - offset;
		}//Unit gets snapped to the nearest cell
		else
		{
			Destroy(unit.gameObject);
		}//If the nearest cell is taken, the unit gets destroyed.

		return unit.transform;
	}
}

public enum MilitaryBranch
{
	Footman,
	Cavalry,
	Archer,
	Catapult
}
