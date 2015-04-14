using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;



public class CardManager : MonoBehaviour
{   
    public GameObject back_card;
    public Transform m_grid_group;
    public Transform m_start;
    public Transform m_game;
    public Transform m_setting;
    public Transform m_description;

    public Transform m_score;

    public List<Sprite> m_sprites;
    public Sprite m_back_sp;

    public AudioClip s_ready;
    public AudioClip s_right;
    public AudioClip s_wrong;
    public AudioClip s_game_over;
    public AudioClip s_win;


    public int m_row = 3;
    public int m_col = 5;
    int m_total;

    int game_lv;            //游戏等级
    GType game_type;        //游戏类型
    int g_score;            //游戏分数
    GState game_state;      //游戏状态

    UserInfo m_user;
    List<int> card_ids = new List<int>();
    List<GameObject> card_objs = new List<GameObject>();  //所有牌的GameObject

    List<int> card_pics = new List<int>();                //所有牌的图案,每轮游戏开始，重排
    List<int> pre_show_cards = new List<int>();             //这轮游戏中 展示牌  id
    List<int> choose_cards = new List<int>();             //玩家猜的牌
        

    //public Transform 
    void Awake()
    {
        print(" Awake ");
        //Sprite sp = Instantiate(Resources.Load("Pic/2")) as Sprite;

        Transform btn_grid = m_start.FindChild("Game-Type/Type");
        for (int i = 0; i < btn_grid.childCount; ++i)
        {
            GameObject btn = btn_grid.GetChild(i).gameObject;
            btn.name = Convert.ToString(i);
            btn.transform.FindChild("Text").GetComponent<Text>().text = GameDefine.game_name[i];
            btn.GetComponent<Button>().onClick.AddListener(delegate() { OnGameTypeClick(btn); });
        }
    }

    void Start()
    {
        m_grid_group.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        print("Start ");

        game_type = GType.AutoOrder;

        m_start.gameObject.SetActive(true);
        m_game.gameObject.SetActive(false);

    }

    public void StartGame()
    {
        UpdateScore();
        m_grid_group.GetComponent<GridLayoutGroup>().constraintCount = m_col;

        //m_total = m_row * m_col;
        for(int i = 0; i < m_grid_group.childCount; ++i)
            Destroy(m_grid_group.GetChild(i).gameObject);
        card_ids.Clear();
        card_objs.Clear();

        for (int x = 0; x < m_row; ++x)
        {
            for (int y = 0; y < m_col; ++y)
            {
                GameObject obj = Instantiate(back_card) as GameObject;
                obj.transform.SetParent(m_grid_group);
                obj.name = "card" + (x * m_col + y);
                obj.transform.localScale = Vector3.one;
                card_objs.Add(obj);                
                obj.GetComponent<Button>().onClick.AddListener(delegate() { OnCardClick(obj); });

                card_ids.Add(x * m_col + y);
            }
        }

        m_start.gameObject.SetActive(false);
        m_game.gameObject.SetActive(true);
        RoundStart();
    }

    void OnGameTypeClick(GameObject obj)
    {
        int type = int.Parse(obj.name);
        game_type = (GType)type;

        m_description.gameObject.SetActive(true);
        m_description.GetComponent<Text>().text = GameDefine.game_descrption[type];
    }

    public void OnCardClick(GameObject obj)
    {
        print(" game state : " + game_state);
        print(obj.name + " click ");

        if (game_state != GState.Playing)
            return;

        int index = card_objs.IndexOf(obj);
        obj.GetComponent<Image>().sprite = m_sprites[card_pics[index]];

        switch(game_type)
        {
            case GType.AutoOrder:
                if (index == pre_show_cards[choose_cards.Count])
                    ClickRight(index);
                else
                    ClickWrong(index);
                break;

            case GType.AutoSeveral:
                if (pre_show_cards.Contains(index) && !choose_cards.Contains(index))
                    ClickRight(index);
                else
                    ClickWrong(index);
                break;

            case GType.AutoTarget:
                if (pre_show_cards.Contains(index) && !choose_cards.Contains(index))
                    ClickRight(index);
                else
                    ClickWrong(index);
                break;

            case GType.AutoDouble:
                break;
            case GType.ClickAll:
                break;
        }
        

    }

    void ClickWrong(int index)
    {
        audio.PlayOneShot(s_wrong);
        UpdateScore(-1);
        StartCoroutine(GameDefine.DelayToDo(delegate() { ShowOne(false, index); }, 0.5f));
    }

    void ClickRight(int index)
    {
        UpdateScore(2);
        audio.PlayOneShot(s_right);
        choose_cards.Add(index);
        if (choose_cards.Count == pre_show_cards.Count)
        {
            game_state = GState.Celebration;
            audio.PlayOneShot(s_win);
            Invoke("RoundStart", 3.5f);
        }
    }

    void UpdateScore(int add_amount = 0)
    {
        g_score = (add_amount == 0) ? 0: g_score + add_amount;
        m_score.GetComponent<Text>().text = Convert.ToString(g_score);
        //if (add_amount > 0)
        //    audio.PlayOneShot(s_right);
        //else
        //    audio.PlayOneShot(s_wrong);
    }


    //开始一轮
    void RoundStart()
    {
        audio.PlayOneShot(s_ready);

        game_state = GState.Resetting;
        m_total = m_col * m_row;

        //Reset Data: all card pictures
        GenerateCardPic();

        //Choose Data: Pre-Show Card
        ChooseCardToShow();

        //Reset Pic:
        ResetCardPic();
        
    }


