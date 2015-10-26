﻿using System.Diagnostics;
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
			var mgr = new WorkerManager(null);
			mgr.WorkerDoWork("TestWorkerDelUnityException");
		}

		[TestMethod]
		public void TestWorkerDelegateUnity()
		{
			var container = new UnityContainer();
			container.AddNewExtension<Interception>();

			container.RegisterInstance<IWorker>(
				new WorkerDelegate() { Work = msg => Debug.WriteLine("WorkerDelegate: " + msg) },
				new ContainerControlledLifetimeManager());

			container.RegisterType<IWorker>(
				new Interceptor<InterfaceInterceptor>(),
				new InterceptionBehavior<LoggingInterceptionBehavior>());

			var mgr = new WorkerManager(container.Resolve<IWorker>());
			mgr.WorkerDoWork("TestWorkerDelegateUnity");
		}

		[TestMethod]
		public void TestWorkerUnity()
		{
			var container = new UnityContainer();
			container.AddNewExtension<Interception>();

			container.RegisterInstance<IWorker>(
				new Worker(),
				new ContainerControlledLifetimeManager());

			container.RegisterType<IWorker>(
				new Interceptor<InterfaceInterceptor>(),
				new InterceptionBehavior<LoggingInterceptionBehavior>());

			var mgr = new WorkerManager(container.Resolve<IWorker>());
			mgr.WorkerDoWork("TestWorkerUnity");
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

			var mgr = new WorkerManager(container.Resolve<IWorker>());
			mgr.WorkerDoWork("TestMoqWithUnity");
		}
	}
}
