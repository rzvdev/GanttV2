using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ganntproj1.Models
{
    public class Log
    {
        private Guid _Id;
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Column(Storage = "_Id", DbType = "uniqueidentifier NOT NULL IDENTITY", CanBeNull = false, IsPrimaryKey = true, IsDbGenerated = true)]
        public Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }

        [Column(Storage = "_Username")]
        public string Username { get; set; }

        [Column(Storage = "_Computername")]
        public string Computername { get; set; }

        [Column(Storage = "_Logdate")]
        public long Logdate { get; set; }

        [Column(Storage = "_Operation")]
        public string Operation { get; set; }
        [Column(Storage = "_Queryon")]
        public string Queryon { get; set; }
        [Column(Storage = "_Program")]
        public string Program { get; set; }
    }
}
