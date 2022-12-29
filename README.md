DevStudyNotes.API

Esta API manipula as notas de estudo de alunos.

Passo a passo para desenvolver a API "DevStudyNotes.API"

_______________________________________________________________________________________________________________

*** API com ASP.NET Core e EF Core ***
_______________________________________________________________________________________________________________

1 - Esta linha de comando cria um projeto com o template/modelo "webapi"  com o nome "DevStudyNotes.API"
dotnet new webapi -o DevStudyNotes.API

2 - No terminal, executar o comando:
dotnet build

3 - Na pasta Controllers, criar a classe StudyNotesController.cs

4 - Herdar a classe ControllerBase para construir a API
public class StudyNotesController : ControllerBase

5 - Incluir namespace:
using Microsoft.AspNetCore.Mvc;

6 - Adicionar uma notação para fazer convenções e simplificar parâmetros
[ApiController]

7 - Definir a rota base (endpoint) da API:
[Route("api/study-notes")]

8 - Criar um ponto de acesso com o método/Action GetAll() >> Consulta todas as notas.
public IActionResult GetAll()
{
    return OK();
}

9 - Criar uma anotação para informar o método/Action GetAll() é um Get.
[HttpGet]

10 - Criar o método/Action GetById(int id) >> Consulta uma nota específica. 
- Informar um HttpGet (VERBO HTTP) com parâmetro.
- endpoint/URL (caminho de acesso) e o método/requisição HTTP:
    api/study-notes/1 HttpGet
  
[HttpGet("{id}")]
public IActionResult GetById(int id)
{
    return OK();
}

11 - Criar uma anotação:
[HttpPost]

12 - Criar o método/Action Post()
public IActionResult Post()
{

}

13 - Criar modelo de entrada e saída de dados:
    - Criar uma pasta com o nome "Models"
    - Criar uma classe com o nome "AddStudyNoteInputModel.cs" >> Modelo de entrada de dados
public class AddStudyNoteInputModel
{
	// Título
	public string Title { get; set; }
	// Descrição
	public string Description { get; set; }
 	// Informa se a nota é pública ou não
	public book IsPublic { get; set; }
}

14 - Passar a classe AddStudyNoteInputModel como parâmetro para requisição HTTP:
public IActionResult Post(AddStudyNoteInputModel model) {}
    
    - Inserir o namespace:
using DevStudyNotes.API.Models;

    - Retornar CreatedAtAction() quando é cadastrado com sucesso:
    // Passar o nome da Action "GetById" e seu parâmetro porque o CreatedAtAction vai
    // retornar no Cabeçalho Location a URL/Caminho para consultar a nota recém-criada.
    // Mas, precisa de algumas informações: (Objeto, Rota, Valor)
return CreatedAtAction("GetById", new { id = studyNote.Id }, model)

16 - Criar o POST para reação (Reaction)
[HttpPost("{id}/reactions")]

17 - Criar o método para cadastrar as Reactions
public IActionResult PostReaction() {}

18 - Criar um modelo de entrada de dados para cadastrar Reactions
    - Na pasta Models, criar uma classe com o nome AddReactionStudyNoteInputModel.cs
    - E inserir as propriedades.

public class AddReactionStudyNoteInputModel
    {
        public int StudyNoteId { get; set; }
        public bool IsPositive { get; set; }                
    }

    - Passar a classe AddReactionStudyNoteInputModel como parâmetro do método PostReaction
public IActionResult PostReaction(int id, AddReactionStudyNoteInputModel model) 
{
    return NoContent();
}

19 - No console, executar:

dotnet build
dotnet run

    - Clicar em uma das URL's
Ex.: https://localhost:7260

    - No browser, inserir "swagger" no final da URl
Ex.: https://localhost:7260/swagger

// O Swagger é um framework composto por diversas ferramentas que, independente da
// linguagem, auxilia a descrição, consumo e visualização de serviços de uma API REST.

