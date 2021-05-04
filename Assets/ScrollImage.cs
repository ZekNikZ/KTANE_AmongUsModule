using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollImage : MonoBehaviour {
	// Scroll the main texture based on time

	public float scrollSpeed = 0.5f;
	public float radius = 0.1f;
	public float scale = 0.3f;
	private SpriteRenderer _rend;
	private Sprite _sprite;

	void Start()
	{
		_rend = GetComponent<SpriteRenderer> ();
		Vector4 newBorder = new Vector4 ( 500, 500, 500, 500);
		var oldSprite = _rend.sprite;
		Rect rect = new Rect( 0,0, oldSprite.texture.width, oldSprite.texture.height);
		_sprite = Sprite.Create(oldSprite.texture, rect, new Vector2(0.5f,0.5f),  100, 1, SpriteMeshType.FullRect, newBorder );
		_rend.sprite = _sprite;
		Debug.Log(_rend.sprite.border);
	}

	void Update()
	{
		float offset = Time.time * scrollSpeed;
		_rend.sprite.pivot.Set((float) (radius * Math.Cos(offset)), (float) (radius * Math.Sin(offset)));
	}
}
