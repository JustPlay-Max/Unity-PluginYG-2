using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using System;

namespace YG
{
    [DefaultExecutionOrder(ImageLoadYGExecutionOrder)]
    public class ImageLoadYG : MonoBehaviour
    {
        private const int ImageLoadYGExecutionOrder = -1;

        public bool startLoad;
#if PLUGIN_YG_2
        [NestedYG("startLoad")]
#endif
        public string urlImage;

        public RawImage rawImage;
        public Image spriteImage;
        public GameObject loadAnimObj;
        [FormerlySerializedAs("log")]
        public bool logError = true;

        public Action onTextureLoad;

        private struct LoadTextures { public string link; public Texture2D texture; }
        private static List<LoadTextures> saveTextures = new List<LoadTextures>();
        private int loadVersion;

        private void Awake()
        {
            if (rawImage)
                rawImage.enabled = false;
            if (spriteImage)
                spriteImage.enabled = false;

            if (startLoad)
                Load();
            else if (loadAnimObj)
                loadAnimObj.SetActive(false);
        }

        public void Load(string url)
        {
            if (string.IsNullOrEmpty(url) || url == "null")
            {
                ClearTexture();
                return;
            }

            Texture2D existingTexture = ExistingTexture(url);
            if (existingTexture)
                SetTexture(existingTexture);
            else
                StartCoroutine(LoadTexture(url));
        }
        public void Load() => Load(urlImage);

        private Texture2D ExistingTexture(string url)
        {
            List<LoadTextures> images = saveTextures;

            for (int i = 0; i < images.Count; i++)
            {
                if (url == images[i].link)
                    return images[i].texture;
            }

            return null;
        }

        public void ClearTexture()
        {
            loadVersion++;

            if (rawImage)
            {
                rawImage.texture = null;
                rawImage.enabled = false;
            }

            if (spriteImage)
            {
                spriteImage.sprite = null;
                spriteImage.enabled = false;
            }

            if (loadAnimObj)
                loadAnimObj.SetActive(false);
        }

        IEnumerator LoadTexture(string url)
        {
            int version = ++loadVersion;

            if (loadAnimObj)
                loadAnimObj.SetActive(true);

            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();

                // A newer load or an explicit texture change takes priority.
                if (version != loadVersion) yield break;

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    if (loadAnimObj)
                        loadAnimObj.SetActive(false);
                    if (logError)
#if UNITY_6_4_OR_NEWER || UNITY_6_5_OR_NEWER || UNITY_6000_6_OR_NEWER
                        Debug.LogError($"ImageLoadYG [{GetEntityId()}] Error: {url}, {webRequest.error}", this);
#else
                        Debug.LogError($"ImageLoadYG [{GetInstanceID()}] Error: {url}, {webRequest.error}", this);
#endif
                }
                else
                {
                    DownloadHandlerTexture handlerTexture = webRequest.downloadHandler as DownloadHandlerTexture;

                    if (handlerTexture.isDone)
                    {
                        Texture2D existingTexture = ExistingTexture(url);
                        if (existingTexture)
                        {
                            SetTexture(existingTexture);
                        }
                        else
                        {
                            Texture2D texture = handlerTexture.texture;

                            saveTextures.Add(new LoadTextures
                            {
                                link = url,
                                texture = texture
                            });
                            SetTexture(texture);
                        }
                    }
                    else if (logError)
#if UNITY_6_4_OR_NEWER || UNITY_6_5_OR_NEWER || UNITY_6000_6_OR_NEWER
                        Debug.LogError($"ImageLoadYG [{GetEntityId()}] Request succeeded without a completed texture: {url}", this);
#else
                        Debug.LogError($"ImageLoadYG [{GetInstanceID()}] Request succeeded without a completed texture: {url}", this);
#endif
                }
            }
        }

        public void SetTexture(Texture2D texture)
        {
            loadVersion++;

            if (rawImage)
            {
                rawImage.texture = texture;
                rawImage.enabled = true;
            }

            if (spriteImage)
            {
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                spriteImage.sprite = Sprite.Create(texture, rect, Vector2.zero);
                spriteImage.enabled = true;
            }

            if (loadAnimObj)
                loadAnimObj.SetActive(false);

            onTextureLoad?.Invoke();
        }
    }
}
