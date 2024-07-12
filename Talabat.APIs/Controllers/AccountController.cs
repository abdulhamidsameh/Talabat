﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMapper _mapper;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null)
				return Unauthorized(new ApiResponse(401,"Invalid Login"));

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

			if (!result.Succeeded)
				return Unauthorized(new ApiResponse(401, "Invalid Login"));

			var userDto = _mapper.Map<ApplicationUser, UserDto>(user);
			userDto.Token = "This Will Be Token";

			return Ok(userDto);


		}

	}
}