    void GenerateCardPic()
    {
        card_pics.Clear();

        switch (game_type)
        {           
            case GType.AutoOrder:
            case GType.AutoSeveral:
            case GType.AutoTarget:                            
                for (int i = 0; i < m_total; ++i)
                {
                    card_pics.Add(UnityEngine.Random.Range(0, m_sprites.Count - 1));
                }
                break;
            case GType.ClickAll:

                List<int> half_card_ids = ChooseNum(m_total / 2);
                for (int i = 0; i < m_total; ++i)
                {
                    card_pics.Add(UnityEngine.Random.Range(0, m_sprites.Count - 1));
                }
                break;
            case GType.AutoDouble:
            default:
                break;
        }

        

    }

    //挑展示牌
    void ChooseCardToShow()
    {
        pre_show_cards.Clear();
        choose_cards.Clear();

        switch (game_type)
        {
            
            case GType.AutoOrder:
            case GType.AutoSeveral:
                pre_show_cards = ChooseNum(5);
                break;
            case GType.AutoTarget:
                pre_show_cards = ChooseNum(1);
                break;
            case GType.AutoDouble:
                int N = UnityEngine.Random.Range(1, 3);
                pre_show_cards = ChooseNum(2 * N);
                break;
            case GType.ClickAll:
            default:
                break;
        }

    }

    //从m_total 中挑num个数，仅保存id 即下标
    List<int> ChooseNum(int num)
    {
        List<int> result = new List<int>();
        while (result.Count < num)
        {
            for (int i = 0; i < m_total; ++i)
            {
                //data[i] already choose, don't choose again
                if (result.Contains(i))
                    continue;

                float p = (num - result.Count) / (float)(m_total - result.Count);
                if (GameDefine.Probability(p))
                {
                    result.Add(i);
                    print(" choose data " + i);
                }
            }
        }

        return result;
    }


    void ResetCardPic()
    {        
        switch (game_type)
        {
            //Show all front, T later, show all back
            case GType.AutoOrder:                               
                ShowCards(false);
                ShowInOrder(pre_show_cards);                            
                break;

            case GType.AutoSeveral:
                ShowCards(false);
                ShowCards(true, pre_show_cards);
                StartCoroutine(GameDefine.DelayToDo(delegate() { ShowCards(false, pre_show_cards, GState.Playing); }, 1f));
                break;

            //Show all back
            case GType.AutoTarget:
                ShowCards(true);
                StartCoroutine(GameDefine.DelayToDo(delegate(){ ShowCards(false,null, GState.Playing);},2f));
                break;
            //Show all back
            case GType.AutoDouble:
            case GType.ClickAll:
                ShowCards(false);
                break;
        }
    }

    // is_front, true: front; false: back;   ids:  card ids going to show 
    void ShowCards(bool is_front, List<int> ids = null, GState state = GState.PreShow)
    {
        if (ids == null)
            ids = card_ids;

        game_state = state;

        for (int i = 0; i < ids.Count; ++i)
        {
            int id = ids[i];
            if (is_front)
            {
                print(" card id " + id);
                card_objs[id].GetComponent<Image>().sprite = m_sprites[card_pics[id]];
            }
            else
            {
                card_objs[id].GetComponent<Image>().sprite = m_back_sp;
            }
        }
    }

    void ShowOne(bool is_front, int id)
    {
        if (is_front)
        {
            //print(" show one card id " + id);
            card_objs[id].GetComponent<Image>().sprite = m_sprites[card_pics[id]];
        }
        else
        {
            //print("hide one " + id);
            card_objs[id].GetComponent<Image>().sprite = m_back_sp;
        }
    }

    void ShowInOrder(List<int> ids)
    {
        game_state = GState.PreShow;
        ShowNext(0, ids);
    }

    // current: index of card to show in ids
    void ShowNext(int current_id, List<int> ids)
    {
        print(" show current " + current_id);
        if (current_id > 0)
        {
            if (current_id >= ids.Count)
            {
                ShowOne(false, ids[ids.Count - 1]);
                game_state = GState.Playing;
                return;
            }
            else
            {                
                print("pre " + (current_id - 1));
                ShowOne(false, ids[current_id - 1]);
            }
        }

        ShowOne(true, ids[current_id]);
        int next = current_id + 1;
        StartCoroutine(GameDefine.DelayToDo(delegate() { ShowNext(next, ids); }, 0.5f));                  
    }
 
    ////Show all card, front or back
    //void ShowAllFront()
    //{
    //    for (int i = 0; i < card_objs.Count; ++i)
    //    {
    //        print(" card count " + card_objs.Count + card_objs[i]);
    //        card_objs[i].GetComponent<Image>().sprite = m_sprites[card_pics[i]];
    //    }

    //}

    //void ShowAllBack()
    //{
    //    for (int i = 0; i < card_objs.Count; ++i)
    //    {
    //        card_objs[i].GetComponent<Image>().sprite = m_back_sp;
    //    }
    //    game_state = GState.Playing;
    //}


    public void OnBack()
    {
        m_game.gameObject.SetActive(false);
        m_start.gameObject.SetActive(true);
    }

    public void OnRestart()
    {
        UpdateScore();

        RoundStart();
    }
}




