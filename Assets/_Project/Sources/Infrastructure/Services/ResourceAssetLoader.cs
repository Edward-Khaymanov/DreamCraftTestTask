using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Project.Infrastructure.Services
{
    public class ResourceAssetLoader : ILocalAssetLoader
    {
        public async UniTask<T> LoadAsync<T>(string path) where T : UnityEngine.Object
        {
            var asset = await Resources.LoadAsync(path);
            if (asset == null)
                throw new Exception($"Failed to load asset: {path}");

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                var isSucceed = ((GameObject)asset).TryGetComponent<T>(out var component);
                if (isSucceed == false)
                    throw new Exception($"Failed to load component for asset:{path}");

                return component;
            }

            return (T)asset;
        }

        public async UniTask<T> LoadFromJsonAsync<T>(string path) where T : class
        {
            var textAsset = (TextAsset)await Resources.LoadAsync(path);
            if (textAsset == null)
                throw new Exception($"Failed to load TextAsset: {path}");

            return JsonConvert.DeserializeObject<T>(textAsset.text);
        }
    }
}
