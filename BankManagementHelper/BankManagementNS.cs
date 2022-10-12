using System.Globalization;

namespace BankManagementHelper
{
    public static class TwoDecimalPlacesExtended
    {
        public static string TwoDecimalPlaces(this decimal value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }

    public class BankManagementNS
    {
        public class Order
        {
            public decimal amount;
            public decimal profit;

            public decimal GetSoros { 
                get
                {
                    return amount + profit;
                }
            }

            public Order(decimal amount, decimal profit)
            {
                this.amount = amount;
                this.profit = profit;
            }
        }

        public enum Strategy
        {
            Fixa,
            Martingale,
            Soros,
            Sorosgale
        }

        private class Martingale
        {
            private int level = 0;
            private int current = 0;

            public int Current
            {
                get
                {
                    return current;
                }
            }
            public int GetLevel
            {
                get
                {
                    return level;
                }
            }

            public void Win()
            {
                current++;
            }
            public void Loss()
            {
                current--;
            }
            public void SetLevel(int level)
            {
                this.level = level;
            }
            public void Reset()
            {
                current = 0;
            }
        }
        private class Soros
        {
            private int level = 0;
            private int current = 0;

            public int Current
            {
                get
                {
                    return current;
                }
            }
            public int GetLevel
            {
                get
                {
                    return level;
                }
            }

            public void Win()
            {
                current++;
            }
            public void Loss()
            {
                current--;
            }
            public void SetLevel(int level)
            {
                this.level = level;
            }
            public void Reset()
            {
                current = 0;
            }
        }
        private class Sorosgale
        {
            private int sorosLevel = 0;
            private int sorosgaleLevel = 0;

            private int currentSorosLevel = 0;
            private int currentSorosgaleLevel = 0;

            public int CurrentSorosLevel
            {
                get
                {
                    return currentSorosLevel;
                }
            }    
            public int CurrentSorosgaleLevel
            {
                get
                {
                    return currentSorosgaleLevel;
                }
            }
            public int GetSorosLevel
            {
                get
                {
                    return sorosLevel;
                }
            }
            public int GetSorosgaleLevel
            {
                get
                {
                    return sorosgaleLevel;
                }
            }

            public void Win()
            {
                currentSorosLevel++;
            }
            public void Loss()
            {
                currentSorosgaleLevel++;
                currentSorosLevel = 1;
            }
            public void SetLevel(int sorosLevel, int sorosgaleLevel)
            {
                this.sorosLevel = sorosLevel;
                this.sorosgaleLevel = sorosgaleLevel;
            }
            public void Reset()
            {
                currentSorosLevel = 0;
                currentSorosgaleLevel = 0;
            }
        }

        private readonly Martingale martingale = new();
        private readonly Soros soros = new();
        private readonly Sorosgale sorosgale = new();
        public Strategy type = Strategy.Fixa;

        private decimal amountInitial = 2;
        private readonly List<Order> orders = new();

        public decimal Wins
        {
            get
            {
                return orders.Sum(e => {
                    if (e.profit > 0)
                        return e.profit;
                    else
                        return 0;
                });
            }
        }
        public decimal Losses
        {
            get
            {
                return orders.Sum(e => {
                    if (e.profit < 0)
                        return e.profit * -1;
                    else
                        return 0;
                });
            }
        }
        public decimal Profit
        {
            get
            {
                return Wins - Losses;
            }
        }

        public void Win(decimal profit)
        {
            orders.Add(new Order(GetAmount, profit));

            switch (type)
            {
                case Strategy.Fixa:
                    break;
                case Strategy.Martingale:
                    martingale.Reset();
                    break;
                case Strategy.Soros:
                    if (soros.GetLevel > 0 && soros.Current < soros.GetLevel)
                    {
                        soros.Win();
                    }
                    else
                    {
                        soros.Reset();
                    }
                    break;
                case Strategy.Sorosgale:
                    if (sorosgale.CurrentSorosLevel < sorosgale.GetSorosLevel)
                    {
                        sorosgale.Win();
                    }
                    else
                    {
                        orders.Clear();
                        sorosgale.Reset();
                    }
                    break;
            }
        }
        public void Loss()
        {
            orders.Add(new Order(GetAmount, GetAmount * -1));

            switch (type)
            {
                case Strategy.Fixa:
                    break;
                case Strategy.Martingale:
                    if (martingale.GetLevel > 0 && martingale.Current < martingale.GetLevel)
                    {
                        martingale.Win();
                    }
                    else
                    {
                        martingale.Reset();
                    }
                    break;
                case Strategy.Soros:
                    soros.Reset();
                    break;
                case Strategy.Sorosgale:
                    if (sorosgale.CurrentSorosgaleLevel < sorosgale.GetSorosgaleLevel)
                    {
                        sorosgale.Loss();
                    }
                    else
                    {
                        orders.Clear();
                        sorosgale.Reset();
                    }
                    break;
            }
        }

