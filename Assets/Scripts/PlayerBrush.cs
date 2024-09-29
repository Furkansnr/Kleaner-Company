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
    private float firstZaxis;
    private float cleanZaxis;
    private bool _backIdle;
    private float health = 100f;
    private float waterBucketYaxis;
    private Material spongeMaterial;
    private Vector3 backIdlePos;
    private bool isBrushing = false; // yeni ekledim basıp basmadığını anlamak için eklendi.
    private void Awake()
    {
        spongeMaterial = GetComponent<Renderer>().material;
        firstZaxis = transform.position.z;
        cleanZaxis = firstZaxis + cleanMaximumZaxis;
        backIdlePos = new Vector3(transform.position.x, transform.position.y, firstZaxis);
        
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

        GameManager.instance.SpongeFilled += SpongeFilled;
        GameManager.instance.DecreaseHealthAction += UpdateSpongeMaterial;
        GameManager.instance.SkillCheckSuccesfull += SkillCheckSuccesfull;
    }

    void Update()
    {
        SetState(GetState());
        CleanState();
        BackIdleState();
    }

    private State GetState()
    {
        State newState = State.Empty;
        if (isBrushing && _playerState == State.Idle)
            newState = State.Clean;
        if (!isBrushing && _playerState == State.Clean)
            newState = State.BackIdle;
        if (_playerState != State.Dirty && health <= 0)
            newState = State.Dirty;

        return newState;
    }

    private void OnBrush(InputValue context)
    {
        Debug.Log(context.isPressed);
        if(context.isPressed)
         {
             isBrushing = true;
         }
         else
         {
             isBrushing = false;
         }
    }

    
     // public bool IsBrushing(bool isBrushed)
     // {
     //     isBrushed = isBrushing;
     //     return isBrushing; 
     // }
    
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

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f,
                dirtLayer))
        {
            hit.collider.GetComponent<DirtClean>().Clean(hit, scaleFactor);
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
    }

    private void DirtyState()
    {
        if (health >= 0) return;
        Vector3 newPos = ObjectMaker.instance.SpawnWaterBucket(new Vector3(
            transform.position.x, transform.position.y, cleanZaxis - 0.35f));
        transform.DOMove(newPos, 0.5f);
        waterBucketYaxis = ObjectMaker.instance.GetWaterBucketYAxis();
    }

    private void SkillCheckSuccesfull(int value)
    {
        Sequence q = DOTween.Sequence().SetLoops(2,LoopType.Yoyo)
            .OnComplete((() =>
            {
                if (value != 5)
                    _playerState = State.Dirty;
                else
                    _playerState = State.BackIdle;
            }));;
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
        GameManager.instance.EmitDecreaseHealthAction(health / 100);
        if (health <= 1)
        {
            GameManager.instance.OpenSkillCheckPanelManager();
        }
    }

    private void SpongeFilled()
    {
        health = 100;
        spongeMaterial.SetFloat("_dirt_power", 0);
    }

    private void BubleActive(bool active)
    {
        if (active) bubleParticle.Play();
        else bubleParticle.Stop();
    }

    private void UpdateSpongeMaterial(float value) => spongeMaterial.SetFloat("_dirt_power", 1 - value);
}