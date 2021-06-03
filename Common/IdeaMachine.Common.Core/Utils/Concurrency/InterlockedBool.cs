using System.Diagnostics;
using System.Threading;

namespace IdeaMachine.Common.Core.Utils.Concurrency
{
	[DebuggerDisplay("{Value}")]
	public struct InterlockedBool
	{
		private int _value;

		public bool Value
		{
			get => _value == 1;
			set => Set(value);
		}

		public InterlockedBool(bool initialValue = false)
		{
			_value = initialValue ? 1 : 0;
		}

		public void Set(bool value)
		{
			if (Value == value)
			{
				return;
			}

			if (!value)
			{
				Interlocked.Increment(ref _value);
			}
			else
			{
				Interlocked.Decrement(ref _value);
			}
		}

		public static implicit operator bool(InterlockedBool interlockedBool) => interlockedBool.Value;

		public static implicit operator InterlockedBool(bool boolValue) => new InterlockedBool() { Value = boolValue };
	}
}