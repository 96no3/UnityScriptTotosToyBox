using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNamaes
{
    public enum Scenes
    {
        TITLE = 0,
        CREDIT,
        LOAD,
        GAME,
        RESULT,
        CHARACTER,
        EXIT
    }
}

public class MySceneManager : MonoBehaviour
{
    public static void changeScene(SceneNamaes.Scenes scenes)
    {
        switch (scenes)
        {
            case SceneNamaes.Scenes.TITLE:
                SceneManager.LoadScene("TitleScene");
                break;
            case SceneNamaes.Scenes.CREDIT:
                SceneManager.LoadScene("CreditScene");
                break;
            case SceneNamaes.Scenes.LOAD:
                SceneManager.LoadScene("NowLoadingScene");
                break;
            case SceneNamaes.Scenes.GAME:
                SceneManager.LoadScene("GameScene");
                break;            
            case SceneNamaes.Scenes.RESULT:
                SceneManager.LoadScene("ResultScene");
                break;
            case SceneNamaes.Scenes.CHARACTER:
                SceneManager.LoadScene("CharaScene");
                break;
            case SceneNamaes.Scenes.EXIT:
                Director.Instance.EndGame();
                break;
        }
    }

    //public static SceneNamaes.Scenes loadNextScene(int stageNo)
    //{
    //    switch (stageNo)
    //    {
    //        case 0:
    //            return SceneNamaes.Scenes.GAME01;
    //        //case 1:
    //        //    return SceneNamaes.Scenes.GAME02;
    //        //case 2:
    //        //    return SceneNamaes.Scenes.GAME03;
    //        //case 3:
    //        //    return SceneNamaes.Scenes.GAME04;
    //        default:
    //            return SceneNamaes.Scenes.TITLE;
    //    }
    //}

    //public static SceneNamaes.Scenes loadThisScene(int stageNo)
    //{
    //    switch (stageNo)
    //    {
    //        case 0:
    //            return SceneNamaes.Scenes.GAME00;
    //        case 1:
    //            return SceneNamaes.Scenes.GAME01;
    //        //case 2:
    //        //    return SceneNamaes.Scenes.GAME02;
    //        //case 3:
    //        //    return SceneNamaes.Scenes.GAME03;
    //        //case 4:
    //        //    return SceneNamaes.Scenes.GAME04;
    //        default:
    //            return SceneNamaes.Scenes.TITLE;
    //    }
    //}
}
