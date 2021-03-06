using System;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("Comments")]
    public class TaskCommentModel
    {
        public TaskCommentModel()
        {
        }

        public TaskCommentModel(Guid id, TaskChangeModel changeInfo, string text)
        {
            if (id == Guid.Empty)
                throw new CreatingException(nameof(id), "Id is invalid");
            
            Id = id;
            ChangeInfo = changeInfo;
            Text = text;
        }
        
        
        public Guid Id { get; set; }
        public TaskChangeModel ChangeInfo { get; set; }
        public string Text { get; set; }
    }
}