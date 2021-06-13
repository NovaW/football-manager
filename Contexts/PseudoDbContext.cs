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

            _deletedPlayerIds = new List<int>();
            _deletedStadiumIds = new List<int>();
            _deletedTeamIds = new List<int>();
        }
        private Dictionary<int, Player> _players;
        private Dictionary<int, Stadium> _stadiums;
        private Dictionary<int, Team> _teams;


        private List<int> _deletedPlayerIds;
        private List<int> _deletedStadiumIds;
        private List<int> _deletedTeamIds;

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
            team.Players = _players.Values.Where(x => x.TeamId == team.Id);
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
                team.Players = players.Where(x => x.TeamId == team.Id);
            }
            return teams.AsQueryable();
        }

        public async Task<Player> AddPlayer(Player player){
            VerifyTeamExists(player.TeamId);
            player.Team = null;

            player.Id = _players.Keys.Union(_deletedPlayerIds)
                            .DefaultIfEmpty(0)
                            .Max() + 1;

            _players.Add(player.Id, player);
        
            return _players[player.Id];
        }

        public async Task<Stadium> AddStadium(Stadium stadium){
            VerifyTeamExists(stadium.HomeTeamId);
            stadium.HomeTeam = null;

            stadium.Id = _stadiums.Keys.Union(_deletedStadiumIds)
                            .DefaultIfEmpty(0)
                            .Max() + 1;

            _stadiums.Add(stadium.Id, stadium);

            if(stadium.HomeTeamId.HasValue){
                _teams[stadium.HomeTeamId.Value].HomeStadiumId = stadium.Id;
            }

            return _stadiums[stadium.Id];
        }

        public async Task<Team> AddTeam(Team team){
            VerifyStadiumExists(team.HomeStadiumId);
            team.HomeStadium = null;
            team.Players = null;
            
            team.Id = _teams.Keys.Union(_deletedTeamIds)
                        .DefaultIfEmpty(0)
                        .Max() + 1;

            _teams.Add(team.Id, team);
            return _teams[team.Id];
        }

        public async Task<Player> RemovePlayer(int playerId){
            if(!_players.ContainsKey(playerId)) { return null; }
            var player = _players[playerId];
            _players.Remove(playerId);
            _deletedPlayerIds.Add(playerId);
            return player;
        }

        public async Task<Stadium> RemoveStadium(int stadiumId){
            if(!_stadiums.ContainsKey(stadiumId)) { return null; }

            // remove links to stadium in teams
            foreach(var team in _teams){
                if(team.Value.HomeStadiumId == stadiumId){
                    team.Value.HomeStadiumId = null;
                }
            }

            var stadium = _stadiums[stadiumId];
            _stadiums.Remove(stadiumId);
            _deletedStadiumIds.Add(stadiumId);
            return stadium;
        }

        public async Task<Team> RemoveTeam(int teamId){
            if(!_teams.ContainsKey(teamId)) { return null; }

            // remove links to team in stadiums
            foreach(var stadium in _stadiums){
                if(stadium.Value.HomeTeamId == teamId){
                    stadium.Value.HomeTeamId = null;
                }
            }

            var team = _teams[teamId];
            _teams.Remove(teamId);
            _deletedTeamIds.Add(teamId);
            return team;
        }

        public async Task<(Team Team, Stadium Stadium)> LinkTeamAndStadium(int teamId, int stadiumId){
            VerifyTeamExists(teamId);
            VerifyStadiumExists(stadiumId);

            _teams[teamId].HomeStadiumId = stadiumId;
            _stadiums[stadiumId].HomeTeamId = teamId;

            return (_teams[teamId], _stadiums[stadiumId]);
        }

        public async Task<Player> TransferPlayer(int playerId, int newTeamId) {
            VerifyPlayerExists(playerId);
            VerifyTeamExists(newTeamId);
            
            _players[playerId].TeamId = newTeamId;

            return _players[playerId];
        }

        public async Task<Team> AddPlayersToTeam(int teamId, IEnumerable<int> playerIds) {
            VerifyTeamExists(teamId);

            foreach(var id in playerIds){
                VerifyPlayerExists(id);
            }
            
            foreach(var id in playerIds){
                _players[id].TeamId = teamId;
            }
            return _teams[teamId];
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