Ao testar a API POST /api/study-notes é confirmado que a mesma está funcionando.
    - Retorna o código 201 (Created)
    - Retorna o "cabeçalho location":
        location: https://localhost:7260/api/study-notes/1 

20 - Adicionar as entidades de domínio.

    - Criar uma pasta com o nome "Entities"

    - Criar uma classe com o nome "StudyNote.cs"

    - Criar uma classe com o nome "StudyNoteReaction.cs" e criar propriedades com
    "private set" (encapsulamento) para limitar o acesso
    // prop <enter> >> Cria propriedade com "private set"
public class StudyNoteReaction
{
	public int Id { get; private set; }
	public bool IsPositive { get; private set; }
    public int StudyNoteId { get; private set; }
}

    - É preciso iniciar o campo IsPositive. E os outros campos serão preenchidos com o uso do
    Entiti Famework Core.
    Para gerar o construtor da classe:
        Selecionar a linha "public bool IsPositive { get; private set; }"
        CTRL + .
        "Gerar construtor"

    public StudyNoteReaction(bool isPositive)
        {
            IsPositive = isPositive;
        }


21 - Definindo a entidade StudyNotes.cs
    // "prog <enter>" gera propriedade com "private set"
public class StudyNote
    {
        public int Id { get; private set; }
        public string Title { get; private set; } 
        public string Description { get; private set; }
        public bool IsPublic { get; private set; }  
        public DateTime CreatedAt { get; private set; }
        public List<StudyNoteReaction> Reactions { get; private set; }

        // O métido "AddReaction", libera reação somente se a nota for pública
        public void AddReaction(bool isPositive)
        {
            // Se a nota não for pública, uma exceção será lançada.
            if(!IsPublic) throw new InvalidOperationException();

            // Adiciona uma reação na lista Reactions
            Reactions.Add(new StudyNoteReaction(isPositive));
        }

        Para criar o método construtor da classe:
	    Selecionar as linhas das propriedades: Title, Description e IsPublic
	    CTRL + .
	    Clicar em "Gerar construtor"
        public StudyNote(string title, string description, bool isPublic)
        {
            Title = title;
            Description = description;
            IsPublic = isPublic;

            // Inicializa Reactions com uma lista vazia.
            Reactions = new List<StudyNoteReaction>();

            // Inicializa CreatedAt com a data de hoje.
            CreatedAt = DateTime.Now;
        }
    }

    // Até aqui foram criadas as API's(StudyNotesController), domínio (Entites) e os 
    // modelos de entrada de dados (InputModel)

22 - Em StudyNotesController.cs, configurar para receber a requisição POST com o modelo 
de entrada de dados.
    // Instanciar a classe StudyNote(InputModel)
    // Adicionar o namespace "using DevStudyNotes.API.Entities;"
    var studyNote = new StudyNote(model.Title, model.Description, model.IsPublic);
    // Nos próximos passos, estes dados serão armazenados no SGBD

_______________________________________________________________________________________________________________

Para gerar o banco de dados:
	- Entity Framework Core >> ORM (Object-Relational Mapper) 
	Métodos que permitem interagir com o BD sem necessitar de código SQL
	
	- Principais conceitos:

		Code First >> Primeiro o código. Para gerar o banco de dados automaticamente 
		a partir das entidades criados no código que vão gerar uma Migration

		DbContext >> Representa o banco de dados
		
		DbSet >> Representa a tabela

		Migration >> Representa o banco de dados a ser gerado no SGBD

	- Para garantir que esteja instalada a ferramenta de CLI, no Prompt de camando, 
	executar o comando:
		dotnet tool install --global dotnet-ef 

    - Configurando relacionamentos entre entidades:
		- Criar uma propriedade que representa a chave estrangeira 
		- Criar propriedade de navegação
		- Configurar o relacionamento entre as entidades via Fluent API (preferível utilizar)
		e Data Annotation, junto com configuração de chave estrangeira
	
	- Possibilidade de uso de banco de dados em memória

    - Gerando e aplicando Migrations
		- dotnet ef migrations add NomeDaMigration
			- Parâmetros opcionais comuns: -o, -s
			Onde -o informa a pasta e -s informa o projeto principal

		// Atualiza o banco de dados, baseado nas Migrations que estão pendentes
		- dotnet ef database update
			- Parâmetro opcional comum: -s
			-s 
