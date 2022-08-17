using UnityEngine;
using UnityEngine.UI;

public class PlayerUiKevin : MonoBehaviour
{
[SerializeField]private GameObject scoreboard;
public void ApplyScorboard(bool _act)
{
    scoreboard.SetActive(_act);
}

    [SerializeField]
    private RectTransform healthbarFill;

    private Player player;
    private WeaponManager weaponManager;


public void SetPlayer(Player _player)
    {
        player = _player;
    }
   

    private void Update()
    {
        SetHealthAmount(player.GetHealthPct());
    }

    void SetHealthAmount(float _amount)
    {
        healthbarFill.localScale = new Vector3(1f, _amount, 1f);
    }

    

}
