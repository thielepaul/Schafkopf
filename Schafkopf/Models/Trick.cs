﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Schafkopf.Hubs;

namespace Schafkopf.Models
{
    public class Trick
    {
        public Card[] Cards = new Card[4];
        public Player[] Player = new Player[4];
        public Card FirstCard;
        public int Count = 0;
        public GameType GameType = GameType.Ramsch;
        public Color Trump = Color.Herz;
        public int Winner = 0;
        private int StartPlayer;

        public Trick(Game game, int startPlayer)
        {
            GameType = game.GameState.AnnouncedGame;
            DetermineTrumpf(game);
            StartPlayer = startPlayer;
        }

        //-------------------------------------------------
        // Card is added to the trick
        // in case that there are too many cards in one trick, an exception is thrown
        //-------------------------------------------------
        public void AddCard(Card card, Player player, Game game)
        {
            if (Count >= 4)
            {
                throw new Exception("There are too many Cards in the trick.");
            }
            Cards[Count] = card;
            Player[Count] = player;


            //Determine the winner of the Trick
            if (Count > 0)
            {
                DetermineWinnerCard(card);
            }
            Count++;
        }

        //-------------------------------------------------
        // FirstCard
        // WinnerCard
        // NewCard
        //-------------------------------------------------
        private void DetermineWinnerCard(Card newCard)
        {
            //Check which one is higher
            if (newCard.GetValue(GameType, Trump, GetFirstCard()) > Cards[Winner].GetValue(GameType, Trump, GetFirstCard()))
            {
                Winner = Count;
            }
        }

        public Player GetWinner()
        {
            return Player[Winner];
        }

        private Card GetFirstCard()
        {
            return Cards[0];
        }

        private void DetermineTrumpf(Game game)
        {
            switch (game.GameState.AnnouncedGame)
            {
                case GameType.Ramsch:
                case GameType.Sauspiel:
                case GameType.Hochzeit:
                    Trump = Color.Herz;
                    break;
                case GameType.Farbsolo:
                case GameType.FarbsoloTout:
                    Trump = game.GameState.Leader.AnnouncedColor;
                    break;
                case GameType.Wenz:
                case GameType.WenzTout:
                    Trump = Color.None;
                    break;
            }
        }

        public async Task SendTrick(SchafkopfHub hub, Game game, List<String> connectionIds)
        {
            for (int i = 0; i < 4; i++)
            {
                Card[] permutedCards = new Card[4];
                for (int j = 0; j < 4; j++)
                {
                    permutedCards[j] = Cards[(j + i) % 4];
                }
                Player player = game.GameState.PlayingPlayers[(StartPlayer + i) % 4];
                foreach (String connectionId in player.GetConnectionIdsWithSpectators())
                {
                    if (!connectionIds.Contains(connectionId))
                    {
                        continue;
                    }
                    await hub.Clients.Client(connectionId).SendAsync(
                        "ReceiveTrick",
                        permutedCards.Select(card => card == null ? "" : card.ToString())
                    );
                }
            }
        }
    }
}
