using UnityEngine;
using UnityEngine.UI;

public class ConnectDevice : MonoBehaviour
{
    public string DeviceName = "";
    public string ServiceUUID = "15171000-4947-11e9-8646-d663bd873d93";
    public string SubscribeCharacteristic = "15171001-4947-11e9-8646-d663bd873d93";

    enum States
    {
        None,
        Scan,
        Connect,
        Subscribe,
        Unsubscribe,
        Disconnect,
    }

    

    float _timeout = 0f;
    string _deviceAddress;
    bool _connected = false; 
    States _state = States.None;

    byte[] _dataBytes = null;
    bool _foundWriteID = false;
    bool _foundSubscribeID = false;

    [SerializeField] Text DeviceNameFoundedText;
    [SerializeField] PlayerMove playerMoveScript;
    [SerializeField] UnityEngine.UI.Text stateText;

    public void Connect()
    {
        Reset();
        SetState(States.Connect, 4f);
    }

    public void Disconnect()
    {
        Reset();
        SetState(States.Disconnect, 4f);
    }

    public void Subscribe()
    {
        SetState(States.Subscribe, 4f);
    }

    void Reset()
    {
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;
        _foundSubscribeID = false;
        _dataBytes = null;        
    } 

    // Use this for initialization
    void Start()
    {
        StartProcess();
    }

    void Update()
    {
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;

                EvaluateBLEStates();
            }
        }
    }

    void StartProcess()
    {
        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {

            SetState(States.Scan, 0.1f);

        }, (error) =>
        {

            BluetoothLEHardwareInterface.Log("Error during initialize: " + error);
        });
    }

    void EvaluateBLEStates()
    {
        switch (_state)
        {
            case States.None:
                break;

            case States.Scan:
                stateText.text = "Scanning";
                BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                {
                    new DeviceObject(address, name);
                    BluetoothLEHardwareInterface.StopScan();
                    DeviceNameFoundedText.text += name;
                    // found a device with the name we want
                    _deviceAddress = address;
                    SetState(States.Connect, 2f);   

                });
                break;

            case States.Connect:
                // set these flags
                stateText.text = "Connecting";
                _foundSubscribeID = false;
               
                BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) => {

                    
                    SetState(States.Subscribe, 2f);    
                    
                });
                stateText.text = "SubscribeFailed";
                break;

            case States.Subscribe:
                BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, ServiceUUID, SubscribeCharacteristic, null, (address, characteristicUUID, bytes) =>
                {
                    stateText.text = "Subscribed";
                    _state = States.None;

                    // we received some data from the device
                    _dataBytes = bytes;
                    float qx = System.BitConverter.ToSingle(bytes, 8);
                    string s = string.Format("{0:N2}", qx);
                    stateText.text = s;
                    playerMoveScript.isKinematic = false;

                });
                break;

            case States.Unsubscribe:
                BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, ServiceUUID, SubscribeCharacteristic, null);
                SetState(States.Disconnect, 4f);
                break;

            case States.Disconnect:
                if (_connected)
                {
                    BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                    {
                        BluetoothLEHardwareInterface.DeInitialize(() =>
                        {
                            stateText.text = "Disconnecting";
                            _connected = false;
                            _state = States.None;
                        });
                    });
                }
                else
                {
                    BluetoothLEHardwareInterface.DeInitialize(() =>
                    {
                        _state = States.None;
                    });
                }
                break;
        }
    }

    void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    bool IsEqual(string uuid1, string uuid2)
    { 
        return (uuid1.CompareTo(uuid2) == 0);
    }    
}