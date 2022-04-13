﻿using Capstone.DAO.Interfaces;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class WorkoutDAO : IWorkoutDAO
    {
        private string getDetails = "SELECT ut.*,e.equipment_name FROM use_tracking ut JOIN equipment e ON ut.equipment_id = e.equipment_id WHERE ut.workout_id = @workout_id ";
        private string getWorkouts = "SELECT * FROM daily_workout WHERE user_id = @user_id";
        private string addDailyWorkout = "INSERT INTO daily_workout(user_id, check_in, check_out) " +
            " OUTPUT Inserted.workout_id " +
            " VALUES (@user_id, Getdate(), '12/31/9999')";
        private string createUseTracking = "INSERT INTO use_tracking (user_id, workout_id, equipment_id, reps, weight, use_start, use_stop) " +
            " OUTPUT Inserted.tracking_id " +
            " VALUES (@user_id, @workout_id, @equipment_id, 0, 0, GETDATE(), '12/31/9999')";
        private string putUseTracking = "UPDATE use_tracking set reps = @reps, weight = @weight, use_stop = GETDATE() where tracking_id = @tracking_id";

        private readonly string connectionString;

        public WorkoutDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;

        }

        public List<DailyWorkout> GetDailyWorkouts(int userId)
        {
            List<DailyWorkout> dailyWorkouts = new List<DailyWorkout>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getWorkouts, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DailyWorkout dailyWorkout = new DailyWorkout();

                        dailyWorkout.UserId = Convert.ToInt32(reader["user_id"]);
                        dailyWorkout.WorkoutId = Convert.ToInt32(reader["workout_id"]);
                        dailyWorkout.CheckIn = Convert.ToDateTime(reader["check_in"]);
                        dailyWorkout.CheckOut = Convert.ToDateTime(reader["check_out"]);
                        dailyWorkouts.Add(dailyWorkout);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return dailyWorkouts;
        }
        public List<UseTracking> GetWorkoutDetails(int workoutId)
        {
            List<UseTracking> returnUseTracking = new List<UseTracking>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(getDetails, conn);
                    cmd.Parameters.AddWithValue("@workout_id", workoutId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UseTracking useTracking = new UseTracking();
                        useTracking.TrackingId = Convert.ToInt32(reader["tracking_id"]);
                        useTracking.UserId = Convert.ToInt32(reader["user_id"]);
                        useTracking.WorkoutId = Convert.ToInt32(reader["workout_id"]);
                        useTracking.EquipmentName = Convert.ToString(reader["equipment_name"]);
                        useTracking.Reps = Convert.ToInt32(reader["reps"]);
                        useTracking.Weight = Convert.ToDecimal(reader["weight"]);
                        useTracking.UseStart = Convert.ToDateTime(reader["use_start"]);
                        useTracking.UseStop = Convert.ToDateTime(reader["use_stop"]);
                        returnUseTracking.Add(useTracking);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnUseTracking;
        }

        public int StartDailyWorkout(int userId)
        {
            int workoutId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(addDailyWorkout, conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    workoutId = (int)cmd.ExecuteScalar();


                }
            }
            catch (SqlException)
            {
                throw;
            }

            return workoutId;
        }

        public int CreateUseTracking(UseTracking useTracking)
        {
            int trackingId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(createUseTracking, conn);
                    cmd.Parameters.AddWithValue("@user_id", useTracking.UserId);
                    cmd.Parameters.AddWithValue("@workout_id", useTracking.WorkoutId);
                    cmd.Parameters.AddWithValue("@equipment_id", useTracking.EquipmentId);
                    trackingId = (int)cmd.ExecuteScalar();

                }
            }
            catch (SqlException)
            {
                throw;
            }

            return trackingId;
        }

        public string PutUseTracking(UseTracking useTracking)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(putUseTracking, conn);
                    cmd.Parameters.AddWithValue("@tracking_id", useTracking.TrackingId);
                    cmd.Parameters.AddWithValue("@reps", useTracking.Reps);
                    cmd.Parameters.AddWithValue("@weight", useTracking.Weight);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return "successful";
        }

    }
}
