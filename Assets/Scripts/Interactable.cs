using System;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector] public bool isMouseOver;

    private AudioSource _audioSource;

    private void Awake()
    {
        gameObject.tag = "Interactable";
        OnAwake();
        _audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(_audioSource);
    }

    private void Update()
    {
        isMouseOver = false;
    }

    private void LateUpdate()
    {
        if (isMouseOver)
            Highlight();
        else
            OffHighlight();
    }

    public virtual void Interact()
    {
        if (_audioSource.clip != null)
            _audioSource.Play();
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