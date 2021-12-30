using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiliaFanExample2 : MonoBehaviour
{
    public string MainCharacterName = "Player";
    public SurroundPosition surroundPosition;
    public string fanSmell;
    public uint FanNumber = 1;
    byte FanSpeed = 255;
    public float FanRadius = 5;
    static float oldDistance = 0;
    //private int frameRate = 0;
	// Use this for initialization
	void Start ()
    {
        if (FanRadius < 0)
            FanRadius = -FanRadius;
        BoxCollider tempBoxCollider = GetComponent<BoxCollider>();
        tempBoxCollider.size = new Vector3(FanRadius, FanRadius, FanRadius);
    }

    void OnTriggerStay(Collider collision)
    {
            GameObject colissionObject = collision.gameObject;
            float newDistance = Vector3.Distance(transform.position, colissionObject.transform.position);
            if (newDistance != oldDistance)
            {
                //Debug.Log(colissionObject.name.ToString());
                if (colissionObject.name.ToString() == MainCharacterName.ToString())
                {
                    float SmellSourceDistance = Vector3.Distance(transform.position, colissionObject.transform.position);
                    FanSpeed = (byte)((FanRadius - SmellSourceDistance) / FanRadius * 255);
                    Cilia.setFan(surroundPosition, fanSmell, FanNumber, FanSpeed);
                }
            }
    }
    void OnTriggerExit(Collider collision)
    {
        Cilia.setFan(surroundPosition,fanSmell, FanNumber, 0);
    }
}
