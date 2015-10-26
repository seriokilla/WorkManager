using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WorkManager.UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		[ExpectedException(typeof(System.Exception))]
		public void TestWorkerDelUnityException()
		{
			var mgr = new WorkManager();
			mgr.WorkerDoWork("TestWorkerDelUnityException");
		}

		[TestMethod]
		public void TestWorkerDelegateUnity()
		{
            var container = new UnityContainer();
			container.RegisterInstance<IWorker>(
				new WorkerDelegate() { Work = msg => Debug.WriteLine("WorkerDelegate: " + msg) }, 
				new ContainerControlledLifetimeManager());

			container.AddNewExtension<Interception>();
		
			container.RegisterType<IWorker>(
				new Interceptor<InterfaceInterceptor>(),
				new InterceptionBehavior<LoggingInterceptionBehavior>());

			var mgr = new WorkManager { GetWorkerFromIocContainer = () => container.Resolve<IWorker>() };
			mgr.WorkerDoWork("TestWorkerDelegateUnity");
		}

		[TestMethod]
		public void TestWorkerUnity()
		{
			var container = new UnityContainer();
			container.RegisterInstance<IWorker>(
				new Worker(), 
				new ContainerControlledLifetimeManager());

			container.AddNewExtension<Interception>();
			container.RegisterType<IWorker, Worker>(
				new Interceptor<InterfaceInterceptor>(), 
				new InterceptionBehavior<LoggingInterceptionBehavior>());

			var mgr = new WorkManager { GetWorkerFromIocContainer = () => container.Resolve<IWorker>() };
			mgr.WorkerDoWork("TestWorkerUnity");
		}

		[TestMethod]
		public void TestMoqNoUnity()
		{
			var m = new Mock<IWorker>();
			m.Setup(f => f.DoWork(It.IsAny<string>()))
			 .Callback<string>(str => { Debug.WriteLine("TestMoqNoUnity: " + str); });

			var mgr = new WorkManager { GetWorkerFromIocContainer = () => m.Object };
			mgr.WorkerDoWork("TestMoqNoUnity");
		}

		[TestMethod]
		public void TestMoqWithUnity()
		{
			var m = new Mock<IWorker>();
			m.Setup(f => f.DoWork(It.IsAny<string>()))
			 .Callback<string>(str => { Debug.WriteLine("TestMoqWithUnity: " + str); });

			var container = new UnityContainer();
			container.RegisterInstance(m.Object);

			var mgr = new WorkManager { GetWorkerFromIocContainer = () => container.Resolve<IWorker>() };
			mgr.WorkerDoWork("TestMoqWithUnity");
		}
	}
}
