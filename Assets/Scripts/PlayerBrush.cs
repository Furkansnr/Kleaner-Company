using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrush : MonoBehaviour
{
     private enum State
    {
        Idle,
        Clean,
        BackIdle,
        Dirty,
        Empty
    }

    [SerializeField] private LayerMask dirtLayer;
    private Tween _cleanTween;
    private Tween _backTween;
    [SerializeField] private State _playerState = State.Idle;
    [SerializeField] private float scaleFactor;
    [SerializeField] private float decreaseSpeed;
    [SerializeField] private float cleanMaximumZaxis = 1f;
    [SerializeField] private ParticleSystem bubleParticle;
    private string playerID;
    private float firstZaxis;
    private float cleanZaxis;
    private bool _backIdle;
    private float health = 100f;
    private float waterBucketYaxis;
    private Material spongeMaterial;
    private Vector3 backIdlePos;
    private KeyCode cleanKeyCode;
    private bool isOpenSkillCheck;
    private void Awake()
    {
        spongeMaterial = GetComponent<Renderer>().material;
        firstZaxis = transform.position.z;
        cleanZaxis = firstZaxis + cleanMaximumZaxis;
        backIdlePos = new Vector3(transform.position.x, transform.position.y, firstZaxis);
        playerID = GetPlayerID();
        cleanKeyCode = GetPlayerCleanKeyCode();
    }


    private void Start()
    {
        _cleanTween = transform.DOMoveZ(cleanZaxis, 0.25f)
            .SetLoops(-1, LoopType.Yoyo).SetAutoKill(false)
            .SetEase(Ease.OutSine);
        _cleanTween.Pause();
        _backTween = transform.DOMoveZ(firstZaxis, 0.25f).OnComplete((() => { _playerState = State.Idle; }))
            .SetAutoKill(false)
            .SetEase(Ease.OutSine);
        _backTween.Pause();
        transform.position = new Vector3(transform.position.x, transform.position.y, firstZaxis);
    }

    private void OnEnable()
    {
        GameManager.instance.SpongeFilled += SpongeFilled;
        GameManager.instance.DecreaseHealthAction += UpdateSpongeMaterial;
        GameManager.instance.GameEnd += GameEnd;
    }

    private void OnDisable()
    {
        GameManager.instance.SpongeFilled -= SpongeFilled;
        GameManager.instance.DecreaseHealthAction -= UpdateSpongeMaterial;
        GameManager.instance.GameEnd -= GameEnd;
        if (IsMethodInAction(GameManager.instance.SkillCheckSuccesfull, nameof(SkillCheckSuccesfull)))
            GameManager.instance.SkillCheckSuccesfull -= SkillCheckSuccesfull;
        if (_cleanTween.IsPlaying())
        {
            BubleActive(false);
            _cleanTween.Kill();
        }
    }

    void Update()
    {
        SetState(GetState());
        CleanState();
        BackIdleState();
    }

    private bool IsMethodInAction(Action<string,int> action, string methodName)
    {
        if (action == null)
            return false;

        foreach (var del in action.GetInvocationList())
        {
            if (del.Method.Name == methodName)
            {
                return true;
            }
        }

        return false;
    }

    private string GetPlayerID()
    {
        string playerid = "";
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (gameObject == transform.parent.GetChild(i).gameObject)
            {
                playerid = GameData.instance.playerIDs[i];
            }
        }

        return playerid;
    }
    private KeyCode GetPlayerCleanKeyCode()
    {
        KeyCode playerKeyCode = KeyCode.A;
        for (int i = 0; i <GameData.instance.playerKeyCode.Length; i++)
        {
            if (playerID==GameData.instance.playerIDs[i])
            {
                playerKeyCode = GameData.instance.playerKeyCode[i];
            }
        }

        return playerKeyCode;
    }

    private State GetState()
    {
        State newState = State.Empty;
        if ( Input.GetKeyDown(cleanKeyCode) &&_playerState == State.Idle)
            newState = State.Clean;
        if (Input.GetKeyUp(cleanKeyCode) && _playerState == State.Clean)
            newState = State.BackIdle;
        if (_playerState != State.Dirty && health <= 0)
            newState = State.Dirty;

        return newState;
    }

    private void SetState(State state)
    {
        if (_playerState == state || state == State.Empty) return;

        switch (state)
        {
            case State.Clean when health > 0:
                _playerState = state;
                BubleActive(true);
                break;

            case State.BackIdle:
                _playerState = state;
                BubleActive(false);
                break;
            case State.Dirty:
                if (_cleanTween.IsPlaying()) _cleanTween.Pause();
                _playerState = state;
                BubleActive(false);
                DirtyState();
                break;
        }
    }

    private void CleanState()
    {
        if (_playerState != State.Clean) return;
        if (!_cleanTween.IsPlaying())
        {
            _cleanTween.Restart();
            _cleanTween.Play();
        }

        DecreaseHealth();
        CleanRay();
    }

    private void CleanRay()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f,
                dirtLayer))
        {
            hit.collider.GetComponent<DirtClean>().Clean(hit, scaleFactor, playerID);
        }
    }

    private void BackIdleState()
    {
        if (_playerState != State.BackIdle) return;
        if (_cleanTween.IsPlaying()) _cleanTween.Pause();
        if (!_backTween.IsPlaying())
        {
            _backTween = transform.DOMove(backIdlePos, 0.25f).OnComplete((() => { _playerState = State.Idle; }))
                .SetAutoKill(false)
                .SetEase(Ease.OutSine);
            _backTween.Play();
        }
        DecreaseHealth();
        CleanRay();
    }

    private void DirtyState()
    {
        GameManager.instance.SkillCheckSuccesfull += SkillCheckSuccesfull;
        if (health >= 0 || isOpenSkillCheck) return;
        isOpenSkillCheck = true;
        Vector3 newPos = ObjectMaker.instance.SpawnWaterBucket(new Vector3(
            transform.position.x, transform.position.y, cleanZaxis - 0.35f),playerID);
        transform.DOMove(newPos, 0.5f);
        waterBucketYaxis = ObjectMaker.instance.GetWaterBucketYAxis();
    }

    private void SkillCheckSuccesfull(string playerID, int value)
    {
        if (this.playerID != playerID) return;
        Sequence q = DOTween.Sequence().SetLoops(2, LoopType.Yoyo)
            .OnComplete((() =>
            {
                if (value != 3)
                    _playerState = State.Dirty;
                else
                {
                    _playerState = State.BackIdle;
                    GameManager.instance.SkillCheckSuccesfull -= SkillCheckSuccesfull;
                }
            }));
        ;
        q.Append(transform.DOMoveY(waterBucketYaxis, 0.5f));
        q.Join(transform.DOScale(transform.localScale / 2, 0.5f));
        float currentDirtPower = spongeMaterial.GetFloat("_dirt_power");
        DOTween.To(() => currentDirtPower, x => currentDirtPower = x, currentDirtPower - 0.2f
                , 0.25f)
            .OnUpdate((() => { spongeMaterial.SetFloat("_dirt_power", currentDirtPower); }));
    }


    private void DecreaseHealth()
    {
        health -= Time.deltaTime * decreaseSpeed;
        GameManager.instance.EmitDecreaseHealthAction(playerID, health / 100);
        if (health <= 0)
        {
            GameManager.instance.OpenSkillCheckPanelManager(playerID);
        }
    }

    private void SpongeFilled(string playerID)
    {
        if (this.playerID != playerID) return;
        health = 100;
        spongeMaterial.SetFloat("_dirt_power", 0);
        isOpenSkillCheck = false;
    }

    private void BubleActive(bool active)
    {
        if (active) bubleParticle.Play();
        else bubleParticle.Stop();
    }

    private void UpdateSpongeMaterial(string playerID, float value)
    {
        if (this.playerID == playerID)
            spongeMaterial.SetFloat("_dirt_power", 1 - value);
    }

    private void GameEnd()
    {
        Destroy(this);
    }
}