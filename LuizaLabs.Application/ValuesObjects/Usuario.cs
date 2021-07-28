using System;

namespace LuizaLabs.Application.ValuesObjects
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string Token { get; set; }
    }
}