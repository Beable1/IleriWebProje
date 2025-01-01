using IleriWeb.Core.DTOs;
using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ExchangeController : Controller
	{
		private readonly ICurrencyService _currencyService;

		public ExchangeController(ICurrencyService currencyService)
		{
			_currencyService = currencyService;
		}

		[HttpPost("receive")]
		public IActionResult ReceiveRate([FromBody] RateDto rateDto)
		{
			if (rateDto == null || rateDto.Rate <= 0)
			{
				return BadRequest("Invalid rate value.");
			}

			_currencyService.UpdateRate(rateDto.Rate);
			Console.WriteLine($"Rate received in ASP.NET: {rateDto.Rate}");
			// İşlemler yapılabilir: Veritabanına kaydetme, işleme, vb.
			return Ok(new { message = "Rate received successfully." });
		}
	}
}
