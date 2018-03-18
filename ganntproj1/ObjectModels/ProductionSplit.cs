using System;
using System.Data.Linq.Mapping;

namespace ganntproj1.ObjectModels
    {
    [Table(Name = "produzionesplit")]
    public class ProductionSplit
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

        private int _Qty;
        [Column(Storage = "_Qty")]
        public int Qty
            {
            get
                {
                return _Qty;
                }
            set
                {
                _Qty = value;
                }
            }

        private DateTime? _Startdate;
        [Column(Storage = "_Startdate")]
        public DateTime? Startdate
            {
            get
                {
                return _Startdate;
                }
            set
                {
                _Startdate = value;
                }
            }

        private DateTime? _Enddate;
        [Column(Storage = "_Enddate")]
        public DateTime? Enddate
            {
            get
                {
                return _Enddate;
                }
            set
                {
                _Enddate = value;
                }
            }

        private bool? _Base;
        [Column(Storage = "_Base")]
        public bool? Base
            {
            get
                {
                return _Base;
                }
            set
                {
                _Base = value;
                }
            }
        }
    }
