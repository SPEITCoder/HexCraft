using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public int PlayerNumber;
	public int Money;
    /// <summary>
    /// Method is called every turn. Allows player to interact with his units.
    /// </summary>         
    public abstract void Play(CellGrid cellGrid);
}