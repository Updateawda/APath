using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAstart : MonoBehaviour
{
    public Material red;
    public Material huang;
    public Material green;

    private Vector2 startpos=Vector2.right*-1;
    private Vector2 endpos;

    Dictionary<string,GameObject> gameObjects = new Dictionary<string,GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        AStartManger.Instance.InitMap(7, 7);
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.name = i+"_"+j;
                gameObjects.Add(go.name, go);
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(i*2,j*2,0);
                AStartNode node = AStartManger.Instance.nodes[i, j];
                if (node.type==NodeType.stop)
                {
                    go.GetComponent<MeshRenderer>().material= red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit))
            {
                hit.transform.gameObject.GetComponent<MeshRenderer>().material = huang;
                string[] pos=hit.transform.name.Split('_');
                if (startpos== Vector2.right * -1)
                {
                    startpos = new Vector2(int.Parse(pos[0]), int.Parse(pos[1]));
                }
                else
                {
                    endpos = new Vector2(int.Parse(pos[0]), int.Parse(pos[1]));
                    List<AStartNode> list=AStartManger.Instance.FindPath(startpos, endpos);
                    if (list!=null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            gameObjects[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                }
                
            }
        }
    }
}
