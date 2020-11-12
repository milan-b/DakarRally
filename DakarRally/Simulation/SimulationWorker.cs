using Contracts;
using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Simulation
{
    public interface ISimulationWorker
    {
        Task SimulateRally(CancellationToken stoppingToken);
    }

    public class SimulationWorker: ISimulationWorker
    {
        private readonly ILogger<SimulationWorker> _logger;
        private readonly IServiceProvider _services;
        private readonly SimulationConfiguration _simConf;
        private const int MS_TO_HOUR_DIFERENCE = 3600000;
        private readonly double DEADLINE_IN_HOURS;
        private readonly int INVERTED_DEADLINE_IN_HOURS;
        private readonly int MAX_RANDOM_FOR_PERCENTAGE;
        private readonly int HALF_OF_MAX_SPEED_CHANGE;
        public SimulationWorker(ILogger<SimulationWorker> logger, IServiceProvider services, IConfiguration configuration)
        {
            _logger = logger;
            _services = services;
            _simConf = new SimulationConfiguration();
            configuration.GetSection(SimulationConfiguration.Simulation).Bind(_simConf);

            DEADLINE_IN_HOURS = (double)_simConf.DeadlineForRealTime / MS_TO_HOUR_DIFERENCE;
            HALF_OF_MAX_SPEED_CHANGE = (int)Math.Ceiling(_simConf.MaxSpeedChange * 0.5);
            //NOTE Could create some fractional error in some cases
            INVERTED_DEADLINE_IN_HOURS = MS_TO_HOUR_DIFERENCE / _simConf.DeadlineForRealTime;
            MAX_RANDOM_FOR_PERCENTAGE = 100 * INVERTED_DEADLINE_IN_HOURS;

        }

        public async Task SimulateRally(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                
                var repository = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
                var simulation = repository.Simulation.FindByCondition(o => o.EndTime == null).FirstOrDefault();
                if (simulation == null)
                {
                    _logger.LogInformation("There is no simulation to execute.");
                }
                else
                {
                    var vehicles = repository.Vehicle.FindByCondition(o => o.RaceId == simulation.RaceId && o.VehicleStatistic.FinishTime == null)
                                .Include(o => o.VehicleStatistic)
                                //.Include(o => o.VehicleType)
                                .ToList();
                    var vehicleTypes = repository.VehicleType.FindAll().ToList();
                    while (!stoppingToken.IsCancellationRequested && simulation.EndTime == null)
                    {
                        var iterationStarTime = DateTime.Now;

                        for (int i = 0; i< vehicles.Count(); i++)
                        {
                            var vehicle = vehicles[i];
                            await Task.Run(() =>
                            {
                                var vehicleType = vehicleTypes.Where(o => o.Name == vehicle.VehicleTypeName).FirstOrDefault();
                                UpdateVehicleStatistic(vehicle, vehicleType);
                                repository.Vehicle.Update(vehicle);
                            });
                        }
                        

                        vehicles.RemoveAll(v => v.VehicleStatistic.FinishTime != null);
                        if(vehicles.Count() < 1)
                        {
                            simulation.EndTime = DateTime.Now;
                            repository.Simulation.Update(simulation);
                            _logger.LogInformation($"Simulation for race:{simulation.RaceId} is finished.");
                        }
                        simulation.LastUpdate = DateTime.Now;
                        repository.Simulation.Update(simulation);

                        await repository.SaveAsync();

                        var executionTime = (DateTime.Now - iterationStarTime).TotalMilliseconds;
                        _logger.LogInformation($"Interation execution time is: {executionTime}ms");
                        if (executionTime > _simConf.DeadlineForRealTime)
                        {
                            _logger.LogError($"Real-time deadline is not meet.\n" +
                                $"Current iteration lasted for {executionTime}ms.\n" +
                                $"Real-time deadline is {_simConf.DeadlineForRealTime}");
                            //throw new Exception($"Real-time deadline is not meet.");
                        }
                        else
                        {
                            await Task.Delay(_simConf.DeadlineForRealTime - (int)Math.Ceiling(executionTime));
                        }
                    }
                }
                
            }
        }

        private void UpdateVehicleStatistic( Vehicle vehicle, VehicleType vehicleType)
        {
            Random random = new Random();
            if (vehicle.VehicleStatistic.HournsUtilFixed > 0)
            {
                vehicle.VehicleStatistic.HournsUtilFixed -= DEADLINE_IN_HOURS;
            }else if (HeavyMalfunctionHappened(vehicle, random, vehicleType))
            {
                vehicle.VehicleStatistic.FinishTime = DateTime.Now;
                vehicle.VehicleStatistic.Status = VehicleStatus.Broken;
                vehicle.VehicleStatistic.Malfunctions++;
                vehicle.VehicleStatistic.CurrentSpeed = 0;
            }
            else if (LightMalfunctionHappened(vehicle, random, vehicleType))
            {
                vehicle.VehicleStatistic.Status = VehicleStatus.UnderRepair;
                vehicle.VehicleStatistic.Malfunctions++;
                vehicle.VehicleStatistic.CurrentSpeed = 0;
                vehicle.VehicleStatistic.HournsUtilFixed = vehicleType.RepairmentTimeInHovers;
            }
            else
            {
                UpdateSpeedAndDistance(vehicle, random, vehicleType);
                if (vehicle.VehicleStatistic.Distance > _simConf.RaceLength)
                {
                    vehicle.VehicleStatistic.Status = VehicleStatus.Finished;
                    vehicle.VehicleStatistic.FinishTime = DateTime.Now;
                }
            }
            

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HeavyMalfunctionHappened(Vehicle vehicle, Random random, VehicleType vehicleType)
        {
            return (random.Next(0, MAX_RANDOM_FOR_PERCENTAGE) < vehicleType.PercentageOfHeavyMalfunctionsPerHour);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool LightMalfunctionHappened(Vehicle vehicle, Random random, VehicleType vehicleType)
        {
            return (random.Next(0, MAX_RANDOM_FOR_PERCENTAGE) < vehicleType.PercentageOfLightMalfunctionsPerHour);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSpeedAndDistance(Vehicle vehicle, Random random, VehicleType vehicleType)
        {
            var currentSpeed = vehicle.VehicleStatistic.CurrentSpeed;
            var newSpeed = random.Next(currentSpeed - HALF_OF_MAX_SPEED_CHANGE, currentSpeed + _simConf.MaxSpeedChange);
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }
            else if (newSpeed > vehicleType.MaxSpeed)
            {
                newSpeed = vehicleType.MaxSpeed;
            }
            vehicle.VehicleStatistic.CurrentSpeed = (ushort)newSpeed;
            vehicle.VehicleStatistic.Distance += newSpeed * DEADLINE_IN_HOURS;
        }
    
    }
}
