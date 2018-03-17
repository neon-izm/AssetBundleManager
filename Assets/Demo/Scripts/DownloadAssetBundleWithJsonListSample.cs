using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoyaFramework;
using AutoyaFramework.AssetBundles;
using UnityEngine;

public class DownloadAssetBundleWithJsonListSample : MonoBehaviour
    {

        /// <summary>
        /// テスト用。全アセットバンドルを消去する
        /// </summary>
        public void DeleteAllAssetBundleCaches()
        {

        }

        // 特定のjsonで記述された「アセットバンドル情報をまとめたリスト」をダウンロードするサンプル
        // 
        IEnumerator Start()
        {

            /*
                this is sample of "preload assetBundles feature".
                the word "preload" in this sample means "download assetBundles without use."
                preloaded assetBundles are stored in storage cache. no difference between preloaded and downloaded assetBundles.
                case2:get preloadList from web, then get described assetBundles.
             */

            Autoya.AssetBundle_DownloadAssetBundleListsIfNeed(status => { }, (code, reason, autoyaStatus) => { });

            // wait downloading assetBundleList.
            while (!Autoya.AssetBundle_IsAssetBundleFeatureReady())
            {
            
                yield return null;
            }
           
        var assetBundleLists = Autoya.AssetBundle_AssetBundleLists();

        // create sample preloadList which contains all assetBundle names in assetBundleList.
        var assetBundleNames = assetBundleLists.SelectMany(list => list.assetBundles).Select(abInfo => abInfo.bundleName).ToArray();
        var newPreloadList = new PreloadList("samplePreloadList", assetBundleNames);

        Autoya.AssetBundle_PreloadByList(
            newPreloadList,
            (willLoadBundleNames, proceed, cancel) =>
            {
                proceed();
            },
            progress =>
            {
                Debug.Log("progress:" + progress);
            },
            () =>
            {
                Debug.Log("preloading all listed assetBundles is finished.");
                
                // then, you can use these assetBundles immediately. without any downloading.
                Autoya.AssetBundle_LoadAsset<GameObject>(
                    "Assets/Demo/____ASSET_BUNDLES/unitychan_std/Prefabs/UnityChan_Std.prefab",
                    (assetName, prefab) =>
                    {
                        Debug.Log("asset:" + assetName + " is successfully loaded as:" + prefab);

                        // instantiate asset.
                        Instantiate(prefab);
                    },
                    (assetName, err, reason, status) =>
                    {
                        Debug.LogError("failed to load assetName:" + assetName + " err:" + err + " reason:" + reason);
                    }
                );
                
                Autoya.AssetBundle_LoadAsset<GameObject>(
                   "Assets/Demo/____ASSET_BUNDLES/unitychan_crs/Prefabs/UnityChan_Crs.prefab",
                   (assetName, prefab) =>
                   {
                       Debug.Log("asset:" + assetName + " is successfully loaded as:" + prefab);

                       // instantiate asset.
                       Instantiate(prefab, new Vector3(1f, 0, 0),Quaternion.identity);
                   },
                   (assetName, err, reason, status) =>
                   {
                       Debug.LogError("failed to load assetName:" + assetName + " err:" + err + " reason:" + reason);
                   }
               );
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

    void OnApplicationQuit()
    {
        Autoya.AssetBundle_DeleteAllStorageCache();
    }

}
