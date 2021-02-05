using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerCore
{
    class ReadWriteLock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7fff0000;
        const int READ_MASK = 0x0000ffff;
        const int MAX_SPIN_COUNT = 5000;

        static int _lock = EMPTY_FLAG;//static이여야 공유가 가능함

        public void ReadLock() 
        {
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    //if((_flag & WRITE_MASK) == 0)
                    //{
                    //    _flag += 1;
                    //    return;
                    //}

                    int expected = _lock & READ_MASK;
                    //ReadLock은 동시다발적으로 들어오는게 가능함
                    if (Interlocked.CompareExchange(ref _lock, expected + 1, expected) == expected)
                        return;
                }

                Thread.Yield();//락을 얻지 못하고 결국 cpu 양보--->컨택스트 스위칭이 일어남
            }
        }

        public void ReadUnLock()
        {
            Interlocked.Decrement(ref _lock);
        }

        public void WriteLock()
        {
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;//현재 write lock을 어떤 쓰레드가 가지고 있는지를 확인
            while (true)
            {
                //최대 5천번 돌고 그 이후에도 락을 얻지 못한다면 cpu소유를 양보함
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    //if (_flag == EMPTY_FLAG)
                    //    _flag = desired;

                    //동시 다발적으로 들어올수가 없음(왜냐하면 이미 desired에 다른 쓰레드 아이디값이 들어있기 때문)
                    if (Interlocked.CompareExchange(ref _lock, desired, EMPTY_FLAG) == EMPTY_FLAG)
                        return;
                }
                Thread.Yield();//락을 얻지 못하고 결국 cpu 양보--->컨택스트 스위칭이 일어남
            }
        }

        public void WriteUnLock()
        {
            Interlocked.Exchange(ref _lock, EMPTY_FLAG);
            //_lock=0x00000000 
        }

    }

}
