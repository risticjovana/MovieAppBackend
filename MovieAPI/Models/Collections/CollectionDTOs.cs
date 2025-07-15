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
            public int ModeratorId { get; set; }
            public int EditorId { get; set; }
        }

    }
}
