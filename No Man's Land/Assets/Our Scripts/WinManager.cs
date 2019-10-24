using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public List<HpController> PlayerList;

    public Text Win_text;
    public float timer;

    public static WinManager ins;
    private void Awake()
    {
        ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        foreach (var hp in GameObject.FindObjectsOfType<HpController>())
        {

            PlayerList.Add(hp);
        }

    }

    public bool Dead = false;
    // Update is called once per frame
    void Update()
    {



        if (PlayerList.Count != 1)
        {




            foreach (var hp in PlayerList)
            {
                if (hp.Hp <= 0 )
                {
                    PlayerList.Remove(hp);
                    Win_text.text = PlayerList[0].gameObject.name + "Win";

                    Dead = true;
                    break;

                }

            }



        }
    }

    /// <summary>
    /// Time to decide
    /// </summary>
    public void TimeWin()
    {
        if (Dead)
        {
            return;
        }

        if (PlayerList[0].Hp == (PlayerList[1].Hp))
        {
            Win_text.text = "Draw";
        }
        else
        {
            if (PlayerList[0].Hp > (PlayerList[1].Hp))
            {
                Win_text.text = PlayerList[0].gameObject.name + "Win";
                PlayerList.Remove(PlayerList[1]);
            }
            else
            {
                Win_text.text = PlayerList[1].gameObject.name + "Win";

                PlayerList.Remove(PlayerList[0]);
            }
        }
        Dead = true;
    }
}
