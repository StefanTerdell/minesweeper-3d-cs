using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    public TextMesh textMesh;
    new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform.position);
    }

    public void Set(int n)
    {
        if (n == 0)
        {
            textMesh.text = "";
        }
        else
        {
            textMesh.text = n.ToString();
        }
    }
}