_______________________________________________________________________________________________________________

// Configurando e utilizando o EF Core
23 - Instalar alguns pacotes/bibliotecas

Parar abrir a extensão Nuget Gallery
CTRL + SHIFT  + P

Clicar em Nuget Gallery
// Para trabalhar com "banco de dados em memória"
Pesquisar por: Microsoft.EntityFrameworkCore.InMemory
Selecionar o projeto DevStudyNotes.API e instalar

// Para trabalhar com o SGBD SQL Server
Pesquisar Microsoft.EntityFrameworCore.SqlServer
Selecionar o projeto DevStudyNotes.API e instalar

Pesquisar por Microsoft.EntityFrameworkCore.Design
Selecionar o projeto DevStudyNotes.API e instalar


// Para adicionar referência:
// CTRL + .

// Configuração para persistir/armazenas os dados
24 - Criar uma pasta com o nome Persistence
    
    - Criar uma classe com o nome StudyNoteDbContext.cs
    - Herda a classe DbContext
    public class StudyNoteDbContext : DbContext
    {
        // No construtor, passar o DbContextOptions com tipo StudyNoteDbContext 
        //com o nome "options" para passar "options" para classe base e funcionar 
        // com o uso do Entity Framework Core 
        public StudyNoteDbContext(DbContextOptions<StudyNoteDbContext> options) : base(options)
        {
            
        }
    }

    Em seguida, criar uma propriedade DbSet para cada tabela a ser gerada.
    public DbSet<StudyNote> StudyNotes { get; set; }
    public DbSet<StudyNoteReaction> StudyNoteReactions { get; set; }
        
    // override para estender ou modificar um método virtual/abstrato
    // e recebe ModelBuilder como parâmetro
