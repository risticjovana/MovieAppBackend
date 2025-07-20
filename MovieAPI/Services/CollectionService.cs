using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.Collections;
using MovieAPI.Models.TicketReservation;

namespace MovieAPI.Services
{
    public class CollectionService
    {
        private readonly AppDbContext _dbContext;

        public CollectionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MovieCollection> CreatePersonalCollectionAsync(string name, string description, int userId)
        {
            var maxId = await _dbContext.MovieCollections.MaxAsync(c => (int?)c.Id) ?? 0;
            var newId = maxId + 1;

            var collection = new MovieCollection
            {
                Id = newId,
                Name = name,
                Description = description,
                Type = MovieCollection.CollectionType.Licna,
                CreatedAt = DateTime.UtcNow,
                SaveCount = 0,
                UserId = userId
            };

            var personal = new PersonalCollection
            {
                CollectionId = newId,
                UserId = userId
            };

            _dbContext.MovieCollections.Add(collection);
            _dbContext.PersonalCollections.Add(personal);
            await _dbContext.SaveChangesAsync();

            return collection;
        }

        public async Task<MovieCollection> CreateEditorialCollectionAsync(string name, string description, int moderatorId, int editorId)
        {
            var maxId = await _dbContext.MovieCollections.MaxAsync(c => (int?)c.Id) ?? 0;
            var newId = maxId + 1;

            var collection = new MovieCollection
            {
                Id = newId,
                Name = name,
                Description = description,
                Type = MovieCollection.CollectionType.Urednicka,
                CreatedAt = DateTime.UtcNow,
                SaveCount = 0,
                UserId = editorId
            };

            var editorial = new EditorialCollection
            {
                CollectionId = newId,
                ModeratorId = moderatorId,
                ContentEditorId = editorId
            };

            _dbContext.MovieCollections.Add(collection);
            _dbContext.EditorialCollections.Add(editorial);
            await _dbContext.SaveChangesAsync();

            return collection;
        }

        public async Task<bool> AddContentToCollectionAsync(int collectionId, int contentId, int userId)
        { 
            var contentExists = await _dbContext.VisualContents.AnyAsync(vc => vc.ContentId == contentId);
            var collectionExists = await _dbContext.MovieCollections.AnyAsync(c => c.Id == collectionId);
            var userExists = await _dbContext.RegularUsers.AnyAsync(u => u.Id == userId);

            if (!contentExists || !collectionExists || !userExists)
                return false;
             
            var itemExists = await _dbContext.CollectionItems
                .AnyAsync(ci => ci.ContentId == contentId && ci.CollectionId == collectionId);

            if (!itemExists)
            {
                var item = new CollectionItem
                {
                    ContentId = contentId,
                    CollectionId = collectionId
                };

                _dbContext.CollectionItems.Add(item);
            }
             
            var alreadySaved = await _dbContext.SavedCollections
                .AnyAsync(sc => sc.UserId == userId && sc.CollectionId == collectionId);

            var alreadyTracked = _dbContext.ChangeTracker
                .Entries<CollectionItem>()
                .Any(e => e.Entity.ContentId == contentId && e.Entity.CollectionId == collectionId);


            if (!alreadySaved)
            {
                _dbContext.SavedCollections.Add(new SavedCollection
                {
                    UserId = userId,
                    CollectionId = collectionId
                });
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<List<MovieCollection>> GetPersonalCollectionsByUserIdAsync(int userId)
        {
            var collections = await (from c in _dbContext.MovieCollections
                                     join pc in _dbContext.PersonalCollections on c.Id equals pc.CollectionId
                                     where pc.UserId == userId
                                     select c)
                                 .ToListAsync();

            return collections;
        }

        public async Task<List<VisualContent>> GetAllContentByCollectionIdAsync(int collectionId)
        {
            var contents = await (from ci in _dbContext.CollectionItems
                                  join vc in _dbContext.VisualContents on ci.ContentId equals vc.ContentId
                                  where ci.CollectionId == collectionId
                                  select vc)
                                  .ToListAsync();

            return contents;
        }

        public async Task<MovieCollection?> GetCollectionInfoByIdAsync(int collectionId)
        {
            var collection = await _dbContext.MovieCollections
                .FirstOrDefaultAsync(c => c.Id == collectionId);

            return collection;
        }
    }
}
