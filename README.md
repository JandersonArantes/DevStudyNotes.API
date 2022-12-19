<!-- 
# DevStudyNotes.API
 Esta API manipula as notas de estudo de alunos.

Passo a passo para desenvolver a API "DevStudyNotes.API"

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

//*******************************************************************************************
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
//*******************************************************************************************

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

//=====================================================================================================
// Gerar Migrations para trabalhar com o SGBD SQL Server
//=====================================================================================================










 -->