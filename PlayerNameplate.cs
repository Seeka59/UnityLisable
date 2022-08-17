using UnityEngine;
using UnityEngine.UI;

public class PlayerNameplate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text usernameText;
    [SerializeField]
    private Player player;

    // Update is called once per frame
    void Update()
    {
    usernameText.text = player.name;    
    }
}
