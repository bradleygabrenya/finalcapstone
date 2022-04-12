﻿using Capstone.DAO.Interfaces;
using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutDAO workoutDAO;
        public WorkoutController(IWorkoutDAO _workoutDAO)
        {
            workoutDAO = _workoutDAO;
        }

        [HttpGet("/workouts/{userId}")]
        //[Authorize]
        public List<DailyWorkout> GetDailyWorkouts(int userId)
        {
            List<DailyWorkout> dailyWorkouts = workoutDAO.GetDailyWorkouts(userId);
            return dailyWorkouts;
        }

        [HttpGet("/workoutdetails/{workoutId}")]
        //[Authorize]
        public List<UseTracking> GetUseTrackings(int workoutId)
        {
            List<UseTracking> useTrackings = workoutDAO.GetWorkoutDetails(workoutId);
            return useTrackings;
        }

        [HttpPost("/workouts/{userId}")]
        public string PostDailyWorkout(int userId)
        {
            string completed = workoutDAO.StartDailyWorkout(userId);
            return completed;
        }

        [HttpPost("/workoutdetails/{workoutId}")]
        public string PostUseTracking(UseTracking useTracking)
        {
            string completed = workoutDAO.CreateUseTracking(useTracking);
            return completed;
        }

    }
}
