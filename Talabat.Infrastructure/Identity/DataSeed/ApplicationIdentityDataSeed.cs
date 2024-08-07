﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure.Identity.DataSeed
{
	public static class ApplicationIdentityDataSeed
	{
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
		{
			if(!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Ahmed Nasr",
					Email = "ahmed.nasr@linkdev.com",
					UserName = "ahmed.nasr",
					PhoneNumber = "01152033799",
				};

				await userManager.CreateAsync(user,"P@ssw0rd");
			}

		}
	}
}
