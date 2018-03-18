using AutoyaFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Autoyaのアセットバンドルを利用したInstanciateのサンプル
/// 凄いところとして、前のシーンでダウンロードしていないprefabでも、パスを指定してあげれば
/// その場で存在しないアセットバンドルのダウンロードも一緒に行ってくれます。
/// とは言え、当然ダウンロード時間が掛かるので、あらかじめダウンロードしておきましょう
/// </summary>
public class AssetBundleInstanciate : MonoBehaviour {

    /// <summary>
    /// 実際のゲームでは生成すべきアセットバンドルの名前とか、初期の出現位置情報を外部パラメータで持っていると思う
    /// その辺りは個別のゲームごとに考えます。
    /// </summary>
    string[] assetBundleNames = { "Assets/Demo/____ASSET_BUNDLES/unitychan_crs/Prefabs/UnityChan_Crs.prefab", "Assets/Demo/____ASSET_BUNDLES/unitychan_std/Prefabs/UnityChan_Std.prefab" };

    public void InstanciateCrs()
    {
        StartCoroutine(InstanciatePrefab(assetBundleNames[0],new Vector3(-1,0,0)));
    }


    public void InstanciateStd()
    {
        StartCoroutine(InstanciatePrefab(assetBundleNames[1],new Vector3(1,0,0)));
    }

    IEnumerator InstanciatePrefab(string prefabName, Vector3 position)
    {
        Autoya.AssetBundle_DownloadAssetBundleListsIfNeed(status => { }, (code, reason, autoyaStatus) => { });

        // wait downloading assetBundleList.
        while (!Autoya.AssetBundle_IsAssetBundleFeatureReady())
        {

            yield return null;
        }

        Autoya.AssetBundle_LoadAsset<GameObject>(
                  prefabName,
                  (assetName, prefab) =>
                  {
                      Debug.Log("asset:" + assetName + " is successfully loaded as:" + prefab);

                       // instantiate asset.
                       Instantiate(prefab, position, Quaternion.identity);
                  },
                  (assetName, err, reason, status) =>
                  {
                      Debug.LogError("failed to load assetName:" + assetName + " err:" + err + " reason:" + reason);
                  }
              );
    }
	
	public void BackToDownloadScene()
    {
        SceneManager.LoadScene("DownloadSceneAutoya");
    }
}
