using MTCG.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    public class InMemoryBattleLobbyDao
    {
        private string _otherUsername;
        // When false, the lock is reset and the threads wait and sleep
        // When true, the lock is set, and the threads can go through
        private ManualResetEvent _IPClocker = new ManualResetEvent(false);
        private readonly object _firstLock = new object();
        private readonly object _secondLock = new object();
        public InMemoryBattleLobbyDao()
        {
            _otherUsername = null;
        }

        public void AddToLobby(string username)
        {
            // Here we only allow one thread at a time
            lock (_firstLock)
            {
                Console.WriteLine("inside of first lock");
                if (_otherUsername == null)
                {
                    Console.WriteLine("inside of first if");
                    // When a thread (A) enters the lobby and there is no other player
                    _otherUsername = username;
                }
                else
                {
                    Console.WriteLine("inside of first else");

                    // When a thread (B) enters and there is already a thread (A) sleeping 
                    _IPClocker.Set(); // to wake up the sleepy (A) thread
                    Battle(username, _otherUsername);
                    // set to null, to indicate that there is no one in the thread
                    _otherUsername = null;
                    return;
                }
            }
            // Here the thread (A) waits until someone elses Sets
            lock (_secondLock)
            {
                Console.WriteLine("inside of second lock");

                _IPClocker.WaitOne();
                Console.WriteLine("inside of second lock after waitOne");

                // Leave it ready to trap the next (A) one
                _IPClocker.Reset();
                Console.WriteLine("inside of second lock after reset");
            }
        }

        public void Battle(string username1, string username2)
        {
            Console.WriteLine("Battle between {0} and {1}", username1, username2);

        }
    }
}
