using IleriWeb.Core.Models;
using IleriWeb.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IleriWeb.Web.Middleware
{
	public class CurrentUserMiddleware
	{
		private readonly RequestDelegate _next;

		public CurrentUserMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
		{
			// Yeni bir scope oluşturuluyor
			using (var scope = serviceProvider.CreateScope())
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

				var userName = context.User?.Identity?.Name ?? string.Empty;
				Console.WriteLine("USERNAME:" + userName);

				if (!string.IsNullOrEmpty(userName))
				{
					var user = await userManager.FindByNameAsync(userName);
					if (user != null)
					{
						var currentUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
						context.Items["CurrentUser"] = currentUser;
					}
				}
			}

			await _next(context);
		}
	}
}
