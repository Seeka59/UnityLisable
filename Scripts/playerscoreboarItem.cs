using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerscoreboarItem : MonoBehaviour
{
    [SerializeField]Text usernameText;
    [SerializeField]Text killsText;
    [SerializeField]Text deathText;

    public void Setup(string username,int kills, int deaths)
    {
        usernameText.text = username;
        killsText.text="Kills :" + kills;
        deathText.text="Deaths :" + deaths;
    }
}
