using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Map
{
    Default
}

public enum GameMode
{
    Default
}

public enum GameQueue
{
    Solo,
    Team
}

[Serializable]
public class GameData
{
    public Map map;
    public GameMode gameMode;
    public GameQueue gameQueue;



public String ToMultiplayQueue(){
    return "solo-queue";
}

}
