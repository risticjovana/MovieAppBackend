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

        public async Task<List<VisualContent>> GetAvailableContentNotInCollectionAsync(int collectionId)
        {
            var allContent = await _dbContext.VisualContents.ToListAsync();
            var usedContentIds = await _dbContext.CollectionItems
                .Where(ci => ci.CollectionId == collectionId)
                .Select(ci => ci.ContentId)
                .ToListAsync();

            var available = allContent
                .Where(vc => !usedContentIds.Contains(vc.ContentId))
                .ToList();

            return available;
        }

        public async Task<List<MovieCollection>> GetAllCollectionsExceptUserAsync(int userId)
        {
            return await _dbContext.MovieCollections
                .Where(c => c.UserId != userId)
                .ToListAsync();
        }

        public async Task<bool> DeleteCollectionAsync(int collectionId)
        {
            var collection = await _dbContext.MovieCollections
                .FirstOrDefaultAsync(c => c.Id == collectionId);

            if (collection == null)
                return false;

            var items = await _dbContext.CollectionItems
                .Where(ci => ci.CollectionId == collectionId)
                .ToListAsync();
            if (items.Any())
                _dbContext.CollectionItems.RemoveRange(items);

            _dbContext.SaveChangesAsync();

            var personal = await _dbContext.PersonalCollections
                .FirstOrDefaultAsync(p => p.CollectionId == collectionId);
            if (personal != null)
                _dbContext.PersonalCollections.Remove(personal);

            _dbContext.SaveChangesAsync();

            var editorial = await _dbContext.EditorialCollections
                .FirstOrDefaultAsync(e => e.CollectionId == collectionId);
            if (editorial != null)
                _dbContext.EditorialCollections.Remove(editorial);

            _dbContext.MovieCollections.Remove(collection);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveContentFromCollectionAsync(int collectionId, int contentId)
        {
            var item = await _dbContext.CollectionItems
                .FirstOrDefaultAsync(ci => ci.CollectionId == collectionId && ci.ContentId == contentId);

            if (item == null)
                return false;

            _dbContext.CollectionItems.Remove(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveCollectionAsync(int userId, int collectionId)
        {
            bool alreadySaved = await _dbContext.SavedCollections
                .AnyAsync(sc => sc.UserId == userId && sc.CollectionId == collectionId);

            if (alreadySaved)
                return false;

            var saved = new SavedCollection
            {
                UserId = userId,
                CollectionId = collectionId
            };

            _dbContext.SavedCollections.Add(saved);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<MovieCollection>> GetSavedCollectionsByUserIdAsync(int userId)
        {
            var savedCollections = await (from sc in _dbContext.SavedCollections
                                          join c in _dbContext.MovieCollections on sc.CollectionId equals c.Id
                                          where sc.UserId == userId
                                          select c)
                                          .ToListAsync();

            return savedCollections;
        }

        public async Task<bool> AddCommentToCollectionAsync(int collectionId, int moderatorId, string text)
        {
            // Ensure the collection exists
            var collectionExists = await _dbContext.MovieCollections.AnyAsync(c => c.Id == collectionId);
            if (!collectionExists)
                return false;


            // Try to get the associated userId from CUVA (SavedCollection)
            var savedEntry = await _dbContext.SavedCollections
                .FirstOrDefaultAsync(sc => sc.CollectionId == collectionId);

            if (savedEntry == null)
                return false;

            var maxId = await _dbContext.Comments.MaxAsync(c => (int?)c.Id) ?? 0;
            var newId = maxId + 1;

            var comment = new Comment
            {
                Id = newId,
                CollectionId = collectionId,
                UserId = savedEntry.UserId, // From CUVA
                ModeratorId = moderatorId,
                Text = text,
                Date = DateTime.UtcNow
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
