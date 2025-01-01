using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;


// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class Service : IService
{
	public string GetData(int value)
	{
		return string.Format("You entered: {0}", value);
	}

	public string ReceiveCurrencyRate(double rate)
	{
		// Gelen döviz kurunu işle (örneğin, loglama veya başka bir işlem)
		Console.WriteLine(string.Format("Received currency rate: {0}", rate));

		using (HttpClient client = new HttpClient())
        {
            var payload = new { Rate = rate };
			HttpResponseMessage response = client.PostAsJsonAsync("http://localhost:5035/api/exchange/receive", payload).Result;


			
            if (response.IsSuccessStatusCode)
            {
                return "Rate forwarded to ASP.NET successfully.";
            }
            else
            {

                return (response.ReasonPhrase);
            }
        }
	}

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}
}
