using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

// ReSharper disable AccessToModifiedClosure (that's the point of these tests)

namespace DD.Cloud.WebApi.TemplateToolkit.Tests
{
	using Mocks;

	/// <summary>
	///		Unit-tests for <see cref="HttpRequestBuilder{TContext}"/>.
	/// </summary>
	[TestClass]
	public sealed class RequestBuilderTests
	{
		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI, then perform an HTTP GET request.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Can_Build_Request_RelativeTemplateUri_Get()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			MockMessageHandler mockHandler = new MockMessageHandler(
				request =>
				{
					Assert.IsNotNull(request);
					Assert.AreEqual(HttpMethod.Get, request.Method);
					Assert.AreEqual(
						new Uri(baseUri, "foo/1234/bar?diddly=bonk"),
						request.RequestUri
					);
					
					return request.CreateResponse(
						HttpStatusCode.OK,
						"Success!",
						new JsonMediaTypeFormatter()
					);
				}
			);

			using (HttpClient mockClient = new HttpClient(mockHandler))
			{
				HttpResponseMessage response = await
					mockClient.BuildRequest(baseUri)
						.WithRelativeRequestUri("foo/{variable}/bar")
						.WithQueryParameter("diddly", "bonk")
						.WithTemplateParameter("variable", 1234)
						.WithTemplateParameter("diddly", "bonk")
						.GetAsync();

				using (response)
				{
					Assert.IsTrue(response.IsSuccessStatusCode);
					Assert.IsNotNull(response.Content);
					Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);

					string responseBody = await response.Content.ReadAsAsync<string>();
					Assert.AreEqual("Success!", responseBody);
				}
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI, expecting a JSON response, and then perform an HTTP POST request.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Can_Build_Request_RelativeUri_ExpectJson_Post()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			MockMessageHandler mockHandler = new MockMessageHandler(
				request =>
				{
					Assert.IsNotNull(request);
					Assert.AreEqual(HttpMethod.Post, request.Method);
					Assert.AreEqual(
						new Uri(baseUri, "foo/bar"),
						request.RequestUri
					);
					Assert.AreEqual(1, request.Headers.Accept.Count);
					Assert.AreEqual(
						"application/json",
						request.Headers.Accept.First().MediaType
					);

					return request.CreateResponse(
						HttpStatusCode.OK,
						"Success!",
						new JsonMediaTypeFormatter()
					);
				}
			);

			using (HttpClient mockClient = new HttpClient(mockHandler))
			{
				HttpResponseMessage response = await
					mockClient.BuildRequest(baseUri)
						.WithRelativeRequestUri("foo/bar")
						.ExpectJson()
						.PostAsync(
							new StringContent("{}")
						);

				using (response)
				{
					Assert.IsTrue(response.IsSuccessStatusCode);
					Assert.IsNotNull(response.Content);
					Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);

					string responseBody = await response.Content.ReadAsAsync<string>();
					Assert.AreEqual("Success!", responseBody);
				}
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI, and then perform a JSON-formatted HTTP POST request.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Can_Build_Request_RelativeUri_PostAsJson()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			MockMessageHandler mockHandler = new MockMessageHandler(
				async request =>
				{
					Assert.IsNotNull(request);
					Assert.AreEqual(HttpMethod.Post, request.Method);
					Assert.AreEqual(
						new Uri(baseUri, "foo/bar"),
						request.RequestUri
					);
					Assert.AreEqual(1, request.Headers.Accept.Count);
					Assert.AreEqual(
						"application/json",
						request.Headers.Accept.First().MediaType
					);

					string requestBody = await request.Content.ReadAsAsync<string>();
					Assert.IsNotNull(requestBody);

					return request.CreateResponse(
						HttpStatusCode.OK,
						Int32.Parse(requestBody),
						new JsonMediaTypeFormatter()
					);
				}
			);

			using (HttpClient mockClient = new HttpClient(mockHandler))
			{
				int responseBody = await
					mockClient.BuildJsonRequest(baseUri)
						.WithRelativeRequestUri("foo/bar")
						.PostAsJsonAsync<int>("1234");

				Assert.AreEqual(1234, responseBody);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI.
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeTemplateUri()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			HttpRequestBuilder<Unit> requestBuilder =
				HttpRequestBuilder.Create(baseUri)
					.WithRelativeRequestUri("{action}/{id}")
					.WithTemplateParameter("action", "foo")
					.WithTemplateParameter("id", "bar");
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/bar"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI with deferred values.
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeTemplateUri_DeferredValues()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			string action = "foo";
			string id = "bar";

			HttpRequestBuilder<Unit> requestBuilder =
				HttpRequestBuilder.Create(baseUri)
					.WithRelativeRequestUri("{action}/{id}")
					.WithTemplateParameter("action", () => action)
					.WithTemplateParameter("id", () => id);

			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/bar"),
					requestMessage.RequestUri
				);
			}

