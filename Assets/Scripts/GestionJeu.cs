using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    Vextor2 dir;

    void Start()
    {
        dir = Vector2.right;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.left;
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;
        else if (Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.down;
    }

    void fixedUpdate()
    {
        float x = Mathf.Round(transform.position.x) + dir.x;
        float y = Mathf.Round(transform.position.y) + dir.y;

        transform.position = new Vector2(x, y);
    }

}
