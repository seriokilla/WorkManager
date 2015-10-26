using System;
using System.Diagnostics;

namespace WorkManager
{
	public interface IWorker
	{
		void DoWork(string msg);
	}

	public class Worker : IWorker
	{
		public void DoWork(string msg)
		{
			Debug.WriteLine("Worker1: " + msg);
		}
	}

	public class WorkerDelegate : IWorker
	{
		public WorkerDelegate()
		{
			Work = param => { throw new Exception("Work delegate not initialized."); };
		}
		public Action<string> Work { protected get; set; }
		public void DoWork(string msg)
		{
			Work(msg);
		}
	}

	public class WorkManager
	{
		private IWorker _worker;
		private IWorker Worker
		{
			get { return _worker ?? (_worker = GetWorkerFromIocContainer()); }
			set { _worker = value; }
		}
		public WorkManager()
		{
			GetWorkerFromIocContainer = () => { throw new Exception("GetWorkerFromIocContainer delegate not initialized."); };
		}

		public WorkManager(IWorker w)
		{
			Worker = w;
		}
		public Func<IWorker> GetWorkerFromIocContainer { private get; set; }
		public void WorkerDoWork(string msg)
		{
			Worker.DoWork(msg);
		}
	}

	
}
