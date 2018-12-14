using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

	public Sprite normalSprite;
	private bool _dancing;
	private Animator _animator;
	private SpriteRenderer _spriteRenderer;

	void Awake()
	{
		_dancing = false;
		_animator = gameObject.GetComponent<Animator>();
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		_animator.enabled = false;
	}

	void OnMouseDown()
	{
		if(!_dancing)
		{
			_dancing = true;
			_animator.enabled = true;
			StartCoroutine(StartDancing());
		}
	}

	IEnumerator StartDancing()
	{
		yield return new WaitForSeconds(1f);
		_dancing = false;
		_animator.enabled = false;
		_spriteRenderer.sprite = normalSprite;
	}
}
