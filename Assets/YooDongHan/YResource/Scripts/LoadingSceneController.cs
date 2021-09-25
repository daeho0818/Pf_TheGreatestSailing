using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string _nextScene;
    
    [SerializeField]
    private Image _progressBar;


    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    public static void LoadScene(string sceneName)
    {
        _nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void LoadScene(Enum.IslandType islandType)
    {
        if(InitScene._islandDic.ContainsKey(islandType))
        {
            SceneManager.LoadScene(InitScene._islandDic[islandType]);
        }
        else
            Debug.Log("Can't find Dictionary Key.");
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                _progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if (_progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
