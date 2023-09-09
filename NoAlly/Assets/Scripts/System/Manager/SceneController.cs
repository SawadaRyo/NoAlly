using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    PanelFade _panelFade = null;

    [Tooltip("")]
    bool _enabled = false;

    public async void Load(string name)
    {
        if (_enabled) return;
        //var token = this.GetCancellationTokenOnDestroy();
        _panelFade.ImageFade(FadeType.FadeOut);
        _enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_panelFade.Interval)); 
        SceneManager.LoadScene(name);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
