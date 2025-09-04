using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Infrastructure.EntryPoints
{
    public class AppStartup : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}