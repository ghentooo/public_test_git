using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.continental.TDM.HelperLibrary
{
    /// <summary>
    /// hold all servers and shares
    /// </summary>
    public class ServerShare
    {
        public enum ConstructurOrder
        {
            Location = 0,
            ShortLoc,
            ClusterHeadName,
            Direction
        }

        /// <summary>
        /// constructor of a servershare
        /// </summary>
        /// <param name="location">location appreviation</param>
        /// <param name="shortloc">location appreviation</param>
        /// <param name="repr">HPC copy cluster head name</param>
        /// <param name="legacy">legacy share for backward compatibility</param>
        public ServerShare(string location, string shortloc, string repr, Constants.Direction dir)
        {
            _location = location;
            _shortloc = shortloc;
            _repr = repr;
            _direction = dir;
        }

        public string Location { get { return _location; } }
        public string ShortLoc { get { return _shortloc; } }
        public string Repr { get { return _repr; } }
        public Constants.Direction Direction { get { return _direction; } }

        private string _location = null;
        private string _shortloc = null;
        private string _repr = null;
        private Constants.Direction _direction = Constants.Direction.disk;
    }
}
