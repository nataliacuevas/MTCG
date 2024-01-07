using MTCG.Classes;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;
using System.Threading;

namespace MTCG.DAL
{

    public class InMemoryBattleLobbyDao : IInMemoryBattleLobbyDao
    {
        private string _otherUsername;
        private string _fightLog;
        // When false, the lock is reset and the threads wait and sleep
        // When true, the lock is set, and the threads can go through
        private readonly ManualResetEvent _IPClocker = new ManualResetEvent(false);
        // These objects are require to have separated "lock" scopes.
        private readonly object _firstLock = new object();
        private readonly object _secondLock = new object();
        // holds a reference to the DB objects to abtain the deck from the username
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
            // Here we only allow one thread at a time.
            // thread(A) is the first thread to come in.
            // thread(B) is the one afterwards.
            lock (_firstLock)
            {
                if (_otherUsername == null)
                {
                    // When a thread (A) enters the lobby and there is no other player
                    // sets this variable, for the thread (B) to go to the "else" statement.
                    _otherUsername = username;
                    _fightLog = "";
                }
                else
                {
                    // When a thread (B) enters and there is already a thread (A) sleeping in the _IPClocker.WaitOne() statement
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
                _IPClocker.WaitOne();
                // Leave it ready to trap the next thread (A) one
                _IPClocker.Reset();
                return _fightLog;
            }
        }
        // Runs fight/battle between two usernames. 
        // Gets the involved decks from the stored DAOs
        // Sets all the updated value with them too.
        public string Fight(string username1, string username2)
        {
            // Selection
            List<string> user1Deck = _stacksDao.SelectCardsInDeckByUsername(username1);
            List<string> user2Deck = _stacksDao.SelectCardsInDeckByUsername(username2);
            List<Card> user1Cards = _cardDao.GetCardsByIdList(user1Deck);
            List<Card> user2Cards = _cardDao.GetCardsByIdList(user2Deck);
            Deck deck1 = new Deck(user1Cards);
            Deck deck2 = new Deck(user2Cards);
            // Fighting
            Battle fight = new Battle();
            var (log, winner) = fight.Match(deck1, deck2, username1, username2);
            log += "\n" + fight.FinalMatchText(winner, username1, username2);
            // Updating
            User user1 = _userDao.SelectUserByUsername(username1);
            User user2 = _userDao.SelectUserByUsername(username2);
            ELOHandler.Update(winner, user1, user2);
            _userDao.UpdateEloWinsLosses(user1);
            _userDao.UpdateEloWinsLosses(user2);
            return log;
        }
    }
}
