using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameMgr:MonoBehaviour
{
    //存储浏览过程中玩家的信息
    public static GameMgr instance;
    public string id;
    //第一个string为房间名，第二个为资源名称，第三个为资源类型
    public Dictionary<string, Dictionary<string, string>> resourelist = new Dictionary<string, Dictionary<string, string>>();
    public List<string> RoomNameList = new List<string>();//存储房间名称的列表
    public GameMgr()
    {
        instance=this;
    }
}
