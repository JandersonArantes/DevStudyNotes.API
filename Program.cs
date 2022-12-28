using DevStudyNotes.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
// *** SGBD SQL Server ***
var connectionString = builder.Configuration.GetConnectionString("DevStudyNotes");
builder.Services.AddDbContext<StudyNoteDbContext>(
    // Informa a string de conexão "connectionString"
    o => o.UseSqlServer(connectionString)
);
*/

// Configurando o Serilog para escrever para o SQL Server e para o Console. 
// Utilizar a string de conexão connectionString
// E criar uma tabela no SQL Server com o nome Logs, caso não exista.
/*
builder.Host.ConfigureAppConfiguration((hostingContext, config) => {
    Serilog.Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.MSSqlServer(connectionString,
            sinkOptions: new MSSqlServerSinkOptions() {
                AutoCreateSqlTable = true,
                TableName = "Logs"
            })
        .WriteTo.Console()
        .CreateLogger();
}).UseSerilog();
*/

// *** BANCO DE DADOS EM MEMÓRIA ***
builder.Services.AddDbContext<StudyNoteDbContext>(
    // Nomeia o Database de "StudyNoteDb"
    o => o.UseInMemoryDatabase("StudyNoteDb")
);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
    // Informações sobre a API
        Title = "DevStudyNotes.API - Notas de estudo",
        Version = "v1",
    // Informações de contato do desenvolvedor Janderson Alves de Arantes
        Contact = new OpenApiContact {
            Name = "Janderson Alves de Arantes",
            Email = "janderson_alves@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/janderson-alves-arantes-0487367b/")
        }
    });

    // Configuração para enviar os comentários para o arquivo XML
    // Por padrão, usar o NomeDoProjeto.xml
    var xmlFile = "DevStudyNotes.API.xml";
    // O caminho do arquivo é o mesmo da pasta base da aplicação
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
