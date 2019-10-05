using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBlock : MonoBehaviour
{
    // Start is called before the first frame update
    private float previousTime;//The prevoious Time
    public float fallTime = 0.9f;//Set up the fall time
    public static int height = 20;//Give the height of boundary
    public static int width = 10;
    public Vector3 rotation;
    public AudioSource direction;
    public AudioSource switchShape;
    public AudioSource blockGone;
    private static Transform[,] grids = new Transform[width, height];
    int points = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction.Play();

            transform.position += new Vector3(-1, 0, 0);
           
            if (!isMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }


        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction.Play();
            transform.position += new Vector3(1, 0, 0);
            if (!isMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }

        }
        /*
         * 
        */
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switchShape.Play();
            transform.RotateAround(transform.TransformPoint(rotation), new Vector3(0, 0, 1), 90);
            if (!isMove())
            {
                transform.RotateAround(transform.TransformPoint(rotation), new Vector3(0, 0, 1), -90);
            }

        }



        if (Time.time - previousTime > fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
            if (!isMove())
            {
                transform.position -= new Vector3(0, -1, 0);
            }
        }
        else if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
            if (!isMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                FindObjectOfType<GenerateBlock>().GenerateRandomBlock();
            }
        }
    }


    bool isMove()
    {
        foreach (Transform elements in transform)
        {
            int x = Mathf.RoundToInt(elements.transform.position.x);
            int y = Mathf.RoundToInt(elements.transform.position.y);
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return false;
            }
            if(grids[x,y]!=null){
                return false;
            }
        }
        return true;
    }
    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                points = points + 10;
                Debug.Log("Your current points are" + points);
                blockGone.Play();
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grids[j, i] == null)
                return false;
        }

        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grids[j, i].gameObject);
            grids[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grids[j, y] != null)
                {
                    grids[j, y - 1] = grids[j, y];
                    grids[j, y] = null;
                    grids[j, y - 1].transform.position = grids[j, y - 1].transform.position -new Vector3(0, 1, 0);
                }
            }
        }
    }




    void AddToGrid()
    {
        /*
        foreach (Transform children in transform)
        {
            int X = Mathf.RoundToInt(children.transform.position.x);
            int Y = Mathf.RoundToInt(children.transform.position.y);

            grids[X, Y] = children;
        }
        */
        for (int i = 0; i < transform.childCount; i++){
            int X = Mathf.RoundToInt(transform.GetChild(i).transform.position.x);
            int Y = Mathf.RoundToInt(transform.GetChild(i).transform.position.y);
            grids[X, Y] = transform.GetChild(i);
        }
    }
}
