public class LocalAtivoSpecification
{
    public string MensagemErro => "O local precisa estar ativo para ser atualizado.";
    public bool IsSatisfiedBy(Local local)
    {
        return local.Ativo;
    }
} 