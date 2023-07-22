using System;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform.Common
{
    internal class MapMove
    {
        internal enum MapType
        {
            Main,
            Black_Forest
        }
        internal enum PlaceInBlackForest
        {
            VineForest
        }
        internal enum PlaceInMain
        {
            BlackForest
        }
        internal struct MapPlace
        {
            internal MapType mapType;
            internal int place;

            public MapPlace(MapType type, int place)
            {
                mapType = type;
                this.place = place;
            }
        }

        internal static void MapMoveTo(MapPlace mapPlace, WindowSource window)
        {
            OpenMap(mapPlace.mapType, window);
            switch(mapPlace.mapType)
            {
                case MapType.Main:
                    GoPlaceForMain(mapPlace.place, window); break;
                case MapType.Black_Forest:
                    GoPlaceForBlackForest(mapPlace.place, window); break;
            }
        }

        private static void OpenMap(MapType mapType, WindowSource window)
        {
            switch (mapType)
            {
                case MapType.Main:
                    MouseInput.MouseClickForMole(window, 39, 515);
                    if (!Wait.WaitForMainMapOpen(window, 10000))
                    {
                        CloseLoadingMap(window);
                        Wait.WaitForMainWindow(window);
                        Click.GoRestaurant(window);
                        Wait.WaitForPlaceChange(window, 5000);
                        MouseInput.MouseClickForMole(window, 39, 515);
                        if (!Wait.WaitForMainMapOpen(window, 5000))
                        {
                            UserInterface.Message.ShowMessageToUser("似乎無法開啟地圖", "錯誤");
                            throw new Exception("cannot open main map");
                        }
                    }
                    break;
                case MapType.Black_Forest:
                    OpenMap(MapType.Main, window);
                    GoPlaceForMain((int)PlaceInMain.BlackForest, window);
                    MouseInput.MouseClickForMole(window, 39, 515);
                    Wait.WaitForBlackForestMapOpen(window);
                    break;
            }
        }

        private static void GoPlaceForMain(int place_i, WindowSource window)
        {
            PlaceInMain place = (PlaceInMain)place_i;
            int x = 0, y = 0;
            switch(place)
            {
                case PlaceInMain.BlackForest:
                    (x, y) = (643, 137); break;
            }
            if((x | y) != 0)
            {
                MouseInput.MouseClickForMole(window, x, y);
                Wait.WaitForPlaceChange(window);
            }
        }
        private static void GoPlaceForBlackForest(int place_i, WindowSource window)
        {
            PlaceInBlackForest place = (PlaceInBlackForest)place_i;
            int x = 0, y = 0;
            switch (place)
            {
                case PlaceInBlackForest.VineForest:
                    (x, y) = (383, 441); break;
            }
            if ((x | y) != 0)
            {
                MouseInput.MouseClickForMole(window, x, y);
                Wait.WaitForPlaceChange(window);
            }
        }
        private static void CloseLoadingMap(WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 762, 255);
        }
    }
}
