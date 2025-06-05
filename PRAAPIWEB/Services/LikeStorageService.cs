using System.Collections.Concurrent;

namespace PRAAPIWEB.Services
{
    public class LikeStorageService
    {
        // Хранит пары PostId -> UserId (кто лайкнул)
        private readonly ConcurrentDictionary<int, HashSet<int>> _postLikes = new();
        // Хранит PostId -> количество лайков
        private readonly ConcurrentDictionary<int, int> _likesCount = new();

        public bool ToggleLike(int postId, int userId)
        {
            // Получаем или создаем набор пользователей для поста
            var userLikes = _postLikes.GetOrAdd(postId, new HashSet<int>());

            lock (userLikes)
            {
                if (userLikes.Contains(userId))
                {
                    // Убираем лайк
                    userLikes.Remove(userId);
                    _likesCount.AddOrUpdate(postId, 0, (_, count) => count - 1);
                    return false;
                }
                else
                {
                    // Добавляем лайк
                    userLikes.Add(userId);
                    _likesCount.AddOrUpdate(postId, 1, (_, count) => count + 1);
                    return true;
                }
            }
        }

        public int GetLikesCount(int postId)
        {
            return _likesCount.TryGetValue(postId, out var count) ? count : 0;
        }

        public bool HasUserLiked(int postId, int userId)
        {
            return _postLikes.TryGetValue(postId, out var userLikes) &&
                   userLikes.Contains(userId);
        }
    }
}
