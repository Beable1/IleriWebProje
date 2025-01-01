using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Services
{
	public interface ICurrencyService
	{
		public double? Rate { get; set; }

		public string? Currency { get; set; }

		void UpdateRate(double newRate);

		void UpdateCurrency(int newCurrency);
	}
}
