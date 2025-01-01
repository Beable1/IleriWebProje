using IleriWeb.Core.Services;

namespace IleriWeb.Web.Middleware
{
	public class CurrencyMiddleware
	{
		private readonly RequestDelegate _next;

		public CurrencyMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, ICurrencyService currencyService)
		{
			// Örnek: Her istek için Rate'i güncelle
			if (!context.Items.ContainsKey("CurrencyRate"))
			{
				context.Items["CurrencyRate"] = currencyService.Rate;
				context.Items["Currency"] = currencyService.Currency;
			}

			await _next(context);
		}
	}

}
