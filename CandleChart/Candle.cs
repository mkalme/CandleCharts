using System;

namespace CandleChart {
    public struct Candle {
        public float Open { get; set; }
        public float Close { get; set; }
        public float Low { get; set; }
        public float High { get; set; }

        public Candle(float open, float close) {
            Open = open;
            Close = close;
            Low = 0;
            High = 0;
        }
        public Candle(float open, float close, float low, float high) {
            Open = open;
            Close = close;
            Low = low;
            High = high;
        }

        public CandleType GetCandleType() {
            if (Close > Open) {
                return CandleType.Up;
            } else if (Close < Open) {
                return CandleType.Down;
            } else {
                return CandleType.Doji;
            }
        }
    }

    public enum CandleType { 
        Up,
        Down,
        Doji
    }
}
