using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct Neopixel
{
    public byte redValue;
    public byte greenValue;
    public byte blueValue;
};
public class Cilia : MonoBehaviour
{
    [Header("------------Networking Section------------")]
    [Header("Most of the time leave this alone")]
    public int CiliaPort = 1995;
    public string CiliaIP = "localhost";
    [Header("-----------Game Profile Section-----------")]
    public string GameProfileName = "Game";
    public SurroundPosition DefaultSurroundGroup = (SurroundPosition) 0;
    /*Smells to add to sdk smell library. Also for setting default game profile. First six will be used*/
    [Header("Adds the following smells to the smell library")]
    [Header("Sets the Cilias to the 1st six smells if the profile didn't exist")]
    public string[] SmellsToAddToSmellLibrary = { "Apple", "BahamaBreeze", "CleanCotton", "Leather", "Lemon", "Rose" };
    private string[] privateLibrary = { "Apple", "BahamaBreeze", "CleanCotton", "Leather", "Lemon", "Rose" };
    [Header("Game Profile Default Lighting Values: 0-255")]
    public Neopixel Light1;
    public Neopixel Light2;
    public Neopixel Light3;
    public Neopixel Light4;
    public Neopixel Light5;
    public Neopixel Light6;
    public string[] surroundGroupStrings = { "FrontCenter", "FrontLeft", "SideLeft", "RearLeft", "RearCenter", "RearRight", "SideRight", "FrontRight" };
    private string[] privateSurroundGroups = { "FrontCenter", "FrontLeft", "SideLeft", "RearLeft", "RearCenter", "RearRight", "SideRight", "FrontRight" };
    static TcpClient CiliaClient = new TcpClient();
    static NetworkStream CiliaStream;
    static byte[] message;
    const int SmellNameLength = 20;

    private void OnValidate()
    {
        //Debug.Log("Smells");
        //for (int i1 = 0; i1 < SmellsToAddToSmellLibrary.Length; i1++)
        //    Debug.Log(SmellsToAddToSmellLibrary[i1].ToString());
        //for (int i1 = 0; i1 < SmellsToAddToSmellLibrary.Length; i1++)
        //    Debug.Log(privateLibrary[i1].ToString());
        if (SmellsToAddToSmellLibrary.Length == privateLibrary.Length)
        {
            for (int i = 0; i < SmellsToAddToSmellLibrary.Length; i++)
            {
                if (SmellsToAddToSmellLibrary[i].Length == 0)
                {
                    SmellsToAddToSmellLibrary[i] = privateLibrary[i];

                }
                else
                {
                    string SmellsToAddCopy = "";
                    if (SmellsToAddToSmellLibrary[i].Length > SmellNameLength)
                        SmellsToAddToSmellLibrary[i] = SmellsToAddToSmellLibrary[i].Substring(0, SmellNameLength);
                    foreach (char c in SmellsToAddToSmellLibrary[i])
                        if (char.IsLetterOrDigit(c))
                            SmellsToAddCopy += c;
                    if (SmellsToAddCopy.Length != 0)
                    { 
                        SmellsToAddToSmellLibrary[i] = SmellsToAddCopy;
                        privateLibrary[i] = SmellsToAddToSmellLibrary[i];
                    }
                    else
                    {
                        SmellsToAddToSmellLibrary[i] = privateLibrary[i];
                    }
                }
            }
        }
        else if (SmellsToAddToSmellLibrary.Length < 6)
        {
            SmellsToAddToSmellLibrary = privateLibrary;
        }
        else
        {
            privateLibrary = SmellsToAddToSmellLibrary;
        }

        if (surroundGroupStrings.Length == privateSurroundGroups.Length)
        {
            for (int i = 0; i < surroundGroupStrings.Length; i++)
            {
                if (surroundGroupStrings[i].Length == 0)
                {
                    surroundGroupStrings[i] = privateSurroundGroups[i];

                }
                else
                {
                    string GroupsToAddCopy = "";
                    if (surroundGroupStrings[i].Length > SmellNameLength)
                        surroundGroupStrings[i] = surroundGroupStrings[i].Substring(0, SmellNameLength);
                    foreach (char c in surroundGroupStrings[i])
                        if (char.IsLetterOrDigit(c))
                            GroupsToAddCopy += c;
                    if (GroupsToAddCopy.Length != 0)
                    {
                        surroundGroupStrings[i] = GroupsToAddCopy;
                        privateSurroundGroups[i] = surroundGroupStrings[i];
                    }
                    else
                    {
                        surroundGroupStrings[i] = privateSurroundGroups[i];
                    }
                }
            }
        }
        else if (surroundGroupStrings.Length > 256)
        {
            surroundGroupStrings = privateSurroundGroups;
        }
        else if (surroundGroupStrings.Length < 1)
        {
            surroundGroupStrings = privateSurroundGroups;
        }
        else
        {
            privateSurroundGroups = surroundGroupStrings;
        }
        //Debug.Log("1");
        
    }

