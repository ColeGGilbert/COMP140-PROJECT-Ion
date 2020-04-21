int Button = 4;
bool first;

#include <Wire.h>
#include <MPU6050.h>

MPU6050 mpu;

// Timers
unsigned long timer = 0;
float timeStep = 0.01;

// Pitch, Roll and Yaw values
float pitch = 0;
float roll = 0;
float yaw = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  pinMode(Button, INPUT);

  // Initialize MPU6050
  while(!mpu.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G))
  {
    Serial.println("Could not find a valid MPU6050 sensor, check wiring!");
    delay(500);
  }
  
  // Calibrate gyroscope. The calibration must be at rest.
  // If you don't want calibrate, comment this line.
  mpu.calibrateGyro();

  // Set threshold sensivty. Default 3.
  // If you don't want use threshold, comment this line or set 0.
  mpu.setThreshold(3);
}

void loop() {
  // put your main code here, to run repeatedly:
  timer = millis();

  // Read normalized values
  Vector norm = mpu.readNormalizeGyro();

  // Calculate Pitch, Roll and Yaw
  pitch = pitch + norm.YAxis * timeStep;
  roll = roll + norm.XAxis * timeStep;
  yaw = yaw + norm.ZAxis * timeStep;
  
  if(Serial.available() > 0){
    String output;
    char x = Serial.read();
    if(x == 'i'){
      if(digitalRead(Button) == HIGH){
        if(first){
          output = "1";
          first = false;
        }
        else{
          output = "0";
        }
      }
      else{
        output = "0";
      }
      output.concat(",");
      output.concat(pitch);
      output.concat(",");
      output.concat(yaw);
      output.concat(",");
      output.concat(roll);
      Serial.println(output);
    }
  }
  if(digitalRead(Button) == LOW){
    first = true;
  }

  // Wait to full timeStep period
  delay((timeStep*1000) - (millis() - timer));
}
