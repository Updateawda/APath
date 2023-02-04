using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class AStartManger
{
    private static AStartManger instance;
    public static AStartManger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AStartManger();
            }
            return instance;
        }
    }
    private int mapw;
    private int maph;
    //地图相关所有格子对象容器
    public AStartNode[,] nodes;
    private List<AStartNode> openlist = new List<AStartNode>();
    private List<AStartNode> closelist = new List<AStartNode>();
    public void InitMap(int w, int h)
    {
        mapw = w;
        maph = h;
        nodes = new AStartNode[w, h];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                AStartNode node;
                if (i==4&&(j==1||j==2||j==3))
                {
                    node = new AStartNode(i, j, NodeType.stop);
                }
                else
                {
                    node = new AStartNode(i, j, NodeType.walk);
                    
                }
                nodes[i, j] = node;

            }
        }
    }
    public List<AStartNode> FindPath(Vector2 startpos, Vector2 endpos)
    {
        //首先判断传入的两个点是否合法
        //1.在地图范围内

        if (startpos.x >= mapw || startpos.x < 0 || startpos.y >= maph || startpos.y < 0
            || endpos.x >= mapw || endpos.x < 0 || endpos.y >= maph || endpos.y < 0)
        {
            Debug.Log("不在地图范围内");
            return null;
        }

        //得到起点和终点对应的格子
        AStartNode start = nodes[(int)startpos.x, (int)startpos.y];
        AStartNode end = nodes[(int)endpos.x, (int)endpos.y];
        //2.是否为阻挡点
        if (start.type == NodeType.stop || end.type == NodeType.stop)
        {
            Debug.Log("当前为阻挡点");
            return null;
        }
        start.father = null;
        //清空列表
        openlist.Clear();
        closelist.Clear();
        //把开始点放入关闭列表中
        closelist.Add(start);
        //从起点开始找周围的点，放入开启列表当中
        //判断这些点是否为边界，是否为阻挡，是否在开启或者关闭列表中，如果都不是放入开启
        while (true)
        {
            FindNodeList(start.x - 1, start.y - 1,start,end);
            FindNodeList(start.x, start.y - 1, start, end);
            FindNodeList(start.x + 1, start.y - 1, start, end);
            FindNodeList(start.x - 1, start.y, start, end);
            FindNodeList(start.x + 1, start.y, start, end);
            FindNodeList(start.x - 1, start.y + 1, start, end);
            FindNodeList(start.x, start.y + 1, start, end);
            FindNodeList(start.x + 1, start.y + 1, start, end);

            if (openlist.Count == 0)
            {
                Debug.Log("为死路");
                return null;
            }
            //选出开启列表中寻路消耗最小的点，放入关闭列表，再从开启列表中移除
            for (int i = 0; i < openlist.Count; i++)
            {
                openlist[i].g = Mathf.Sqrt((openlist[i].x - startpos.x) * (openlist[i].x - startpos.x) + (openlist[i].y - startpos.y) * (openlist[i].y - startpos.y));
                openlist[i].h = Mathf.Abs(openlist[i].x - endpos.x) + Mathf.Abs(openlist[i].y - endpos.y);
                openlist[i].f = openlist[i].g + openlist[i].h;
            }
            openlist.Sort(sortopenlist);
            closelist.Add(openlist[0]);
            //复制新的起点进行下一个寻路
            start = openlist[0];
            openlist.RemoveAt(0);
            //如果这个点是终点，那么返回最终结果
            //如果不是终点那么继续寻路
            if (start.x == endpos.x && start.y == endpos.y)
            {
                List<AStartNode> path = new List<AStartNode>();
                path.Add(end);
                while(end.father!=null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                path.Reverse();
                return path ;
            }
        }
    }
    private int sortopenlist(AStartNode a, AStartNode b)
    {
        if (a.f.Equals(b.f))
        {
            return 1;
        }
        return a.f > b.f ? 1 : -1;
    }

    private void FindNodeList(int x, int y,AStartNode start,AStartNode end)
    {
        if (x < 0 || x >= mapw || y < 0 || y >= maph)
        {
            return;
        }
        AStartNode node = nodes[x, y];
        if (node == null || node.type == NodeType.stop ||
            openlist.Contains(node) || closelist.Contains(node))
        {
            return;

        }
        node.father= start;
        node.g= Mathf.Sqrt((x - start.x) * (x - start.x) + (y - start.y) * (y - start.y));
        node.h= Mathf.Abs(x - end.x) + Mathf.Abs(y - end.y);
        node.f = node.g + node.h;
        
        openlist.Add(node);
    }
}
