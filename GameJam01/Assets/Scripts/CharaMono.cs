using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharaMono : MonoBehaviour
{

    private float speed = 1.0F;

    //Synchronized variables


    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
		move ();
    }

    void move()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
}
