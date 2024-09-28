using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace TowerDefence.Tools
{
	public class GizmosDrawer : MonoInstallerBase
	{
		private List<Action> _actions = new List<Action>();
		private List<Action> _infiniteActions = new List<Action>();
		private List<TemporaryAction> _timeActions = new List<TemporaryAction>();

		public static GizmosDrawer Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
		}

		public void AddOneFrameTask(Action drawGizmoAction)
		{
			_actions.Add(drawGizmoAction);
		}

		public void AddForeverTask(Action drawGizmoAction)
		{
			_infiniteActions.Add(drawGizmoAction);
		}

		public void AddTemporaryTask(Action drawGizmoAction, float seconds)
		{
			_timeActions.Add(new TemporaryAction(drawGizmoAction, Time.time, seconds));
		}

		private void OnDrawGizmos()
		{
			foreach (var action in _actions.ToArray())
			{
				action.Invoke();
				_actions.Remove(action);
			}

			foreach (var action in _infiniteActions.ToArray())
			{
				action.Invoke();
			}

			foreach (var action in _timeActions.ToArray())
			{
				if (action.StartAt + action.Duration < Time.time)
				{
					_timeActions.Remove(action);
					continue;
				}

				action.DrawAction.Invoke();
			}
		}

		public override void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance<GizmosDrawer>(this);
		}


		private class TemporaryAction
		{
			public TemporaryAction(Action drawAction, float startAt, float duration)
			{
				DrawAction = drawAction;
				StartAt = startAt;
				Duration = duration;
			}

			public Action DrawAction { get; }

			public float StartAt { get; }

			public float Duration { get; }
		}
	}
}
