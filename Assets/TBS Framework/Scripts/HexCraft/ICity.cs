using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public abstract class ICity : Unit {

	public Color PlayerColor;

	public string CityName;

	private Transform Highlighter;

	public MilitaryBranch _unitType;

	public event EventHandler BigCitySelected;
	public event EventHandler CityOccupied;
	public event EventHandler<UnitCreateEventArgs> CreatingUnit;

	public override void Initialize()
	{
		base.Initialize();
		SetColor(PlayerColor);

		Highlighter = transform.Find("Highlighter");
		if (Highlighter != null)
		{
			Highlighter.position = transform.position + new Vector3(0, 0, 1.5f);
			foreach (Transform cubeTransform in Highlighter)
				Destroy(cubeTransform.GetComponent<BoxCollider>());
		}     
		//gameObject.transform.position = Cell.transform.position + new Vector3(0, 0, -1.5f);
	}

	protected override void OnDestroyed()
	{
//		if (CityOccupied != null)
//		{
//			CityOccupied.Invoke(this, new EventArgs());
//		}
	}
		
	public override void OnUnitSelected()
	{
		base.OnUnitSelected ();
		OnCitySelected();
	}
	public virtual void OnCitySelected()
	{//to Ui only
		if(BigCitySelected != null)
			BigCitySelected.Invoke(this, new EventArgs());
	}

	public void OnTurnEnd(CellGrid cellGrid)
	{
		base.OnTurnEnd();
		//_city.OnUnitSelected();
		//_cityCell = _city.Cell;

//		List<Cell> pathsInRange = this.GetAvailableDestinations(cellGrid.Cells);
		//var cellsNotInRange = cellGrid.Cells.Except(pathsInRange);

		//		foreach (var cell in cellsNotInRange)
		//		{
		//			cell.UnMark();
		//		}
		//		foreach (var cell in pathsInRange)
		//		{
		//			cell.MarkAsReachable();//able to create a unit
		//		}

		//		if (_city.ActionPoints <= 0) return;

		foreach (var currentUnit in cellGrid.Units)
		{
			if (currentUnit.PlayerNumber.Equals(this.PlayerNumber) && this.Cell.GetDistance(currentUnit.Cell) <= this.MovementPoints)
			{
				currentUnit.Supply += this.Supply;
				Debug.Log ("Unit Supply is:" + currentUnit.Supply);
			}
		}
		Debug.Log("City Finish OnturnEnd");
	}


	protected override void Defend(Unit other, int damage)
	{
		base.Defend(other, damage);
		UpdateHpBar();
	}

	public override void Move(Cell destinationCell, List<Cell> path)
	{
		
	}

	public override void MarkAsAttacking(Unit other)
	{
		//StartCoroutine(Jerk(other));
	}
	public override void MarkAsDefending(Unit other)
	{
		StartCoroutine(Glow(new Color(1, 0.5f, 0.5f), 1));
	}
	public override void MarkAsDestroyed()
	{	}

	//when a cell near city is clicked
	public virtual void OnUnitCreating(Cell cell)
	{
		if(CreatingUnit != null)
			//send to CellGrid
			CreatingUnit.Invoke(this, new UnitCreateEventArgs(cell, _unitType));
	}

	private IEnumerator Jerk(Unit other)
	{
		var heading = other.transform.position - transform.position;
		var direction = heading / heading.magnitude;
		float startTime = Time.time;

		while (startTime + 0.25f > Time.time)
		{
			transform.position = Vector3.Lerp(transform.position, transform.position + (direction / 2.5f), ((startTime + 0.25f) - Time.time));
			yield return 0;
		}
		startTime = Time.time;
		while (startTime + 0.25f > Time.time)
		{
			transform.position = Vector3.Lerp(transform.position, transform.position - (direction / 2.5f), ((startTime + 0.25f) - Time.time));
			yield return 0;
		}
		transform.position = Cell.transform.position + new Vector3(0, 0, -1.5f); ;
	}
	private IEnumerator Glow(Color color, float cooloutTime)
	{
		float startTime = Time.time;

		while (startTime + cooloutTime > Time.time)
		{
			SetColor(Color.Lerp(PlayerColor, color, (startTime + cooloutTime) - Time.time));
			yield return 0;
		}

		SetColor(PlayerColor);
	}

	public override void MarkAsFriendly()
	{
		SetHighlighterColor(new Color(0.8f,1,0.8f));
	}
	public override void MarkAsReachableEnemy()
	{
		SetHighlighterColor(Color.red);
	}
	public override void MarkAsSelected()
	{
		SetHighlighterColor(new Color(0,1,0));
	}
	public override void MarkAsFinished()
	{
		Debug.Log("Shold not be mark as finished");
		SetColor(PlayerColor - Color.gray);
		SetHighlighterColor(new Color(0.8f, 1, 0.8f));
	}
	public override void UnMark()
	{
		SetColor(PlayerColor);
		SetHighlighterColor(Color.white);
		if (Highlighter == null) return;
		Highlighter.position = transform.position + new Vector3(0, 0, 1.52f);
	}

	private void UpdateHpBar()
	{
		if (GetComponentInChildren<Image>() != null)
		{
			GetComponentInChildren<Image>().transform.localScale = new Vector3((float)((float)HitPoints / (float)TotalHitPoints), 1, 1);
			GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.green,
				(float)((float)HitPoints / (float)TotalHitPoints));
		}
	}
	private void SetColor(Color color)
	{
		//GetComponent<Renderer>().material.color = color;
	}
	private void SetHighlighterColor(Color color)
	{

		if (Highlighter == null) return;

		Highlighter.position = transform.position + new Vector3(0, 0, 1.48f);
		for (int i = 0; i < Highlighter.childCount; i++)
		{
			var rendererComponent = Highlighter.transform.GetChild(i).GetComponent<Renderer>();
			if (rendererComponent != null)
				rendererComponent.material.color = color;
		}
	}
}

public class UnitCreateEventArgs : EventArgs
{
	public Cell Cell;
	public MilitaryBranch UnitType;

	public UnitCreateEventArgs(Cell cell, MilitaryBranch unitType)
	{
		Cell = cell;
		UnitType = unitType;
	}
}
