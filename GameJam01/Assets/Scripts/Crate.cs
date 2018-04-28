using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Crate : NetworkBehaviour
{

    [
        Header("Time before despawn"),
        Tooltip("Nombre de secondes avant la disparition de la crate"),
        RangeAttribute(0, 10)
    ]
    public int secondesBeforeDespawn;
	
    // Use this for initialization
    void Start()
    {
        // Au start on determine ce qui sera le type de bonus.
        Destroy(this.gameObject, secondesBeforeDespawn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On détruit la caisse quand le joueur passe dessus.
        Debug.Log("ça touche");
        if (collision.gameObject.GetComponent<PlayerControl>())
        {

            Destroy(gameObject);
        }
    }
}
