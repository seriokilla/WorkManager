using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
namespace WorkManager.UnitTests
{
	public static class EnumExtensions
	{
		public static string ToUserFriendlyString(this Enum value)
		{
			return Enum.GetName(value.GetType(), value);
		}
	}

	public enum MyNum
	{
		one,
		two,
		three,
		four,
		five
	}

	/// <summary>
	/// Summary description for UnitTest2
	/// </summary>
	[TestClass]
	public class UnitTest2
	{
		public UnitTest2()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestMethod1()
		{
			var n = MyNum.five;
			Debug.WriteLine(n.ToUserFriendlyString());

			string sTest = null;
			var sLeft = sTest?.Substring(0, 1);
		}


	}
}
