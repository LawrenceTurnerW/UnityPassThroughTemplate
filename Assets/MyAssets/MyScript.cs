using System.Collections.Generic;
using Meta.XR.Samples;
using UnityEngine;
using UnityEngine.Events;
using PassthroughCameraSamples;

public class MyScript : MonoBehaviour
{
    [SerializeField] private WebCamTextureManager m_webCamTextureManager;
    [SerializeField] private float m_refreshTime = 0.1f;
    [SerializeField] private GameObject m_cubePrefab;
    private float m_refreshCurrentTime = 0.0f;

    private void Update()
    {
        var webCamTexture = m_webCamTextureManager.WebCamTexture;
        if (webCamTexture != null)
        {
            if (!IsWaiting())
            {
                // WebCamTextureの状態を確認&refreshTimeを更新
                CheckAndUpdateRefreshTime(webCamTexture);
                // キューブを生成してテクスチャを貼り付け
                SpawnCube(webCamTexture);
            }
        }
        else
        {
            Debug.LogWarning("WebCamTextureが有効ではありません");
        }
    }

    private void CheckAndUpdateRefreshTime(WebCamTexture webCamTexture)
    {
        if (!webCamTexture.isPlaying)
        {
            Debug.LogWarning("WebCamTextureが利用できない状態です");
        }
        m_refreshCurrentTime = m_refreshTime;
    }

    /*
    毎秒処理を行うと、パフォーマンスが低下するので一定時間毎に処理を行うように
    毎フレーム処理したい場合は、m_refreshTimeを0.0fに設定
    参考:https://github.com/oculus-samples/Unity-PassthroughCameraApiSamples
    */
    private bool IsWaiting()
    {
        m_refreshCurrentTime -= Time.deltaTime;
        return m_refreshCurrentTime > 0.0f;
    }

    private void SpawnCube(WebCamTexture webCamTexture)
    {
        if (m_cubePrefab == null)
        {
            Debug.LogWarning("cubeプレハブが設定されていません");
            return;
        }
        // 座標 (0, 1, 0) にキューブを生成
        GameObject cube = Instantiate(m_cubePrefab, new Vector3(0, 1, 0), Quaternion.identity);
        // キューブの Renderer に WebCamTexture を設定
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = webCamTexture;
        }
        else
        {
            Debug.LogWarning("生成されたキューブにRendererコンポーネントが見つかりません");
        }
    }
}
