using MTCG.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MTCG.DAL;
using MTCG.HttpServer.Schemas;
using MTCG.BLL;
using MTCG.Models;
using MTCG.DAL.Interfaces;
using System.Runtime.CompilerServices;

namespace MTCG.DAL
{

    public class InMemoryBattleLobbyDao : IInMemoryBattleLobbyDao
    {
        private string _otherUsername;
        private string _fightLog;
        // When false, the lock is reset and the threads wait and sleep
        // When true, the lock is set, and the threads can go through
        private readonly ManualResetEvent _IPClocker = new ManualResetEvent(false);
        private readonly object _firstLock = new object();
        private readonly object _secondLock = new object();
        //we hold a reference to the DB objects to abtain the deck from the username
        private readonly DatabaseCardDao _cardDao;
        private readonly DatabaseStacksDao _stacksDao;
        private readonly DatabaseUserDao _userDao;

        public InMemoryBattleLobbyDao(DatabaseCardDao cardDao, DatabaseStacksDao stacksDao, DatabaseUserDao userDao)
        {
            _otherUsername = null;
            _fightLog = "";  // necesary for both players to have the same battle log
            _cardDao = cardDao;
            _stacksDao = stacksDao;
            _userDao = userDao;
            
        }

        public string AddToLobby(string username)
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
                    _fightLog = "";
                }
                else
                {
                    Console.WriteLine("inside of first else");

                    // When a thread (B) enters and there is already a thread (A) sleeping 

                    _fightLog = Fight(username, _otherUsername);
                    // set to null, to indicate that there is no one in the thread
                    _otherUsername = null;
                    _IPClocker.Set(); // to wake up the sleepy (A) thread
                    return _fightLog; 
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
                return _fightLog;
            }
        }

        public string Fight(string username1, string username2)
        {
            Console.WriteLine("Battle between {0} and {1}", username1, username2);
            List<string> user1Deck = _stacksDao.SelectCardsInDeckByUsername(username1);
            List<string> user2Deck = _stacksDao.SelectCardsInDeckByUsername(username2);
            List<Card> user1Cards = _cardDao.GetCardsByIdList(user1Deck);
            List<Card> user2Cards = _cardDao.GetCardsByIdList(user2Deck);
            Deck deck1 = new Deck(user1Cards);
            Deck deck2 = new Deck(user2Cards);

            Battle fight = new Battle();

            var (log, winner) = fight.Match(deck1, deck2, username1, username2);
            log += "\n" + fight.FinalMatchText(winner, username1, username2);
            User user1 = _userDao.SelectUserByUsername(username1);
            User user2 = _userDao.SelectUserByUsername(username2);
            ELOHandler.Update(winner, user1, user2);
            _userDao.UpdateEloWinsLosses(user1);
            _userDao.UpdateEloWinsLosses(user2);
            Console.WriteLine(log);

            return log;
        }
    }
}
