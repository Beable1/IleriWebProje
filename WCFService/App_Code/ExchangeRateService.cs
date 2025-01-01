
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WCFService;

/// <summary>
/// Summary description for Service1
/// </summary>
public class ExchangeRateService : IExchangeRateService
{
	public string ReceiveCurrencyRate(double rate)
	{
		// Gelen döviz kurunu işle (örneğin, loglama veya başka bir işlem)
		return ("Received currency rate: {rate}");
	}
}