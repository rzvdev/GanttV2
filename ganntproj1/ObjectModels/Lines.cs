using System;
using System.Data.Linq.Mapping;

namespace ganntproj1.ObjectModels
    {
    [Table(Name = "lines")]
    public class Lines
        {
        private string _Line;
        [Column(IsPrimaryKey = true, Storage = "_Line")]
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
        }
    }
