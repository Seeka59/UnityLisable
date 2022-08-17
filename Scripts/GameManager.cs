using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
// public int nbrCo = NetworkServer.connection.Count;

private const string playerIdPrefix = "Player";

private static Dictionary<string, Player> players = new Dictionary<string, Player>();

public MatchSetting matchSetting;

public static GameManager instance;

[SerializeField]private GameObject _sceneCamera;

IEnumerator Start()
{
    while(!NetworkServer.active)
    {
        yield return null;
    }
    
}

private void Awake()
{
    if(instance == null)
    {
        instance = this;
        return;
    }
    Debug.LogError("Plus d'une instance de GameManerger dans la scene");
}

public void SetSceneCameraActive(bool isActive)
    {
        if(_sceneCamera == null)
        {
            return;
        }

        _sceneCamera.SetActive(isActive);
    }

    public static void RegisterPlayer(string netId, Player player)
    {
    string playerId =playerIdPrefix + netId;
    players.Add(playerId, player);
    player.transform.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }
    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

}
