using System;
using UnityEngine;
using UnityEngine.UI;

public class HexGuiController : MonoBehaviour
{
    public CellGrid CellGrid;
    public GameObject UnitsParent;
	public GameObject CityParent;

    public Button NextTurnButton;
    public Image UnitImage;
    public Text InfoText;
    public Text StatsText;
	public Text CityInfo;

	public Button CreateFootmanButton;

	private Cell _selectedCell;

    void Start()
    {
        //UnitImage.color = Color.gray;

        CellGrid.GameStarted += OnGameStarted;
        CellGrid.TurnEnded += OnTurnEnded;
        CellGrid.GameEnded += OnGameEnded;
		CellGrid.UnitCreated += OnUnitCreated;

		CreateFootmanButton.onClick.AddListener(delegate {OnCreateFootMan(this,new EventArgs());} );
		NextTurnButton.onClick.AddListener (delegate {CellGrid.EndTurn ();});
    }

    private void OnGameStarted(object sender, EventArgs e)
    {
        foreach (Transform unit in UnitsParent.transform)
        {
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
        }

        foreach (Transform cell in CellGrid.transform)
        {
            cell.GetComponent<Cell>().CellHighlighted += OnCellHighlighted;
            cell.GetComponent<Cell>().CellDehighlighted += OnCellDehighlighted;
        }

		foreach (Transform city in CityParent.transform) 
		{
			city.GetComponent<ICity>().UnitClicked += OnCityClicked;
			city.GetComponent<ICity>().UnitSelected += OnCitySelected;
			city.GetComponent<ICity> ().BigCitySelected += OnBigCitySelected;
			city.GetComponent<ICity>().UnitDeselected += OnCityDeselected;
		}

        OnTurnEnded(sender,e);
    }

    private void OnGameEnded(object sender, EventArgs e)
    {
        InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1) + " wins!";
    }
    private void OnTurnEnded(object sender, EventArgs e)
    {
        NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);

		InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber +1) + "\nMoney: " + (sender as CellGrid).CurrentPlayer.Money;
    }
	private void OnUnitCreated(object sender, EventArgs e)
	{
		InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber +1) + "\nMoney: " + (sender as CellGrid).CurrentPlayer.Money;
		foreach (Transform unit in UnitsParent.transform)
		{
			unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
			unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
			unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
		}
	}
    private void OnCellDehighlighted(object sender, EventArgs e)
    {
        UnitImage.color = Color.gray;
        StatsText.text = "";
    }
    private void OnCellHighlighted(object sender, EventArgs e)
    {
        UnitImage.color = Color.gray;
        StatsText.text = "Movement Cost: " + (sender as Cell).MovementCost;
    }
    private void OnUnitAttacked(object sender, AttackEventArgs e)
    {
        if (!(CellGrid.CurrentPlayer is HumanPlayer)) return;
        OnUnitDehighlighted(sender, new EventArgs());

        if ((sender as Unit).HitPoints <= 0) return;

        OnUnitHighlighted(sender, e);
    }
    private void OnUnitDehighlighted(object sender, EventArgs e)
    {
        StatsText.text = "";
        UnitImage.color = Color.gray;
    }
    private void OnUnitHighlighted(object sender, EventArgs e)
    {
		var unit = sender as UnitAdv;
		StatsText.text = unit.UnitName + "\nHit Points: " + unit.HitPoints +"/"+unit.TotalHitPoints + "\nAttack: " + unit.AttackFactor + "\nDefence: " + unit.DefenceFactor + "\nRange: " + unit.AttackRange + "\nSupply: " + unit.Supply;
        UnitImage.color = unit.PlayerColor;

    }

	private void OnCityClicked(object sender, EventArgs e)
	{
		var city = sender as ICity;
		CityInfo.text = "Clicked a city." + "\nHit Points: " + city.HitPoints + "/" + city.TotalHitPoints + "\nSupply: " + city.Supply;
	}
	//revive a OnCreatingUnit event from city 
	private void OnCitySelected(object sender, EventArgs e)
	{
		//var city = sender as ICity;
		//_selectedCell = e.Cell;
		//CreateFootmanButton.interactable = true;
	}
	private void OnBigCitySelected(object sender, EventArgs e)
	{
		CreateFootmanButton.interactable = true;
	}
	private void OnCityDeselected(object sender, EventArgs e)
	{
		CreateFootmanButton.interactable = false;
	}

	//when player push FootMan button on UI
	private void OnCreateFootMan(object sender, EventArgs e)
	{
		CityInfo.text = "Change type to footman...";
		foreach (Transform city in CityParent.transform) 
		{
			//city.GetComponent<ICity>()._unitType = ;
			city.GetComponent<ICity>()._unitType = MilitaryBranch.Footman;

		}
	}

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
