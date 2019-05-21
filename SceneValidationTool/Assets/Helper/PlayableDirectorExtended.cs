using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Silverback.EditorTools
{
	[Serializable]
	public struct PlayableDirectorExtended
	{
		public PlayableDirector PlayableDirector => m_PlayableDirector;
		public DirectorWrapMode DirectorWrapMode => m_DirectorWrapMode;
		public bool State => m_State;

		[SerializeField]
		private PlayableDirector m_PlayableDirector;

		[SerializeField]
		private DirectorWrapMode m_DirectorWrapMode;

		[SerializeField]
		private bool m_State;

		public PlayableDirectorExtended(
			PlayableDirector _PlayableDirector,
			DirectorWrapMode _DirectorWrapMode,
			bool _State)
		{
			m_PlayableDirector = _PlayableDirector;
			m_DirectorWrapMode = _DirectorWrapMode;
			m_State = _State;
		}
	}
}
