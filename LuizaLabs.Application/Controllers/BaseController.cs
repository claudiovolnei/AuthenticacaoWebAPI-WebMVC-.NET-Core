using System;
using System.Collections.Generic;
using System.Security.Claims;
using LuizaLabs.Application.ValuesObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace LuizaLabs.Application.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHttpContextAccessor _contextAcessor;
        public BaseController(IHttpContextAccessor contextAcessor)
        {
            _contextAcessor = contextAcessor;
        }
        private Usuario _usuario;
        // protected Usuario Usuario
        // {
        //     get
        //     {
        //         var claims = new List<Claim>
        //         {
        //             new Claim(ClaimTypes.Authentication, Convert.ToString(_usuario.Id))
        //         };

        //         var userIdentity = new ClaimsIdentity(claims, ClaimTypes.Name);
        //         _contextAcessor.HttpContext.User = new ClaimsPrincipal(userIdentity);


        //         try
        //         {
        //             if (_contextAcessor.HttpContext.User.Identity.IsAuthenticated == true)
        //             {

        //             }
        //         }
        //         catch (Exception ex)
        //         {

        //         }
        //     }
        // }
    }
}