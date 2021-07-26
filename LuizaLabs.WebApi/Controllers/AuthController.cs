using System;
using System.Security.Principal;
using System.Threading.Tasks;
using LuizaLabs.WebApi.Models;
using LuizaLabs.WebApi.Repositories.Interfaces;
using LuizaLabs.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LuizaLabs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthController(IConfiguration config, IUsuarioRepository usuarioRepository)
        {
            _config = config;
            _usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            try
            {
                var usuarioValido = await _usuarioRepository.GetUsuarioAsync(usuario);
                if (usuarioValido != null)
                {
                    usuarioValido.Token = AuthenticationServiceJWT.GerarToken(_config);
                    return Ok(usuarioValido);
                }
                else
                {
                    return Unauthorized("Usuário e senha não encontrado!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}