        public decimal GetAmount
        {
            get
            {
                switch (type)
                {
                    case Strategy.Fixa:
                        return amountInitial;
                    case Strategy.Martingale:
                        return martingale.Current == 0 ? amountInitial : (orders.Last().profit * -1) * 2;
                    case Strategy.Soros:
                        return soros.Current == 0 ? amountInitial : orders.Last().profit * 2;
                    case Strategy.Sorosgale:
                        if (sorosgale.CurrentSorosgaleLevel == 0 && sorosgale.CurrentSorosLevel == 0)
                        {
                            return amountInitial;
                        }
                        else if (sorosgale.CurrentSorosgaleLevel == 0 && sorosgale.CurrentSorosLevel > 0)
                        {
                            return orders.Last().GetSoros;
                        }
                        else if (sorosgale.CurrentSorosgaleLevel > 0 && sorosgale.CurrentSorosLevel == 1) {
                            return (Profit / 2) * -1;
                        }
                        else if (sorosgale.CurrentSorosgaleLevel > 0 && sorosgale.CurrentSorosLevel > 1)
                        {
                            return orders.Last().GetSoros;
                        }
                        else
                        {
                            throw new Exception("BankManagement.cs GetAmount() - 'if' not expected");
                        }
                    default:
                        throw new Exception("BankManagement.cs GetAmount() - unknown Type enum " + type);

                }
            }
        }
        public decimal GetAmountInitial
        {
            get
            {
                return amountInitial;
            }
        }
        public int GetMartingaleLevel
        {
            get
            {
                return martingale.GetLevel;
            }
        }
        public int GetCurrentMartingale
        {
            get
            {
                return martingale.Current;
            }
        }
        public int GetSorosLevel
        {
            get
            {
                return soros.GetLevel;
            }
        }
        public int GetCurrentSoros
        {
            get
            {
                return soros.Current;
            }
        }
        public int[] GetSorosgaleLevel
        {
            get
            {
                return new int[] { sorosgale.GetSorosLevel, sorosgale.GetSorosgaleLevel };
            }
        }
        public int[] GetCurrentSorosgale
        {
            get
            {
                return new int[] { sorosgale.CurrentSorosLevel, sorosgale.CurrentSorosgaleLevel };
            }
        }

        public void SetAmount(decimal amount)
        {
            amountInitial = amount;
        }
        public void SetMartingale(int level)
        {
            martingale.SetLevel(level);
        }
        public void SetSoros(int level)
        {
            soros.SetLevel(level);
        }
        public void SetSorosgale(int sorosLevel, int sorosgaleLevel)
        {
            sorosgale.SetLevel(sorosLevel, sorosgaleLevel);
        }

        public void Reset()
        {
            martingale.Reset();
            soros.Reset();
            sorosgale.Reset();

            orders.Clear();
        }
        
        override
        public string ToString()
        {
            switch (type)
            {
                case Strategy.Fixa:
                    return string.Format("Mão Fixa: {0}", amountInitial);
                case Strategy.Martingale:
                    return string.Format("Martingale: {0}/{1}", martingale.Current, martingale.GetLevel);
                case Strategy.Soros:
                    return string.Format("Soros: {0}/{1}", soros.Current, soros.GetLevel);
                case Strategy.Sorosgale:
                    return string.Format("Sorosgale: {0}\nSoros: {1}", sorosgale.GetSorosgaleLevel, sorosgale.GetSorosLevel);
                    return "";
                default:
                    throw new Exception("BankManagement.cs ToString() - type not expected: " + type);
            }
        }
    }
}