using Sunset.Api;
using Microsoft.EntityFrameworkCore;
using Sunset.Negocio;
using Sunset.Repositorio;
using Sunset.Repositorio.Contexto;
using Sunset.Repositorio.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// A connection string vem do appsettings.json, nunca do código-fonte.
var connectionString = builder.Configuration.GetConnectionString("Padrao")
    ?? throw new InvalidOperationException("Connection string 'Padrao' não encontrada no appsettings.json.");

// ---------- Injeção de dependência ----------
// Lista de montagem do sistema: cada linha ensina o ASP.NET Core a criar uma peça.
// Quando um controller pede um QuadraNegocio no construtor, o ASP.NET monta a cadeia:
//   controller -> QuadraNegocio -> IQuadraRepositorio (QuadraRepositorio) -> AppDbContext
// ATENÇÃO: este bloco é igual no Program.cs da Web. Mudou aqui, mude lá também.

// 1) O banco
builder.Services.AddDbContext<AppDbContext>(opcoes => opcoes.UseSqlServer(connectionString));

// 2) Repositórios (interface -> classe)
builder.Services.AddScoped<IQuadraRepositorio, QuadraRepositorio>();
builder.Services.AddScoped<IAgendamentoRepositorio, AgendamentoRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();

// 3) Negócio
builder.Services.AddScoped<QuadraNegocio>();
builder.Services.AddScoped<AgendamentoNegocio>();
builder.Services.AddScoped<UsuarioNegocio>();
builder.Services.AddScoped<RelatorioNegocio>();

// Cada entidade nova ganha duas linhas aqui: uma de repositório e uma de negócio.
// --------------------------------------------

builder.Services.AddControllers(opcoes =>
        MensagensPtBr.Aplicar(opcoes.ModelBindingMessageProvider))
    .ConfigureApiBehaviorOptions(opcoes =>
    {
        // Erro de validação sai no mesmo formato dos outros erros da Api: {"erro": "mensagem"}.
        // Assim o app Flutter mostra sempre uma mensagem em português.
        opcoes.InvalidModelStateResponseFactory = contexto =>
        {
            var mensagem = contexto.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault(m => !string.IsNullOrWhiteSpace(m));

            // Erro de JSON mal formado vem em inglês do .NET; nesse caso usamos um texto nosso.
            if (mensagem == null || mensagem.Contains("JSON") || mensagem.Contains("could not"))
                mensagem = "Dados inválidos. Confira os campos e tente de novo.";

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new { erro = mensagem });
        };
    });

// CORS: deixa o app Flutter rodando no navegador (outra porta) chamar a Api.
// Para o PIM está liberado para qualquer origem; num sistema real ficaria restrito.
builder.Services.AddCors(opcoes => opcoes.AddDefaultPolicy(
    politica => politica.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// OpenAPI: gera a documentação dos endpoints em /openapi/v1.json.
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Scalar: tela para ver e testar os endpoints no navegador, em /scalar.
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
