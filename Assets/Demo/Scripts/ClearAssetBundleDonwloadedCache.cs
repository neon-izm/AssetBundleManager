using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoyaFramework;
using AutoyaFramework.AssetBundles;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// アセットバンドルのキャッシュクリアを行うためだけのシーンで使う
/// アセットバンドルダウンロード中などは、キャッシュのファイルシステムロックが掛かってしまう。
/// その為、こういった独立したシーンにしておいて、更にOnDestroyで明示的に呼ぶ、という安全策
/// 実際に運用するならexpireのタイミングをちゃんと書いたりすると思いますが、たぶんこういうサンプルアプリだと
/// 同一アセットバンドルのまま、ダウンロードを何回もテストすると思うので…
/// </summary>
public class ClearAssetBundleDonwloadedCache : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        bool ret = Caching.ClearCache();
        Debug.Log("AssetBundle clear cache:" + ret);
    }

    /// <summary>
    /// Caching.ClearCacheがアプリ使用中だとファイルロックされて
    /// 動かない事が多い(Unity2017.2f3+UnityEditor Windows)ので、OnDestroyでキャッシュクリアを呼ぶ
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
