int Button = 4;
bool first;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(Button, INPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){
    char x = Serial.read();
    if(x == 'i'){
      if(digitalRead(Button) == HIGH){
        if(first){
          Serial.println("1");
          first = false;
        }
        else{
          Serial.println("0");
        }
      }
      else{
        Serial.println("0");
      }
    }
  }

  if(digitalRead(Button) == LOW){
    first = true;
  }
}
