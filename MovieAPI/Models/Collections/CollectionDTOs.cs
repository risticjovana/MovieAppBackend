namespace MovieAPI.Models.Collections
{
    public class CollectionDTOs
    {
        public class CreatePersonalCollectionRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int UserId { get; set; }
        }

        public class CreateEditorialCollectionRequest
        {
            public string Name { get; set; }
            public string Description { get; set; } 
            public int EditorId { get; set; }
        }

        public class AddCommentRequest
        {
            public int CollectionId { get; set; }
            public int ModeratorId { get; set; }
            public string Text { get; set; } = string.Empty;
        }

        public class CollectionCommentDTO
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public DateTime CreatedAt { get; set; }
            public int ModeratorId { get; set; }
        }
    }
}
