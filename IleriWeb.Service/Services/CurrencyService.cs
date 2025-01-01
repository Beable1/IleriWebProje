using IleriWeb.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Service.Services
{
	public class CurrencyService : ICurrencyService
	{
		private readonly IMemoryCache _memoryCache;
		private const string RateKey = "CurrencyRate";
		private const string CurrencyKey = "Currency";

		public CurrencyService(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public double? Rate	
		{
			get
			{
				_memoryCache.TryGetValue(RateKey, out double? rate);
				return rate;
			}

			set { }
		}

		public string Currency
		{
			get
			{
				if (!_memoryCache.TryGetValue(CurrencyKey, out string? currency) || string.IsNullOrEmpty(currency))
				{
					// Eğer cache'de değer yoksa veya null ise varsayılan olarak "TRY" döndür
					return "TRY";
				}
				_memoryCache.Set(CurrencyKey, currency);
				return currency;
			}
			set
			{
				// Yeni bir değer atanırsa cache'e kaydet
				if (!string.IsNullOrEmpty(value))
				{
					_memoryCache.Set(CurrencyKey, value);
				}
			}
		}



		public void UpdateCurrency(int newCurrency)
		{
			string Currency;
			if (newCurrency == 1) {
				 Currency = "USD";
			}
			else if (newCurrency == 2)
			{
				Currency = "EUR";
			}else
			{
				Currency = "TRY";
			}
			_memoryCache.Set(CurrencyKey, Currency);
		}
		public void UpdateRate(double newRate)
		{
			_memoryCache.Set(RateKey, newRate);
		}
	}
}
