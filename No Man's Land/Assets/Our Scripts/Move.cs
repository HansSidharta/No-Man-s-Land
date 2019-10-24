using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
	public Button leftButton;
	public Button rightButton;
	public GameObject chara;
    private Rigidbody charaBody;
    private float speed = 0.5f;// movement speed
    // Start is called before the first frame update
    void Start()
    {
        charaBody = chara.GetComponent<Rigidbody>();
        Button l = leftButton.GetComponent<Button>();// get left button
        Button r = rightButton.GetComponent<Button>();// get right button
        l.onClick.AddListener(LeftClick);
        r.onClick.AddListener(RightClick);
    }


    private void LeftClick()// if left button is clicked
    {
        chara.transform.Translate(-speed, 0f, 0f);

    }

    private void RightClick()// if right button is clicked
    {
        chara.transform.Translate(speed, 0f, 0f);
    }
}
