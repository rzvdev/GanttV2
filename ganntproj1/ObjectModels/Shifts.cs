using System;
using System.Data.Linq.Mapping;

namespace ganntproj1.ObjectModels
    {
    [Table(Name = "shifts")]
    public class Shiftx
        {
        private int _Id;
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

        private string _Shift;
        [Column(Storage = "_Shift")]
        public string Shift
            {
            get
                {
                return _Shift;
                }
            set
                {
                _Shift = value;
                }
            }

        private DateTime _Starttime;
        [Column(Storage = "_Starttime")]
        public DateTime Starttime
            {
            get
                {
                return _Starttime;
                }
            set
                {
                _Starttime = value;
                }
            }

        private DateTime _Endtime;
        [Column(Storage = "_Endtime")]
        public DateTime Endtime
            {
            get
                {
                return _Endtime;
                }
            set
                {
                _Endtime = value;
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

        private string _Specialnote;
        [Column(Storage = "_Specialnote")]
        public string Specialnote
            {
            get
                {
                return _Specialnote;
                }
            set
                {
                _Specialnote = value;
                }
            }
        }
    }
