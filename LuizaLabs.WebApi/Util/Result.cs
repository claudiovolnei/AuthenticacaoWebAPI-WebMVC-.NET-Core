namespace LuizaLabs.WebApi.Util
{
    public class Result<T>
    {
        public bool Status { get; set; }

        public string Mensagem { get; set; }

        public T Dados { get; set; }
    }
}