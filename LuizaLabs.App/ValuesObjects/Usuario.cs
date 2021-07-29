using System;
using System.ComponentModel.DataAnnotations;

namespace LuizaLabs.App.ValuesObjects
{
    public class Usuario
    {
        public Guid Id { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome usuário obrigatório.")]
        public string Nome { get; set; }
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Email usuário obrigatório.")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Senha usuário obrigatório.")]
        [StringLength(255, ErrorMessage = "Senha deve conter de 8 à 255 caracteres.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessage = "Confirmação de senha obrigatório.")]
        [StringLength(255, ErrorMessage = "Senha deve conter de 8 à 255 caracteres.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Senha")]
        public string ConfirmaSenha { get; set; }
        public string Token { get; set; }
    }
}