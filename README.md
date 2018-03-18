# Autoya Assetbundle Sample
#### 含まれている機能
- AssetBundleのダウンロード(preload)
- AssetGrapthTools を利用したAutoya向けアセットバンドルリスト出力

#### プロジェクトの概要
Unity向けの各種処理が全部入りされているAutoyaの中でAssetBundleを利用したサンプルプロジェクトです。  
*DownloadSceneAutoya*、*AssetBundleSceneAutoya*の2シーン構造です。(ClearCacheSceneはダウンロード済みアセットバンドルの消去に使用しています)
*DownloadSceneAutoya*でサーバからAssetBundleをダウンロード後、*AssetBundleSceneAutoya*で取得したアセットを表示します。  
*※サンプルでダウンロードするAssetBundleにユニティちゃんアセットを使用しています。*

## ビルド環境
Unity 2017.2.0f3  
Windows 10 

## known issue
1. Unity2017.1.0p5からUnityEditor上でのアセットバンドルをInstantiateした際に
```
Assertion failed: Assertion failed on expression: 'Thread::CurrentThreadIsMainThread()'
```
のエラーが出ます。これは実機上では起きない+バグレポート済みです。

2. Unity2017.3.x~Unity2017.3.1p4 でアセットバンドルのサイズ取得周りのバグがあるようです。
Unity2017.2系での利用をお勧めします。


## Unity-Chan ライセンス
本リポジトリには、UnityChanがAssetsとして含まれています。 以下のライセンスに従います。

<div><img src="http://unity-chan.com/images/imageLicenseLogo.png" alt="ユニティちゃんライセンス"><p>このアセットは、『<a href="http://unity-chan.com/contents/license_jp/" target="_blank">ユニティちゃんライセンス</a>』で提供されています。このアセットをご利用される場合は、『<a href="http://unity-chan.com/contents/guideline/" target="_blank">キャラクター利用のガイドライン</a>』も併せてご確認ください。</p></div>
