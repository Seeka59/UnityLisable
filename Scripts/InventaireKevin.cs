using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventaireKevin : MonoBehaviour
{
    private int wood = 0;
    private int pierre = 0;
    bool activation = false;

    public void InventaireIncrem(string _arg,int _nbr)
    {
        if(_arg == "pierre")
        {
            pierre = pierre + _nbr;
        }
        else if(_arg == "wood")
        {
            Debug.Log("Tu avant " + wood);
            wood = wood + _nbr;
            Debug.Log("Tu apres " + wood);
        }
    }

    public void InventaireDecrem(string _arg,int _nbr)
    {
        if(_arg =="pierre")
        {
            pierre = pierre - _nbr;
        }
        if(_arg =="wood")
        {
            wood = wood - _nbr;
        }
    }

    public void ActivationInventaire()
    {
       
            activation = !activation;
            GetComponent<Canvas>().enabled = activation;
        
    }

}
