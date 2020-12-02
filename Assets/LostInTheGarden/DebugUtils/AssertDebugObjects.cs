using System.Diagnostics;
using UnityEngine;

namespace LostintheGarden.DebugUtils
{
	public class AssertDebugObjects
	{
		public class AssertAtMostOncePerFrame
		{
			private int lastFrame = -1;

			[Conditional(AssertDebug.AssertDefine)]
			public void AtMostOncePerFrame()
			{
				var currentFrame = Time.frameCount;
				if (currentFrame == lastFrame)
				{
					Log.Error("Assert was called more than once in this frame. (frame: " + Time.frameCount + ")");
				}
				lastFrame = currentFrame;
			}
		}
	}

}