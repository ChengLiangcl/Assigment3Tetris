using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBlock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] block;
    void Start()
    {
        GenerateRandomBlock();
    }

    public void GenerateRandomBlock(){
        Instantiate(block[Random.Range(0,block.Length)], transform.position, Quaternion.identity);
    }

}
