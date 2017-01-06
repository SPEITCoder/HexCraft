using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public int PlayerNumber;
	public int Money;
	public KindomInfo GetKindomInfo{ get; private set;}
    /// <summary>
    /// Method is called every turn. Allows player to interact with his units.
    /// </summary>         
    public abstract void Play(CellGrid cellGrid);

	void Start()
	{
		GetKindomInfo = KindomInfoHelper.getKindomInfobyNumber(PlayerNumber);
	}
}