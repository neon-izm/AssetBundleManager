using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 書き捨てのモーダルダイアログ
/// あんまりマネしないように…
/// </summary>

public class SimpleModalDialog : MonoBehaviour {

    [SerializeField]
    RectTransform panel;
    [SerializeField]
    Button OKbutton;
    [SerializeField]
    Button Cancelbutton;
    [SerializeField]
    Text message;

    public void Hide()
    {
        OKbutton.onClick.RemoveAllListeners();
        Cancelbutton.onClick.RemoveAllListeners();
        panel.gameObject.SetActive(false);
    }

    public void Show(UnityEngine.Events.UnityAction ok, UnityEngine.Events.UnityAction cancel,string dialogMessage = "")
    {
        panel.gameObject.SetActive(true);
        OKbutton.onClick.AddListener(ok);
        OKbutton.onClick.AddListener(Hide);
        Cancelbutton.onClick.AddListener(cancel);
        Cancelbutton.onClick.AddListener(Hide);
        message.text = dialogMessage;
    }



	// Use this for initialization
	void Start () {
		if(OKbutton==null || Cancelbutton== null || panel== null)
        {
            Debug.LogError("SimpleModalDialogの設定が変。インスペクタ上で確認してください");
            Destroy(this);
        }

        Hide();
	}
	
}
