using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public RectTransform uiGroup;

    public void Enter()
    {
        uiGroup.anchoredPosition = Vector3.zero;
    }
    public void Exit()
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }


}
