using System;
using System.Data.Linq.Mapping;

namespace ganntproj1.ObjectModels
    {
    [Table(Name = "orderlock")]
    public class OrderLock
        {
        private int _Id;
        [Column(IsPrimaryKey = true, Storage = "_Id")]
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

        private string _Commessa;
        [Column(Storage = "_Commessa")]
        public string Commessa
            {
            get
                {
                return _Commessa;
                }
            set
                {
                _Commessa = value;
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

        private bool _Islock;
        [Column(Storage = "_Islock")]
        public bool Islock
            {
            get
                {
                return _Islock;
                }
            set
                {
                _Islock = value;
                }
            }
        private bool _Islockprod;
        [Column(Storage = "_Islockprod")]
        public bool Islockprod
        {
            get
            {
                return _Islockprod;
            }
            set
            {
                _Islockprod = value;
            }
        }
        private DateTime _Lockdate;
        [Column(Storage = "_Lockdate")]
        public DateTime Lockdate
        {
            get
            {
                return _Lockdate;
            }
            set
            {
                _Lockdate = value;
            }
        }
    }
}
