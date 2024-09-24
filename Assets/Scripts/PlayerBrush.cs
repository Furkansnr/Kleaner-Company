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
        BackIdle
    }

    [SerializeField] private LayerMask dirtLayer;
    private Tween _cleanTween;
    private Tween _backTween;
    [SerializeField] private State _playerState = State.Idle;
    [SerializeField] private float scaleFactor;
    private bool _backIdle;


    private void Start()
    {
        _cleanTween = transform.DOMoveZ(1.4f, 0.25f)
            .SetLoops(-1, LoopType.Yoyo).SetAutoKill(false)
            .SetEase(Ease.OutSine);
        _cleanTween.Pause();
        _backTween = transform.DOMoveZ(0.65f, 0.25f).OnComplete((() =>
        {
            _playerState = State.Idle;
        })).SetAutoKill(false)
            .SetEase(Ease.OutSine);
        _backTween.Pause();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.65f);
    }

    void Update()
    {
        if (_playerState == State.Clean)
        {
            if (!_cleanTween.IsPlaying())
            {
                _cleanTween.Restart();
                _cleanTween.Play();
            }

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f,
                    dirtLayer))
            {
                hit.collider.GetComponent<DirtClean>().Clean(hit,scaleFactor);
            }
        }

        else if (_playerState == State.BackIdle)
        {
            if (_cleanTween.IsPlaying()) _cleanTween.Pause();
            if (!_backTween.IsPlaying())
            {
                _backTween = transform.DOMoveZ(0.65f, 0.25f).OnComplete((() =>
                    {
                        _playerState = State.Idle;
                    })).SetAutoKill(false)
                    .SetEase(Ease.OutSine);
                _backTween.Play();
            }
        }

        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && _playerState == State.Idle)
        {
            _playerState = State.Clean;
        }

        if (Input.GetMouseButtonUp(0) && _playerState == State.Clean)
        {
            _playerState = State.BackIdle;
        }
    }
}