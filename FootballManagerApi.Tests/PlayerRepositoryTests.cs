using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FootballManagerApi.Contexts;
using FootballManagerApi.Models;
using Microsoft.EntityFrameworkCore;
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
            var options = new DbContextOptionsBuilder<FootballManagerContext>()
            .UseInMemoryDatabase(databaseName: "FootballManager")
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

        private Player GetGenericPlayerModel()
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


        //I want to make new player models in case the repo changes the original when I pass it in
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
            var player = GetGenericPlayerModel();
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

            var player = GetGenericPlayerModel();
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



        #endregion

        #region AddPlayer

        #endregion

        #region RemovePlayer

        #endregion

        #region TransferPlayer

        #endregion
    }
}
