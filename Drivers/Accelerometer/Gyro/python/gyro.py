from DUE.DUEController import DUEController
import array

class GyroscopeController: 

    LSM6DS3_WHO_AM_I_REG = 0X0F
    LSM6DS3_CTRL1_XL = 0X10
    LSM6DS3_CTRL2_G = 0X11

    LSM6DS3_STATUS_REG = 0X1E

    LSM6DS3_CTRL7_G = 0X16
    LSM6DS3_CTRL8_XL = 0X17

    LSM6DS3_OUTX_L_G = 0X22


    LSM6DS3_OUTX_L_XL = 0X28  

    def __init__(self, dueController: DUEController, slaveAddress = 0x6A ) -> None:
        self.dueController = dueController
        self.slaveAddress = slaveAddress
        self.AccelerationSampleRate = 104.0
        self.GyroscopeSampleRate = 104.0        

    def __Initialize(self)    :
        dataRead = [0]
        dataWrite = [0]

        dataWrite[0] = GyroscopeController.LSM6DS3_WHO_AM_I_REG


        self.dueController.I2c.WriteRead(self.SlaveAddress, dataWrite, dataRead)

        #set the gyroscope control register to work at 104 Hz, 2000 dps and in bypass mode
        self.__WriteRegister(GyroscopeController.LSM6DS3_CTRL2_G, 0x4C)

        # Set the Accelerometer control register to work at 104 Hz, 4 g,and in bypass mode and enable ODR/4
        # low pass filter (check figure9 of LSM6DS3's datasheet)
        self.WriteRegister(GyroscopeController.LSM6DS3_CTRL1_XL, 0x4A)

        # set gyroscope power mode to high performance and bandwidth to 16 MHz
        self.WriteRegister(GyroscopeController.LSM6DS3_CTRL7_G, 0x00)

        # Set the ODR config register to ODR/4
        self.WriteRegister(GyroscopeController.LSM6DS3_CTRL8_XL, 0x09)

    def __WriteRegister(self, register: int, data: int) :
        dataWrite = [register, data ]

        self.dueController.I2c.Write(self.slaveAddress, dataWrite, 0, len(dataWrite))

    def __ReadRegister(self, register: int) :
        dataWrite = [ register ]
        dataRead = [0]

        self.dueController.I2c.WriteRead(self.slaveAddress, dataWrite, 0, len(dataWrite), dataRead, 0, len(dataRead))

        return dataRead[0]
    
    def __ReadRegisters(self, register: int, readcount: int) -> bytearray:
        dataWrite = [ register ]
        dataRead = [0] *readcount

        self.dueController.I2c.WriteRead(self.slaveAddress, dataWrite, 0, len(dataWrite), dataRead, 0, len(dataRead))

        return dataRead
    
    def AccelerationAvailable(self) -> bool:
        if ((self.__ReadRegister(GyroscopeController.LSM6DS3_STATUS_REG) & 0x01) != 0):
            return self.__ReadAcceleration()            
        
        return False
    
    def __ReadAcceleration(self) -> bool:
        data = self.__ReadRegisters(GyroscopeController.LSM6DS3_OUTX_L_XL, 6);

        raw0 = data[0] | (data[1] << 8)
        raw1 = data[2] | (data[3] << 8)
        raw2 = data[3] | (data[5] << 8)

        if (data != 0) :
            self.__x = (float)(raw0 * 4.0 / 32768.0)
            self.__y = (float)(raw1 * 4.0 / 32768.0)
            self.__z = (float)(raw2 * 4.0 / 32768.0)

            return True
        
        return False
        
        
    def get_x(self)->float:
        return self.__x
    
    def set_x(self):
        return
    
    X = property(get_x, set_x)

    def get_y(self)->float:
        return self.__y
    
    def set_y(self):
        return
    
    Y = property(get_y, set_y)

    def get_z(self)->float:
       return self.__z
    
    def set_z(self):
        return
    
    Z = property(get_z, set_z)
        
        
          
    

        
        

        

        





