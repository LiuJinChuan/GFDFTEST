using System;

namespace GFDF.Infrastruct.Core
{
    public class IdWorker
    {
        private long workerId;
        private long twepoch = 1577808000000L;  //基准时间2012-6-1  毫秒
        private long sequence = 0L;
        //private int workerIdBits = 2;   //产生器编号位数 2^2
        private long maxWorkerId = 3;  //2^10-1 
        //private int sequenceBits = 10;  //顺序号位数  容纳2^10
        private long sequenceMask = 1023; //2^10-1
        //位移量  毫秒-产生器编号-流水号
        private int workerIdShift = 10;
        private int timestampLeftShift = 12;

        private long lastTimestamp = -1L;

        public IdWorker(long workerId)
        {
            if (workerId > this.maxWorkerId || workerId < 0)
            {
                throw new Exception(String.Format("worker Id can't be greater than {0} or less than 0",
                    this.maxWorkerId));
            }
            this.workerId = workerId;
        }

        public long nextId()
        {
            object lockobj = new object();
            lock (lockobj)
            {
                long timestamp = this.timeGen();   //当前时间毫秒数 
                if (this.lastTimestamp == timestamp)
                {      //同一毫秒的ID产生 修改sequence部分
                    this.sequence = this.sequence + 1 & this.sequenceMask;
                    if (this.sequence == 0)
                    {  //同一毫秒ID产生数超过允许最大数，则直到下一毫秒再产生。
                        timestamp = this.tilNextMillis(this.lastTimestamp);
                    }
                }
                else
                {
                    this.sequence = 0;
                }
                if (timestamp < this.lastTimestamp)
                {   //不允许时间被修改到以前。 ？要考虑程序关闭后的时间修改， 关闭时保存 开启时载入的方式。
                    throw new Exception(String
                        .Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds",
                            (this.lastTimestamp - timestamp)));
                }

                this.lastTimestamp = timestamp;
                return timestamp - this.twepoch << this.timestampLeftShift | this.workerId << this.workerIdShift
                        | this.sequence;   //(当前时间-基准时间)的毫秒数 产生器编号  顺序号
            }
        }

        private long tilNextMillis(long lastTimestamp)
        {
            long timestamp = this.timeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = this.timeGen();
            }
            return timestamp;
        }

        //以1970-1-1为基准计算毫秒
        private long timeGen()
        {
            return (System.DateTime.Now.ToUniversalTime().Ticks-621355968000000000) / 10000;
        }
        /// <summary>
        /// unix时间戳 ms
        /// </summary>
        /// <param name="idwork"></param>
        /// <returns></returns>
        public long GetTime(long idwork) {
            return ((idwork >> this.timestampLeftShift) + this.twepoch);
        }

        //提供的时间戳获取系统ID （机器码、随机码部分为0）
        public long utctoid(long milliutc)
        {
            return (milliutc - this.twepoch) << this.timestampLeftShift;
        }
    }
}