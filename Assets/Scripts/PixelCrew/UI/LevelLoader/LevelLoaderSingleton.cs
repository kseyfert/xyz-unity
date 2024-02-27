using System.Collections;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.LevelLoader
{
    public class LevelLoaderSingleton : SingletonMonoBehaviour
    {
        private static readonly int Enabled = Animator.StringToHash("Enabled");
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            InitLoader();
        }

        private static void InitLoader()
        {
            SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive);
        }

        [SerializeField] private float transitionTime;
        
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            
            _animator = GetComponentInChildren<Animator>();
            DontDestroyOnLoad(gameObject);
        }

        public void Show(string sceneName)
        {
            StartCoroutine(StartAnimation(sceneName));
        }

        private IEnumerator StartAnimation(string sceneName)
        {
            _animator.SetBool(Enabled, true);
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
            _animator.SetBool(Enabled, false);
        }
    }
}