using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreboard : MonoBehaviour
{
    [SerializeField]GameObject playerScoreboardItem;
    [SerializeField]Transform playerScoreboardList;

  private void OnEnable()
  {
      Player[] players = GameManager.GetAllPlayers();

    foreach(Player player in players)
    {
        GameObject itemGO = Instantiate(playerScoreboardItem,playerScoreboardList);
        
    }
  }
  private void OnDisable()
  {


  }
}
