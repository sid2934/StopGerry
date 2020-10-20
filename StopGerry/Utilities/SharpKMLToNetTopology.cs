using System;
using System.Collections.Generic;
using System.IO;
using NetTopologySuite.Geometries;

namespace StopGerry.Utilities
{
    public class SharpKMLToNetTopology
    {
        public static NetTopologySuite.Geometries.Geometry GeometryToGeometry(SharpKml.Dom.Geometry geo)
        {
            if (geo is SharpKml.Dom.MultipleGeometry)
            {
                var polyList = new List<NetTopologySuite.Geometries.Polygon>();
                foreach (var subGeo in (geo as SharpKml.Dom.MultipleGeometry).Geometry)
                {
                    polyList.Add(PolygonToPolygon(subGeo as SharpKml.Dom.Polygon));
                }
                return new NetTopologySuite.Geometries.MultiPolygon(polyList.ToArray());
            }
            else if (geo is SharpKml.Dom.Polygon)
            {
                return PolygonToPolygon(geo as SharpKml.Dom.Polygon);
            }
            else
            {
                throw new InvalidDataException();
            }
        }

        internal static NetTopologySuite.Geometries.Polygon PolygonToPolygon(SharpKml.Dom.Polygon poly)
        {
            var holes = new List<NetTopologySuite.Geometries.LinearRing>();
            if (poly.InnerBoundary.Count != 0)
            {
                //If there are InnerBoundary we must create the holes for the NetTopologySuite.Geometries.Polygon
                //lets hope we dont need this, but we might.
                foreach(var innerBoundary in poly.InnerBoundary)
                {
                    var innerShell = LinearRingToLinearRing(innerBoundary.LinearRing);
                    holes.Add(innerShell);
                }
            }

            var shell = LinearRingToLinearRing(poly.OuterBoundary.LinearRing);


            return new NetTopologySuite.Geometries.Polygon(shell, holes.ToArray());
        }


        internal static NetTopologySuite.Geometries.LinearRing LinearRingToLinearRing(SharpKml.Dom.LinearRing linearRing)
        {
            var netTopologyCoordinateList = new List<NetTopologySuite.Geometries.Coordinate>();
            foreach (var coord in linearRing.Coordinates)
            {
                netTopologyCoordinateList.Add(new Coordinate(coord.Longitude, coord.Latitude));
            }
            return new NetTopologySuite.Geometries.LinearRing(netTopologyCoordinateList.ToArray());
        }
    }
}