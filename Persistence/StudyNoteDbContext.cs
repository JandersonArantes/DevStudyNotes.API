using DevStudyNotes.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevStudyNotes.API.Persistence
{
    public class StudyNoteDbContext : DbContext
    {
        // No construtor, passar o DbContextOptions com tipo StudyNoteDbContext 
        //com o nome "options" para passar "options" para classe base e funcionar 
        // com o uso do Entity Framework Core 
        public StudyNoteDbContext(DbContextOptions<StudyNoteDbContext> options) : base(options)
        {
            
        }

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
        
        
        
    }
}