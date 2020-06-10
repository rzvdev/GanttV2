using System;
using System.Data.Linq.Mapping;

namespace ganntproj1.Models
    {
    [Table(Name = "lines")]
    public class Lines
        {
        private int _Id;

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Column(Storage = "_Id", DbType = "Int NOT NULL IDENTITY", CanBeNull = false, IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
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

        private string _Line;
        [Column(Storage = "_Line")]
        public string Line
            {
            get
                {
                return _Line;
                }
            set
                {
                _Line = value;
                }
            }

        private int _Members;
        [Column(Storage = "_Members")]
        public int Members
            {
            get
                {
                return _Members;
                }
            set
                {
                _Members = value;
                }
            }

        private int _Abatimento;
        [Column(Storage = "_Abatimento")]
        public int Abatimento
            {
            get
                {
                return _Abatimento;
                }
            set
                {
                _Abatimento = value;
                }
            }

        private string _Department;
        [Column(Storage = "_Department")]
        public string Department
            {
            get
                {
                return _Department;
                }
            set
                {
                _Department = value;
                }
            }

        private string _Description;
        [Column(Storage = "_Description")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
    }    
}    
