using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiliaLightExample : MonoBehaviour
{
    public string MainCharacterName = "Player";
    public uint LightNumber = 1;
    public bool AllLights = false;
    public byte LightRed = 255;
    public byte LightGreen = 255;
    public byte LightBlue = 255;
    public SurroundPosition surroundPosition;

    void OnTriggerEnter(Collider collision)
    {
        GameObject colissionObject = collision.gameObject;
        Debug.Log(colissionObject.name.ToString());
        if (colissionObject.name.ToString() == MainCharacterName.ToString())
        { 
            if(AllLights == true)
            {
                for(uint i = 1; i < 7; i++)
                {
                    Cilia.setLight(surroundPosition, i, LightRed, LightGreen, LightBlue);
                }
            }
            else
                Cilia.setLight(surroundPosition,LightNumber, LightRed, LightGreen, LightBlue);
        }
    }
}
