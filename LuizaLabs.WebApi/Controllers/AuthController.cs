using System;
using System.Security.Principal;
using System.Threading.Tasks;
using LuizaLabs.WebApi.Models;
using LuizaLabs.WebApi.Repositories.Interfaces;
using LuizaLabs.WebApi.Services;
using LuizaLabs.WebApi.Util;
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

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            var result = new Result<Usuario>();
            try
            {
                var usuarioValido = await _usuarioRepository.GetUsuarioAsync(usuario);
                if (usuarioValido != null)
                {
                    usuarioValido.Token = AuthenticationServiceJWT.GerarToken(_config);
                    result.Status = true;
                    result.Dados = usuarioValido;
                    result.Mensagem = "Login realizado com sucesso!";
                    return Ok(result);
                }
                else
                {
                    result.Status = false;
                    result.Dados = null;
                    result.Mensagem = "Usuário ou senha inválidos!";
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Mensagem = ex.Message;
                return BadRequest(result);
            }
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            var result = new Result<Usuario>();
            try
            {
                if (usuario == null)
                {
                    result.Status = false;
                    result.Mensagem = "Usuário invalido ou nulo.";
                    return NotFound(result);
                }

                var validarUsuario = new Usuario(usuario.Nome, usuario.Email, usuario.Senha);
                _usuarioRepository.Add(validarUsuario);

                if (await _usuarioRepository.SaveChangesAsync())
                {
                    result.Status = true;
                    result.Mensagem = "Usuário criado com sucesso!";
                    return Ok(result);
                }

                return BadRequest("Problemas ao registrar usuário.");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}