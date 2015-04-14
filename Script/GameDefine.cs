using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Game Type
public enum GType
{
    AutoOrder = 0,                            //自动展示， 挑出N张牌，pre show, 按顺序找出 pre show 的N张牌
    AutoSeveral,                           //自动展示，挑出N张牌， 找出preshow  的N张牌
    AutoTarget,                           //自动展示， 所有正面T秒，找出所有跟目标一样的牌
    AutoDouble,                           //自动展示， 随机出 2*N 张牌，成对出现，再背面， 挑出刚刚成对的牌
    //ClickDouble,                          //背面展示， 点击    
    ClickAll,                             //点击展示， 先所有反面，点击处正面，找出所有成对的牌，直到全部找完。

}

//Game State
public enum GState
{
    Resetting,                      //重置
    PreShow,                        //预先展示 (有的游戏类型没有预先展示)
    Playing,
    Checking,                       //检查答案
    GameOver,
    Celebration,

}

public class UserInfo
{
    public string m_name;
    public int m_level;
    public int m_nut;                //核桃数

}

class GameDefine
{
    private static GameDefine m_instance = null;
    public static GameDefine Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new GameDefine();
            return m_instance;
        }        
    }
    private static string[] toolbarStrings = { "Toolbar1", "Toolbar2", "Toolbar3" };



        //r为概率，返回是否出现
    public static bool Probability(float r)
    {
        int num = UnityEngine.Random.Range(1, 100);
        return num <= r * 100;
    }

    public static IEnumerator DelayToDo(Action action, float delay_time)
    {
        yield return new WaitForSeconds(delay_time);
        action();
    }

    public static string[] game_name = { "按顺序找牌", "找位置", "找目标", "成双成对", "全部配对" };
    public static string[] game_descrption = {
    "随机依次展示若干张牌,然后盖牌，请按顺序点击找出刚刚出现的牌，正确得2分，错误扣1分",
    "随机依次展示若干张牌,然后盖牌，请点击找出刚刚出现的牌，正确得2分，错误扣1分",
    "随机挑出目标，展示所有正面后盖牌，找出所有目标牌",
    "随机出偶数张牌，成对出现，再盖牌， 挑出刚刚成对的牌",
    //ClickDouble,                          //背面展示， 点击    
    "点击翻牌，找出成对的牌，直到全部找完" };
}


