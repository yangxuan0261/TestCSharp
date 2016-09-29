﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TestContainer
{
    public static void print<K, V>(Dictionary<K, V> _dir)
    {
        Console.WriteLine("");
        foreach (KeyValuePair<K, V> kvp in _dir)
        {
            Console.WriteLine("--- key={0},value={1}", kvp.Key, kvp.Value);
        }
    }

    public static void test1()
    {
        Dictionary<string, string> myDic = new Dictionary<string, string>();
        myDic.Add("aaa", "111");
        myDic.Add("bbb", "222");
        myDic.Add("ccc", "333");
        myDic.Add("ddd", "444");

        print(myDic);
        try
        {
            myDic.Add("ddd", "ddd");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("此键已经存在：" + ex.Message);
        }
        //解决add()异常的方法是用ContainsKey()方法来判断键是否存在
        if (!myDic.ContainsKey("ddd"))
            myDic.Add("ddd", "ddd");
        else
            Console.WriteLine("此键已经存在：");


        //而使用索引器来负值时，如果建已经存在，就会修改已有的键的键值，而不会抛出异常
        myDic["ddd"] = "ddd";
        myDic["eee"] = "555";
        print(myDic);

        //使用索引器来取值时，如果键不存在就会引发异常
        try
        {
            Console.WriteLine("不存在的键fff的键值为：" + myDic["fff"]);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine("没有找到键引发异常：" + ex.Message);
        }


        //解决上面的异常的方法是使用ContarnsKey() 来判断时候存在键，如果经常要取健值得化最好用 TryGetValue方法来获取集合中的对应键值
        myDic.Add("fff", "fffffffffffffffff");
        string value = "";
        if (myDic.TryGetValue("fff", out value))
            Console.WriteLine("键fff的值为：" + value);
        else
            Console.WriteLine("没有找到对应键的键值");




        //foreach values
        Console.WriteLine("\n--- foreach values");
        foreach (string s in myDic.Values)
        {
            Console.WriteLine("--- value={0}", s);
        }

        //foreach keys
        Console.WriteLine("\n--- foreach keys");
        var keys = myDic.Keys;
        foreach (string s in keys)
        {
            Console.WriteLine("--- key={0}", s);
        }

        //remove key
        myDic.Remove("aaa");
        print(myDic);

        //size
        Console.WriteLine("--- size={0}", myDic.Count());
        //myDic.Clear();
        //Console.WriteLine("--- size={0}", myDic.Count());

        /* cant remove during foreach
        foreach (KeyValuePair<string, string> kvp in myDic)
        {
            if (kvp.Key == "fff")
            myDic.Remove(kvp.Key);
        }
        Console.WriteLine("--- size={0}", myDic.Count());
        */


        //put key need to be remove to LinkedList(dont use List)
        //and foreach LinkedList, remove from 
        myDic.Add("tt", "ttttt");
        myDic.Add("yy", "yyyyyy");
        Console.WriteLine("--- size={0}", myDic.Count());
        LinkedList<string> tmpList = new LinkedList<string>();
        foreach (string k in myDic.Keys)
        {
            if (k.Count() == 2)
                tmpList.AddFirst(k);
        }

        foreach (string k in tmpList)
            myDic.Remove(k);
        Console.WriteLine("--- size={0}", myDic.Count());

        //lambda find
        myDic.Add("i", "iiii");
        myDic.Add("o", "oooo");
        IEnumerable<KeyValuePair<string, string>> result = myDic.Where(_pair =>
        {
            if (_pair.Key.Count() == 1)
                return true;
            else
                return false;
        });

        IEnumerator<KeyValuePair<string, string>> ret2 = result.GetEnumerator();
        while (ret2.MoveNext())
            Console.WriteLine("--- ret2, key={0}, value={1}", ret2.Current.Key, ret2.Current.Value);
    }

    public static void test2()
    {
        List<int> mylist = new List<int>();
        mylist.Add(111);
        mylist.Add(222);
        Console.WriteLine("--- size={0}", mylist.Count());
    }

    /// <summary>
    /// test list sort
    /// </summary>
    public static void test3()
    {
        Dictionary<int, string> mydic = new Dictionary<int, string>();
        mydic.Add(8, "888");
        mydic.Add(5, "555");
        mydic.Add(7, "777");
        mydic.Add(6, "666");
        print(mydic);

        List<int> keys = mydic.Keys.ToList();
        keys.Sort();
        foreach (int k in keys)
            Console.WriteLine("--- key:{0}, value:{1}", k, mydic[k]);
    }

    struct Info
    {
        public Info(int _a, string _b)
        {
            num = _a;
            name = _b;
        }
        public int num;
        public string name;
    }

    public static void test4()
    {
        Dictionary<int, Info> ht = new Dictionary<int, Info>();
        ht.Add(1, new Info(1, "aaa"));
        ht.Add(2, new Info(2, "asd"));
        ht.Add(3, new Info(3, "bbb"));

        //涉及复制拷贝，效率低
        //List<Info> tmp = new List<Info>(ht.Values);
        //foreach (var w in tmp)
        //{
        //    if (w.name == "asd")
        //    {
        //        ht.Remove(w.num);
        //    }
        //}

        //要删除的东西的key丢在临时容器tmp，然后遍历tmp，remove
        List<int> tmp = new List<int>();
        foreach (var w in ht)
        {
            if (w.Value.name == "asd")
            {
                tmp.Add(w.Key);
            }
        }

        foreach (var key in tmp)
        {
            ht.Remove(key);
        }

        foreach (var w in ht)
        {
            Console.WriteLine("--- key:{0}, value:{1}", w.Key, w.Value.name);
        }

    }

    public static void test5()
    {


        List<int> ht = new List<int>();
        ht.Add(1);
        ht.Add(2);
        ht.Add(3);
        ht.Add(4);
        ht.Add(5);

        for (int i=0;i<ht.Count;++i)
        {
            /*
            if (ht[i] == 3) //正确的删除姿势
            {
                ht.Remove(ht[i]);
                i -= 1; //-1 是因为删除时 后面的 往 前面 挪了一位
            }

            if (ht[i] == 2) //正确的删除姿势
            {
                ht.Remove(ht[i]);
                i -= 1;
            }

            */

            if (ht[i] == 3) //正确的删除姿势
            {
                ht.RemoveAt(i);
                i -= 1;
            }

            if (ht[i] == 2) //正确的删除姿势
            {
                ht.RemoveAt(i);
                i -= 1;
            }
        }

        //错误的删除姿势
        //foreach (var num in ht)
        //{
        //    if (num == 1)
        //        ht.Remove(num);
        //}

        foreach (var num in ht)
            Console.WriteLine(num);
    }

}
