using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoyaFramework;
using AutoyaFramework.AssetBundles;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearAssetBundleDonwloadedCache : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        bool ret = Caching.ClearCache();
        Debug.Log("AssetBundle clear cache:" + ret);
    }

    /// <summary>
    /// Caching.ClearCacheがアプリ使用中だとファイルロックされて
    /// 動かない事が多い(Unity2017.2f3+UnityEditor Windows)ので、OnDestroyで呼ぶ
    /// </summary>
    void OnDestroy()
    {
        Autoya.AssetBundle_DeleteAllStorageCache();
    }

    public void ApplicationQuitAndDeleteAllCache()
    {
        Application.Quit();
    }

    public void GoToDownloadScene()
    {
        SceneManager.LoadScene("DownloadSceneAutoya");
    }

	// Update is called once per frame
	void Update () {
        
		
	}
}
