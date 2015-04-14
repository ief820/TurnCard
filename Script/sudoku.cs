using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 先找所有1的位置，随机生成0~8不重复的数字，依次表示该排在 随机列，再找所有2的位置。。。。。*/

class sudoku:MonoBehaviour
{
    int m_row = 9;
    int m_col = 9;

    int[,] m_number = new int[9,9];            //所有数据            
    bool[,] m_area = new bool[9, 9];              //9个区域内的当前数字
    int m_current;              //当前数字

    void Awake()
    {
        GenMatrix();
    }


    void GenMatrix()
    {

        for (int i = 0; i < m_row; ++i)
        {
            for (int j = 0; j < m_col; ++j)
            {
                m_number[i, j] = 0;
                m_area[i, j] = false;
            }
        }

        //for (int i = 0; i < 9; ++i)
        //    GenData(i);
        for (int i = 0; i < m_row; i = i + 3)
            GenRow(i);

            ShowData();
    }

    void ShowData()
    {
        string s;
        for (int i = 0; i < m_row; ++i)
        {
            s = null;
            for (int j = 0; j < m_col; ++j)
                s = s + m_number[i, j] + " , ";

            print(s);
        }
    }

    void GenData(int x)
    {
        m_current = x + 1;  

        string s = null;
        List<int> result = ChooseNum(9, 9);
        for (int i = 0; i < m_row; ++i)
        {          
            //s = s + result[i] + " , ";
            if (m_number[i, result[i]] == 0 && IsEmpty(i, result[i], m_current))
            {
                m_number[i, result[i]] = m_current;
                SetAera(i, result[i], m_current);
            }
        }
        //print(s);
    }

   

    void SetAera(int row, int col, int number)
    {
        int area = 3 * (row/3) + col/3;
        m_area[area, number - 1] = true;
    }

    bool IsEmpty(int row, int col, int number)
    {
        int area = 3 * (row / 3) + col / 3;
        print("IsEmpty ");
        return m_area[area, number - 1] == false;
    }

    public  List<int> ChooseNum(int num, int total)
    {
        System.Random random = new System.Random();

        List<int> result = new List<int>();
        string str = null;
        while (result.Count < num)
        {
            int x = UnityEngine.Random.Range(0,total);
            if (!result.Contains(x))
            {
                str = str + x + " ; ";
                result.Add(x);
            } 
            
        }

        //print(str);
        return result;
    }

    //只用考虑列，不用考虑区域
    void GenRow(int row)
    {
        List<int> null_cols = new List<int>();
        List<int> arrange = Arrange(Numbers());
        if(row == 0)
            for(int i = 0; i < arrange.Count; ++i)
            {
                m_number[row, i] = arrange[i];
                SetAera(row, i, arrange[i]);
            }
        else
            for (int i = 0; i < arrange.Count; ++i)
            {
                if (m_number[0, i] != arrange[i] && (i == 3 ? true: m_number[3,i] != arrange[i]))
                {
                    m_number[row, i] = arrange[i];
                    SetAera(row, i, arrange[i]);
                }
                else
                {
                    null_cols.Add(i);
                }                
            }

        if(null_cols.Count > 0)
            ChangePos(arrange, null_cols, row);
            
    }

    //null_cols 里为row 行有冲突的列，把 arrange 里数据交换，解决冲突
    void ChangePos(List<int> arrange, List<int> null_cols, int row)
    {        
        while(null_cols.Count > 0)
        {
            int i = 0; 
            print(" 有冲突的列 " + null_cols[i] + " 行 " + row + " 个数 "+ null_cols.Count);
            int col = null_cols[i];

            if (m_number[0, col] != arrange[col] && (row == 3 ? true : m_number[3, col] != arrange[col]))
            {
                m_number[row, col] = arrange[col];
                SetAera(row, col, arrange[col]);
            }
            else
            {
                for (int j = null_cols.Count - 1; j > i; --j)
                {
                    int choose = null_cols[j];
                    if (m_number[0, col] != arrange[choose] && (row == 3 ? true : m_number[3, col] != arrange[choose]))
                    {
                        Swap(arrange, col, choose);
                        m_number[row, col] = arrange[col];
                        SetAera(row, col, arrange[col]);
                        print("冲突解决交换列 " + col + "  " + choose);
                        print(" and then , what to do hahah, I'm so boring, I want to go home to see my big house , I don't want to work");
                        print(" I want to go home, I want to travel, hahaha  lol, hihihi ");
                        print(" and then, and hi, and I'm not comfortable, I want to go home");
                        print(" I'm really boring, I don't know what to do, oh , I want to by another house, I have been worked for 2 years in total, yeah ");
                        print(" I'm in a period, hello hi hi hi, I want to see new house, Can I buy a twenty years old house? Is good, 1.5 million ");
                        print(" I come from hubei province, city suizhou, I want to see my new house, what makes a house worth of money, place? area, or age ");
                        print(" I want to be rich, I want to be a boss, No.1 ");
                        break;
                    }
                }
            }                        
        }           
    }

    void Swap(List<int> data, int a, int b)
    {
        int temp;
        temp = data[a];
        data[a] = data[b];
        data[b] = temp;
    }

    //source 的一个随机排列, 并且不改变source 的值
    public List<int> Arrange(List<int> source)
    {
        string s = null;
        List<int> result = new List<int>();
        while (source.Count > 0)
        {
            int x = UnityEngine.Random.Range(0, source.Count - 1);
            result.Add(source[x]);
            s = s + source[x] + " , ";
            source.RemoveAt(x);            
        }
        print(s);
        return result;
    }

    public List<int> Numbers()
    {
        List<int> nums = new List<int>();
        for (int i = 0; i < 9; ++i)
            nums.Add(i + 1);

        return nums;
    }

    public List<int> Copy(List<int> source)
    {
        List<int> nums = new List<int>();
        for (int i = 0; i < source.Count; ++i)
            nums.Add(source[i]);

        return nums;
    }
}

