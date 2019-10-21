using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObj : NetworkBehaviour
{
    public GameObject PlayerPrefab;
    GameObject myPlayerUnit;
    

    void Start()
    {
        if (hasAuthority == false)
        {
            return;
        }

        Debug.Log("Player Spawned");

        CmdSpawnPlayer();


    }

    [Command]
    void CmdSpawnPlayer()
    {

        GameObject unit = Instantiate(PlayerPrefab);

        myPlayerUnit = unit;

        NetworkServer.SpawnWithClientAuthority(unit, connectionToClient);

    }
}
