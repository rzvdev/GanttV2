using System;
using System.Data.Linq.Mapping;
namespace ganntproj1.Models
    {
    [Table(Name = "Comenzi")]
    public class Orders
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

        private string _NrComanda;
        [Column(Storage = "_NrComanda")]
        public string NrComanda
            {
            get
                {
                return _NrComanda;
                }
            set
                {
                _NrComanda = value;
                }
            }

        private int _IdArticol;
        [Column(Storage = "_IdArticol")]    //FK
        public int IdArticol
            {
            get
                {
                return _IdArticol;
                }
            set
                {
                _IdArticol = value;
                }
            }

        private DateTime? _DataLivrare;
        [Column(Storage = "_DataLivrare")]
        public DateTime? DataLivrare
            {
            get
                {
                return _DataLivrare;
                }
            set
                {
                _DataLivrare = value;
                }
            }

        private DateTime? _Dvc;
        [Column(Storage = "_Dvc")]
        public DateTime? Dvc
            {
            get
                {
                return _Dvc;
                }
            set
                {
                _Dvc = value;
                }
            }

        private DateTime? _Rdd;
        [Column(Storage = "_Rdd")]
        public DateTime? Rdd
            {
            get
                {
                return _Rdd;
                }
            set
                {
                _Rdd = value;
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

        private int? _Carico;
        [Column(Storage = "_Carico")]
        public int? Carico
            {
            get
                {
                return _Carico;
                }
            set
                {
                _Carico = value;
                }
            }

        private int _Cantitate;
        [Column(Storage = "_Cantitate")]
        public int Cantitate
            {
            get
                {
                return _Cantitate;
                }
            set
                {
                _Cantitate = value;
                }
            }

        private DateTime? _DataProduzione;
        [Column(Storage = "_DataProduzione")]
        public DateTime? DataProduzione
            {
            get
                {
                return _DataProduzione;
                }
            set
                {
                _DataProduzione = value;
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

        private int _IdStare;
        [Column(Storage = "_IdStare")]
        public int IdStare
            {
            get
                {
                return _IdStare;
                }
            set
                {
                _IdStare = value;
                }
            }

        private DateTime? _DataFine;
        [Column(Storage = "_DataFine")]
        public DateTime? DataFine
            {
            get
                {
                return _DataFine;
                }
            set
                {
                _DataFine = value;
                }
            }
        private bool? _CaricoTrigger;
        [Column(Storage = "_CaricoTrigger")]
        public bool? CaricoTrigger
        {
            get
            {
                return _CaricoTrigger;
            }
            set
            {
                _CaricoTrigger = value;
            }
        }
        private bool? _QtyInstead;
        [Column(Storage = "_QtyInstead")]
        public bool? QtyInstead
        {
            get
            {
                return _QtyInstead;
            }
            set
            {
                _QtyInstead = value;
            }
        }
        private int? _Duration;
        [Column(Storage = "_Duration")]
        public int? Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
            }
        }
    }
}
