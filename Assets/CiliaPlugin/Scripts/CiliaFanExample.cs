using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiliaFanExample : MonoBehaviour
{
    public string MainCharacterName = "Player";
    
    public uint FanNumber = 1;
    public byte FanSpeed = 255;
    public SurroundPosition surroundPosition;
    public string fanSmell;
    void OnTriggerEnter(Collider collision)
    {
        GameObject colissionObject = collision.gameObject;
        Debug.Log(colissionObject.name.ToString());
        if (colissionObject.name.ToString() == MainCharacterName.ToString())
        {
            Cilia.setFan(surroundPosition,fanSmell,FanNumber, FanSpeed);
        }
    }
}
