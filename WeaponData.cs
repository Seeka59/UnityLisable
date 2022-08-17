using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "MyGame/WeaponData")]

public class WeaponData : ScriptableObject
{

    public string name = "M4A4";
    public float damage = 10f;
    public float range = 100f;

    public GameObject graphics;    
}