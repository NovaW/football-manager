using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FootballManagerApi.Contexts;
using FootballManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FootballManagerApi.Tests
{
    public class PlayerRepositoryTests : IDisposable
    {
        FootballManagerContext _dbContext;
        PlayerRepository _playerRepository;

        //setup before each test
        public PlayerRepositoryTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<FootballManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestFootballManager")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

            _dbContext = new FootballManagerContext(options);
            _playerRepository = new PlayerRepository(_dbContext);
        }

        //clean up after each test
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private Player GetRandomPlayerModelWithoutTeam()
        {
            return new Player
            {
                FirstName = RandomString(8),
                LastName = RandomString(8),
                HeightInCentimeters = random.Next(1, 200),
                DateOfBirth = RandomString(8),
                Nationality = RandomString(8)
            };
        }


        // to make new player models in case the repo changes the original model when it's passed in
        private Player DuplicatePlayer(Player player)
        {
            return new Player
            {
                FirstName = player.FirstName,
                LastName = player.LastName,
                HeightInCentimeters = player.HeightInCentimeters,
                Nationality = player.Nationality,
                DateOfBirth = player.DateOfBirth,
                TeamId = player.TeamId,
                Team = player.Team
            };
        }

        #region GetPlayer

        [Fact]
        public async Task GetPlayerTest_WithoutTeam()
        {
            var player = GetRandomPlayerModelWithoutTeam();
            var expected = DuplicatePlayer(player);

            var addResult = _dbContext.Player.Add(player);
            var playerId = (int) addResult.Entity.Id;

            await _dbContext.SaveChangesAsync();

            var result = await _playerRepository.GetPlayer(playerId);

            Assert.Equal(expected.FirstName, result.FirstName);
            Assert.Equal(expected.LastName, result.LastName);
            Assert.Equal(expected.HeightInCentimeters, result.HeightInCentimeters);
            Assert.Equal(expected.DateOfBirth, result.DateOfBirth);
            Assert.Equal(expected.Nationality, result.Nationality);
            Assert.Null(result.TeamId);
            Assert.Null(result.Team);
        }

        [Fact]
        public async Task GetPlayerTest_WithTeam()
        {
            var expectedTeamName = RandomString(8);
            var team = new Team { Name = expectedTeamName };
            var teamAddResult = _dbContext.Team.Add(team);
            var teamId = (int) teamAddResult.Entity.Id;

            await _dbContext.SaveChangesAsync();

            var player = GetRandomPlayerModelWithoutTeam();
            player.TeamId = teamId;
            var expected = DuplicatePlayer(player);

            var playerAddResult = _dbContext.Player.Add(player);
            var playerId = (int) playerAddResult.Entity.Id;

            await _dbContext.SaveChangesAsync();

            var result = await _playerRepository.GetPlayer(playerId);

            Assert.Equal(expected.FirstName, result.FirstName);
            Assert.Equal(expected.LastName, result.LastName);
            Assert.Equal(expected.HeightInCentimeters, result.HeightInCentimeters);
            Assert.Equal(expected.DateOfBirth, result.DateOfBirth);
            Assert.Equal(expected.Nationality, result.Nationality);
            Assert.Equal(teamId, result.TeamId);
            Assert.NotNull(result.Team);
            Assert.Equal(teamId, result.Team.Id);
            Assert.Equal(expectedTeamName, result.Team.Name);
        }

        #endregion

        #region GetAllPlayers

        [Fact]
        public async Task GetAllPlayersTest()
        {
            var players = new List<Player>();

            // add 10 players without teams
            for (int i = 0; i < 10; i++)
            {
                var player = GetRandomPlayerModelWithoutTeam();
                players.Add(player);
            }

            // add 10 players with the same team
            var expectedTeamName = RandomString(8);
            var team = new Team { Name = expectedTeamName };
            var teamAddResult = _dbContext.Team.Add(team);
            var teamId = (int)teamAddResult.Entity.Id;

            for (int i = 0; i < 10; i++)
            {
                var player = GetRandomPlayerModelWithoutTeam();
                player.TeamId = teamId;
                player.Team = team;
                players.Add(player);
            }

            // add 10 players with random teams
            for (int i = 0; i < 10; i++)
            {
                var player = GetRandomPlayerModelWithoutTeam();
                var randomTeam = new Team { Name = RandomString(8) };
                var randomTeamAddResult = _dbContext.Team.Add(randomTeam);
                player.TeamId = (int)randomTeamAddResult.Entity.Id;
                players.Add(player);
            }

            var expectedResults = new Dictionary<long, Player>();

            // add all players to DB
            foreach (var player in players)
            {
                var addResult = _dbContext.Player.Add(player);
                await _dbContext.SaveChangesAsync();

                var playerId = addResult.Entity.Id;
                var expectedResult = DuplicatePlayer(player);
                expectedResult.Id = playerId;

                expectedResults.Add(playerId, expectedResult);
            }

            var actualResults = await _playerRepository.GetAllPlayers();

            // Check same number of results
            Assert.Equal(expectedResults.Count(), actualResults.Count());

            // Check details match
            foreach (var actual in actualResults)
            {
                var expected = expectedResults[actual.Id];

                Assert.Equal(expected.FirstName, actual.FirstName);
                Assert.Equal(expected.LastName, actual.LastName);
                Assert.Equal(expected.HeightInCentimeters, actual.HeightInCentimeters);
                Assert.Equal(expected.DateOfBirth, actual.DateOfBirth);
                Assert.Equal(expected.Nationality, actual.Nationality);
                Assert.Equal(expected.TeamId, actual.TeamId);
                if (expected.Team == null)
                {
                    Assert.Null(actual.Team);
                }
                else
                {
                    Assert.NotNull(actual.Team);
                    Assert.Equal(expected.Team.Id, actual.Team.Id);
                    Assert.Equal(expected.Team.Name, actual.Team.Name);
                }
            }
        }

        #endregion

        #region AddPlayer

        [Fact]
        public async Task AddPlayerTest_WithoutTeam()
        {
            var player = GetRandomPlayerModelWithoutTeam();
            var expected = DuplicatePlayer(player);

            var methodResult = await _playerRepository.AddPlayer(player);
            var playerInDb = _dbContext.Player.Find(methodResult.Id);

            // check that returned result matches database
            Assert.Equal(methodResult.FirstName, playerInDb.FirstName);
            Assert.Equal(methodResult.LastName, playerInDb.LastName);
            Assert.Equal(methodResult.HeightInCentimeters, playerInDb.HeightInCentimeters);
            Assert.Equal(methodResult.DateOfBirth, playerInDb.DateOfBirth);
            Assert.Equal(methodResult.Nationality, playerInDb.Nationality);

            // check that no team was recorded
            Assert.Null(methodResult.Team);
            Assert.Null(methodResult.TeamId);
            Assert.Null(playerInDb.Team);
            Assert.Null(playerInDb.TeamId);

            // check that returned result matches the model given to the method
            Assert.Equal(expected.FirstName, methodResult.FirstName);
            Assert.Equal(expected.LastName, methodResult.LastName);
            Assert.Equal(expected.HeightInCentimeters, methodResult.HeightInCentimeters);
            Assert.Equal(expected.DateOfBirth, methodResult.DateOfBirth);
            Assert.Equal(expected.Nationality, methodResult.Nationality);
        }

        [Fact]
        public async Task AddPlayerTest_WithTeam()
        {
            var expectedTeamName = RandomString(8);
            var team = new Team { Name = expectedTeamName };
            var teamAddResult = _dbContext.Team.Add(team);

            var player = GetRandomPlayerModelWithoutTeam();
            player.TeamId = (int)teamAddResult.Entity.Id;
            var expected = DuplicatePlayer(player);

            var methodResult = await _playerRepository.AddPlayer(player);
            var playerInDb = _dbContext.Player.Find(methodResult.Id);

            // check that returned result matches database
            Assert.Equal(methodResult.FirstName, playerInDb.FirstName);
            Assert.Equal(methodResult.LastName, playerInDb.LastName);
            Assert.Equal(methodResult.HeightInCentimeters, playerInDb.HeightInCentimeters);
            Assert.Equal(methodResult.DateOfBirth, playerInDb.DateOfBirth);
            Assert.Equal(methodResult.Nationality, playerInDb.Nationality);

            // check that a team was recorded
            Assert.NotNull(methodResult.Team);
            Assert.NotNull(methodResult.TeamId);
            Assert.NotNull(playerInDb.Team);
            Assert.NotNull(playerInDb.TeamId);

            // check that team Id is consistent
            Assert.Equal(methodResult.TeamId, methodResult.Team.Id);
            Assert.Equal(playerInDb.TeamId, playerInDb.Team.Id);

            // check that team details match on returned result and db result
            Assert.Equal(methodResult.TeamId, playerInDb.TeamId);
            Assert.Equal(methodResult.Team.Id, playerInDb.Team.Id);
            Assert.Equal(methodResult.Team.Name, playerInDb.Team.Name);

            // check that returned result matches the model given to the method
            Assert.Equal(expected.FirstName, methodResult.FirstName);
            Assert.Equal(expected.LastName, methodResult.LastName);
            Assert.Equal(expected.HeightInCentimeters, methodResult.HeightInCentimeters);
            Assert.Equal(expected.DateOfBirth, methodResult.DateOfBirth);
            Assert.Equal(expected.Nationality, methodResult.Nationality);

            // check that team details are as expected
            Assert.Equal(expected.TeamId, methodResult.TeamId);
            Assert.Equal(expectedTeamName, methodResult.Team.Name);
        }

        #endregion

        #region RemovePlayer

        [Fact]
        public async Task RemovePlayer_WithoutTeam()
        {
            var player = GetRandomPlayerModelWithoutTeam();
            var addResult = _dbContext.Player.Add(player);
            var playerId = (int) addResult.Entity.Id;
            await _playerRepository.RemovePlayer(playerId);

            // check player was removed from DB
            var numberOfDatabaseMatches = _dbContext.Player.Count(x => x.Id == playerId);
            Assert.Equal(0, numberOfDatabaseMatches);
        }

        [Fact]
        public async Task RemovePlayer_WithTeam()
        {
            var team = new Team { Name = RandomString(8)};
            var teamAddResult = _dbContext.Team.Add(team);
            var teamId = (int)teamAddResult.Entity.Id;

            var player = GetRandomPlayerModelWithoutTeam();
            player.TeamId = teamId;

            var playerAddResult = _dbContext.Player.Add(player);
            var playerId = (int)playerAddResult.Entity.Id;

            await _playerRepository.RemovePlayer(playerId);

            // check player was removed from DB
            var numberOfDatabaseMatches = _dbContext.Player.Count(x => x.Id == playerId);
            Assert.Equal(0, numberOfDatabaseMatches);
        }

        #endregion

        #region TransferPlayer

        #endregion
    }
}
