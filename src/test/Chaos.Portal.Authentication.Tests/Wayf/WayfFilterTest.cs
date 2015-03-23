using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Chaos.Portal.Authentication.Wayf;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Chaos.Portal.Authentication.Tests.Wayf
{
	[TestFixture]
	public class WayfFilterTest : TestBase
	{
		[Test]
		public void Validate_GivenValidAttributes_ShouldReturnTrue()
		{
			var filter = new WayfFilter(CreateFilter());
			var attributes = new Dictionary<string, IList<string>>
			{
				{"mail", new[] {"Dawkins@chaos.com"}},
				{"eduPersonTargetedID", new[] {"WAYF-DK-315880b0f9ef14662c6cbee76b9db72ac82d200a"}}
			};

			var result = filter.Validate(attributes);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Validate_GivenValidAttributesMathSecondFilterGroup_ShouldReturnTrue()
		{
			var filter = new WayfFilter(CreateFilter());
			var attributes = new Dictionary<string, IList<string>>
			{
				{"eduPersonTargetedID", new[] {"ThisContainsSomeValueRightHere"}},
				{"mail", new[] {"Dawkins@chaoqweeeees.com"}}
			};

			var result = filter.Validate(attributes);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Validate_GivenInvalidAttributes_ShouldReturnFalse()
		{
			var filter = new WayfFilter(CreateFilter());
			var attributes = new Dictionary<string, IList<string>>
			{
				{"eduPersonTargetedID", new[] {"test@chaos.com"}},
				{"mail", new[] {"Dawkins@order.com"}}
			};

			var result = filter.Validate(attributes);

			Assert.That(result, Is.False);
		}

		[Test]
		public void Validate_GivenWayfTestAttributes_ShouldReturnTrue()
		{
			var filter = new WayfFilter(CreateFilter());
			var rawAttributes = "{\"sn\":[\"Jensen\"],\"gn\":[\"Jens\"],\"cn\":[\"Jens farmer\"],\"eduPersonPrincipalName\":[\"jj@testidp.wayf.dk\"],\"mail\":[\"jens.jensen@institution.dk\"],\"organizationName\":[\"Institution\"],\"eduPersonAssurance\":[\"2\"],\"schacPersonalUniqueID\":[\"urn:mace:terena.org:schac:personalUniqueID:dk:CPR:0708741234\"],\"eduPersonScopedAffiliation\":[\"student@course1.testidp.wayf.dk\",\"staff@course1.testidp.wayf.dk\",\"staff@course1.testidp.wsayf.dk\"],\"preferredLanguage\":[\"en\"],\"eduPersonEntitlement\":[\"test\"],\"eduPersonPrimaryAffiliation\":[\"student\"],\"schacCountryOfCitizenship\":[\"DK\"],\"eduPersonTargetedID\":[\"WAYF-DK-315880b0f9ef14662c6cbee76b9db72ac82d200a\"],\"schacHomeOrganization\":[\"testidp.wayf.dk\"],\"urn:oid:2.5.4.4\":[\"Jensen\"],\"urn:oid:2.5.4.42\":[\"Jens\"],\"urn:oid:2.5.4.3\":[\"Jens farmer\"],\"urn:oid:1.3.6.1.4.1.5923.1.1.1.6\":[\"jj@testidp.wayf.dk\"],\"urn:oid:0.9.2342.19200300.100.1.3\":[\"jens.jensen@institution.dk\"],\"urn:oid:2.5.4.10\":[\"Institution\"],\"urn:oid:1.3.6.1.4.1.5923.1.1.1.11\":[\"2\"],\"urn:oid:1.3.6.1.4.1.25178.1.2.15\":[\"urn:mace:terena.org:schac:personalUniqueID:dk:CPR:0708741234\"],\"urn:oid:1.3.6.1.4.1.5923.1.1.1.9\":[\"student@course1.testidp.wayf.dk\",\"staff@course1.testidp.wayf.dk\",\"staff@course1.testidp.wsayf.dk\"],\"urn:oid:2.16.840.1.113730.3.1.39\":[\"en\"],\"urn:oid:1.3.6.1.4.1.5923.1.1.1.7\":[\"test\"],\"urn:oid:1.3.6.1.4.1.5923.1.1.1.5\":[\"student\"],\"urn:oid:1.3.6.1.4.1.25178.1.2.5\":[\"DK\"],\"urn:oid:1.3.6.1.4.1.5923.1.1.1.10\":[\"WAYF-DK-315880b0f9ef14662c6cbee76b9db72ac82d200a\"],\"urn:oid:1.3.6.1.4.1.25178.1.2.9\":[\"testidp.wayf.dk\"],\"groups\":[\"realm-testidp.wayf.dk\",\"users\",\"members\"]}";
			var attributes = JsonConvert.DeserializeObject<Dictionary<string, IList<string>>>(rawAttributes);

			var result = filter.Validate(attributes);

			Assert.That(result, Is.True);
		}

		[Test]
		public void Validate_GivenFileToLoad_ShouldLoadFilter()
		{
			var filter = new WayfFilter(Path.GetFullPath("WayfFilter.json"));
			var attributes1 = new Dictionary<string, IList<string>>
			{
				{"eduPersonTargetedID", new[] {"test@chaos.com"}},
				{"mail", new[] {"Dawkins@super.com"}}
			};
			var attributes2 = new Dictionary<string, IList<string>>
			{
				{"eduPersonTargetedID", new[] {"WAYF-DK-315880b0f9ef14662c6cbee76b9db72ac82d200a"}},
				{"mail", new[] {"Dawkins@chaos.com"}}
			};
			var attributes3 = new Dictionary<string, IList<string>>
			{
				{"eduPersonTargetedID", new[] {"WAYF-DK-315880b0f9ef14662c6cbee76b9db72ac82d200a"}},
				{"mail", new[] {"Dawkins@test.com"}}
			};

			var result1 = filter.Validate(attributes1);
			var result2 = filter.Validate(attributes2);
			var result3 = filter.Validate(attributes3);

			Assert.That(result1, Is.False);
			Assert.That(result2, Is.True);
			Assert.That(result3, Is.False);
		}

		private IList<IDictionary<string, Regex>> CreateFilter()
		{
			return new[]
			{
				new Dictionary<string, Regex>
				{
					{"eduPersonTargetedID", new Regex("[x]|[y]|WAYF-DK")},
					{"mail", new Regex(".+@chaos\\.com")}
				},
				new Dictionary<string, Regex>
				{
					{"eduPersonTargetedID", new Regex("SomeValue")}
				},new Dictionary<string, Regex>
				{
					{"eduPersonPrincipalName", new Regex("jj@testidp.wayf.dk")}
				},
			};
		}
	}
}