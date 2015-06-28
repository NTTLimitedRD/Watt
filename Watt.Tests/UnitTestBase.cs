using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace DD.Cloud.WebApi.TemplateToolkit.Tests
{
	/// <summary>
	///		Base class for unit-test sets.
	/// </summary>
	public abstract class UnitTestBase
	{
		/// <summary>
		///		Initialise <see cref="UnitTestBase"/>.
		/// </summary>
		protected UnitTestBase()
		{
		}

		/// <summary>
		///		The unit-test execution context.
		/// </summary>
		public TestContext TestContext
		{
			get;
			set;
		}

		/// <summary>
		///		The activity Id for the current unit-test.
		/// </summary>
		protected Guid UnitTestActivityId
		{
			get;
			private set;
		}

		/// <summary>
		///		Perform per-test initialisation.
		/// </summary>
		[TestInitialize]
		public void TestInitialize()
		{
			// Ensure that failed assertions via Debug.Assert do not show any UI if no debugger is attached.
			DefaultTraceListener defaultTraceListener =
					Debug.Listeners
						.OfType<DefaultTraceListener>()
						.FirstOrDefault();
			if (defaultTraceListener != null)
				defaultTraceListener.AssertUiEnabled = Debugger.IsAttached;

			Assert.AreEqual(Guid.Empty, UnitTestActivityId, "Unit-test activity Id has already been initialised.");

			UnitTestActivityId = Guid.NewGuid();
			EventSource.SetCurrentThreadActivityId(UnitTestActivityId);

			TestContext.WriteLine("[UnitTestBase.TestInitialize] Unit-test activity Id is '{0}", UnitTestActivityId);

			OnTestInitialize();
		}

		/// <summary>
		///		Perform per-test clean-up.
		/// </summary>
		[TestCleanup]
		public void TestCleanup()
		{
			Assert.AreNotEqual(Guid.Empty, UnitTestActivityId, "Unit-test activity Id has not been initialised.");

			ExceptionDispatchInfo cleanupException = null;
			try
			{
				OnTestCleanup();
			}
			catch (Exception eCleanup)
			{
				cleanupException = ExceptionDispatchInfo.Capture(eCleanup);
			}

			EventSource.SetCurrentThreadActivityId(Guid.Empty);
			UnitTestActivityId = Guid.Empty;

			if (cleanupException != null)
				cleanupException.Throw();
		}

		/// <summary>
		///		Called when per-test state is being initialised.
		/// </summary>
		protected virtual void OnTestInitialize()
		{
		}

		/// <summary>
		///		Called when per-test state is being cleaned up.
		/// </summary>
		protected virtual void OnTestCleanup()
		{
		}
	}
}
