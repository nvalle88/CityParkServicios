using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppCityParkServices.Clases;
using System.Collections.ObjectModel;

namespace AppCityParkServices.Constants
{
    public class Constants

    {
        public static  ObservableCollection<Position> polygonEMOV= new ObservableCollection<Position>
        {
            new Position{latitude=-2.908594,longitude= -79.039565},
            new Position{latitude=-2.909783,longitude= -79.039554},
            new Position{latitude=-2.910083,longitude= -79.037666},
            new Position{latitude=-2.908497,longitude= -79.037194},
            new Position{latitude=-2.908122,longitude= -79.037977},

        };

        public static ObservableCollection<Position> polygonQuito = new ObservableCollection<Position>
        {
           
            new Position{latitude=-0.175783,longitude= -78.485361},
            new Position{latitude=-0.175783,longitude= -78.481055},
            new Position{latitude=-0.170210,longitude= -78.479919},
            new Position{latitude=-0.169270,longitude= -78.484454},

        };



    }
}