using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// CellGrid class keeps track of the game, stores cells, units and players objects. It starts the game and makes turn transitions. 
/// It reacts to user interacting with units or cells, and raises events related to game progress. 
/// </summary>
public class CellGrid : CustomUnitGenerator
{
    public event EventHandler GameStarted;
    public event EventHandler GameEnded;
    public event EventHandler TurnEnded;
	public event EventHandler UnitCreated;
    
    private CellGridState _cellGridState;//The grid delegates some of its behaviours to cellGridState object.
    public CellGridState CellGridState
    {
        private get
        {
            return _cellGridState;
        }
        set
        {
            if(_cellGridState != null)
                _cellGridState.OnStateExit();
            _cellGridState = value;
            _cellGridState.OnStateEnter();
        }
    }

    public int NumberOfPlayers { get; private set; }

    public Player CurrentPlayer
    {
        get { return Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)); }
    }
    public int CurrentPlayerNumber { get; private set; }

    public Transform PlayersParent;

    public List<Player> Players { get; private set; }
    public List<Cell> Cells { get; private set; }
    public List<Unit> Units { get; private set; }
	public List<ICity> Citys { get; private set; }

    void Start()
    {
        Players = new List<Player>();
        for (int i = 0; i < PlayersParent.childCount; i++)
        {
            var player = PlayersParent.GetChild(i).GetComponent<Player>();
            if (player != null)
                Players.Add(player);
            else
                Debug.LogError("Invalid object in Players Parent game object");
        }
        NumberOfPlayers = Players.Count;
        CurrentPlayerNumber = Players.Min(p => p.PlayerNumber);

        Cells = new List<Cell>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
            if (cell != null)
                Cells.Add(cell);
            else
                Debug.LogError("Invalid object in cells paretn game object");

//			if (cell.gameObject.GetComponent<CraftHexagon>() == null)
//				Debug.LogError("No CreatHexagon for cell");
//			if (cell.gameObject.GetComponent<CraftHexagon>().LandForm == ELandForm.big_city
//				|| cell.gameObject.GetComponent<CraftHexagon>().LandForm == ELandForm.small_city)
//			{
//				ICity city = cell.gameObject.GetComponent<CraftHexagon>().city;
//				if (city != null)
//					Citys.Add(city);
//				else
//					Debug.LogError("Invalid object in citys");
//			}
        }
		Debug.Log("Cells Number: " + Cells.Count);
      
        foreach (var cell in Cells)
        {
            cell.CellClicked += OnCellClicked;
            cell.CellHighlighted += OnCellHighlighted;
            cell.CellDehighlighted += OnCellDehighlighted;
        }

		var gridGenerator = GetComponent<RectHexGridGenerator>();
		if (gridGenerator != null)
			Citys = gridGenerator.GenerateLandform();
		else
			Debug.LogError("No LandForm Generator");
		Debug.Log("City Number: " + Citys.Count);
		foreach (var city in Citys)
		{
			city.UnitClicked += OnCityClicked;
			city.CreatingUnit += OnUnitCreated;
			city.UnitDestroyed += OnCityOccupied;
		}
             
        var unitGenerator = GetComponent<IUnitGenerator>();
        if (unitGenerator != null)
        {
            Units = unitGenerator.SpawnUnits(Cells);
            foreach (var unit in Units)
            {
                unit.UnitClicked += OnUnitClicked;
                unit.UnitDestroyed += OnUnitDestroyed;
            }
        }
        else
            Debug.LogError("No IUnitGenerator script attached to cell grid");
        
        StartGame();
    }

    private void OnCellDehighlighted(object sender, EventArgs e)
    {
        CellGridState.OnCellDeselected(sender as Cell);
    }
    private void OnCellHighlighted(object sender, EventArgs e)
    {
        CellGridState.OnCellSelected(sender as Cell);
    } 
    private void OnCellClicked(object sender, EventArgs e)
    {
        CellGridState.OnCellClicked(sender as Cell);
    }
		
    private void OnUnitClicked(object sender, EventArgs e)
    {
        CellGridState.OnUnitClicked(sender as Unit);
    }

	private void OnCityClicked(object sender, EventArgs e)
	{
		CellGridState.OnCityClicked(sender as ICity);
	}

	private void OnCityOccupied(object sender, AttackEventArgs e)
	{
		var city = sender as ICity;
		city.PlayerNumber = CurrentPlayerNumber;
		city.HitPoints = city.TotalHitPoints;
		city.Initialize();
	}

	private void OnUnitCreated(object sender, UnitCreateEventArgs e)
	{
		if (e.Cell.IsTaken)
		{
			Debug.Log("Fail to create a unit in a taken cell");
			return;
		}
		if (CurrentPlayer.Money < gameObject.GetComponent<OnGameUnitGenerator> ().GetUnitCost (e.UnitType))
		{
			Debug.Log("Money not enough!");
			return;
		}
		Transform unitTransform;
		unitTransform = gameObject.GetComponent<OnGameUnitGenerator>().SpawnUnit(e.Cell, e.UnitType, CurrentPlayerNumber);
		Unit unit = unitTransform.gameObject.GetComponent<Unit>();
		if (unit != null)
		{
			Units.Add(unit);
			unit.Cell = e.Cell;
			unit.Cell.IsTaken = true;
			unit.Initialize();
			unit.OnTurnStart();

			unit.UnitClicked += OnUnitClicked;
			unit.UnitDestroyed += OnUnitDestroyed;

			CurrentPlayer.Money -= unit.UnitCost;

			if (UnitCreated != null)
				UnitCreated.Invoke(this, new EventArgs ());

			Debug.Log("Unit Number: " + Units.Count);
		}
		else
			Debug.LogError("No Unit script in unit");
	}
    private void OnUnitDestroyed(object sender, AttackEventArgs e)
    {
        Units.Remove(sender as Unit);
        var totalPlayersAlive = Units.Select(u => u.PlayerNumber).Distinct().ToList(); //Checking if the game is over
        if (totalPlayersAlive.Count == 1)
        {
            if(GameEnded != null)
                GameEnded.Invoke(this, new EventArgs());
        }
    }
    
    /// <summary>
    /// Method is called once, at the beggining of the game.
    /// </summary>
    public void StartGame()
    {
        if(GameStarted != null)
            GameStarted.Invoke(this, new EventArgs());

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
		Citys.FindAll(c => c.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(c => { c.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);
    }
    /// <summary>
    /// Method makes turn transitions. It is called by player at the end of his turn.
    /// </summary>
    public void EndTurn()
    {
		if (Units.Select (u => u.PlayerNumber).Distinct ().Count () == 1 &&
		    
			(Citys.Select (c => c.PlayerNumber).Distinct ().Count () == 1 ||
			(Citys.Select (c => c.PlayerNumber).Distinct ().Count () == 2 && 
				Citys.FindAll(c => c.PlayerNumber.Equals(-1)).Count > 0)) ) {
					return;

		}
        CellGridState = new CellGridStateTurnChanging(this);

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnEnd(); });
		Citys.FindAll(c => c.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(c => { 
			c.OnTurnEnd(this);
			CurrentPlayer.Money += c.UnitCost;
		});

        CurrentPlayerNumber = (CurrentPlayerNumber + 1) % NumberOfPlayers;
        while (Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).Count == 0 && 
				Citys.FindAll(c => c.PlayerNumber.Equals(CurrentPlayerNumber)).Count == 0)
        {
            CurrentPlayerNumber = (CurrentPlayerNumber + 1)%NumberOfPlayers;
        }//Skipping players that are defeated.



        if (TurnEnded != null)
            TurnEnded.Invoke(this, new EventArgs());

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
		Citys.FindAll(c => c.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(c => { c.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);     
    }
}
