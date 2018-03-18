using System;
using System.Data.Linq.Mapping;

namespace ganntproj1.ObjectModels
{
    [Table(Name = "orderclose")]
    public class OrderClose
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
    }
}
