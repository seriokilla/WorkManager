using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using InterceptionBehavior;

namespace WorkManager.UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestWorkerDelegateRegisterInstance()
		{
			var container = new UnityContainer();
			container.AddNewExtension<Interception>();
			container.RegisterType<IWorker>(
				new Interceptor<InterfaceInterceptor>(),
				new InterceptionBehavior<LoggingInterceptionBehavior>()
			);

			container.RegisterInstance<IWorker>(
				new WorkerDelegate() { Work = msg => Debug.WriteLine("WorkerDelegate: " + msg) }
			);
			container.RegisterInstance<IWorkerManager>(
				new WorkerManager(container.Resolve<IWorker>())
			);

			var mgr = container.Resolve<IWorkerManager>();
			mgr.WorkerDoWork("TestWorkerDelegateRegisterInstance");
		}

		[TestMethod]
		public void TestWorkerDelegateRegisterType()
		{
			var container = new UnityContainer();
			container.AddNewExtension<Interception>();
			container.RegisterType<IWorker, WorkerDelegate>(
				new InjectionProperty("Work", (Action<string>)(msg => Debug.WriteLine("WorkerDelegate: " + msg))),
                new Interceptor<InterfaceInterceptor>(),
				new InterceptionBehavior<LoggingInterceptionBehavior>()
			);

			container.RegisterType<IWorkerManager, WorkerManager>(
				new InjectionConstructor(container.Resolve<IWorker>()),
				new Interceptor<InterfaceInterceptor>(),
				new InterceptionBehavior<LoggingInterceptionBehavior>()
			);

			var mgr = container.Resolve<IWorkerManager>();
			mgr.WorkerDoWork("TestWorkerDelegateRegisterType");
		}

		[TestMethod]
		public void TestWorkerRegisterType()
		{
			var container = new UnityContainer();
			container.AddNewExtension<Interception>();

			container.RegisterType<IWorker, Worker>();
			container.RegisterType<IWorkerManager, WorkerManager>(
				new InjectionConstructor(container.Resolve<IWorker>())
			);

			var mgr = container.Resolve<IWorkerManager>();
			mgr.WorkerDoWork("TestWorkerRegisterType");
		}

		[TestMethod]
		public void TestMoqNoUnity()
		{
			var m = new Mock<IWorker>();
			m.Setup(f => f.DoWork(It.IsAny<string>()))
			 .Callback<string>(str => { Debug.WriteLine("MoqWorker: " + str); });

			var mgr = new WorkerManager(m.Object);
			mgr.WorkerDoWork("TestMoqNoUnity");
		}

		[TestMethod]
		public void TestMoqWithUnity()
		{
			var m = new Mock<IWorker>();
			m.Setup(f => f.DoWork(It.IsAny<string>()))
			 .Callback<string>(str => { Debug.WriteLine("MoqWorker: " + str); });

			var container = new UnityContainer();
			container.RegisterInstance(m.Object);
			container.RegisterInstance(new WorkerManager(container.Resolve<IWorker>()));

			var mgr = container.Resolve<IWorkerManager>();
			mgr.WorkerDoWork("TestMoqWithUnity");
		}
	}
}
