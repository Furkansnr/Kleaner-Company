using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using DG.Tweening;

public class SkillCheck : MonoBehaviour
{
    public RectTransform indicator;
    public RectTransform successArea;
    public GameObject SkillCheckParent;
    public int skillChecksDone,checkSpeed,checksSuccesfullyDone;
    public int ymin, ymax,indcymin,indcymax;
    public bool checkFinished,upDown = true,checkUseable = true;
    [SerializeField] private Slider slider;
    public Image fillImage;

    private void Start()
    {
        GameManager.instance.SkillCheckAppears += OpenSkillCheckPanel;
        SkillCheckParent.transform.localScale = Vector3.zero;
        SkillCheckParent.SetActive(false);
    }

    void Update()
    {
        MoveIndicator();
        CheckSkill();
    }

    private void CloseSkillCheckPanel()
    {
        
        skillChecksDone = -1;
        checkUseable = true;
        upDown = true;
        indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, -10);
        //SkillCheckParent.SetActive(false);
        SkillCheckParent.transform.DOScale(Vector3.zero, 0.7f).SetEase(Ease.OutExpo).
            OnComplete(() => SkillCheckParent.SetActive(false));
    }

    private void OpenSkillCheckPanel()
    {
        skillChecksDone = -1;
        checkUseable = true;
        upDown = true;
        indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, -10);
        SkillCheckParent.SetActive(true);
        SkillCheckParent.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutCirc);
        //SkillCheckParent.transform.DOScale(Vector3.zero, 1f).From().SetEase(Ease.InBounce);

    }
    
    public void MoveIndicator()
    {
        if (indicator.anchoredPosition.y >= indcymax) //220
        {
            upDown = false;
            successArea.anchoredPosition = new Vector2(successArea.anchoredPosition.x, Random.Range(-90,-190));
            RandomizeSkillCheckHeight();
            if (!checkFinished)
            {
                skillChecksDone++;   
            }
            checkFinished = false;
        }
        else if (indicator.anchoredPosition.y <= indcymin) //-10
        {
            upDown = true;
            successArea.anchoredPosition = new Vector2(successArea.anchoredPosition.x, Random.Range(90,190));
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
            checkFinished = true;
            skillChecksDone++;
            if (IsIndicatorInSuccessArea())
            {
                Debug.Log("Başarılı!");
                StartCoroutine(SkillCheckDelay(1.1f));
                checksSuccesfullyDone++;
                //slider.value = checksSuccesfullyDone;
                fillImage.fillAmount = (float)checksSuccesfullyDone / 5f;
                GameManager.instance.EmitSkillCheckSuccessfull(checksSuccesfullyDone);
                if (checksSuccesfullyDone >= 5)
                {
                    GameManager.instance.EmitSpongeFilled();
                    fillImage.fillAmount = 0;
                    checksSuccesfullyDone = 0;
                    slider.value = checksSuccesfullyDone;
                    CloseSkillCheckPanel();
                }
            }
            else
            {
                StartCoroutine(SkillCheckDelay(0.5f));
                Debug.Log("Başarısız!");
            }
            
        }
        
    }

    public IEnumerator SkillCheckDelay(float delayTime)
    {
        checkUseable = false;
        yield return new WaitForSeconds(delayTime);
        checkUseable = true;
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
