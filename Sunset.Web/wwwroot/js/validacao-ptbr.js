// Mensagens do jQuery Validation em português (o padrão da biblioteca é inglês).
// Só afeta campos validados no navegador; as mensagens dos DTOs já são em português.
$.extend($.validator.messages, {
    required: "Preencha este campo.",
    remote: "Corrija este campo.",
    email: "Informe um e-mail válido.",
    url: "Informe uma URL válida.",
    date: "Informe uma data válida.",
    dateISO: "Informe uma data válida (AAAA-MM-DD).",
    number: "Informe um número válido.",
    digits: "Use apenas dígitos.",
    creditcard: "Informe um número de cartão válido.",
    equalTo: "Digite o mesmo valor novamente.",
    maxlength: $.validator.format("Use no máximo {0} caracteres."),
    minlength: $.validator.format("Use no mínimo {0} caracteres."),
    rangelength: $.validator.format("Use entre {0} e {1} caracteres."),
    range: $.validator.format("Informe um valor entre {0} e {1}."),
    max: $.validator.format("Informe um valor menor ou igual a {0}."),
    min: $.validator.format("Informe um valor maior ou igual a {0}.")
});
