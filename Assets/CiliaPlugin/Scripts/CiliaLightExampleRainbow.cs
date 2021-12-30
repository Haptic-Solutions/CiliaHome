using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiliaLightExampleRainbow: MonoBehaviour
{
    
    int counter = 0;
    public string MainCharacterName = "Player";
    public SurroundPosition surroundPosition;
    public int SpeedDivider = 1;
    private uint LightNumber = 1;
    private byte[] L1 = { 255, 255, 0 };
    private byte[] L2 = { 255, 255, 0 };
    private byte[] L3 = { 128, 255, 0 };
    private byte[] L4 = { 128, 255, 0 };
    private byte[] L5 = { 128, 255, 0 };
    private byte[] L6 = { 128, 255, 0 };
    private int colorDuration = 3;
    private int colorDurI = 0;
    private int colorI = 0;
    public float timer = 0.0f;

    void OnTriggerStay(Collider collision)
    {
        GameObject colissionObject = collision.gameObject;
        //Debug.Log(colissionObject.name.ToString());

        
        
        if (colissionObject.name.ToString() == MainCharacterName.ToString())
        {
            timer += Time.deltaTime;
            if (timer > SpeedDivider)
            {
                timer = 0.0f;
                LightNumber = 1;
                Cilia.setLight(surroundPosition, LightNumber++, L1[0], L1[1], L1[2]);
                Cilia.setLight(surroundPosition, LightNumber++, L2[0], L2[1], L2[2]);
                Cilia.setLight(surroundPosition, LightNumber++, L3[0], L3[1], L3[2]);
                Cilia.setLight(surroundPosition, LightNumber++, L4[0], L4[1], L4[2]);
                Cilia.setLight(surroundPosition, LightNumber++, L5[0], L5[1], L5[2]);
                Cilia.setLight(surroundPosition, LightNumber++, L6[0], L6[1], L6[2]);
                L1[0] = L2[0];
                L1[1] = L2[1];
                L1[2] = L2[2];
                L2[0] = L3[0];
                L2[1] = L3[1];
                L2[2] = L3[2];
                L3[0] = L4[0];
                L3[1] = L4[1];
                L3[2] = L4[2];
                L4[0] = L5[0];
                L4[1] = L5[1];
                L4[2] = L5[2];
                L5[0] = L6[0];
                L5[1] = L6[1];
                L5[2] = L6[2];

                switch (colorI)
                {
                    case 0:
                        L6[0] = 0; L6[1] = 255; L6[2] = 0; break;
                    case 1:
                        L6[0] = 0; L6[1] = 255; L6[2] = 128; break;
                    case 2:
                        L6[0] = 0; L6[1] = 255; L6[2] = 255; break;
                    case 3:
                        L6[0] = 0; L6[1] = 128; L6[2] = 255; break;
                    case 4:
                        L6[0] = 0; L6[1] = 0; L6[2] = 255; break;
                    case 5:
                        L6[0] = 128; L6[1] = 0; L6[2] = 255; break;
                    case 6:
                        L6[0] = 255; L6[1] = 0; L6[2] = 255; break;
                    case 7:
                        L6[0] = 255; L6[1] = 0; L6[2] = 128; break;
                    case 8:
                        L6[0] = 255; L6[1] = 0; L6[2] = 0; break;
                    case 9:
                        L6[0] = 255; L6[1] = 128; L6[2] = 0; break;
                    case 10:
                        L6[0] = 255; L6[1] = 255; L6[2] = 0; break;
                    case 11:
                        L6[0] = 128; L6[1] = 255; L6[2] = 0; break;
                    default:
                        break;
                }

                if (colorDurI++ == colorDuration)
                {
                    colorI++;
                    if (colorI == 12)
                        colorI = 0;
                    colorDurI = 0;
                }
            }
        }
    }
}
