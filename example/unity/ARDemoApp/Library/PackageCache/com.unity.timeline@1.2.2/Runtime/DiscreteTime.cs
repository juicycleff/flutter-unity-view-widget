using System;

namespace UnityEngine.Timeline
{
    struct DiscreteTime : IComparable
    {
        const double k_Tick = 1e-12;
        public static readonly DiscreteTime kMaxTime = new DiscreteTime(Int64.MaxValue);

        readonly Int64 m_DiscreteTime;

        public static double tickValue { get { return k_Tick; } }

        public DiscreteTime(DiscreteTime time)
        {
            m_DiscreteTime = time.m_DiscreteTime;
        }

        DiscreteTime(Int64 time)
        {
            m_DiscreteTime = time;
        }

        public DiscreteTime(double time)
        {
            m_DiscreteTime = DoubleToDiscreteTime(time);
        }

        public DiscreteTime(float time)
        {
            m_DiscreteTime = FloatToDiscreteTime(time);
        }

        public DiscreteTime(int time)
        {
            m_DiscreteTime = IntToDiscreteTime(time);
        }

        public DiscreteTime(int frame, double fps)
        {
            m_DiscreteTime = DoubleToDiscreteTime(frame * fps);
        }

        public DiscreteTime OneTickBefore()
        {
            return new DiscreteTime(m_DiscreteTime - 1);
        }

        public DiscreteTime OneTickAfter()
        {
            return new DiscreteTime(m_DiscreteTime + 1);
        }

        public Int64 GetTick()
        {
            return m_DiscreteTime;
        }

        public int CompareTo(object obj)
        {
            if (obj is DiscreteTime)
                return m_DiscreteTime.CompareTo(((DiscreteTime)obj).m_DiscreteTime);
            return 1;
        }

        public bool Equals(DiscreteTime other)
        {
            return m_DiscreteTime == other.m_DiscreteTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is DiscreteTime)
                return Equals((DiscreteTime)obj);
            return false;
        }

        static Int64 DoubleToDiscreteTime(double time)
        {
            double number = (time / k_Tick) + 0.5;
            if (number < Int64.MaxValue && number > Int64.MinValue)
                return (Int64)number;
            throw new ArgumentOutOfRangeException("Time is over the discrete range.");
        }

        static Int64 FloatToDiscreteTime(float time)
        {
            float number = (time / (float)k_Tick) + 0.5f;
            if (number < Int64.MaxValue && number > Int64.MinValue)
                return (Int64)number;
            throw new ArgumentOutOfRangeException("Time is over the discrete range.");
        }

        static Int64 IntToDiscreteTime(int time)
        {
            return DoubleToDiscreteTime(time);
        }

        static double ToDouble(Int64 time)
        {
            return time * k_Tick;
        }

        static float ToFloat(Int64 time)
        {
            return (float)ToDouble(time);
        }

        public static explicit operator double(DiscreteTime b)
        {
            return ToDouble(b.m_DiscreteTime);
        }

        public static explicit operator float(DiscreteTime b)
        {
            return ToFloat(b.m_DiscreteTime);
        }

        public static explicit operator Int64(DiscreteTime b)
        {
            return b.m_DiscreteTime;
        }

        public static explicit operator DiscreteTime(double time)
        {
            return new DiscreteTime(time);
        }

        public static explicit operator DiscreteTime(float time)
        {
            return new DiscreteTime(time);
        }

        public static implicit operator DiscreteTime(Int32 time)
        {
            return new DiscreteTime(time);
        }

        public static explicit operator DiscreteTime(Int64 time)
        {
            return new DiscreteTime(time);
        }

        public static bool operator==(DiscreteTime lhs, DiscreteTime rhs)
        {
            return lhs.m_DiscreteTime == rhs.m_DiscreteTime;
        }

        public static bool operator!=(DiscreteTime lhs, DiscreteTime rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator>(DiscreteTime lhs, DiscreteTime rhs)
        {
            return lhs.m_DiscreteTime > rhs.m_DiscreteTime;
        }

        public static bool operator<(DiscreteTime lhs, DiscreteTime rhs)
        {
            return lhs.m_DiscreteTime < rhs.m_DiscreteTime;
        }

        public static bool operator<=(DiscreteTime lhs, DiscreteTime rhs)
        {
            return lhs.m_DiscreteTime <= rhs.m_DiscreteTime;
        }

        public static bool operator>=(DiscreteTime lhs, DiscreteTime rhs)
        {
            return lhs.m_DiscreteTime >= rhs.m_DiscreteTime;
        }

        public static DiscreteTime operator+(DiscreteTime lhs, DiscreteTime rhs)
        {
            return new DiscreteTime(lhs.m_DiscreteTime + rhs.m_DiscreteTime);
        }

        public static DiscreteTime operator-(DiscreteTime lhs, DiscreteTime rhs)
        {
            return new DiscreteTime(lhs.m_DiscreteTime - rhs.m_DiscreteTime);
        }

        public override string ToString()
        {
            return m_DiscreteTime.ToString();
        }

        public override int GetHashCode()
        {
            return m_DiscreteTime.GetHashCode();
        }

        public static DiscreteTime Min(DiscreteTime lhs, DiscreteTime rhs)
        {
            return new DiscreteTime(Math.Min(lhs.m_DiscreteTime, rhs.m_DiscreteTime));
        }

        public static DiscreteTime Max(DiscreteTime lhs, DiscreteTime rhs)
        {
            return new DiscreteTime(Math.Max(lhs.m_DiscreteTime, rhs.m_DiscreteTime));
        }

        public static double SnapToNearestTick(double time)
        {
            Int64 discreteTime = DoubleToDiscreteTime(time);
            return ToDouble(discreteTime);
        }

        public static float SnapToNearestTick(float time)
        {
            Int64 discreteTime = FloatToDiscreteTime(time);
            return ToFloat(discreteTime);
        }

        public static Int64 GetNearestTick(double time)
        {
            return DoubleToDiscreteTime(time);
        }
    }
}
