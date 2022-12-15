using SQLite;
using System;

namespace Drinks4Us.Models
{
    [Table("logging")]
    public class Log
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("time")]
        public DateTime DateTime { get; set; }
    }
}