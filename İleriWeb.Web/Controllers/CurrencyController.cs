using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Services;

namespace IleriWeb.Web.Controllers
{
	public class CurrencyController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICurrencyService _currencyService;

		public CurrencyController(IHttpClientFactory httpClientFactory, ICurrencyService currencyService)
		{
			_httpClientFactory = httpClientFactory;
			_currencyService = currencyService;
		}


		public async Task<IActionResult> SendCurrencyRate(int id)
		{
			var client = _httpClientFactory.CreateClient();
				_currencyService.UpdateCurrency(id);
			// Gönderilecek veriyi JSON formatına çevir
			var payload = new { rate = id };
			var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

			// API'ye istek at
			var response = await client.PostAsync("http://localhost:3000/api/currency-rate", jsonContent);

			if (response.IsSuccessStatusCode)
			{
				var responseData = await response.Content.ReadAsStringAsync();
				var refererUrl = Request.Headers["Referer"].ToString();
				return Redirect(refererUrl);
			}

			return BadRequest(new { Message = "Failed to send request", Error = response.ReasonPhrase });
		}
	}
}
