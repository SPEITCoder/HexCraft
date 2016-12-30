using UnityEngine.UI;

public class Hero : UnitAdv
{
    private Button _specialAbilityButton;
    private bool _abilityUsed;

    public override void Initialize()
    {
        base.Initialize();
        _specialAbilityButton = GetComponentInChildren<Button>();
        _specialAbilityButton.gameObject.SetActive(false);
        _specialAbilityButton.onClick.AddListener(TriggerSpecialAbility);
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
    }
    public override void OnUnitSelected()
    {
        if (!_abilityUsed)
        {
            Invoke("EnableSpecialAbilityButton",0.1f);
        }       
    }
    public override void OnUnitDeselected()
    {
        _specialAbilityButton.gameObject.SetActive(false);
    }

    private void EnableSpecialAbilityButton() 
    {
        _specialAbilityButton.gameObject.SetActive(true);
        _specialAbilityButton.interactable = true;
    }
    private void TriggerSpecialAbility()
    {
        //Hero has specail ability that allows him to raise his attack by 2 for duration of 3 turns.
        //This ability can be triggered once a game.
        if (!_abilityUsed)
        {
            _abilityUsed = true;
            _specialAbilityButton.gameObject.SetActive(false);
        }  
    }
}
