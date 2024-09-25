using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerBrush : MonoBehaviour
{
    private enum State
    {
        Idle,
        Clean,
        BackIdle,
        Empty
    }

    [SerializeField] private LayerMask dirtLayer;
    private Tween _cleanTween;
    private Tween _backTween;
    [SerializeField] private State _playerState = State.Idle;
    [SerializeField] private float scaleFactor;
    [SerializeField] private float decreaseSpeed;
    [SerializeField] private ParticleSystem bubleParticle;
    private bool _backIdle;
    private float health = 100f;


    private void Start()
    {
        _cleanTween = transform.DOMoveZ(1.4f, 0.25f)
            .SetLoops(-1, LoopType.Yoyo).SetAutoKill(false)
            .SetEase(Ease.OutSine);
        _cleanTween.Pause();
        _backTween = transform.DOMoveZ(0.65f, 0.25f).OnComplete((() => { _playerState = State.Idle; }))
            .SetAutoKill(false)
            .SetEase(Ease.OutSine);
        _backTween.Pause();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.65f);

        GameManager.instance.SpongeFilled += SpongeFilled;
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
        if (Input.GetMouseButtonDown(0) && _playerState == State.Idle)
            newState = State.Clean;

        if (Input.GetMouseButtonUp(0) && _playerState == State.Clean)
            newState = State.BackIdle;
        if (_playerState == State.Clean && health <= 0)
            newState = State.BackIdle;
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
            _backTween = transform.DOMoveZ(0.65f, 0.25f).OnComplete((() => { _playerState = State.Idle; }))
                .SetAutoKill(false)
                .SetEase(Ease.OutSine);
            _backTween.Play();
        }
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

    private void SpongeFilled() => health = 100;

    private void BubleActive(bool active)
    {
        if(active) bubleParticle.Play();
        else bubleParticle.Stop();
    }
}