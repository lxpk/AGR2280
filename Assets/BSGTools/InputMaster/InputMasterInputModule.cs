using System;
using BSGTools.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BSGTools.Events {
	[AddComponentMenu("BSGTools/InputMaster/InputMaster Input Module")]
	public class InputMasterInputModule : PointerInputModule {
		private float m_NextAction;

		private Vector2 m_LastMousePosition;
		private Vector2 m_MousePosition;

		protected InputMasterInputModule() { }

		GameObject lastHighlightedObj;
		Selectable lastHighlightedSelectable;
		public event Action<Selectable, Selectable> MouseHighlightChanged;
		public event Action<GameObject, GameObject> MouseHighlightObjChanged;

		[SerializeField]
		string m_Horizontal = "Horizontal",
		m_Vertical = "Vertical",
		m_Submit = "Submit",
		m_Cancel = "Cancel";

		AxisControl horizontal, vertical;
		ActionControl submit, cancel;

		[SerializeField]
		private float m_InputActionsPerSecond = 10;

		[SerializeField]
		private bool m_AllowActivationOnMobileDevice;

		public bool allowActivationOnMobileDevice {
			get { return m_AllowActivationOnMobileDevice; }
			set { m_AllowActivationOnMobileDevice = value; }
		}

		public float inputActionsPerSecond {
			get { return m_InputActionsPerSecond; }
			set { m_InputActionsPerSecond = value; }
		}

		public override void UpdateModule() {
			m_LastMousePosition = m_MousePosition;
			m_MousePosition = Input.mousePosition;

			horizontal = InputMaster.GetAxis(m_Horizontal);
			vertical = InputMaster.GetAxis(m_Vertical);
			submit = InputMaster.GetAction(m_Submit);
			cancel = InputMaster.GetAction(m_Cancel);
		}

		public override bool IsModuleSupported() {
			// Check for mouse presence instead of whether touch is supported,
			// as you can connect mouse to a tablet and in that case we'd want
			// to use StandaloneInputModule for non-touch input events.
			return m_AllowActivationOnMobileDevice || Input.mousePresent;
		}

		public override bool ShouldActivateModule() {
			if(!base.ShouldActivateModule())
				return false;

			var shouldActivate = submit.state == State.Down;
			shouldActivate |= cancel.state == State.Down;
			shouldActivate |= !Mathf.Approximately(horizontal.value, 0.0f);
			shouldActivate |= !Mathf.Approximately(horizontal.value, 0.0f);
			shouldActivate |= (m_MousePosition - m_LastMousePosition).sqrMagnitude > 0.0f;
			shouldActivate |= Input.GetMouseButtonDown(0);
			return shouldActivate;
		}

		public override void ActivateModule() {
			base.ActivateModule();
			m_MousePosition = Input.mousePosition;
			m_LastMousePosition = Input.mousePosition;

			var toSelect = eventSystem.currentSelectedGameObject;
			if(toSelect == null)
				toSelect = eventSystem.lastSelectedGameObject;
			if(toSelect == null)
				toSelect = eventSystem.firstSelectedGameObject;

			eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
		}

		public override void DeactivateModule() {
			base.DeactivateModule();
			ClearSelection();
		}

		public override void Process() {
			bool usedEvent = SendUpdateEventToSelectedObject();

			if(eventSystem.sendNavigationEvents) {
				if(!usedEvent)
					usedEvent |= SendMoveEventToSelectedObject();

				if(!usedEvent)
					SendSubmitEventToSelectedObject();
			}

			ProcessMouseEvent();
		}

		/// <summary>
		/// Process submit keys.
		/// </summary>
		private bool SendSubmitEventToSelectedObject() {
			if(eventSystem.currentSelectedGameObject == null)
				return false;

			var data = GetBaseEventData();
			if(submit.state == State.Down)
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

			if(cancel.state == State.Down)
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
			return data.used;
		}

		private bool AllowMoveEventProcessing(float time) {
			bool allow = horizontal.rValue != 0f;
			allow |= vertical.rValue != 0f;
			allow |= (time > m_NextAction);
			return allow;
		}

		private Vector2 GetRawMoveVector() {
			return new Vector2(horizontal.value, vertical.value);
		}

		/// <summary>
		/// Process keyboard events.
		/// </summary>
		private bool SendMoveEventToSelectedObject() {
			float time = Time.unscaledTime;

			if(!AllowMoveEventProcessing(time))
				return false;

			Vector2 movement = GetRawMoveVector();
			// Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
			var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
			if(!Mathf.Approximately(axisEventData.moveVector.x, 0f)
				|| !Mathf.Approximately(axisEventData.moveVector.y, 0f)) {
				ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			}
			m_NextAction = time + 1f / m_InputActionsPerSecond;
			return axisEventData.used;
		}

		/// <summary>
		/// Process all mouse events.
		/// </summary>
		private void ProcessMouseEvent() {
			var mouseData = GetMousePointerEventData();

			var pressed = mouseData.AnyPressesThisFrame();
			var released = mouseData.AnyReleasesThisFrame();

			var leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;

			if(!UseMouse(pressed, released, leftButtonData.buttonData))
				return;

			// Process the first mouse button fully
			ProcessMousePress(leftButtonData);
			ProcessMove(leftButtonData.buttonData);
			ProcessDrag(leftButtonData.buttonData);

			// Now process right / middle clicks
			ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);

			if(!Mathf.Approximately(leftButtonData.buttonData.scrollDelta.sqrMagnitude, 0.0f)) {
				var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(leftButtonData.buttonData.pointerCurrentRaycast.gameObject);
				ExecuteEvents.ExecuteHierarchy(scrollHandler, leftButtonData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		private static bool UseMouse(bool pressed, bool released, PointerEventData pointerData) {
			if(pressed || released || pointerData.IsPointerMoving() || pointerData.IsScrolling())
				return true;

			return false;
		}

		private bool SendUpdateEventToSelectedObject() {
			if(eventSystem.currentSelectedGameObject == null)
				return false;

			var data = GetBaseEventData();
			ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
			return data.used;
		}

		/// <summary>
		/// Process the current mouse press.
		/// </summary>
		private void ProcessMousePress(MouseButtonEventData data) {
			var pointerEvent = data.buttonData;
			var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;
			var currentOverSelectable = (currentOverGo != null) ? currentOverGo.GetComponent<Selectable>() : null;

			if(lastHighlightedObj != currentOverGo && MouseHighlightObjChanged != null)
				MouseHighlightObjChanged(lastHighlightedObj, currentOverGo);
			if(currentOverSelectable != lastHighlightedSelectable && MouseHighlightChanged != null)
				MouseHighlightChanged(lastHighlightedSelectable, currentOverSelectable);

			lastHighlightedObj = currentOverGo;
			lastHighlightedSelectable = currentOverSelectable;

			// PointerDown notification
			if(data.PressedThisFrame()) {
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

				DeselectIfSelectionChanged(currentOverGo, pointerEvent);

				// search for the control that will receive the press
				// if we can't find a press handler set the press
				// handler to be what would receive a click.
				var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

				// didnt find a press handler... search for a click handler
				if(newPressed == null)
					newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

				// Debug.Log("Pressed: " + newPressed);

				float time = Time.unscaledTime;

				if(newPressed == pointerEvent.lastPress) {
					var diffTime = time - pointerEvent.clickTime;
					if(diffTime < 0.3f)
						++pointerEvent.clickCount;
					else
						pointerEvent.clickCount = 1;

					pointerEvent.clickTime = time;
				}
				else {
					pointerEvent.clickCount = 1;
				}

				pointerEvent.pointerPress = newPressed;
				pointerEvent.rawPointerPress = currentOverGo;

				pointerEvent.clickTime = time;

				// Save the drag handler as well
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

				if(pointerEvent.pointerDrag != null)
					ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
			}

			// PointerUp notification
			if(data.ReleasedThisFrame()) {
				// Debug.Log("Executing pressup on: " + pointer.pointerPress);
				ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

				// Debug.Log("KeyCode: " + pointer.eventData.keyCode);

				// see if we mouse up on the same element that we clicked on...
				var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

				// PointerClick and Drop events
				if(pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick) {
					ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if(pointerEvent.pointerDrag != null) {
					ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
				}

				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;

				if(pointerEvent.pointerDrag != null && pointerEvent.dragging)
					ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;

				// redo pointer enter / exit to refresh state
				// so that if we moused over somethign that ignored it before
				// due to having pressed on something else
				// it now gets it.
				if(currentOverGo != pointerEvent.pointerEnter) {
					HandlePointerExitAndEnter(pointerEvent, null);
					HandlePointerExitAndEnter(pointerEvent, currentOverGo);
				}
			}
		}
	}
}