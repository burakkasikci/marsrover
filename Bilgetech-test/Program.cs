using System;
using System.Collections.Generic;
using System.Linq; 

namespace Bilgetech_test
{
    class Program
    {
        #region Variables
        static int maxArea, xArea, yArea, tempValue = 0, vehicleCount = 2,iCount=0;
        static string input, allowLetters;
        static List<RoverEntity> modelList = new List<RoverEntity>();
        static bool resume = true;
        #endregion

        static void Main(string[] args)
        {
            getAreaInformation();
            for(iCount = 0; iCount < vehicleCount; iCount++)
            {
                getVehicleCoordinatInformation();
                getDirections();

                foreach (var item in modelList[iCount].readDirections)
                {
                    modelList[iCount].newDirections = item;
                    modelList[iCount] = move(modelList[iCount]);
                }
            }


            foreach (var item in modelList)
            {
                Console.WriteLine(item.coordinatX.ToString() + item.coordinatY.ToString() + item.activeDirection + item.message);
            }
            Console.ReadKey();
        }

        #region Information
        /// <summary>
        /// Oluşturulacak alan boyut bilgilerini alan method
        /// </summary>
        private static void getAreaInformation()
        {
            do
            {
                Console.WriteLine("Oluşturulacak alan boyutunu XY şeklinde giriniz : ");
                input = Console.ReadLine();
            } while ((int.TryParse(input, out maxArea) == false) || maxArea < 10 || maxArea > 100);

            xArea = Convert.ToInt32(input.Substring(0, 1));
            yArea = Convert.ToInt32(input.Substring(1, 1));
        }

        /// <summary>
        /// Aracın ilk koordinatını ve yön bilgisini alan method
        /// İlk 2 karakter koordinat için olduğu için, maksimum oluşturulan alan boyutunda ve sayı olmalıdır.
        /// Yön bilgisi olarak N,E,S,W harfleri haricinde bir girişe izin verilmemektedir.
        /// </summary>
        private static void getVehicleCoordinatInformation()
        {
            char[] readValue;
            do
            {
                resume = true;
                allowLetters = "NESW";
                Console.WriteLine("Aracın koordinatını ve yönünü giriniz (Sadece N,E,S,W harflerini kullanabilirsiniz.) : ");
                readValue = Console.ReadLine().ToUpper().ToCharArray();
                if (readValue.Count() != 3)
                    resume = false;

                if (resume && (int.TryParse(readValue[0].ToString(), out tempValue) == false) || tempValue > xArea)
                    resume = false;

                if (resume && (int.TryParse(readValue[1].ToString(), out tempValue) == false) || tempValue > yArea)
                    resume = false;

                if (resume)
                {
                    foreach (char letter in allowLetters)
                    {
                        if (readValue[2] == letter)
                        {
                            break;
                        }

                        if (allowLetters.IndexOf(letter) == allowLetters.Count() - 1)
                        {
                            resume = false;
                        }
                    }
                }
            } while (resume == false);

            modelList.Add(new RoverEntity()
            {
                coordinatX = Convert.ToInt32(readValue[0].ToString()),
                coordinatY = Convert.ToInt32(readValue[1].ToString()),
                activeDirection = readValue[2]
            });
        }

        /// <summary>
        /// Aracın ilerleyeceği yön bilgilerini alan method
        /// Yön bilgileri olarak R,L,M harfleri haricinde bir girişe izin verilmemektedir.
        /// </summary>
        private static void getDirections()
        {
            do
            {
                resume = true;
                allowLetters = "RLM";
                Console.WriteLine("Aracın ilerleyeceği yönleri giriniz (Sadece R,L,M harflerini kullanabilirsiniz.) : ");
                modelList[iCount].readDirections = Console.ReadLine().ToUpper().ToCharArray();
                foreach (char c in modelList[iCount].readDirections)
                {
                    foreach (char letter in allowLetters)
                    {
                        if (c == letter)
                        {
                            break;
                        }

                        if (allowLetters.IndexOf(letter) == allowLetters.Count() - 1)
                        {
                            resume = false;
                        }
                    }
                }
            } while (resume == false);

        }
        #endregion

        #region Process
        /// <summary>
        /// Gelen değeri gerekli yöne çeviren veya ilerleten method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static RoverEntity move(RoverEntity entity)
        {
            if (entity.newDirections != 'M')
            {
                entity.activeDirection = findNewDirections(entity.activeDirection, entity.newDirections);
            }
            else
            {
                switch (entity.activeDirection)
                {
                    case 'N':
                        entity.coordinatY++;
                        break;
                    case 'E':
                        entity.coordinatX++;
                        break;
                    case 'S':
                        entity.coordinatY--;
                        break;
                    case 'W':
                        entity.coordinatX--;
                        break;
                }
                if (entity.coordinatX < 0 || entity.coordinatX > xArea || entity.coordinatY < 0 || entity.coordinatY > yArea)
                {
                    entity.message = " - Araç belirtilen sınırların dışına çıktı.";
                }

            }
            return entity;
        }

        /// <summary>
        /// Aktif olarak gelen yönü, dönülecek olan yöne çeviren method
        /// </summary>
        /// <param name="activeDirections"></param>
        /// <param name="newDirections"></param>
        /// <returns></returns>
        private static char findNewDirections(char activeDirections, char newDirections)
        {
            char newValue = default(char);
            switch (activeDirections)
            {
                case 'N':
                    if (newDirections == 'L')
                        newValue = 'W';
                    else
                        newValue = 'E';
                    break;
                case 'E':
                    if (newDirections == 'L')
                        newValue = 'N';
                    else
                        newValue = 'S';
                    break;
                case 'S':
                    if (newDirections == 'L')
                        newValue = 'E';
                    else
                        newValue = 'W';
                    break;
                case 'W':
                    if (newDirections == 'L')
                        newValue = 'S';
                    else
                        newValue = 'N';
                    break;
            }
            return newValue;
        }
        #endregion
    }
}
