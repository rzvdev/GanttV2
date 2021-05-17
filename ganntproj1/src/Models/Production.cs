namespace ganntproj1.Models
{
    using System;
    using System.Data.Linq.Mapping;

    /// <summary>
    /// Defines the <see cref="Production" />
    /// </summary>
    [Table(Name = "produzione")]
    public class Production
    {
        /// <summary>
        /// Defines the _Id
        /// </summary>
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

        /// <summary>
        /// Defines the _Data
        /// </summary>
        private DateTime _Data;

        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        [Column(Storage = "_Data")]
        public DateTime Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
            }
        }

        /// <summary>
        /// Defines the _Commessa
        /// </summary>
        private string _Commessa;

        /// <summary>
        /// Gets or sets the Commessa
        /// </summary>
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

        /// <summary>
        /// Defines the _Capi
        /// </summary>
        private int _Capi;

        /// <summary>
        /// Gets or sets the Capi
        /// </summary>
        [Column(Storage = "_Capi")]
        public int Capi
        {
            get
            {
                return _Capi;
            }
            set
            {
                _Capi = value;
            }
        }

        /// <summary>
        /// Defines the _Line
        /// </summary>
        private string _Line;

        /// <summary>
        /// Gets or sets the Line
        /// </summary>
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

        /// <summary>
        /// Defines the _Members
        /// </summary>
        private int _Members;

        /// <summary>
        /// Gets or sets the Members
        /// </summary>
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

        /// <summary>
        /// Defines the _Department
        /// </summary>
        private string _Department;

        /// <summary>
        /// Gets or sets the Department
        /// </summary>
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

        /// <summary>
        /// Defines the _Abovenormal
        /// </summary>
        private bool? _Abovenormal;

        /// <summary>
        /// Gets or sets the Abovenormal
        /// </summary>
        [Column(Storage = "_Abovenormal")]
        public bool? Abovenormal
        {
            get
            {
                return _Abovenormal;
            }
            set
            {
                _Abovenormal = value;
            }
        }

        /// <summary>
        /// Defines the _Times
        /// </summary>
        private DateTime? _Times;

        /// <summary>
        /// Gets or sets the Times
        /// </summary>
        [Column(Storage = "_Times")]
        public DateTime? Times
        {
            get
            {
                return _Times;
            }
            set
            {
                _Times = value;
            }
        }

        /// <summary>
        /// Defines the _Dailyqty
        /// </summary>
        private int? _Dailyqty;

        /// <summary>
        /// Gets or sets the Dailyqty
        /// </summary>
        [Column(Storage = "_Dailyqty")]
        public int? Dailyqty
        {
            get
            {
                return _Dailyqty;
            }
            set
            {
                _Dailyqty = value;
            }
        }

        /// <summary>
        /// Defines the _Price
        /// </summary>
        private double? _Price;

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        [Column(Storage = "_Price")]
        public double? Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
            }
        }

        /// <summary>
        /// Defines the _Includehours
        /// </summary>
        private bool? _Includehours;

        /// <summary>
        /// Gets or sets the IncludeHours
        /// </summary>
        [Column(Storage = "_Includehours")]
        public bool? IncludeHours
        {
            get
            {
                return _Includehours;
            }
            set
            {
                _Includehours = value;
            }
        }

        /// <summary>
        /// Defines the _Abatim
        /// </summary>
        private int? _Abatim;

        /// <summary>
        /// Gets or sets the Abatim
        /// </summary>
        [Column(Storage = "_Abatim")]
        public int? Abatim
        {
            get
            {
                return _Abatim;
            }
            set
            {
                _Abatim = value;
            }
        }

        /// <summary>
        /// Defines the _qtyH
        /// </summary>
        private double? _qtyH;

        /// <summary>
        /// Gets or sets the QtyH
        /// </summary>
        [Column(Storage = "_qtyH")]
        public double? QtyH
        {
            get
            {
                return _qtyH;
            }
            set
            {
                _qtyH = value;
            }
        }
        /// <summary>
        /// Defines the _Department
        /// </summary>
        private string _Shift;

        /// <summary>
        /// Gets or sets the Department
        /// </summary>
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


        /// <summary>
        /// Defines the _qtyH
        /// </summary>
        private double? _Settinghours;

        /// <summary>
        /// Gets or sets the QtyH
        /// </summary>
        [Column(Storage = "_Settinghours")]
        public double? SettingHours
        {
            get
            {
                return _Settinghours;
            }
            set
            {
                _Settinghours = value;
            }
        }
    }
}
