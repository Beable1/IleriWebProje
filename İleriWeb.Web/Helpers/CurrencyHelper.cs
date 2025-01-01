namespace IleriWeb.Web.Helpers
{
    public class CurrencyHelper
    {

        public static decimal ConvertCurrency(decimal price, double? rate)
		{

			if (rate == null)
			{
				return price;
			}
			var dprice = ((double)price);
			return ((decimal)(dprice * rate));
		}

	}
}
