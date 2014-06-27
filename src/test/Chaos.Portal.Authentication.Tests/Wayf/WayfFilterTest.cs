using System.Collections.Generic;
using System.Text.RegularExpressions;
using Chaos.Portal.Authentication.Wayf;
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
				{"mail", new[] {"Dawkins@chaos.com"}}
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
		public void Validate_GivenFileToLoad_ShouldLoadFilter()
		{
			var filter = new WayfFilter("WayfFilter.json");
			var attributes1 = new Dictionary<string, IList<string>>
			{
				{"eduPersonTargetedID", new[] {"test@chaos.com"}},
				{"mail", new[] {"Dawkins@super.com"}}
			};
			var attributes2 = new Dictionary<string, IList<string>>
			{
				{"eduPersonTargetedID", new[] {"Kiko"}},
				{"mail", new[] {"Dawkins@chaos.com"}}
			};

			var result1 = filter.Validate(attributes1);
			var result2 = filter.Validate(attributes2);

			Assert.That(result1, Is.False);
			Assert.That(result2, Is.True);
		}

		private IDictionary<string, Regex> CreateFilter()
		{
			return new Dictionary<string, Regex>
			{
				{"eduPersonTargetedID", new Regex("[x]|[y]")},
				{"mail", new Regex(".+@chaos\\.com")}
			};
		}
	}
}