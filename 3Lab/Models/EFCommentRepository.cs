using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class EFCommentRepository : ICommentRepository
    {
        #region private
        private ApplicationDbContext _context;
        #endregion

        public EFCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> Сomment => _context.Comments;
        public Comment DeleteComment(int CommentID)
        {
            Comment dbEntry = _context.Comments
            .FirstOrDefault(p => p.ID == CommentID);
            if (dbEntry != null)
            {
                _context.Comments.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }

        public void SaveComment(Comment comment)
        {
            if (comment.ID == 0)
            {
                _context.Comments.Add(comment);
            }
            else
            {
                Comment dbEntry = _context.Comments
                    .FirstOrDefault(p => p.ID == comment.ID);
                if (dbEntry != null)
                {
                    dbEntry.Message = comment.Message;
                    dbEntry.Rating = comment.Rating;
                    dbEntry.Time = DateTime.Now;
                }
            }
            _context.SaveChanges();
        }
    }
}
