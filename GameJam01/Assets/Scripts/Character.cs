using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Character : NetworkBehaviour
{

    private float speed = 1.0F;

    //Synchronized variables
    [SyncVar] Vector2 synchronizedPosition;


    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            transform.position = Vector2.Lerp(transform.position, synchronizedPosition, Time.deltaTime * 10);
        }
        else
        {
            move();

            //Network synchronisation
            SendPosition();
        }
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

    //Network function

    [Client]
    void SendPosition()
    {
        CmdSendMyPositionToServer(transform.position);
    }

    [Command]
    void CmdSendMyPositionToServer(Vector2 position)
    {
        synchronizedPosition = position;
    }
}
