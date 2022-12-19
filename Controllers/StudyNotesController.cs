using DevStudyNotes.API.Entities;
using DevStudyNotes.API.Models;
using DevStudyNotes.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevStudyNotes.API.Controllers
{
    [ApiController]
    [Route("api/study-notes")]
    public class StudyNotesController : ControllerBase
    {
        private readonly StudyNoteDbContext _context;

        // Configuração para acessar o DbContext por Injeção de Dependência
        public StudyNotesController(StudyNoteDbContext context)
        {
            _context = context;
        }
        // api/study-notes HTTP GET
        [HttpGet]
        public IActionResult GetAll()
        {
            // Cria a variável studyNotes que recebe a Entidade/Tabela StudyNotes
            var studyNotes = _context.StudyNotes
                .Include(s => s.Reactions)
                .ToList();

            // Retorna/Consulta todas as "notas de estudo"
            return Ok(studyNotes);
        }

        // api/study-notes/1 HTTP GET
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // studyNotes recebe a nota de estudo com Id = id
            var studyNotes = _context.StudyNotes
                // Precisa incluir as Reactions porque por padrão não estão incluídas.
                .Include(s => s.Reactions)
                .SingleOrDefault(s => s.Id == id);

            // Se não for encontrada nenhum nota de estudo, retorna NotFound
            if (studyNotes == null) return NotFound();

            // Retorna a nota de estudo com o id informado
            return Ok(studyNotes);
        }

        // api/study-notes HTTP POST
        [HttpPost]
        public IActionResult Post(AddStudyNoteInputModel model)
        {
            // Instanciar a classe StudyNote para receber o model(InputModel)
            var studyNote = new StudyNote(model.Title, model.Description, model.IsPublic);

            // Adiciona studyNote em StudyNotes
            _context.StudyNotes.Add(studyNote);
            // Sempre que ocorrer alteração de dados, deve-se efetivar a operação com SaveChanges()            
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = studyNote.Id }, model);
        }

        // api/study-notes/1/reactions HTTP POST
        [HttpPost("{id}/reactions")]
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
    }
}