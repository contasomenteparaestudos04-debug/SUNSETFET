using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Sunset.Api;

// O ASP.NET Core vem com as mensagens de erro de conversão em inglês
// (ex.: "The value 'abc' is not valid for Preço"). Aqui trocamos todas por português.
// É chamada no Program.cs deste projeto. (Existe uma cópia igual na outra camada de apresentação.)
public static class MensagensPtBr
{
    public static void Aplicar(DefaultModelBindingMessageProvider p)
    {
        p.SetValueMustNotBeNullAccessor(_ => "Preencha este campo.");
        p.SetMissingBindRequiredValueAccessor(campo => $"Informe o campo {campo}.");
        p.SetMissingKeyOrValueAccessor(() => "É preciso informar um valor.");
        p.SetMissingRequestBodyRequiredValueAccessor(() => "O corpo da requisição é obrigatório.");
        p.SetValueIsInvalidAccessor(valor => $"O valor '{valor}' não é válido.");
        p.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"O valor '{valor}' não é válido para {campo}.");
        p.SetUnknownValueIsInvalidAccessor(campo => $"O valor informado não é válido para {campo}.");
        p.SetNonPropertyAttemptedValueIsInvalidAccessor(valor => $"O valor '{valor}' não é válido.");
        p.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor informado não é válido.");
        p.SetValueMustBeANumberAccessor(campo => $"O campo {campo} precisa ser um número.");
        p.SetNonPropertyValueMustBeANumberAccessor(() => "O campo precisa ser um número.");
    }
}
