using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    PanelFade _panelFade = null;

    public async void Load(string name)
    {
        var token = this.GetCancellationTokenOnDestroy();
        _panelFade.ImageFade(PanelFade.FadeType.FadeOut);
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
