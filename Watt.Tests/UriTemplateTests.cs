using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DD.Cloud.WebApi.TemplateToolkit.Tests
{
	using Templates;

	/// <summary>
	///		Unit-tests for URI templating functionality.
	/// </summary>
	[TestClass]
	public sealed class UriTemplateTests
		: UnitTestBase
	{
		/// <summary>
		///		Create a new URI templating unit-test suite.
		/// </summary>
		public UriTemplateTests()
		{
		}

		/// <summary>
		///		Verify that template segments can be parsed from a URI.
		/// </summary>
		[TestMethod]
		public void Can_Parse_TemplateSegments_From_Uri()
		{
			IReadOnlyList<TemplateSegment> segments = TemplateSegment.Parse(
				"api/{controller}/{action}/{id?}/properties"
			);
			
			Assert.AreEqual(6, segments.Count);
			Assert.IsInstanceOfType(segments[0], typeof(RootUriSegment));
			
			Assert.IsInstanceOfType(segments[1], typeof(LiteralUriSegment));
			LiteralUriSegment apiSegment = (LiteralUriSegment)segments[1];
			Assert.AreEqual("api", apiSegment.Value);
			
			Assert.IsInstanceOfType(segments[2], typeof(ParameterizedUriSegment));
			ParameterizedUriSegment controllerSegment = (ParameterizedUriSegment)segments[2];
			Assert.IsTrue(controllerSegment.IsDirectory);
			Assert.AreEqual("controller", controllerSegment.TemplateParameterName);
			Assert.IsFalse(controllerSegment.IsOptional);

			Assert.IsInstanceOfType(segments[3], typeof(ParameterizedUriSegment));
			ParameterizedUriSegment actionSegment = (ParameterizedUriSegment)segments[3];
			Assert.IsTrue(actionSegment.IsDirectory);
			Assert.AreEqual("action", actionSegment.TemplateParameterName);
			Assert.IsFalse(actionSegment.IsOptional);

			Assert.IsInstanceOfType(segments[4], typeof(ParameterizedUriSegment));
			ParameterizedUriSegment idSegment = (ParameterizedUriSegment)segments[4];
			Assert.IsTrue(idSegment.IsDirectory);
			Assert.AreEqual("id", idSegment.TemplateParameterName);
			Assert.IsTrue(idSegment.IsOptional);

			Assert.IsInstanceOfType(segments[5], typeof(LiteralUriSegment));
			LiteralUriSegment propertiesSegment = (LiteralUriSegment)segments[5];
			Assert.IsFalse(propertiesSegment.IsDirectory);
			Assert.AreEqual("properties", propertiesSegment.Value);
		}

		/// <summary>
		///		Verify that template segments can be parsed from a URI with a query component.
		/// </summary>
		[TestMethod]
		public void Can_Parse_TemplateSegments_From_Uri_WithQuery()
		{
			IReadOnlyList<TemplateSegment> segments = TemplateSegment.Parse(
				"api/{controller}/{action}/{id?}/properties?propertyIds={propertyGroupIds}&diddly={dee?}&foo=bar"
			);

			Assert.AreEqual(9, segments.Count);
			Assert.IsInstanceOfType(segments[0], typeof(RootUriSegment));

			Assert.IsInstanceOfType(segments[1], typeof(LiteralUriSegment));
			LiteralUriSegment apiSegment = (LiteralUriSegment)segments[1];
			Assert.AreEqual("api", apiSegment.Value);

			Assert.IsInstanceOfType(segments[2], typeof(ParameterizedUriSegment));
			ParameterizedUriSegment controllerSegment = (ParameterizedUriSegment)segments[2];
			Assert.IsTrue(controllerSegment.IsDirectory);
			Assert.AreEqual("controller", controllerSegment.TemplateParameterName);
			Assert.IsFalse(controllerSegment.IsOptional);

			Assert.IsInstanceOfType(segments[3], typeof(ParameterizedUriSegment));
			ParameterizedUriSegment actionSegment = (ParameterizedUriSegment)segments[3];
			Assert.IsTrue(actionSegment.IsDirectory);
			Assert.AreEqual("action", actionSegment.TemplateParameterName);
			Assert.IsFalse(actionSegment.IsOptional);

			Assert.IsInstanceOfType(segments[4], typeof(ParameterizedUriSegment));
			ParameterizedUriSegment idSegment = (ParameterizedUriSegment)segments[4];
			Assert.IsTrue(idSegment.IsDirectory);
			Assert.AreEqual("id", idSegment.TemplateParameterName);
			Assert.IsTrue(idSegment.IsOptional);

			Assert.IsInstanceOfType(segments[5], typeof(LiteralUriSegment));
			LiteralUriSegment propertiesSegment = (LiteralUriSegment)segments[5];
			Assert.IsFalse(propertiesSegment.IsDirectory);
			Assert.AreEqual("properties", propertiesSegment.Value);

			Assert.IsInstanceOfType(segments[6], typeof(ParameterizedQuerySegment));
			ParameterizedQuerySegment propertyIdsSegment = (ParameterizedQuerySegment)segments[6];
			Assert.AreEqual("propertyIds", propertyIdsSegment.QueryParameterName);
			Assert.AreEqual("propertyGroupIds", propertyIdsSegment.TemplateParameterName);
			Assert.IsFalse(propertyIdsSegment.IsOptional);

			Assert.IsInstanceOfType(segments[7], typeof(ParameterizedQuerySegment));
			ParameterizedQuerySegment diddlySegment = (ParameterizedQuerySegment)segments[7];
			Assert.AreEqual("diddly", diddlySegment.QueryParameterName);
			Assert.AreEqual("dee", diddlySegment.TemplateParameterName);
			Assert.IsTrue(diddlySegment.IsOptional);

			Assert.IsInstanceOfType(segments[8], typeof(LiteralQuerySegment));
			LiteralQuerySegment fooSegment = (LiteralQuerySegment)segments[8];
			Assert.AreEqual("foo", fooSegment.QueryParameterName);
			Assert.AreEqual("bar", fooSegment.QueryParameterValue);
		}

		/// <summary>
		///		Verify that template with a query component can be populated.
		/// </summary>
		[TestMethod]
		public void Can_Populate_Template_WithQuery()
		{
			UriTemplate template = new UriTemplate(
				"api/{controller}/{action}/{id?}/properties?propertyIds={propertyGroupIds}&diddly={dee?}&foo=bar"
			);

			Uri generatedUri = template.Populate(
				baseUri: new Uri("http://test-host/"), 
				templateParameters: new Dictionary<string, string>
				{
					{ "controller", "organizations" },
					{ "action", "distinct" },
					{ "propertyGroupIds", "System.OrganizationCommercial;EnterpriseMobility.OrganizationAirwatch" }
				}
			);

			Assert.AreEqual(
				"http://test-host/api/organizations/distinct/properties?propertyIds=System.OrganizationCommercial%3BEnterpriseMobility.OrganizationAirwatch&foo=bar",
				generatedUri.AbsoluteUri
			);
		}
	}
}
