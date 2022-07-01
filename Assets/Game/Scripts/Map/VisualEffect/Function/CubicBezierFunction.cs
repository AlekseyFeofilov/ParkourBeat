using System;

namespace Game.Scripts.Map.VisualEffect.Function
{
    
    // Функция кубической кривой Безье
    // https://cubic-bezier.com/
    public class CubicBezierFunction : ITimingFunction
    {
        private const double Epsilon = 1e-6;
        
        private double _x1, _y1, _x2, _y2;

        private double _cx, _bx, _ax, _cy, _by, _ay;

        public double X1
        {
            get => _x1;
            set
            {
                _x1 = value;
                Recalc();
            }
        }
        
        public double Y1
        {
            get => _y1;
            set
            {
                _y1 = value;
                Recalc();
            }
        }
        
        public double X2
        {
            get => _x2;
            set
            {
                _x2 = value;
                Recalc();
            }
        }
        
        public double Y2
        {
            get => _y2;
            set
            {
                _y2 = value;
                Recalc();
            }
        }

        // Конструктор прямой
        public CubicBezierFunction()
        {
            _x1 = 0;
            _y1 = 0;
            _x2 = 1;
            _y2 = 1;
            Recalc();
        }
        
        // Конструктор кривой Безье с настраиваемыми параметрами
        public CubicBezierFunction(double x1, double y1, double x2, double y2)
        {
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
            Recalc();
        }

        public float Get(float time)
        {
            return (float) SampleCurveY(SolveCurveX(time));
        }

        private void Recalc()
        {
            _cx = 3.0 * _x1;
            _bx = 3.0 * (_x2 - _x1) - _cx;
            _ax = 1.0 - _cx - _bx;

            _cy = 3.0 * _y1;
            _by = 3.0 * (_y2 - _y1) - _cy;
            _ay = 1.0 - _cy - _by;
        }

        private double SampleCurveX(double t) {
            return ((_ax * t + _bx) * t + _cx) * t;
        }

        private double SampleCurveY(double t) {
            return ((_ay * t + _by) * t + _cy) * t;
        }

        private double SampleCurveDerivativeX(double t) {
            return (3.0 * _ax * t + 2.0 * _bx) * t + _cx;
        }

        private double SolveCurveX(double x) {
            double x2;

            // Сначала попробуем несколько итераций методом Ньютона — обычно очень быстро.
            double t2 = x;
            for (int i = 0; i < 8; i++) {
                x2 = SampleCurveX(t2) - x;
                if (Math.Abs(x2) < Epsilon)
                    return t2;
                double d2 = SampleCurveDerivativeX(t2);
                if (Math.Abs(d2) < Epsilon)
                    break;
                t2 -= x2 / d2;
            }

            // Если решение не найдено - используем разделение пополам (bi-section)
            double t0 = 0.0;
            double t1 = 1.0;
            t2 = x;

            if (t2 < t0) return t0;
            if (t2 > t1) return t1;

            while (t0 < t1) {
                x2 = SampleCurveX(t2);
                if (Math.Abs(x2 - x) < Epsilon)
                    return t2;
                if (x > x2) t0 = t2;
                else t1 = t2;

                t2 = (t1 - t0) * .5 + t0;
            }

            // Результат
            return t2;
        }
    }
}