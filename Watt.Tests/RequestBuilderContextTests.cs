using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// ReSharper disable AccessToModifiedClosure (that's the point of these tests)

namespace DD.Cloud.WebApi.TemplateToolkit.Tests
{
	using Mocks;

	/// <summary>
	///		Unit-tests for <see cref="HttpRequestBuilder{TContext}"/> that use a context for resolving deferred parameters.
	/// </summary>
	[TestClass]
	public sealed class RequestBuilderContextTests
	{
		/// <summary>
		///		Verify that a request builder can build and then invoke request with an absolute and then relative template URI (with query parameters) with deferred values that come from a supplied context value.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Can_Invoke_GetRequest_RelativeTemplateUriWithQuery_DeferredValues_FromContext()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			Uri expectedUri = null;
			MockMessageHandler mockHandler = new MockMessageHandler(
				requestMessage =>
				{
					Assert.AreEqual(
						expectedUri,
						requestMessage.RequestUri
					);

					return requestMessage.CreateResponse(HttpStatusCode.OK);
				}
			);

			TestParameterContext testParameterContext = new TestParameterContext();
			using (HttpClient client = new HttpClient(mockHandler))
			{
				HttpRequestBuilder<TestParameterContext> requestBuilder =
					HttpRequestBuilder.Create<TestParameterContext>(baseUri)
						.WithRelativeRequestUri("{action}/{id}?flag={flag?}")
						.WithTemplateParameter("action", context => context.Action)
						.WithTemplateParameter("id", context => context.Id)
						.WithTemplateParameter("flag", context => context.Flag);

				testParameterContext.Action = "foo";
				testParameterContext.Id = 1;
				testParameterContext.Flag = true;

				expectedUri = new Uri(baseUri, "foo/1?flag=True");
                await client.GetAsync(requestBuilder, testParameterContext);

				testParameterContext.Flag = false;

				expectedUri = new Uri(baseUri, "foo/1?flag=False");
				await client.GetAsync(requestBuilder, testParameterContext);
				
				testParameterContext.Action = "diddly";
				testParameterContext.Id = -17;
				testParameterContext.Flag = null;

				expectedUri = new Uri(baseUri, "diddly/-17");
				await client.GetAsync(requestBuilder, testParameterContext);
			}
		}

		/// <summary>
		///		Verify that a request builder can build and then invoke request with an absolute and then relative template URI (with query parameters) and then converted to a more-derived context type, with deferred values that come from a supplied more-derived context value.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Can_Invoke_GetRequest_RelativeTemplateUriWithQuery_DeferredValues_FromConversionToDerivedContext()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			Uri expectedUri = null;
			MockMessageHandler mockHandler = new MockMessageHandler(
				requestMessage =>
				{
					Assert.AreEqual(
						expectedUri,
						requestMessage.RequestUri
					);

					return requestMessage.CreateResponse(HttpStatusCode.OK);
				}
			);

			DerivedTestParameterContext testParameterContext = new DerivedTestParameterContext();
			using (HttpClient client = new HttpClient(mockHandler))
			{
				HttpRequestBuilder<TestParameterContext> requestBuilder =
					HttpRequestBuilder.Create<TestParameterContext>(baseUri)
						.WithRelativeRequestUri("{action}/{id}")
						.WithTemplateParameter("action", context => context.Action)
						.WithTemplateParameter("id", context => context.Id);

				HttpRequestBuilder<DerivedTestParameterContext> derivedRequestBuilder =
					requestBuilder.WithDerivedContext<DerivedTestParameterContext>()
						.WithRelativeRequestUri("data/{derivedOnly?}")
						.WithTemplateParameter("derivedOnly", context => context.DerivedOnly);

				testParameterContext.Action = "foo";
				testParameterContext.Id = 7;
                testParameterContext.DerivedOnly = new Guid("9b880cce-64b1-469f-989b-4326938e43a9");

				expectedUri = new Uri(baseUri, "foo/7");
				await client.GetAsync(requestBuilder, testParameterContext);

				expectedUri = new Uri(baseUri, "foo/7/data/9b880cce-64b1-469f-989b-4326938e43a9");
				await client.GetAsync(derivedRequestBuilder, testParameterContext);

				testParameterContext.DerivedOnly = null;

				expectedUri = new Uri(baseUri, "foo/7");
				await client.GetAsync(requestBuilder, testParameterContext);

				expectedUri = new Uri(baseUri, "foo/7/data/");
				await client.GetAsync(derivedRequestBuilder, testParameterContext);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI (with query parameters) with deferred values that come from a supplied context value.
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeTemplateUriWithQuery_DeferredValues_FromContext()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			HttpRequestBuilder<TestParameterContext> requestBuilder =
				HttpRequestBuilder.Create<TestParameterContext>(baseUri)
					.WithRelativeRequestUri("{action}/{id}?flag={flag?}")
					.WithTemplateParameter("action", context => context.Action)
					.WithTemplateParameter("id", context => context.Id)
					.WithTemplateParameter("flag", context => context.Flag);

			TestParameterContext testParameterContext = new TestParameterContext
			{
				Action = "foo",
				Id = 1,
				Flag = true
			};
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get, testParameterContext))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/1?flag=True"),
					requestMessage.RequestUri
				);
			}

			testParameterContext.Flag = false;
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get, testParameterContext))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/1?flag=False"),
					requestMessage.RequestUri
				);
			}

			testParameterContext.Action = "diddly";
			testParameterContext.Id = -17;
			testParameterContext.Flag = null;
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get, testParameterContext))
			{
				Assert.AreEqual(
					new Uri(baseUri, "diddly/-17"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI (with query parameters) with deferred values that come from the request builder's default (intrinsic) context.
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeTemplateUriWithQuery_DeferredValues_FromDefaultContext()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			TestParameterContext testParameterContext = new TestParameterContext
			{
				Action = "foo",
				Id = 1,
				Flag = true
			};

			HttpRequestBuilder<TestParameterContext> requestBuilder =
				HttpRequestBuilder.Create<TestParameterContext>(baseUri)
					.WithRelativeRequestUri("{action}/{id}?flag={flag?}")
					.WithTemplateParameter("action", context => context.Action)
					.WithTemplateParameter("id", context => context.Id)
					.WithTemplateParameter("flag", context => context.Flag);

			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get, testParameterContext))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/1?flag=True"),
					requestMessage.RequestUri
				);
			}

			testParameterContext.Flag = false;
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get, testParameterContext))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/1?flag=False"),
					requestMessage.RequestUri
				);
			}

			testParameterContext.Action = "diddly";
			testParameterContext.Id = -17;
			testParameterContext.Flag = null;
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get, testParameterContext))
			{
				Assert.AreEqual(
					new Uri(baseUri, "diddly/-17"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		A parameter-resolution context class used for tests.
		/// </summary>
		class TestParameterContext
		{
			/// <summary>
			///		The "Action" parameter.
			/// </summary>
			public string Action
			{
				get;
				set;
			}

			/// <summary>
			///		The "Id" parameter.
			/// </summary>
			public int Id
			{
				get;
				set;
			}

			/// <summary>
			///		The "Flag" parameter.
			/// </summary>
			public bool? Flag
			{
				get;
				set;
			}
		}

		/// <summary>
		///		A parameter context derived from <see cref="TestParameterContext"/> (used to test conversions).
		/// </summary>
		class DerivedTestParameterContext
			: TestParameterContext
		{
			public Guid? DerivedOnly
			{
				get;
				set;
			}
		}
	}
}
