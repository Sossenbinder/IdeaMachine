using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class TupleAwaiterExtensions
	{
		public static TaskAwaiter<(T0, T1)> GetAwaiter<T0, T1>(this (Task<T0>, Task<T1>) tuple)
		{
			var (t0, t1) = tuple;

			return Task.WhenAll(t0, t1)
					.ContinueWith(_ => (t0.Result, t1.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2)> GetAwaiter<T0, T1, T2>(this (Task<T0>, Task<T1>, Task<T2>) tuple)
		{
			var (t0, t1, t2) = tuple;

			return Task.WhenAll(t0, t1, t2)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3)> GetAwaiter<T0, T1, T2, T3>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>) tuple)
		{
			var (t0, t1, t2, t3) = tuple;

			return Task.WhenAll(t0, t1, t2, t3)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4)> GetAwaiter<T0, T1, T2, T3, T4>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>) tuple)
		{
			var (t0, t1, t2, t3, t4) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5)> GetAwaiter<T0, T1, T2, T3, T4, T5>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7, T8)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7, t8) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7, t8)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7, T8, T9)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>, Task<T12>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>, Task<T12>, Task<T13>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result, t13.Result))
					.GetAwaiter();
		}

		public static TaskAwaiter<(T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> GetAwaiter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this (Task<T0>, Task<T1>, Task<T2>, Task<T3>, Task<T4>, Task<T5>, Task<T6>, Task<T7>, Task<T8>, Task<T9>, Task<T10>, Task<T11>, Task<T12>, Task<T13>, Task<T14>) tuple)
		{
			var (t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) = tuple;

			return Task.WhenAll(t0, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14)
					.ContinueWith(_ => (t0.Result, t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result, t13.Result, t14.Result))
					.GetAwaiter();
		}

	}
}