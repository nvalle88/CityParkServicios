using AppCityParkServices.Clases;
using AppCityParkServices.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

/*
 Creado por Brian Molina 
 En DigitalStrategy.com.ec
 Es una clase que nos permite Saber si estamos dentro de un Poligono 
 o ver la distancia entre dos puntos.
*/

namespace AppCityParkServices.Utils
{
    public class GeoUtils
    {
        public static bool EstaEnMiSector(ObservableCollection<Position>Sector, Parqueo parqueo)
            {
                if (Sector.Count < 3)
                {
                    return false;
                }

                bool inside = false;
                Position p1, p2;

                Position oldPoint = Sector[Sector.Count - 1];

                foreach (Position newPoint in Sector)
                {
                    //Oredenamos los puntos  p1.lat <= p2.lat;
                    if (newPoint.latitude > oldPoint.latitude)
                    {
                        p1 = oldPoint;
                        p2 = newPoint;
                    }
                    else
                    {
                        p1 = newPoint;
                        p2 = oldPoint;
                    }

                   // verifica si el punto esta dentro .
                
                    if ((newPoint.latitude < parqueo.Latitud) == (parqueo.Latitud <= oldPoint.latitude)
                        && (parqueo.Longitud - p1.longitude) * (p2.latitude - p1.latitude)
                         < (p2.longitude - p1.longitude) * (parqueo.Latitud - p1.latitude))
                    {
                        inside = !inside;
                    }

                    oldPoint = newPoint;
                }

                return inside;


            }

        public static  bool  EstaCercaDeMi (Position MyPosition, Plaza Plaza, double radio)
        {
            //Radios en KM        
               if (GetDistance(MyPosition.latitude, MyPosition.longitude, (double)Plaza.Latitud, (double)Plaza.Longitud)<=radio)
                {
                return true;
                }
            return false;
        }

        static double  GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // El radio de la tierra en Kilometros
            var dLat = ToRadians(lat2 - lat1);  // Convertimos de grados a radianes
            var dLon = ToRadians(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distancia en Kilometros
            return d;
        }

        static double ToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }


    }
}