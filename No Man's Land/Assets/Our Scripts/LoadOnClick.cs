﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOnClick : MonoBehaviour
{

    public void LoadScene(int Scene)
    {
        Application.LoadLevel(Scene);
    }
}
