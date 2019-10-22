using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{

    public int Hp = 0;

    Canvas canvas;

    public GameObject ui_go;
    RectTransform ui;

    Slider Hp_ui;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        ui_go = Instantiate(ui_go);
        ui_go.transform.SetParent(canvas.transform, false);
        Hp_ui = ui_go.transform.GetChild(0).GetComponent<Slider>();
        ui = ui_go.GetComponent<RectTransform>();



    }

    // Update is called once per frame
    void Update()
    {
        ui.anchoredPosition = GetRectPostion();
        Hp_ui.value = Hp;
    }

    Vector2 GetRectPostion()
    {
        Vector2 _rectPos = Vector2.one;
        Vector2 _screnPos = Camera.main.WorldToScreenPoint(transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((canvas.transform as RectTransform), _screnPos, canvas.worldCamera, out _rectPos);
        _rectPos += new Vector2(40, 150);
        return _rectPos;

    }
}
