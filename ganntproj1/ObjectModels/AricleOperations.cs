using System;
using System.Data.Linq.Mapping;

namespace ganntproj1.ObjectModels
    {

    [Table(Name = "OperatiiArticol")]
    public class AricleOperations
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

        private int _IdArticol;
        [Column(Storage = "_IdArticol")]
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

        private double? _BucatiOra;
        [Column(Storage = "_BucatiOra")]
        public double? BucatiOra
            {
            get
                {
                return _BucatiOra;
                }
            set
                {
                _BucatiOra = value;
                }
            }

        private double? _Centes;
        [Column(Storage = "_Centes")]
        public double? Centes
            {
            get
                {
                return _Centes;
                }
            set
                {
                _Centes = value;
                }
            }

        }
    }
