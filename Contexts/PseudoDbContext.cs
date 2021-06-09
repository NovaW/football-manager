using System;
using System.Collections.Generic;
using System.Linq;
using FootballManager.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FootballManager {
    public class PseudoDbContext : IPseudoDbContext {
        public PseudoDbContext(){
            _players = new Dictionary<int, Player>();
            _stadiums = new Dictionary<int, Stadium>();
            _teams = new Dictionary<int, Team>();
        }
        private Dictionary<int, Player> _players;
        private Dictionary<int, Stadium> _stadiums;
        private Dictionary<int, Team> _teams;

        public async Task<IQueryable<Player>> GetPlayers() {
            var players = _players.Values;
            foreach(var player in players){
                player.Team = _teams[player.TeamId];
            }
            return players.AsQueryable();
        }

        public async Task<IQueryable<Stadium>> GetStadiums() {
            var stadiums = _stadiums.Values;
            foreach(var stadium in stadiums){
                stadium.HomeTeam = _teams[stadium.HomeTeamId];
            }
            return stadiums.AsQueryable();
        }

        public async Task<IQueryable<Team>> GetTeams() {
            var teams = _teams.Values;
            var players = _players.Values;
            foreach(var team in teams){
                team.HomeStadium = _stadiums[team.HomeStadiumId];
                team.Players = players.Where(x => x.TeamId == team.Id).ToHashSet();
            }
            return teams.AsQueryable();
        }

        public async Task<Player> AddPlayer(Player player){
            if(_players.Count == 0){
                player.Id = 1;
            }else{
                player.Id = _players.Keys.Max() + 1;
            }
            _players.Add(player.Id, player);
            return player;
        }

        public async Task<Stadium> AddStadium(Stadium stadium){
            if(_stadiums.Count == 0){
                stadium.Id = 1;
            }else{
                stadium.Id = _stadiums.Keys.Max() + 1;
            }
            _stadiums.Add(stadium.Id, stadium);
            return stadium;
        }

        public async Task<Team> AddTeam(Team team){
            if(_teams.Count == 0){
                team.Id = 1;
            }else{
                team.Id = _teams.Keys.Max() + 1;
            }
            _teams.Add(team.Id, team);
            return team;
        }

        public async Task<Player> RemovePlayer(int playerId){
            var player = _players[playerId];
            _players.Remove(playerId);
            return player;
        }

        public async Task<Stadium> RemoveStadium(int stadiumId){
            var stadium = _stadiums[stadiumId];
            _stadiums.Remove(stadiumId);
            return stadium;
        }

        public async Task<Team> RemoveTeam(int teamId){
            var team = _teams[teamId];
            _teams.Remove(teamId);
            return team;
        }

    }
}