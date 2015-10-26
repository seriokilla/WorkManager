using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace WorkManager.UnitTests
{
	class LoggingInterceptionBehavior : IInterceptionBehavior
	{
		public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
		{
			// Before invoking the method on the original target.
			WriteLog($"Invoking method {input.MethodBase} at {DateTime.Now.ToLongTimeString()}");

			// Invoke the next behavior in the chain.
			var result = getNext()(input, getNext);

			// After invoking the method on the original target.
			WriteLog(
				result.Exception != null
					? $"Method {input.MethodBase} threw exception {result.Exception.Message} at {DateTime.Now.ToLongTimeString()}"
					: $"Method {input.MethodBase} returned {result.ReturnValue} at {DateTime.Now.ToLongTimeString()}");

			return result;
		}

		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		public bool WillExecute => true;

		private static void WriteLog(string message)
		{
			Debug.WriteLine(message);
		}
	}
}
