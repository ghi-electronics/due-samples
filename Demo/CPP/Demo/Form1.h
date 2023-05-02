#pragma once

namespace CppCLRWinFormsProject {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
    using namespace System::IO::Ports;

	/// <summary>
	/// Summary for Form1
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{
	public:
		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
			}
		}
    private: System::Windows::Forms::Button^ btSend;
    protected:

	private: System::Windows::Forms::Label^ label1;
    private: System::Windows::Forms::TextBox^ textid;
    private: System::Windows::Forms::Button^ btSound;
    private: System::Windows::Forms::Button^ btLedon;
    private: System::Windows::Forms::Button^ btLedoff;
    private: System::Windows::Forms::Label^ label2;
    private: System::Windows::Forms::TextBox^ comport;
    protected:

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
            this->btSend = (gcnew System::Windows::Forms::Button());
            this->label1 = (gcnew System::Windows::Forms::Label());
            this->textid = (gcnew System::Windows::Forms::TextBox());
            this->btSound = (gcnew System::Windows::Forms::Button());
            this->btLedon = (gcnew System::Windows::Forms::Button());
            this->btLedoff = (gcnew System::Windows::Forms::Button());
            this->label2 = (gcnew System::Windows::Forms::Label());
            this->comport = (gcnew System::Windows::Forms::TextBox());
            this->SuspendLayout();
            // 
            // btSend
            // 
            this->btSend->Location = System::Drawing::Point(229, 146);
            this->btSend->Name = L"btSend";
            this->btSend->Size = System::Drawing::Size(413, 53);
            this->btSend->TabIndex = 0;
            this->btSend->Text = L"Send (to Pulse only)";
            this->btSend->UseVisualStyleBackColor = true;
            this->btSend->Click += gcnew System::EventHandler(this, &Form1::btSend_Click);
            // 
            // label1
            // 
            this->label1->AutoSize = true;
            this->label1->Location = System::Drawing::Point(103, 97);
            this->label1->Name = L"label1";
            this->label1->Size = System::Drawing::Size(91, 25);
            this->label1->TabIndex = 1;
            this->label1->Text = L"Input text";
            this->label1->Click += gcnew System::EventHandler(this, &Form1::label1_Click);
            // 
            // textid
            // 
            this->textid->Location = System::Drawing::Point(229, 93);
            this->textid->Name = L"textid";
            this->textid->Size = System::Drawing::Size(413, 29);
            this->textid->TabIndex = 2;
            // 
            // btSound
            // 
            this->btSound->Location = System::Drawing::Point(229, 219);
            this->btSound->Name = L"btSound";
            this->btSound->Size = System::Drawing::Size(413, 53);
            this->btSound->TabIndex = 3;
            this->btSound->Text = L"Play sound (on Pulse only)";
            this->btSound->UseVisualStyleBackColor = true;
            this->btSound->Click += gcnew System::EventHandler(this, &Form1::btSound_Click);
            // 
            // btLedon
            // 
            this->btLedon->Location = System::Drawing::Point(229, 297);
            this->btLedon->Name = L"btLedon";
            this->btLedon->Size = System::Drawing::Size(200, 53);
            this->btLedon->TabIndex = 4;
            this->btLedon->Text = L"Led On";
            this->btLedon->UseVisualStyleBackColor = true;
            this->btLedon->Click += gcnew System::EventHandler(this, &Form1::btLedon_Click);
            // 
            // btLedoff
            // 
            this->btLedoff->Location = System::Drawing::Point(442, 297);
            this->btLedoff->Name = L"btLedoff";
            this->btLedoff->Size = System::Drawing::Size(200, 53);
            this->btLedoff->TabIndex = 5;
            this->btLedoff->Text = L"Led Off";
            this->btLedoff->UseVisualStyleBackColor = true;
            this->btLedoff->Click += gcnew System::EventHandler(this, &Form1::btLedoff_Click);
            // 
            // label2
            // 
            this->label2->AutoSize = true;
            this->label2->Location = System::Drawing::Point(12, 38);
            this->label2->Name = L"label2";
            this->label2->Size = System::Drawing::Size(207, 25);
            this->label2->TabIndex = 6;
            this->label2->Text = L"Input comport (COMx)";
            // 
            // comport
            // 
            this->comport->Location = System::Drawing::Point(229, 38);
            this->comport->Name = L"comport";
            this->comport->Size = System::Drawing::Size(103, 29);
            this->comport->TabIndex = 7;
            // 
            // Form1
            // 
            this->AutoScaleDimensions = System::Drawing::SizeF(11, 24);
            this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
            this->ClientSize = System::Drawing::Size(799, 423);
            this->Controls->Add(this->comport);
            this->Controls->Add(this->label2);
            this->Controls->Add(this->btLedoff);
            this->Controls->Add(this->btLedon);
            this->Controls->Add(this->btSound);
            this->Controls->Add(this->textid);
            this->Controls->Add(this->label1);
            this->Controls->Add(this->btSend);
            this->Name = L"Form1";
            this->Text = L"DUE\'s Demo";
            this->ResumeLayout(false);
            this->PerformLayout();

        }
