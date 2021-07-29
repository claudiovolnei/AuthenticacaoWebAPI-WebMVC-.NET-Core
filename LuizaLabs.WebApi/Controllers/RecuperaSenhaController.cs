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
    public class RecuperaSenhaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRecuperaSenhaRepository _recuperaSenhaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public RecuperaSenhaController(IConfiguration config, IRecuperaSenhaRepository recuperaSenhaRepository, IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _config = config;
            _recuperaSenhaRepository = recuperaSenhaRepository;
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
                var usuarioValido = await _usuarioRepository.GetUsuarioEmail(usuario.Email);
                if (usuarioValido != null)
                {
                    _recuperaSenhaRepository.Add(new RecuperacaoSenhaUsuario
                    {
                        UsuarioId = usuarioValido.Id,
                        SenhaAnterior = usuarioValido.Senha,
                        Ativa = true,
                        HorarioSolicitacao = DateTime.Now
                    });

                    if (await _recuperaSenhaRepository.SaveChangesAsync())
                    {
                        result.Status = true;
                        result.Mensagem = "Recuperação de senha criada com sucesso!";
                        return Ok(result);
                    }
                    else
                    {
                        result.Status = false;
                        result.Mensagem = "Problema ao gerar recuperação de senha!";
                        return BadRequest(result);
                    }
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
    }
}