			action = "diddly";
			id = "dee";

			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "diddly/dee"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI (with query parameters) .
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeTemplateUriWithQuery()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			HttpRequestBuilder<Unit> requestBuilder =
				HttpRequestBuilder.Create(baseUri)
					.WithRelativeRequestUri("{action}/{id}?flag={flag}")
					.WithTemplateParameter("action", "foo")
					.WithTemplateParameter("id", "bar")
					.WithTemplateParameter("flag", "true");
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI (with query parameters) with deferred values.
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeTemplateUriWithQuery_DeferredValues()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			string action = "foo";
			string id = "bar";
			string flag = "true";

			HttpRequestBuilder<Unit> requestBuilder =
				HttpRequestBuilder.Create(baseUri)
					.WithRelativeRequestUri("{action}/{id}?flag={flag?}")
					.WithTemplateParameter("action", () => action)
					.WithTemplateParameter("id", () => id)
					.WithTemplateParameter("flag", () => flag);

			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}

			action = "diddly";
			id = "dee";
			flag = null;

			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "diddly/dee"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI with query parameters.
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeUriWithQuery()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			HttpRequestBuilder<Unit> requestBuilder =
				HttpRequestBuilder.Create(baseUri)
					.WithRelativeRequestUri("foo/bar")
					.WithQueryParameter("flag", "true");
			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI with query parameters that have deferred values.
		/// </summary>
		[TestMethod]
		public void Can_Build_Request_RelativeUriWithQuery_DeferredValues()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			string flag = "true";

			HttpRequestBuilder<Unit> requestBuilder =
				HttpRequestBuilder.Create(baseUri)
					.WithRelativeRequestUri("foo/bar")
					.WithQueryParameter("flag", () => flag);

			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}

			flag = null;

			using (HttpRequestMessage requestMessage = requestBuilder.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.AreEqual(
					new Uri(baseUri, "foo/bar"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request not attached to a client, with an absolute and then relative URI, then attach it to a client, and then perform a JSON-formatted HTTP POST request.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Can_Build_Detached_Request_RelativeUri_AttachToClient_PostAsJson()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			MockMessageHandler mockHandler = new MockMessageHandler(
				async request =>
				{
					Assert.IsNotNull(request);
					Assert.AreEqual(HttpMethod.Post, request.Method);
					Assert.AreEqual(
						new Uri(baseUri, "foo/bar"),
						request.RequestUri
					);
					Assert.AreEqual(1, request.Headers.Accept.Count);
					Assert.AreEqual(
						"application/json",
						request.Headers.Accept.First().MediaType
					);

					string requestBody = await request.Content.ReadAsAsync<string>();
					Assert.IsNotNull(requestBody);

					return request.CreateResponse(
						HttpStatusCode.OK,
						Int32.Parse(requestBody),
						new JsonMediaTypeFormatter()
					);
				}
			);

			using (HttpClient mockClient = new HttpClient(mockHandler))
			{
				HttpRequestBuilder<Unit> request =
					HttpRequestBuilder.Create(baseUri)
						.UseJson()
						.WithRelativeRequestUri("foo/bar")
						.WithClient(mockClient);

				int responseBody = await request.PostAsJsonAsync<int>("1234");

				Assert.AreEqual(1234, responseBody);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a relative request not attached to a client, attach it to a client with an absolute base URI, and then perform a JSON-formatted HTTP POST request.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Can_Build_Detached_RelativeUri_Request_AttachToClient_PostAsJson()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			MockMessageHandler mockHandler = new MockMessageHandler(
				async request =>
				{
					Assert.IsNotNull(request);
					Assert.AreEqual(HttpMethod.Post, request.Method);
					Assert.AreEqual(
						new Uri(baseUri, "foo/bar"),
						request.RequestUri
					);
					Assert.AreEqual(1, request.Headers.Accept.Count);
					Assert.AreEqual(
						"application/json",
						request.Headers.Accept.First().MediaType
					);

					string requestBody = await request.Content.ReadAsAsync<string>();
					Assert.IsNotNull(requestBody);

					return request.CreateResponse(
						HttpStatusCode.OK,
						Int32.Parse(requestBody),
						new JsonMediaTypeFormatter()
					);
				}
			);

			using (HttpClient mockClient = new HttpClient(mockHandler))
			{
				mockClient.BaseAddress = baseUri;

				HttpRequestBuilder<Unit> request =
					HttpRequestBuilder.Create("foo/bar")
						.UseJson();

				int responseBody = await
					request.WithClient(mockClient)
						.PostAsJsonAsync<int>("1234");

				Assert.AreEqual(1234, responseBody);
			}
		}

		/// <summary>
		///		Verify that a request builder cannot build a request not attached to a client, with an absolute and then relative URI, and then perform a JSON-formatted HTTP POST request without first attaching it to a client.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[TestMethod]
		public async Task Cannot_Invoke_Detached_Request_RelativeUri_PostAsJson()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			try
			{
				HttpRequestBuilder<Unit> request = HttpRequestBuilder.Create(baseUri);

				await
					request
						.UseJson()
						.WithRelativeRequestUri("foo/bar")
						.PostAsJsonAsync<int>("1234");

				Assert.Fail("Request builder invocation should have failed bacause it was not attached to an HttpClient.");
			}
			catch (InvalidOperationException)
			{
				// Pass
			}
		}
	}
}
