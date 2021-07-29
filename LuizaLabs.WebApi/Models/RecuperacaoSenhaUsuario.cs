using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuizaLabs.WebApi.Models
{
    public class RecuperacaoSenhaUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Display(Name = "Senha Anterior")]
        public string SenhaAnterior { get; set; }
        [Display(Name = "Nova Senha")]
        public string SenhaNova { get; set; }
        [Display(Name = "Confirmação Nova Senha")]
        public string ConfirmacaoSenhaNova { get; set; }
        public bool Ativa { get; set; }
        public DateTime HorarioSolicitacao { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }


    }
}