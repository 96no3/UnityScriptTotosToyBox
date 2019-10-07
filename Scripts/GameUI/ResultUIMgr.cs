using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIMgr : MonoBehaviour
{
    [SerializeField] private Animator missionClearAnim;
    [SerializeField] private Animator helpedNumAnim;

    [Header("UI")]
    [SerializeField] private Image missionClearImage;   // GradeAnim用
    [SerializeField] private Image npcNumImage;

    [SerializeField] private Sprite[] npcNumSprite;
    [SerializeField] private Sprite[] missionClearSprite;

    private void Start()
    {
        missionClearImage.sprite = missionClearSprite[0];
        npcNumImage.sprite = npcNumSprite[0];
        missionClearAnim.SetTrigger("None");
        helpedNumAnim.SetTrigger("None");
    }

    public void MissionClearAnim(int n)
    {
        missionClearImage.sprite = missionClearSprite[n];
        missionClearAnim.SetTrigger("Clear");
    }

    public void SetHelpedNumAnim()
    {
        npcNumImage.sprite = npcNumSprite[GameInstance.Instance.goalNpcList.Count];
        helpedNumAnim.SetTrigger("In");
    }

    public Sprite SetNumSprite(int n)
    {
        return npcNumSprite[n];
    }
}
