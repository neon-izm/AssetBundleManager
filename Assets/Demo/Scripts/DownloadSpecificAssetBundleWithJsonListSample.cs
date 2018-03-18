using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoyaFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// webサイト上にjson形式でダウンロードするアセットバンドルのリストを置いておいて、そのリストにあるアセットバンドルを依存性含めて
/// ローカルキャッシュに保存するサンプル
/// </summary>
public class DownloadSpecificAssetBundleWithJsonListSample : MonoBehaviour
{

    //ダウンロードして良いか聞く為のダイアログ
    [SerializeField]
    SimpleModalDialog Dialog;


    public void DownloadBySpecificJsonListName(string jsonListName="unity_chan_crs.json")
    {
        StartCoroutine(DownloadBySpecificJsonListNameCoroutine(jsonListName));
    }


    IEnumerator DownloadBySpecificJsonListNameCoroutine(string jsonListName )
    {
        //jsonListNameを指定して、
        //ASSETBUNDLES_URL_DOWNLOAD_PRELOADLIST + jsonListNameのリストをインターネット経由で取得する。
        //その後、取得したjsonの中で指定されたアセットバンドルをpreLoad(つまりダウンロード)する

        Autoya.AssetBundle_DownloadAssetBundleListsIfNeed(status => { }, (code, reason, autoyaStatus) => { });

        // wait downloading assetBundleList.
        while (!Autoya.AssetBundle_IsAssetBundleFeatureReady())
        {

            yield return null;
        }

        /*
			get preloadList from web.
			the base filePath settings is located at AssetBundlesSettings.ASSETBUNDLES_URL_DOWNLOAD_PRELOADLIST.

			this preloadList contains 1 assetBundleName, "bundlename", contains 1 asset, "textureName.png"

			note that:
				this feature requires the condition:"assetBundleList is stored." for getting assetBundleInfo. (crc, hash, and dependencies.)
		 */

        //使わないけど、インターネット上のフルパスは何か、を表示しておく
        var fullPathForJsonListURL = AutoyaFramework.Settings.AssetBundles.AssetBundlesSettings.ASSETBUNDLES_URL_DOWNLOAD_PRELOADLIST + jsonListName;
        Debug.Log("このURLのjsonに書いてある未キャッシュのアセットバンドルを全部ダウンロードします"+fullPathForJsonListURL);

        // download preloadList from web then preload described assetBundles.
        Autoya.AssetBundle_Preload(
            jsonListName,
            (willLoadBundleNames, proceed, cancel) =>
            {
                //ここで、（ダウンロード予定のリストは取得した後、アセットバンドルのダウンロードを始める直前の処理を差し込めます
                //ユースケースとしては「〇〇バイトのダウンロードを行います。よろしいですか？」みたいな感じです。
                var totalWeight = Autoya.AssetBundle_GetAssetBundlesWeight(willLoadBundleNames);

                //もし、ダウンロード済だったらダイアログを出さずにさっさとこの関数を抜けたい。という場合は以下のように書いてください
                /*
                if (totalWeight < 1)
                {
                    proceed();
                }
                */
                Debug.Log(jsonListName + ":------will loading---------" + totalWeight + " byte");
                foreach (var item in willLoadBundleNames)
                {
                    Debug.Log(item);
                }
                //ダイアログを出して、ダウンロードして良いか聞く。これは便利…
                Dialog.Show(new UnityEngine.Events.UnityAction( proceed), new UnityEngine.Events.UnityAction(cancel),totalWeight+"バイトのダウンロードを行います。良いですか");
                Debug.Log("------end------");
                
            },
            progress =>
            {
                //アセットバンドルのダウンロードが一個終わる度にここが呼び出され、progressの値が0から1に増えていきます。
                //大変残念ですが、ダウンロードすべきアセットバンドルが1個だけの場合は、このprogressは1だけになります。
                //なぜかというと1個づつアセットバンドルがダウンロード完了した毎に、Autoyaはprogressのイベントを発火するためです。
                //こういう小さなデモアプリだと1しか出なくて不便だ…と思われるかもしれませんが、実際の運用では問題ないです。
                Debug.Log("progress:" + progress);
            },
            () =>
            {
                //ここでダウンロードが全部終わった、あるいは全部キャッシュ済みだった時の処理を書く
                //Instanciateするとか、実際に使うシーンに遷移するとか
                //一応今回はメインで使うシーンに遷移、というパターンのデモアプリにしています。
                Debug.Log("preloading all listed assetBundles is finished.");


                //もし、ダウンロード直後にInstanciateしたい、とかなら、以下のように書きます
                /*
                Autoya.AssetBundle_LoadAsset<GameObject>(
                   "Assets/Demo/____ASSET_BUNDLES/unitychan_crs/Prefabs/UnityChan_Crs.prefab",
                   (assetName, prefab) =>
                   {
                       Debug.Log("asset:" + assetName + " is successfully loaded as:" + prefab);

                       // instantiate asset.
                       Instantiate(prefab, new Vector3(1f, 0, 0), Quaternion.identity);
                   },
                   (assetName, err, reason, status) =>
                   {
                       Debug.LogError("failed to load assetName:" + assetName + " err:" + err + " reason:" + reason);
                   }
               );
               */
            },
            (code, reason, autoyaStatus) =>
            {
                Debug.LogError("preload failed. code:" + code + " reason:" + reason);
            },
            (downloadFailedAssetBundleName, code, reason, autoyaStatus) =>
            {
                Debug.LogError("failed to preload assetBundle:" + downloadFailedAssetBundleName + ". code:" + code + " reason:" + reason);
            },
            10 // 10 parallel download! you can set more than 0.
        );
    }
    public void GoToCacheClearScene()
    {
        SceneManager.LoadScene("ClearCacheScene");
    }


}
