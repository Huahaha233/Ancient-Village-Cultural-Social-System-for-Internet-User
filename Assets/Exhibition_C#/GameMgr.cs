using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour
{
    //存储浏览过程中玩家的信息
    public static GameMgr instance;

    public string id;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }
}
