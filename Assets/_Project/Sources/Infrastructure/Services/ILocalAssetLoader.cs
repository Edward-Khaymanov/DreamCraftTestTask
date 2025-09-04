using Cysharp.Threading.Tasks;

namespace Project.Infrastructure.Services
{
    public interface ILocalAssetLoader
    {
        public UniTask<T> LoadAsync<T>(string path) where T : UnityEngine.Object;
        public UniTask<T> LoadFromJsonAsync<T>(string path) where T : class;
    }
}
