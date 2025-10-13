using System;

namespace Models
{
    [Serializable]
	public class IPInfo
	{
		public string ip;
		public string city;
		public string region;
		public string country;
		public string country_code;
        public string countryCode;
		public string country_name;
		public bool in_eu;
		public string postal;
		public float longitude;
		public float latitude;
		public string timezone;
		public string currency;
        public string languages;

		public string org;
		public IPInfo()
		{
		}
	}

}
