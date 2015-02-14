﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see license file in the main folder

using Aura.Data;
using Aura.Data.Database;
using System;
using System.Text.RegularExpressions;

namespace Aura.Channel.World.Weather
{
	/// <summary>
	/// Official random weather pattern, based on data loaded from db.
	/// </summary>
	public class WeatherProviderTable : IWeatherProvider
	{
		private int _tableId;

		public string Name { get; private set; }

		public WeatherProviderTable(string name)
		{
			_tableId = Convert.ToInt32(name.Substring("type".Length)) - 1;
			this.Name = name;
		}

		private int NameToId(string name)
		{
			if (Regex.IsMatch(name, "type[0-9]+"))
				return Convert.ToInt32(name.Substring("type".Length)) - 1;

			return -1;
		}

		public WeatherDetails GetWeather(DateTime dt)
		{
			return AuraData.WeatherDataDb.GetWeather(_tableId, dt);
		}

		public float GetWeatherAsFloat(DateTime dt)
		{
			var details = this.GetWeather(dt);
			if (details == null)
				return 0.5f;

			if (details.Type == WeatherType.Clear)
				return 0.5f;

			if (details.Type == WeatherType.Clouds)
				return 1.0f;

			return 1.95f + (0.5f / 20 * details.RainStrength);
		}
	}
}
