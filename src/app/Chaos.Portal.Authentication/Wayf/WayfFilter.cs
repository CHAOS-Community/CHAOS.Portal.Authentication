﻿using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CHAOS.Extensions;
using Newtonsoft.Json;

namespace Chaos.Portal.Authentication.Wayf
{
	public class WayfFilter : IWayfFilter
	{
		private string _filePath;
		private IList<IDictionary<string, Regex>> _filters;

		public WayfFilter(string filePath)
		{
			_filePath = filePath.ValidateIsNotNullOrEmpty("filePath"); ;
		}

		public WayfFilter(IList<IDictionary<string, Regex>> filters)
		{
			_filters = filters;
		}

		public bool Validate(IDictionary<string, IList<string>> attributes)
		{
			var filters = GetFilter();
			return filters.Any(g => g.All(f => attributes.ContainsKey(f.Key) && attributes[f.Key].Count > 0 && f.Value.IsMatch(attributes[f.Key][0])));
		}

		public IDictionary<string, Regex> GetValidatingRule(IDictionary<string, IList<string>> attributes)
		{
			var filters = GetFilter();
			return filters.FirstOrDefault(g => g.Any(f => attributes.ContainsKey(f.Key) && attributes[f.Key].Count > 0 && f.Value.IsMatch(attributes[f.Key][0])));
		}

		public static IList<IDictionary<string, Regex>> ParseFilter(string filterData)
		{
			var stringFilters = JsonConvert.DeserializeObject<IList<IDictionary<string, string>>>(filterData);
			return stringFilters.Select<IDictionary<string, string>, IDictionary<string, Regex>>(g => g.ToDictionary(f => f.Key, f => new Regex(f.Value))).ToList();
		}

		private IList<IDictionary<string, Regex>> GetFilter()
		{
			if (_filters != null) return _filters;

			if (!Path.IsPathRooted(_filePath))
				_filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "../" + _filePath);

			var fileContent = File.ReadAllText(_filePath);
			_filters = ParseFilter(fileContent);

			return _filters;
		}
	}
}