protected override void OnModelCreating(ModelBuilder builder)
        {
            // Informar qual entidade configurar
            // Usar expressão Lambda
            builder.Entity<StudyNote>(e => {
                // Determina a chave primária
                e.HasKey(s => s.Id);

                // Configurar o relacionamento.
                // Um StudyNote tem muitas Reactions
                e.HasMany(s => s.Reactions)
                    // Uma Reaction tem apenas uma StudyNote
                    .WithOne()
                    // Determina a chave estrangeira
                    .HasForeignKey(r => r.StudyNoteId)
                    // Não permite remover em cascata
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<StudyNoteReaction>(sn => {
                sn.HasKey(s => s.Id);
            });
        }  

24 - Em Program.cs

// *** BANCO DE DADOS EM MEMÓRIA ***
builder.Services.AddDbContext<StudyNoteDbContext>(
    // Nomeia o Database de "StudyNoteDb"
    o => o.UseInMemoryDatabase("StudyNoteDb")
);

25 - Configuração para acessar o DbContext por Injeção de Dependência 
no construtor da StudyNotesController.cs
// Injeção de Dependência: é um padrão de projeto que permite baixo acoplamento do código. É um código que possui uma alta dependência de um outro código, onde o relacionamento entre os dois é muito forte.

 private readonly StudyNoteDbContext _context;

        // Configuração para acessar o DbContext por Injeção de Dependência
        public StudyNotesController(StudyNoteDbContext context)
        {
            _context = context;
        }

26 - Cria a variável studyNotes que recebe StudyNotes
public IActionResult GetAll()
        {
            // Cria a variável studyNotes que recebe StudyNotes
            var studyNotes = _context.StudyNotes.ToList();

            // Retorna/Consulta todas as "notas de estudo"
            return Ok(studyNotes);
        }

public IActionResult GetById(int id)
        {
            // studyNotes recebe a nota de estudo com Id = id
            var studyNotes = _context.StudyNotes.SingleOrDefault(s => s.Id == id);

            // Se não for encontrada nenhum nota de estudo, retorna NotFound
            if (studyNotes == null) return NotFound();

            // Retorna a nota de estudo com o id informado
            return Ok(studyNotes);
        }

public IActionResult Post(AddStudyNoteInputModel model)
        {
            // Instanciar a classe StudyNote para receber o model(InputModel)
            var studyNote = new StudyNote(model.Title, model.Description, model.IsPublic);

            // Adiciona studyNote em StudyNotes
            _context.StudyNotes.Add(studyNote);
            // Sempre que ocorrer alteração de dados, deve-se efetivar a operação com SaveChanges()            
            _context.SaveChanges();
            
            return CreatedAtAction("GetById", new { id = 1 }, model);
        }

public IActionResult PostReaction(int id, AddReactionStudyNoteInputModel model)
        {
            // studyNotes recebe a nota de estudo com Id = id
            var studyNotes = _context.StudyNotes.SingleOrDefault(s => s.Id == id);

            // Se a nota de estudo não foi encontra, retorna BadRequest
            if (studyNotes == null) return BadRequest();

            // Adiciona a reação informada
            studyNotes.AddReaction(model.IsPositive);
            // SaveChanges() para persistir as operações.
            _context.SaveChanges();
            return NoContent();
        }

27 - Testar a aplicação. No terminal:
// Para abrir o terminal
// CTRL + `
dotnet run

- Obs.: No momento está sendo executado o "BANCO DE DADOS EM MEMÓRIA".
- Ao executar (Consultar todos) GET api/study-notes retorna uma lista vazia. Ou seja, está consultando:
Response body
[]

- Em POST api/study-notes para testar o cadastro de  uma nota de estudo:
Request body
{
  "title": "Azure 204",
  "description": "AZ 204",
  "isPublic": true
}
- Clicar em Execute
Retorna o cabeçalho: location https://localhost:7260/api/study-notes/1

- Clicar novamente em Execute
Retorna o cabeçalho: location https://localhost:7260/api/study-notes/2

- Clicando em GET >> Execute
Retorna todas as notas de estudo cadastradas

- Informar o id = 1 em GET api/study-notes/{id}
Retorna a nota de estudo com id = 1

- Informar o id = 900 (não foi cadastrado)
Retorna Not Found porque foi configurado para isso quando o id não é localizado.

- Cadastrar uma Reaction positiva e uma negativa
- Consultar todas as notas
- Não exibe o cadastro das Reactions. Por isso, fazer a configuração:
public IActionResult GetById(int id)
        {
            // studyNotes recebe a nota de estudo com Id = id
            var studyNotes = _context.StudyNotes
            *** // Precisa incluir as Reactions porque por padrão não estão incluídas. ***
            *** .Include(s => s.Reactions) ***
                .SingleOrDefault(s => s.Id == id);

            // Se não for encontrada nenhum nota de estudo, retorna NotFound
            if (studyNotes == null) return NotFound();

            // Retorna a nota de estudo com o id informado
            return Ok(studyNotes);
        }

- Cadastrar Reaction para testar
- Consultar tudo em GET para conferir as notas e reações cadastradas

_______________________________________________________________________________________________________________

*** Gerar Migrations para trabalhar com o SGBD SQL Server ***
_______________________________________________________________________________________________________________

- No arquivo appsettings.json, configurar:
"ConnectionStrings": {
    "DevStudyNotes": "Server=DESKTOP-NQL6GK8\\SQLEXPRESS; Database=DbDevStudyNotes2; Integrated Security=True; trustServerCertificate=True"
  }
// "DevStudyNotes" é o nome para conexão
// DbDevStudyNotes2 é o nome para o banco de dados a ser gerado no SQL Server

- No arquivo Program.cs, criar uma variável para acessar a string de conexão "DevStudyNotes"
// *** SGBD SQL Server ***
var connectionString = builder.Configuration.GetConnectionString("DevStudyNotes");
builder.Services.AddDbContext<StudyNoteDbContext>(
    // Informa a string de conexão "connectionString"
    o => o.UseSqlServer(connectionString)
);

- Comentar a configuração para usar BANCO DE DADOS EM MEMÓRIA:
/*
// *** BANCO DE DADOS EM MEMÓRIA ***
builder.Services.AddDbContext<StudyNoteDbContext>(
    // Nomeia o Database de "StudyNoteDb"
    o => o.UseInMemoryDatabase("StudyNoteDb")
);
*/

28 - No terminal, executar a linha de comando abaixo para crair a Migration:
// Obs.: O diretório deve ser do projeto
// Criando a Migration
dotnet ef migrations add PrimeiraMigration -o Persistence/Migrations

// Executando a migração no SGBD SQL Server
dotnet ef database update


29 - Executar a aplicação
dotnet run

    - Testar as requisições e repostas com o Swagger e abrir o SQL Server e consultar as tabelas
    StudyNotes e StudyNoteReactions.

_______________________________________________________________________________________________________________

*** SWAGGER, LOGS e PUBLICAÇÃO NA NUVEM ***
_______________________________________________________________________________________________________________

* Documentação com Swagger *
- Além de gerar a documentação de API de forma mais "crua" a partir dos
  Controllers e Actions, é possível detalhar diversos dados
- Por exemplo, informações como descrição, parâmetros e códigos de 
- Com isso, a integração e comunicação entre membros de equipe e outras equipes
  é bastante facilitada

* O que adiconar ao documentar APIs? *
- Descrição
- Exemplo de objeto para envio (POST, PUT)
- Códigos de resposta possíveis no endpoint
- Parâmetros e suas descrições

30 - Configuração para gerar arquivo de documentação para o Swagger e também para não ficar recebendo 
"Warn" por não documentar uma Action, por exemplo.
No arquivo .csproj adicionar:

<PropertyGroup>
	<GenerateDocumentationFile>true</GenerateDocumentationFile>
	<NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>

31 - Instalar a extensão C# XML Documentation Comments

32 - Em StudyNotesController.cs:

    - Acima de POST, para gerar uma estrutura de um comentário , pressionar ///
    - Em <summary>, digitar:
        - Cadastrar uma nota de estudo
    - Abaixo de <summary>, adicionar
        <remarks>
        { "title": "Estudos AZ-400", "description": "Sobre o Azure Blob Storage", "isPublic": true}
        </remarks>
    - Comentário:
        /// <param name="model">Dados de uma nota de estudo</param>
    - Comentário:
        /// <returns>Objeto recém-criado</returns>
    - Pode-se adicionar um <response> para cada resposta que puder retornar na API:
        /// <response code="201">Sucesso</response>
    
33 - Configurando o Swagger

    - Em Program.cs:
        - Atualizar o método AddSwaggerGen com dados para documentação/comentários e para geração do arquivo
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


_______________________________________________________________________________________________________________

*** LOGS com SERILOG ***
_______________________________________________________________________________________________________________

- Logs representam eventos da aplicação ou infraestrutura, podendo ser 
armazenados em arquivos físicos, bancos de dados, etc;
- É recomendável utilizar logs junto a uma ferramente de logs e monitoramento
mais robusta, já que analisar o log diretamente pode se tornar um problema 
dependendo do fluxo, e proatividade é essencial na resolulação de erros.

- Serilog é uma biblioteca que permite logging para aplicações .NET 
- Tem suporte a armazenamento em diversos formatos, como SQL Server,
SQLite, PostgreSQL, entre outros
- A biblioteca para ASP.NET Core é a Serilog.AspNetCore

34 - Adicionar 2 pacotes Serilog
    - CTRL + SHIFT + P >> Open NuGet Gallery
        // Pacote para registrar os eventos no SQL Server
        1 - Serilog.Sinks.MSSqlServer
        2 - Serilog.AspNetCore

35 - Configurar execução do Swagger para publicação
    Em Program.cs:
    // A configuração atual executa o Swagger, somente em ambinete de desenvolvimento
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Com esta configuração fica garantida a execução do Swagger na publicação também:
    if (true)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

36 - Configurar o Serilog
    Em Program.cs:
// Configurando o Serilog para escrever para o SQL Server e para o Console. 
// Utilizar a string de conexão connectionString
// E criar uma tabela no SQL Server com o nome Logs, caso não exista.
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

37 - Na Action GetAll(), configurar uma mensagem para que seja registrada no log
    Log.Information("GetAll is called");
    // Se lançar uma exceção, ela será escrita no log
    throw new Exception("GetAll threw an error.");

38 - Testando o funcionamento do Log
    - No terminal, executar
        dotnet run
    - No Swagger, chamar GetAll
        - Será lançada uma exceção
    - Abrir o SQL Server e consultar a tabela Logs
        - Serão apresentados os logs 

_______________________________________________________________________________________________________________

*** PUBLICAÇÃO no MICROSOFT AZURE ***
_______________________________________________________________________________________________________________

- Para publicação de aplicações ASP.NET Core, o Microsoft Azure oferece, entre outros
serviços, o Azure App Service
- Esse serviço tem uma camada gratuita para testes!
- Uma dica: fique muito atento para não esquecer recursos de testes ativos na sua
conta, já que pode resulta em uma $urpresa

39 - Acessar https://azure.microsoft.com/pt-br/
    - Criar um recurso +
    - Aplicativo Web >> Criar
    - Nome: APIdoJanderson
    - Publicar: Código
    - Pilha de runtime: .NET 7 (STS)
    - Sistema Operacional: Linux
    - Plano de preços: F1
    - Revisar + criar
    - Criar

40 - Configurar a aplicação para usar BANCO DE DADOS EM MEMÓRIA 
porque não será utilizado o Microsoft SQL Server no Microsoft Azure
    - Em GetAll(), comentar as linhas de log
        // Log.Information("GetAll is called");
        // Se lançar a uma exceção, ela será escrita no log
        //throw new Exception("GetAll threw an error.");
    - Em Progam.cs comentar o código:
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

    - Comentar:
/*
// *** SGBD SQL Server ***
var connectionString = builder.Configuration.GetConnectionString("DevStudyNotes");
builder.Services.AddDbContext<StudyNoteDbContext>(
    // Informa a string de conexão "connectionString"
    o => o.UseSqlServer(connectionString)
);
*/

    - Configurar:
// *** BANCO DE DADOS EM MEMÓRIA ***
builder.Services.AddDbContext<StudyNoteDbContext>(
    // Nomeia o Database de "StudyNoteDb"
    o => o.UseInMemoryDatabase("StudyNoteDb")
);

    - Conferir instalação da extensão:
        - Azure App Service
    
41 - Clicar no ícone do Azure (Shift + Alt + A), na barra vertical, no canto esquerdo
    - Caso tenha algum problema com o Azure:
	    - CTRL + SHIFT + P
	    - Azure: Sign Out
	    - Logar novamente no Azure
    - Clicar com o botão direito no serviço "APIdoJanderson"
        - Clicar em "Deploy to Web App..."
        - Informar o projeto DevStudyNotes.API
    - Acessar o Azure e copiar e acessar a URL para testar a aplicação. 
        - Obs.: Adiconar "/swagger" no final da URl
_______________________________________________________________________________________________________________





