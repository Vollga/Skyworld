using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform spawn;

    GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider player)
    {
        if(player = playerObject.GetComponent<CapsuleCollider>())
        {
            playerObject.GetComponent<Transform>().position = spawn.position;
        }
    }
}
