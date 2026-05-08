using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneTransition : MonoBehaviour
{

    private static SceneTransition instance;
    private static bool shouldPlayEndAnimation = false;


    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;


    public Text LoadingPercentage;
    public Image LoadingProgressBar;

    public static void SwitchToScene(string sceneName)
    {

        instance.componentAnimator.SetTrigger(name: "SceneStart");


        instance.loadingSceneOperation = SceneManager.LoadSceneAsync("sceneName");


        instance.loadingSceneOperation.allowSceneActivation = false;
    }
    void Start()
    {

        instance = this;


        componentAnimator = GetComponent<Animator>();


        if (shouldPlayEndAnimation) componentAnimator.SetTrigger(name: "SceneEnd");
    }

    
    void Update()
    {
        if (loadingSceneOperation != null)
        {
            LoadingPercentage.text = Mathf.RoundToInt(f: loadingSceneOperation.progress * 100) + "%";

            LoadingProgressBar.fillAmount = loadingSceneOperation.progress;
        }
    }


    public void OnAnimationOver()
    {

        shouldPlayEndAnimation = true;

        loadingSceneOperation.allowSceneActivation = true;
    }
}
