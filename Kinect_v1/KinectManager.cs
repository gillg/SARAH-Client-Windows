﻿using System;
using System.Drawing;
using System.Collections.Generic;

using Microsoft.Kinect;


namespace net.encausse.sarah.kinect1 {
  public class KinectManager : IDisposable {

    // -------------------------------------------
    //  SINGLETON
    // -------------------------------------------

    private static KinectManager manager = null;
    private KinectManager() { }

    public static KinectManager GetInstance() {
      if (manager == null) {
        manager = new KinectManager();
      }
      return manager;
    }

    public void Dispose() {
      if (Sensors == null) { return; }
      foreach (var sensor in Sensors) {
        sensor.Dispose();
      }
    }

    // -------------------------------------------
    //  UTILITY
    // ------------------------------------------

    protected void Log(string msg) {
      SARAH.GetInstance().Log("Kinect1", msg);
    }

    protected void Debug(string msg) {
      SARAH.GetInstance().Debug("Kinect1", msg);
    }

    protected void Error(string msg) {
      SARAH.GetInstance().Error("Kinect1", msg);
    }

    protected void Error(Exception ex) {
      SARAH.GetInstance().Error("Kinect1", ex);
    }

    // -------------------------------------------
    //  INIT 
    // ------------------------------------------

    public List<Kinect> Sensors { get; set; }
    public void InitSensors() {

      // Cached sensors
      Sensors = new List<Kinect>();

      // Looking for a valid sensor 
      var i = 0;
      foreach (var potential in KinectSensor.KinectSensors) {
        if (potential.Status != KinectStatus.Connected) { continue; }
        Kinect sensor = new Kinect("Kinect_v1_"+(i++), potential);
        Sensors.Add(sensor);
      }

      // Little warning
      if (Sensors.Count <= 0) {
        Error("No Kinect Sensor");
      }
    }

    public void StartSensors() {
      foreach (var sensor in Sensors) {
        sensor.Start();
      }
    }

    public bool Ready() {
      foreach (var sensor in Sensors) {
        if (!sensor.Ready()) { return false; }
      }
      return true;
    }

  }
}
