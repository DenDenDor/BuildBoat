using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : AbstractJoystick

{
	Vector2 joystickPosition = Vector2.zero;
	private Camera cam = new Camera();

	void Start()
	{
		joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
	}

	public override void OnDrag(PointerEventData eventData)
	{
		Vector2 direction = eventData.position - joystickPosition;
		inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
		ClampJoystick();
		handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleDistance;
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		inputVector = Vector2.zero;
		handle.anchoredPosition = Vector2.zero;
	}
	
}