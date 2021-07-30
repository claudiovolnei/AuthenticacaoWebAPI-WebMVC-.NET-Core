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
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            var result = new Result<RecuperacaoSenhaUsuario>();
            try
            {
                var usuarioValido = await _usuarioRepository.GetUsuarioEmail(usuario.Email);
                if (usuarioValido != null)
                {
                    var recuperacaoSenha = new RecuperacaoSenhaUsuario
                    {
                        UsuarioId = usuarioValido.Id,
                        SenhaAnterior = usuarioValido.Senha,
                        Ativa = true,
                        HorarioSolicitacao = DateTime.Now
                    };

                    _recuperaSenhaRepository.Add(recuperacaoSenha);

                    if (await _recuperaSenhaRepository.SaveChangesAsync())
                    {
                        result.Status = true;
                        result.Mensagem = "Recuperação de senha criada com sucesso!<br> " +
                        "Acessar : " + _config["UrlApp"] + " /Account/AlterarSenha?Id=" + recuperacaoSenha.Id;
                        result.Dados = recuperacaoSenha;
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

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AlterarSenha([FromBody] RecuperacaoSenhaUsuario recuperacaoSenhaUsuario)
        {
            var result = new Result<RecuperacaoSenhaUsuario>();
            try
            {

                var recuperacaoSenha = await _recuperaSenhaRepository.GetRecuperaSenhaId(recuperacaoSenhaUsuario.Id);
                if (recuperacaoSenha == null)
                {
                    result.Status = false;
                    result.Mensagem = "Recuperação não encontrada!";
                    return NotFound(result);
                }
                else
                {
                    recuperacaoSenha.SenhaNova = recuperacaoSenhaUsuario.SenhaNova;
                    recuperacaoSenha.ConfirmacaoSenhaNova = recuperacaoSenhaUsuario.ConfirmacaoSenhaNova;
                    recuperacaoSenha.Ativa = false;
                    _recuperaSenhaRepository.Update(recuperacaoSenha);
                    if (await _recuperaSenhaRepository.SaveChangesAsync())
                    {
                        var usuario = await _usuarioRepository.GetUsuarioId(recuperacaoSenha.UsuarioId);
                        if (usuario != null)
                        {
                            usuario.Senha = recuperacaoSenha.SenhaNova;
                            _usuarioRepository.Update(usuario);
                            if (await _usuarioRepository.SaveChangesAsync())
                            {
                                result.Status = true;
                                result.Mensagem = "Senha alterada com sucesso";
                                return Ok(result);
                            }
                            else
                            {
                                result.Status = false;
                                result.Mensagem = "Problema ao salvar nova senha!";
                                return BadRequest(result);
                            }
                        }
                        else
                        {
                            result.Status = false;
                            result.Mensagem = "Dados do usuário não encontrado";
                            return NotFound(result);
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Mensagem = "Problema ao salvar nova senha!";
                        return BadRequest(result);
                    }
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
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = new Result<RecuperacaoSenhaUsuario>();
            try
            {
                var recuperaSenha = await _recuperaSenhaRepository.GetRecuperaSenhaId(id);
                if (recuperaSenha != null)
                {
                    result.Status = true;
                    result.Mensagem = "Recuperação de senha encontrado com sucess!";
                    result.Dados = recuperaSenha;
                    return Ok(result);
                }
                else
                {
                    result.Status = false;
                    result.Dados = null;
                    result.Mensagem = "Recuperação de senha não encontrado ou expirou!";
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