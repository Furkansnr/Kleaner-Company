using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class SkillCheck : MonoBehaviour
{

    public RectTransform indicator;
    public RectTransform successArea;
    public GameObject SkillCheckParent;
    public int skillChecksDone, checkSpeed, checksSuccesfullyDone;
    public int ymin, ymax, indcymin, indcymax;
    public bool checkFinished, upDown = true, checkUseable = true;
    [SerializeField] private Slider slider;
    private string playerID;


    private void OnEnable()
    {
        GameManager.instance.SkillCheckAppears += OpenSkillCheckPanel;
    }

    private void OnDestroy()
    {
        GameManager.instance.SkillCheckAppears -= OpenSkillCheckPanel;
    }


    private void Start()
    {
        SkillCheckParent.SetActive(false);
    }

    void Update()
    {
        MoveIndicator();
        CheckSkill();
        //CloseSkillCheckPanel();
    }

    public void SetPlayerID(string playerID) => this.playerID = playerID;

    private void CloseSkillCheckPanel()
    {
        skillChecksDone = -1;
        checkUseable = true;
        upDown = true;
        indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, -10);
        SkillCheckParent.SetActive(false);
    }

    private void OpenSkillCheckPanel(string playerID)
    {
        print(this.playerID == playerID);
        if (this.playerID != playerID) return;
        skillChecksDone = -1;
        checkUseable = true;
        upDown = true;
        indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, -10);
        SkillCheckParent.SetActive(true);
    }

    public void MoveIndicator()
    {
        if (indicator.anchoredPosition.y >= indcymax) //220
        {
            successArea.anchoredPosition = new Vector2(successArea.anchoredPosition.x, ymax); //-180
            upDown = false;
            checkUseable = true;
            successArea.anchoredPosition = new Vector2(successArea.anchoredPosition.x, Random.Range(-90, -190));
            RandomizeSkillCheckHeight();
            if (!checkFinished)
            {
                skillChecksDone++;
            }

            checkFinished = false;
        }
        else if (indicator.anchoredPosition.y <= indcymin) //-10
        {
            successArea.anchoredPosition = new Vector2(successArea.anchoredPosition.x, ymin); //-10
            upDown = true;
            checkUseable = true;
            successArea.anchoredPosition = new Vector2(successArea.anchoredPosition.x, Random.Range(90, 190));
            RandomizeSkillCheckHeight();
            if (!checkFinished)
            {
                skillChecksDone++;
            }

            checkFinished = false;
        }

        if (upDown)
        {
            indicator.transform.Translate(Vector3.up * Time.deltaTime * checkSpeed);
        }
        else
        {
            indicator.transform.Translate(Vector3.down * Time.deltaTime * checkSpeed);
        }
    }

    private void CheckSkill()
    {
        if (Input.GetKeyDown(KeyCode.Space) && checkUseable)
        {
            checkUseable = false;
            checkFinished = true;
            skillChecksDone++;
            if (IsIndicatorInSuccessArea())
            {
                Debug.Log("Başarılı!");
                checksSuccesfullyDone++;
                slider.value = checksSuccesfullyDone;
                GameManager.instance.EmitSkillCheckSuccessfull(playerID ,checksSuccesfullyDone);
                if (checksSuccesfullyDone >= 5)
                {
                    GameManager.instance.EmitSpongeFilled(playerID);
                    checksSuccesfullyDone = 0;
                    slider.value = checksSuccesfullyDone;
                    CloseSkillCheckPanel();
                }
            }
            else
            {
                Debug.Log("Başarısız!");
            }
        }
    }

    public void RandomizeSkillCheckHeight()
    {
        float heightSucces = Random.Range(26, 35);
        successArea.sizeDelta = new Vector2(successArea.sizeDelta.x, heightSucces);
    }

    private bool IsIndicatorInSuccessArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(successArea, indicator.position);
    }
}