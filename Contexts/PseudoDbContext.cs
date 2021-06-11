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

        public async Task<Player> GetPlayer(int playerId) {
            if(!_players.ContainsKey(playerId)) { return null; }
            var player = _players[playerId];
            player.Team = player.TeamId.HasValue ? _teams[player.TeamId.Value] : null;
            return player;
        }
        public async Task<Stadium> GetStadium (int stadiumId) {
            if(!_stadiums.ContainsKey(stadiumId)) { return null; }
            var stadium = _stadiums[stadiumId];
            stadium.HomeTeam = stadium.HomeTeamId.HasValue ? _teams[stadium.HomeTeamId.Value] : null;
            return stadium;
        }
        public async Task<Team> GetTeam(int teamId) { 
            if(!_teams.ContainsKey(teamId)) { return null; }
            var team = _teams[teamId];
            team.HomeStadium = team.HomeStadiumId.HasValue ? _stadiums[team.HomeStadiumId.Value] : null;
            team.Players = _players.Values.Where(x => x.TeamId == team.Id).ToHashSet();
            return team;
        }

        public async Task<IQueryable<Player>> GetPlayers() {
            var players = _players.Values;
            foreach(var player in players){
                player.Team = player.TeamId.HasValue ? _teams[player.TeamId.Value] : null;
            }
            return players.AsQueryable();
        }

        public async Task<IQueryable<Stadium>> GetStadiums() {
            var stadiums = _stadiums.Values;
            foreach(var stadium in stadiums){
                stadium.HomeTeam = stadium.HomeTeamId.HasValue ? _teams[stadium.HomeTeamId.Value] : null;
            }
            return stadiums.AsQueryable();
        }

        public async Task<IQueryable<Team>> GetTeams() {
            var teams = _teams.Values;
            var players = _players.Values;
            foreach(var team in teams){
                team.HomeStadium = team.HomeStadiumId.HasValue ? _stadiums[team.HomeStadiumId.Value] : null;
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

            VerifyTeamExists(player.TeamId);

            _players.Add(player.Id, player);
            return player;
        }

        public async Task<Stadium> AddStadium(Stadium stadium){
            if(_stadiums.Count == 0){
                stadium.Id = 1;
            }else{
                stadium.Id = _stadiums.Keys.Max() + 1;
            }

            VerifyTeamExists(stadium.HomeTeamId);

            _stadiums.Add(stadium.Id, stadium);
            return stadium;
        }

        public async Task<Team> AddTeam(Team team){
            if(_teams.Count == 0){
                team.Id = 1;
            }else{
                team.Id = _teams.Keys.Max() + 1;
            }
            
            foreach(var player in team.Players){
                VerifyPlayerExists(player.Id);
            }
            VerifyStadiumExists(team.HomeStadiumId);

            _teams.Add(team.Id, team);
            return team;
        }

        public async Task<Player> RemovePlayer(int playerId){
            if(!_players.ContainsKey(playerId)) { return null; }
            var player = _players[playerId];
            _players.Remove(playerId);
            return player;
        }

        public async Task<Stadium> RemoveStadium(int stadiumId){
            if(!_stadiums.ContainsKey(stadiumId)) { return null; }
            var stadium = _stadiums[stadiumId];
            _stadiums.Remove(stadiumId);
            return stadium;
        }

        public async Task<Team> RemoveTeam(int teamId){
            if(!_teams.ContainsKey(teamId)) { return null; }
            var team = _teams[teamId];
            _teams.Remove(teamId);
            return team;
        }

        public async Task LinkTeamAndStadium(int teamId, int stadiumId){
            VerifyTeamExists(teamId);
            VerifyStadiumExists(stadiumId);

            _teams[teamId].HomeStadiumId = stadiumId;
            _stadiums[stadiumId].HomeTeamId = teamId;
        }

        private void VerifyPlayerExists(int? playerId){
            if(playerId.HasValue && !_players.ContainsKey(playerId.Value)){
                throw new ArgumentException($"No player with Id {playerId} exists.");
            }
        }

        private void VerifyStadiumExists(int? stadiumId){
            if(stadiumId.HasValue && !_stadiums.ContainsKey(stadiumId.Value)){
                throw new ArgumentException($"No stadium with Id {stadiumId} exists.");
            }
        }

        private void VerifyTeamExists(int? teamId){
            if(teamId.HasValue && !_teams.ContainsKey(teamId.Value)){
                throw new ArgumentException($"No team with Id {teamId} exists.");
            }
        }

    }
}