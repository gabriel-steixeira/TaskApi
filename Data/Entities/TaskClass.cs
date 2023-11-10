using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskApi.Data.Entities
{
    [Table("Tasks")]
    public class TaskClass
    {
        [Key]
        [Column("ID_TAREFA")]
        public int ID_TAREFA { get; set; }

        [Column("TITULO")]
        public string TITULO { get; set; }

        [Column("DESCRICAO")]
        public string DESCRICAO { get; set; }

        [Column("DATA")]
        public DateTime DATA { get; set; }

        [Column("STATUS")]
        public TaskStatusEnum STATUS { get; set; }

        [NotMapped]
        public string? STATUS_TAREFA { get; set; }
    }
}
