using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStartManger 
{
    private static AStartManger instance;
    public static AStartManger Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new AStartManger();
            }
            return instance;
        }
    }
    private int mapw;
    private int maph;
    //��ͼ������и��Ӷ�������
    public AStartNode[,] nodes;
    private List<AStartNode> openlist=new List<AStartNode>();
    private List<AStartNode> closelist=new List<AStartNode>();
    public void InitMap(int w,int h)
    {
        maph = w;
        mapw = h;
        nodes = new AStartNode[w, h];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                AStartNode node = new AStartNode(i, j, Random.Range(1,20)<10? NodeType.stop : NodeType.walk);
                nodes[i,j] = node;
                
            }
        }
    }
    public List<AStartNode> FindPath(Vector2 startpos,Vector2 endpos)
    {
        //�����жϴ�����������Ƿ�Ϸ�
        //1.�ڵ�ͼ��Χ��
        
        if (startpos.x>mapw||startpos.x<0|| startpos.y > maph|| startpos.y < 0
            || endpos.x > mapw || endpos.x < 0 || endpos.y > maph || endpos.y < 0)
        {
            Debug.Log("���ڵ�ͼ��Χ��");
            return null;
        }
        
        //�õ������յ��Ӧ�ĸ���
        AStartNode start = nodes[(int)startpos.x, (int)startpos.y];
        AStartNode end = nodes[(int)endpos.x, (int)endpos.y];
        //2.�Ƿ�Ϊ�赲��
        if (start.type==NodeType.stop|| end.type == NodeType.stop)
        {
            Debug.Log("��ǰΪ�赲��");
            return null;
        }
        //����б�
        openlist.Clear();
        closelist.Clear();
        //�ѿ�ʼ�����ر��б���
        closelist.Add(start);
        //����㿪ʼ����Χ�ĵ㣬���뿪���б���
        //�ж���Щ���Ƿ�Ϊ�߽磬�Ƿ�Ϊ�赲���Ƿ��ڿ������߹ر��б��У���������Ƿ��뿪��
        while(true)
        {
            FindNodeList(start.x - 1, start.y - 1);
            FindNodeList(start.x , start.y - 1);
            FindNodeList(start.x +1, start.y - 1);
            FindNodeList(start.x - 1, start.y );
            FindNodeList(start.x +1, start.y );
            FindNodeList(start.x - 1, start.y +1);
            FindNodeList(start.x , start.y +1);
            FindNodeList(start.x +1, start.y +1);
           
            if (openlist.Count == 0)
            {
                Debug.Log("Ϊ��·");
                return null;
            }
            //ѡ�������б���Ѱ·������С�ĵ㣬����ر��б��ٴӿ����б����Ƴ�
            for (int i = 0; i < openlist.Count; i++)
            {
                openlist[i].g = Mathf.Sqrt((openlist[i].x - startpos.x) * (openlist[i].x - startpos.x) + (openlist[i].y - startpos.y) * (openlist[i].y - startpos.y));
                openlist[i].h = Mathf.Abs(openlist[i].x - endpos.x) + Mathf.Abs(openlist[i].y - endpos.y);
                openlist[i].f = openlist[i].g + openlist[i].h;
            }
            openlist.Sort(sortopenlist);
            closelist.Add(openlist[0]);
            //�����µ���������һ��Ѱ·
            start = openlist[0];
            openlist.RemoveAt(0);
            //�����������յ㣬��ô�������ս��
            //��������յ���ô����Ѱ·
            if (start.x == endpos.x && start.y == endpos.y)
            {
                Debug.Log("�ҵ����յ�");
                closelist.Reverse();
                return closelist;
            }
        }
    }
    private int sortopenlist(AStartNode a,AStartNode b)
    {
        if (a.f.Equals(b.f))
        {
            return 1;
        }
        return a.f > b.f ? 1 : -1;
    }

    private void FindNodeList(int x,int y)
    {
        if (x<0||x>=mapw||y<0||y>=maph)
        {
            return;
        }
        AStartNode node = nodes[x, y];
        if (node == null || node.type == NodeType.stop ||
            openlist.Contains(node) || closelist.Contains(node))
        {
            return;

        }
        openlist.Add(node);
    }
}
