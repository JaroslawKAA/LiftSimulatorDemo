using System;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector] private bool _isMouseOver;
    private AudioSource _audioSource;

    public virtual void Interact()
    {
        if (_audioSource.clip != null)
            _audioSource.Play();
    }

    public void IsMouseOver()
    {
        _isMouseOver = true;
    }

    private void Awake()
    {
        gameObject.tag = "Interactable";
        _audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(_audioSource);
        OnAwake();
    }

    private void Update()
    {
        _isMouseOver = false;
    }

    private void LateUpdate()
    {
        if (_isMouseOver)
            Highlight();
        else
            OffHighlight();
    }

    protected virtual void Highlight()
    {
    }

    protected virtual void OffHighlight()
    {
    }

    protected virtual void OnAwake()
    {
    }
}