using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    SceneNamaes.Scenes nextScene = SceneNamaes.Scenes.TITLE;
        
    public void startFadeOut(SceneNamaes.Scenes _nextScene)
    {
        nextScene = _nextScene;
        GetComponent<Animator>().SetTrigger("startFadeOut");
    }

    public void FadeOut()
    {
        GetComponent<Animator>().SetBool("IsFade", true);
    }

    void completeFadeOut()
    {
        MySceneManager.changeScene(nextScene);
    }

    void FadeFinish()
    {
        GetComponent<Animator>().SetBool("IsFade", false);
    }


}
