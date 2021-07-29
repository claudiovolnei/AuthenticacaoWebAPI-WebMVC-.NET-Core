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
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IConfiguration config, IUsuarioRepository usuarioRepository)
        {
            _config = config;
            _usuarioRepository = usuarioRepository;
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UsuarioId(Guid id)
        {
            var result = new Result<Usuario>();
            try
            {
                var usuarioValido = await _usuarioRepository.GetUsuarioId(id);
                if (usuarioValido != null)
                {
                    result.Status = true;
                    result.Dados = usuarioValido;
                    result.Mensagem = "Usuário encontrado com sucesso!";
                    return Ok(result);
                }
                else
                {
                    result.Status = false;
                    result.Dados = null;
                    result.Mensagem = "Usuário não encontrado!";
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
        [HttpGet]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UsuarioEmail(string email)
        {
            var result = new Result<Usuario>();
            try
            {
                var usuarioValido = await _usuarioRepository.GetUsuarioEmail(email);
                if (usuarioValido != null)
                {
                    result.Status = false;
                    result.Dados = usuarioValido;
                    result.Mensagem = "Usuário já cadastrado no sistema!";
                    return Unauthorized(result);
                }
                else
                {
                    result.Status = true;
                    result.Mensagem = "Usuário não possui cadastro!";
                    return Ok(result);

                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Mensagem = ex.Message;
                return BadRequest(result);
            }
        }
    }
}