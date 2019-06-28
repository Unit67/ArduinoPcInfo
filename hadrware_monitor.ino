#include <LiquidCrystal_I2C.h>

struct data{
  String type;
  int value;  
};
data dane[5];
LiquidCrystal_I2C lcd(0x27, 16, 2);
unsigned long timer=0;
void setup() {
  Serial.begin(115200);
  lcd.begin();
  lcd.backlight();
  lcd.clear();
  lcd.setCursor(6,0);
  lcd.print("BOOT");
  delay(1000);
  lcd.setCursor(0,0);
  lcd.print("CPU:   % T:   *C");
  lcd.setCursor(0,1);
  lcd.print("GPU:   % T:   *C");
}

void loop() {
  if(Serial.available()){
    for(uint8_t i = 0; i<5;i++){
      dane[i].type = Serial.readStringUntil(':');
      dane[i].value = Serial.readStringUntil(';').toInt();
    }
  }
  if(timer<millis()){
    timer = millis()+1000;
    //lcd.clear();
    for(uint8_t i = 0; i<5; i++){
      if(dane[i].type=="CU"){
        lcd.setCursor(4,0);
        lcd.print("   ");
        if(dane[i].value==100){
          lcd.setCursor(4,0);
          lcd.print(100);
        }else if(dane[i].value>9){
          lcd.setCursor(5,0);
          lcd.print(dane[i].value);
        }else{
          lcd.setCursor(6,0);
          lcd.print(dane[i].value);
        }
        continue;
      }else if(dane[i].type=="GU"){
        lcd.setCursor(4,1);
        lcd.print("   ");
        if(dane[i].value==100){
          lcd.setCursor(4,1);
          lcd.print(100);
        }else if(dane[i].value>9){
          lcd.setCursor(5,1);
          lcd.print(dane[i].value);
        }else{
          lcd.setCursor(6,1);
          lcd.print(dane[i].value);
        }
        continue;
      }else if(dane[i].type=="CT"){
        lcd.setCursor(11,0);
        lcd.print("   ");
        if(dane[i].value>100){
          lcd.setCursor(11,0);
          lcd.print(dane[i].value);
        }else{
          lcd.setCursor(12,0);
          lcd.print(dane[i].value);
        }
        continue;
      }
      else if(dane[i].type=="GT"){
        lcd.setCursor(11,1);
        lcd.print("   ");
        if(dane[i].value>100){
          lcd.setCursor(11,1);
          lcd.print(dane[i].value);
        }else{
          lcd.setCursor(12,1);
          lcd.print(dane[i].value);
        }
        continue;
      }
    }
  }
}
