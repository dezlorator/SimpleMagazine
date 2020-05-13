using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Сomment { get; }
        void SaveComment(Comment comment);
        Comment DeleteComment(int CommentID);
    }
}
