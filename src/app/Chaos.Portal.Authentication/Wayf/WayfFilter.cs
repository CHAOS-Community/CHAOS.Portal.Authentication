using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CHAOS.Extensions;
using Newtonsoft.Json;

namespace Chaos.Portal.Authentication.Wayf
{
	public class WayfFilter : IWayfFilter
	{
		private readonly string _filePath;
		private IDictionary<string, Regex> _filters;

		public WayfFilter(string filePath)
		{
			_filePath = filePath.ValidateIsNotNullOrEmpty("filePath"); ;
		}

		public WayfFilter(IDictionary<string, Regex> filters)
		{
			_filters = filters;
		}

		public bool Validate(IDictionary<string, IList<string>> attributes)
		{
			var filters = GetFilter();
			return filters.Any(f => attributes.ContainsKey(f.Key) && attributes[f.Key].Count > 0 && f.Value.IsMatch(attributes[f.Key][0]));
		}

		private IDictionary<string, Regex> GetFilter()
		{
			if (_filters != null) return _filters;

			var fileContent = File.ReadAllText(_filePath);
			var stringFilters = JsonConvert.DeserializeObject<IDictionary<string, string>>(fileContent);
			_filters = stringFilters.ToDictionary(f => f.Key, f => new Regex(f.Value));

			return _filters;
		}
	}
}