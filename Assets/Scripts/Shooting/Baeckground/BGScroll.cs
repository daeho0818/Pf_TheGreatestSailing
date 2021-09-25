using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    GameObject BG1;
    GameObject BG2;
    void Start()
    {
        BG1 = transform.GetChild(0).gameObject;
        BG2 = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        BG1.transform.Translate(Vector2.right * 5 * Time.deltaTime);
        if (BG1.transform.localPosition.x > 37.21f)
        {
            BG1.transform.Translate(new Vector2(-37.21f * 2, 0));
        }

        BG2.transform.Translate(Vector2.right * 5 * Time.deltaTime);
        if (BG2.transform.localPosition.x > 37.21f)
        {
            BG2.transform.Translate(new Vector2(-37.21f * 2, 0));
        }
    }
}
