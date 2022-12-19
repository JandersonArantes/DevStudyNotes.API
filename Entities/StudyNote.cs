namespace DevStudyNotes.API.Entities
{
    public class StudyNote
    {
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

        public int Id { get; private set; }
        public string Title { get; private set; }   
        public string Description { get; private set; }
        public bool IsPublic { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<StudyNoteReaction> Reactions { get; private set; }  

        // Libera reação somente se a nota for pública
        public void AddReaction(bool isPositive)
        {
            if(!IsPublic) throw new InvalidOperationException();

            Reactions.Add(new StudyNoteReaction(isPositive));
        }

    }
}