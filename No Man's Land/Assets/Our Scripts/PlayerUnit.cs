using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour
{

    void Update()
    {
        if (hasAuthority == false)
            return;

        else
        {
            if (Input.GetKeyDown(KeyCode.W))
                this.transform.Translate(0, 0, 1);

            if (Input.GetKeyDown(KeyCode.S))
                this.transform.Translate(1, 0, -1);

            if (Input.GetKeyDown(KeyCode.A))
                this.transform.Translate(-1, 0, 0);

            if (Input.GetKeyDown(KeyCode.D))
                this.transform.Translate(1, 0, 0);

            if (Input.GetKeyDown(KeyCode.LeftControl))
                this.transform.Translate(0, -1, 0);

            if (Input.GetKeyDown(KeyCode.Space))
                this.transform.Translate(0, 1, 0);

            
        }

        

    }
}
