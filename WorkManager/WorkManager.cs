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
			Debug.WriteLine("Worker: " + msg);
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

	public interface IWorkerManager
	{
		void WorkerDoWork(string msg);
	}

	public class WorkerManager : IWorkerManager
	{
		private readonly IWorker _worker;
		public WorkerManager(IWorker w)
		{
			_worker = w;
		}
		public void WorkerDoWork(string msg)
		{
			_worker.DoWork(msg);
		}
	}


}