#pragma endregion
	    private: System::Void label1_Click(System::Object^ sender, System::EventArgs^ e) {
	    }


        private: System::Void btSound_Click(System::Object^ sender, System::EventArgs^ e) {

            SerialPort^ serialport = gcnew SerialPort();

            serialport->PortName = this->comport->Text->ToUpper();

            if (serialport->PortName->Contains("COM") == false) {
                serialport->PortName = "COM" + serialport->PortName;
            }

            serialport->BaudRate = 9600;

            try {
                serialport->Open();
            }
            catch (Exception^ e)
            {
                MessageBox::Show("Could not open the port " + serialport->PortName);

                return;
            }

            if (serialport->IsOpen) {

                serialport->Write("sound(1000,100,100)\n");

                serialport->Close();
            }
          
        }
        private: System::Void btLedon_Click(System::Object^ sender, System::EventArgs^ e) {
            SerialPort^ serialport = gcnew SerialPort();

            serialport->PortName = this->comport->Text;
            serialport->BaudRate = 9600;

            serialport->PortName = this->comport->Text->ToUpper();

            if (serialport->PortName->Contains("COM") == false) {
                serialport->PortName = "COM" + serialport->PortName;
            }

            try {
                serialport->Open();
            }
            catch (Exception^ e)
            {
                MessageBox::Show("Could not open the port " + serialport->PortName);

                return;
            }

            if (serialport->IsOpen) {

                serialport->Write("dwrite(108,1)\n");

                serialport->Close();
            }
        }
        private: System::Void btLedoff_Click(System::Object^ sender, System::EventArgs^ e) {
            SerialPort^ serialport = gcnew SerialPort();

            serialport->PortName = this->comport->Text;
            serialport->BaudRate = 9600;

            serialport->PortName = this->comport->Text->ToUpper();

            if (serialport->PortName->Contains("COM") == false) {
                serialport->PortName = "COM" + serialport->PortName;
            }

            try {
                serialport->Open();
            }
            catch (Exception^ e)
            {
                MessageBox::Show("Could not open the port " + serialport->PortName);

                return;
            }

            if (serialport->IsOpen) {

                serialport->Write("dwrite(108,0)\n");

                serialport->Close();
            }
        }
        private: System::Void btSend_Click(System::Object^ sender, System::EventArgs^ e) {
            SerialPort^ serialport = gcnew SerialPort();

            serialport->PortName = this->comport->Text;
            serialport->BaudRate = 9600;

            serialport->PortName = this->comport->Text->ToUpper();

            if (serialport->PortName->Contains("COM") == false) {
                serialport->PortName = "COM" + serialport->PortName;
            }

            try {
                serialport->Open();
            }
            catch (Exception^ e)
            {
                MessageBox::Show("Could not open the port " + serialport->PortName);

                return;
            }

            if (serialport->IsOpen) {

                serialport->Write("lcdclear(0)\n");
                System::Threading::Thread::Sleep(100);
                serialport->Write("lcdtexts(\"" + this->textid->Text + "\", 1, 0, 0, 1, 1)\n");
                System::Threading::Thread::Sleep(100);
                serialport->Write("lcdshow()\n");

                serialport->Close();
            }
        }
    };
}
