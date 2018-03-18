using System.Data.Linq.Mapping;

namespace ganntproj1.ObjectModels
    {
    [Table(Name = "Articole")]
    public class Articles
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

        private string _Articol;
        [Column(Storage = "_Articol")]
        public string Articol
            {
            get
                {
                return _Articol;
                }
            set
                {
                _Articol = value;
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

        private int _IdSector;
        [Column(Storage = "_IdSector")]
        public int IdSector
            {
            get
                {
                return _IdSector;
                }
            set
                {
                _IdSector = value;
                }
            }

        private double? _Prezzo;
        [Column(Storage = "_Prezzo")]
        public double? Prezzo
        {
            get
            {
                return _Prezzo;
            }
            set
            {
                _Prezzo = value;
            }
        }

        private string _Finete;
        [Column(Storage = "_Finete")]
        public string Finete
        {
            get
            {
                return _Finete;
            }
            set
            {
                _Finete = value;
            }
        }

        private string _Stagione;
        [Column(Storage = "_Stagione")]
        public string Stagione
        {
            get
            {
                return _Stagione;
            }
            set
            {
                _Stagione = value;
            }
        }
    }
}
