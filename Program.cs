using static System.Console;
using static System.Math;
using static Work.Constant;

namespace Work
{
    //WARNING! ЗДЕСЬ АБСОЛЮТНАЯ ЕРЕСЬ. ОНА МОЖЕТ НЕ СОВПАДАТЬ С ДЕЙСТВИТЕЛЬНОСТЬЮ И БЫТЬ ОТКРОВЕННОЙ ЛОЖЬЮ!
    
    /*  Для уравнения:
            y1' = y1 * x + y2 * x^2,
            y2' = -y1 * x^2 + y2 * x;
    */
    
    public static class Constant
    {
        public const double h = 0.01; 
    }
    
    class Program
    {
        
        static void Main() => EulerMethod();

        static void EulerMethod()
        {
            #region InitialValue
            var y1 = 1.0; 
            var y2 = 1.0;
            var a = 0.0;
            var b = 3.0;
            var n = 10;
            #endregion
            
            WriteLine($"Начальные условия:\nу1(а) = {y1}; у2(а) = {y2};\nа = {a}; b = {b}; h = {h}\nРезультат:");
            WriteLine($" i  |   y1   |   y2   |    x\n------------------------------ ");
            
            var step = (b - a) / n;
            var i = 0;
            
            for (double x = a; x < b; x += step)
            {
                var p = new Param(y1, y2, x);

                var y1new = y1 + 0.5 * (p.K1 + p.K2);
                var y2new = y2 + 0.5 * (p.G1 + p.G2);
                
                y1 = y1new;
                y2 = y2new;

                var r1 = 1.0 / 12.0 * Pow(h, 3) * new DY(y1, y2, x).Y1_3;
                var r2 = 1.0 / 12.0 * Pow(h, 3) * new DY(y1, y2, x).Y2_3;
                
                if (Max(Abs(r1), Abs(r2)) >= h)
                    step -= h;

                i++;
                WriteLine($"[{i}] | {y1:0.0000} | {y2:0.0000} | {x:0.0000}");
            }
        }
    }

    class DY
    {
        private double y1_3;
        private double y2_3;

        public double Y1_3 => y1_3;
        public double Y2_3 => y2_3;

        public DY(double y1, double y2, double x)
        {
            var y1_1 = y1 * x + y2 * Pow(x,2);
            var y2_1 = y2 * x - y1 * Pow(x,2);
            
            var y1_2 = y1 + y1_1 * x + y2_1 * Pow(x,2) + 2 * x * y2;
            var y2_2 = -y1_1 * Pow(x,2) + y2_1 * x - 2 * x * y1 + y2;
            
            y1_3 = y1_1 + y1_2 * x + y1_1 + y2_2 * Pow(x, 2) + 2 * y2 + 2* x * y2_1 + 2 * x * y2_1;
            y2_3 = -y1_2 * Pow(x, 2) - 2 * x * y1_1 + y2_2 * x + y2_1 - 2 * y1 - 2 * x * y1_1 + y2_1;
        }
    }
    
    class Param
    {   
        static double calculateK1(double y1, double y2, double x) => y1 * x + y2 * Pow(x,2);
        static double calculateG1(double y1, double y2, double x) => y2 * x - y1 * Pow(x,2);
        static double calculateK2(double y1, double y2, double x, double k1, double g1) => (y1 + k1) * x + (y2 + g1) * Pow(x,2);
        static double calculateG2(double y1, double y2, double x, double k1, double g1) => (y2 + g1) * x - (y1 + k1) * Pow(x,2);

        private double k1;
        private double g1;
        private double k2;
        private double g2;

        public double K1 => k1;
        public double G1 => g1;
        public double K2 => k2;
        public double G2 => g2;
                
        public Param(double y1, double y2, double x)
        {
            k1 = calculateK1(y1, y2, x);
            g1 = calculateG1(y1, y2, x);
            k2 = calculateK2(y1, y2, x + h, k1, g1);
            g2 = calculateG2(y1, y2, x + h, k1, g1);
        }
    }
}

