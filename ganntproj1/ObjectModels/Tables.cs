namespace ganntproj1.ObjectModels
{
    using System.Data.Linq;

    /// <summary>
    /// Defines the <see cref="Tables" />
    /// </summary>
    public class Tables
    {
        /// <summary>
        /// Gets the Orders
        /// </summary>
        public static Table<Orders> Orders => Config.GetOlyConn().GetTable<Orders>();

        /// <summary>
        /// Gets the Articles
        /// </summary>
        public static Table<Articles> Articles => Config.GetOlyConn().GetTable<Articles>();

        /// <summary>
        /// Gets the ArticleOperations
        /// </summary>
        public static Table<AricleOperations> ArticleOperations => Config.GetOlyConn().GetTable<AricleOperations>();

        /// <summary>
        /// Gets the Productions
        /// </summary>
        public static Table<Production> Productions => Config.GetGanttConn().GetTable<Production>();

        /// <summary>
        /// Gets the Lines
        /// </summary>
        public static Table<Lines> Lines => Config.GetGanttConn().GetTable<Lines>();

        /// <summary>
        /// Gets the ProductionSplits
        /// </summary>
        public static Table<ProductionSplit> ProductionSplits => Config.GetGanttConn().GetTable<ProductionSplit>();

        /// <summary>
        /// Gets the OrderLocks
        /// </summary>
        public static Table<OrderLock> OrderLocks => Config.GetGanttConn().GetTable<OrderLock>();

        /// <summary>
        /// Gets the Shifts
        /// </summary>
        public static Table<Shiftx> Shifts => Config.GetGanttConn().GetTable<Shiftx>();

        /// <summary>
        /// Gets the OrderCloses
        /// </summary>
        public static Table<OrderClose> OrderCloses => Config.GetGanttConn().GetTable<OrderClose>();
    }
}
