//
//  ViewController.swift
//  Demo
//
//  Created by GHI on 4/27/23.
//

import Cocoa
import SerialPort

class ViewController: NSViewController {
    var serialport = SerialPort(portpath: "/dev/cu.usbmodemDUE_SC131");
    //var SerialPort: SerialPort = NSNull;
    var ledOn = false;
    
    func Connect() -> Bool {
        if (serialport.IsOpened())
        {
            serialport.Close();
        }
        
        serialport = SerialPort(portpath: "/dev/cu.usbmodemDUE_SC131");
        
        if (serialport.Open()) {
            serialport.Timeout = 100;
            return true;
        }
        
        serialport = SerialPort(portpath: "/dev/cu.usbmodemDUE_SC0071");
        if (serialport.Open()) {
            serialport.Timeout = 100;
            return true;
        }
        
        return false;
    }

    override func viewDidLoad() {
        super.viewDidLoad()
        
            //self.view.window?.setFrame(NSRect(x:0,y:0,width: 800,height: 800), display: true);
        
      
        
        gettext.stringValue = "Hello world!";
        
        // Do any additional setup after loading the view.
    }

    override var representedObject: Any? {
        didSet {
        // Update the view, if already loaded.
        }
    }

    @IBAction func turnledon(_ sender: Any) {
        
        let ledon = "dwrite(100,1)";
        
        SendCmd(cmd: ledon);
        
        
        
        
    }
    
    @IBAction func ledoffclick(_ sender: Any) {
        let ledoff = "dwrite(100,0)";
        SendCmd(cmd: ledoff);
    }
    @IBOutlet weak var gettext: NSTextField!
    
    @IBAction func send(_ sender: Any) {
        var clearcmd = "lcdclear(0)";
        
        if (!SendCmd(cmd: clearcmd)) {
            return;
        }
        
        var text = gettext.stringValue;
        var scale_x = "1";
        var scale_y = "1";
        
        if (text.count > 25){
            var t = text.dropLast(text.count - 25);
            text = String(t);
            scale_x = "1";
            scale_y = "1";
            
        }
        
        else if (text.count > 11) {
            scale_x = "1";
            scale_y = "1";
        }
        else if (text.count > 7) {
            scale_x = "2";
            scale_y = "2";
        }
        else  {
            scale_x = "3";
            scale_y = "3";
        }
         
            
        
        var str = "lcdtexts(" + #"""# + text + #"""# + ",1,0,0," + scale_x + ", " + scale_y + ")";
        
        SendCmd(cmd: str);
        
        var showcmd = "lcdshow()";
        
        SendCmd(cmd: showcmd);
        
    }
    
    func SendCmd(cmd: String) -> Bool {
        if (!Connect()) {
            MessageBox(message: "Error!", informative: "No DUE device found!\nConnect the device and try again!");
            return false;
        }
        
        var cmd = cmd + "\n";
        serialport.Write(data: [CChar](cmd.utf8CString), offset: 0, count:Int32(cmd.count));
        
        usleep(1000);
        
        var dataRead : [CChar] = [0];

        while (true){
            if (serialport.Read(data: &dataRead, offset: 0, count: 1) == 0) {
                break;
            }
            
        }
        
        return true;
        
    }
    
    func MessageBox(message: String, informative: String) {


        let alert = NSAlert();

        alert.messageText = message;

        alert.informativeText = informative;

        alert.addButton(withTitle: "OK")

        alert.runModal()

    }
    
    @IBAction func playsoundclick(_ sender: Any) {
        var cmd = "sound(1000,500,100)";
        
        SendCmd(cmd: cmd);
    }
    
  
}

