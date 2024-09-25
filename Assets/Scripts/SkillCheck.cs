
using System;
using UnityEngine;

public class SkillCheck : MonoBehaviour
{
    public RectTransform indicator;
    public RectTransform successArea;
    public GameObject SkillCheckParent;
    public int skillChecksDone,checkSpeed,checksSuccesfullyDone;
    public int ymin, ymax,indcymin,indcymax;
    public bool checkFinished,upDown = true,checkUseable = true;

    private void Start()
    {
        GameManager.instance.SkillCheckAppears += OpenSkillCheckPanel;
        SkillCheckParent.SetActive(false);
    }

    void Update()
    {
        MoveIndicator();
        CheckSkill();
        //CloseSkillCheckPanel();
    }

    private void CloseSkillCheckPanel()
    {
        
        skillChecksDone = -1;
        checkUseable = true;
        upDown = true;
        indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, -10);
        SkillCheckParent.SetActive(false);
        
    }

    private void OpenSkillCheckPanel()
    {
        
        Debug.Log("sa açılsana");
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
                if (checksSuccesfullyDone >= 5)
                {
                    GameManager.instance.EmitSpongeFilled();
                    checksSuccesfullyDone = 0;
                    CloseSkillCheckPanel();
                }
            }
            else
            {
                Debug.Log("Başarısız!");
            }
            
        }
        
    }

    private bool IsIndicatorInSuccessArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(successArea, indicator.position);
    }
}