    /*
     * lightNumber from 1-6
     * red from 1-255
     * green from 1-255
     * blue from 1-255
     */
    public static void setLight(SurroundPosition surroundPosition, uint lightNumber, byte red, byte green, byte blue)
    {
        if(lightNumber > 6)
        {
            lightNumber = 6;
        }
        string toSend = "";
        if (surroundPosition == SurroundPosition.All)
            toSend = "[" + surroundPosition.ToString() + ",N" + lightNumber.ToString("D1") + red.ToString("D3") + green.ToString("D3") + blue.ToString("D3") + "]";
        else 
            toSend = "[G<" + (byte)surroundPosition + ">,N" + lightNumber.ToString("D1") + red.ToString("D3") + green.ToString("D3") + blue.ToString("D3") + "]" ;
        sendMessageToCilia(toSend);
    }

    public static void setFan(SurroundPosition surroundPosition, string fanSmell, uint fanNumber, byte fanSpeed)
    {
        if(fanNumber > 6)
        {
            fanNumber = 6;
        }
        string toSend = "";
        if(surroundPosition == SurroundPosition.All)
            toSend = "[" + surroundPosition.ToString() + "F," + fanSmell + "," + fanSpeed.ToString("D3") + "]";
        else
            toSend = "[G<" + (byte)surroundPosition + ">F," + fanSmell + "," + fanSpeed.ToString("D3")+ "]";
        sendMessageToCilia(toSend);
    }

    // Use this for initialization
    void Start()
    {
        /*Connect Networking*/
        CiliaClient.Connect(CiliaIP, CiliaPort);
        CiliaStream = CiliaClient.GetStream();
        /*Start Creating Messages for setting up library and prfiles*/
        string smellsForLibraryMessage = "[!#AddToLibrary|";
        string gameProfileMessage = "[!#LoadProfile|" + GameProfileName + "," + (int)DefaultSurroundGroup + ",";
        for (int i = 0; i < SmellsToAddToSmellLibrary.Length; i++)
        {
            smellsForLibraryMessage += SmellsToAddToSmellLibrary[i] + ",";
        }
        for (int i = 0; i < 6; i++)
        {
            gameProfileMessage += SmellsToAddToSmellLibrary[i] + ",";
        }
        gameProfileMessage += getLightString(Light1) + ",";
        gameProfileMessage += getLightString(Light2) + ",";
        gameProfileMessage += getLightString(Light3) + ",";
        gameProfileMessage += getLightString(Light4) + ",";
        gameProfileMessage += getLightString(Light5) + ",";
        gameProfileMessage += getLightString(Light6) + ",";
        smellsForLibraryMessage = smellsForLibraryMessage.TrimEnd(',');
        smellsForLibraryMessage += "]";
        
        for (int i = 0; i < privateSurroundGroups.Length; i++)
        {
            gameProfileMessage += privateSurroundGroups[i] + ",";
        }
        gameProfileMessage = gameProfileMessage.TrimEnd(',');
        gameProfileMessage += "]";
        sendMessageToCilia(smellsForLibraryMessage);
        sendMessageToCilia(gameProfileMessage);

	}

    static string getLightString(Neopixel neo)
    {
        return neo.redValue.ToString("D3") + neo.greenValue.ToString("D3") + neo.blueValue.ToString("D3");
    }

    static void sendMessageToCilia(string messageToSend)
    {
        message = System.Text.Encoding.ASCII.GetBytes(messageToSend);

        CiliaStream.Write(message, 0, message.Length);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnApplicationQuit()
    {
        Debug.Log("closing client\n");
        CiliaStream.Close();
        CiliaClient.Close();
    }

    public void AddSurroundGroups()
    {
        string myString = "public enum SurroundPosition { ";
        if (surroundGroupStrings.Length > 1)
        {
            myString += "" + surroundGroupStrings[0];
            for (int i = 1; i < surroundGroupStrings.Length; i++)
            {
                myString += ", " + surroundGroupStrings[i];
            }
            myString += ", All};";
            string[] mystrings = { myString, "" };
            File.WriteAllLines("Assets/CiliaPlugin/Scripts/SurroundPosition.cs", mystrings);
            Debug.Log("cat");
        }
    }
}
