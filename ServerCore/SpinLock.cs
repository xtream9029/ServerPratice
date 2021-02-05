using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class SpinLock
    {
        int _lock = 0;

        public void Acquire()
        {
            int expected = 0;
            int desired = 1;

            while (true)
            {
                //Compare-And-Swap 방식
                //하드웨어로 구현
                // 쓰레드들은 끊임없이  락을 얻으려 시도함
                if (Interlocked.CompareExchange(ref _lock, desired, expected) == expected)
                {
                    return;
                }
            }
        }

        public void Release()
        {
            //Interlocked.Exchange(ref _lock, 0);
            _lock = 0;
        }
    }
}
