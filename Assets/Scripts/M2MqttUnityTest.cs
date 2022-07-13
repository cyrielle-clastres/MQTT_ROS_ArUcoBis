/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>
namespace M2MqttUnity.Examples
{
    /// <summary>
    /// Script for testing M2MQTT with a Unity UI
    /// </summary>
    public class M2MqttUnityTest : M2MqttUnityClient
    {
        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
        [Header("User Interface")]
        public InputField consoleInputField;
        public Toggle encryptedToggle;
        public InputField addressInputField;
        public InputField portInputField;
        public Button connectButton;
        public Button disconnectButton;
        public Button testPublishButton;
        public Button clearButton;
        public PlacementCube scriptinverse;
        public Blob script_robot;
        public RobotVirtuel script_robot_virtuel;
        public GameObject triedre_effecteur;
        public GameObject triedre_robot_virtuel;

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;
        private int count = 0;
        public void PublishPosition()
        {
            count++;
            if ((cube != null) && (count % 2 == 0) && (triedre != null) && (scriptinverse.fixe == 2))
            {
                //On calcule la position du cube dans le repère indirect
                Matrix4x4 m = Matrix4x4.TRS(cube.transform.position, cube.transform.rotation, new Vector3(1, 1, 1));
                Matrix4x4 cube_robot = scriptinverse.mat_monde_robot * m;

                //On calcule la position du cube dans le repère direct de la table
                Vector3 Point = new Vector3(cube_robot[0,3], cube_robot[2,3], cube_robot[1,3]);
                Quaternion Quaternion = new Quaternion(-cube_robot.rotation.x, -cube_robot.rotation.z, -cube_robot.rotation.y, cube_robot.rotation.w);

                //On envoie la position et la rotation du cube dans le repère direct à MQTT qui fera la transmission à ROS
                String pos = JsonUtility.ToJson(Point);
                String rot = JsonUtility.ToJson(Quaternion);
                String final = "{\"position\":" + pos + " , \"orientation\":" + rot + "}";
                client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(final)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        public void PublishTriedrePosition()
        {
            if ((count % 2 == 0) && (scriptinverse.fixe == 2) && (script_robot_virtuel.SetJoints == true) && (script_robot_virtuel.SetTriedre == true))
            {
                //On calcule la position du triedre dans le repère indirect
                Matrix4x4 m = Matrix4x4.TRS(triedre_robot_virtuel.transform.position, triedre_robot_virtuel.transform.rotation, new Vector3(1, 1, 1));
                Matrix4x4 triedre_robot = scriptinverse.mat_monde_robot * m;

                //On calcule la position du cube dans le repère direct de la table
                Vector3 Point = new Vector3(triedre_robot[0, 3], triedre_robot[2, 3], triedre_robot[1, 3]);
                Quaternion Quaternion = new Quaternion(-triedre_robot.rotation.x, -triedre_robot.rotation.z, -triedre_robot.rotation.y, triedre_robot.rotation.w);

                //On envoie la position et la rotation du cube dans le repère direct à MQTT qui fera la transmission à ROS
                String pos = JsonUtility.ToJson(Point);
                String rot = JsonUtility.ToJson(Quaternion);
                String final = "{\"position\":" + pos + " , \"orientation\":" + rot + "}";
                client.Publish("robot_pose", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(final)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        public void TestPublish()
        {
            client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            AddUiMessage("Test message published.");
        }

        public void SetBrokerAddress(string brokerAddress)
        {
            if (addressInputField && !updateUI)
            {
                this.brokerAddress = brokerAddress;
            }
        }

        public void SetBrokerPort(string brokerPort)
        {
            if (portInputField && !updateUI)
            {
                int.TryParse(brokerPort, out this.brokerPort);
            }
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        public void SetUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text = msg;
                updateUI = true;
            }
        }

        public void AddUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text += msg + "\n";
                updateUI = true;
            }
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            if (autoTest)
            {
                TestPublish();
            }
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "position/mqtt" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "position_robot" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "position/mqtt" });
            client.Unsubscribe(new string[] { "position_robot" });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            AddUiMessage("CONNECTION LOST!");
        }

        private void UpdateUI()
        {
            if (client == null)
            {
                if (connectButton != null)
                {
                    connectButton.interactable = true;
                    disconnectButton.interactable = false;
                    testPublishButton.interactable = false;
                }
            }
            else
            {
                if (testPublishButton != null)
                {
                    testPublishButton.interactable = client.IsConnected;
                }
                if (disconnectButton != null)
                {
                    disconnectButton.interactable = client.IsConnected;
                }
                if (connectButton != null)
                {
                    connectButton.interactable = !client.IsConnected;
                }
            }
            if (addressInputField != null && connectButton != null)
            {
                addressInputField.interactable = connectButton.interactable;
                addressInputField.text = brokerAddress;
            }
            if (portInputField != null && connectButton != null)
            {
                portInputField.interactable = connectButton.interactable;
                portInputField.text = brokerPort.ToString();
            }
            if (encryptedToggle != null && connectButton != null)
            {
                encryptedToggle.interactable = connectButton.interactable;
                encryptedToggle.isOn = isEncrypted;
            }
            if (clearButton != null && connectButton != null)
            {
                clearButton.interactable = connectButton.interactable;
            }
            updateUI = false;
        }

        protected override void Start()
        {
            SetUiMessage("Ready.");
            updateUI = true;
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            StoreMessage(msg);
            if (count % 2 == 0)
            {
                if (topic == "position/mqtt")
                {
                    PosRot PositionAndRotation = JsonUtility.FromJson<PosRot>(msg);
                    if ((triedre_effecteur != null) && (scriptinverse.fixe == 2))
                    {
                        //On récupère les coordonnées envoyées par ROS et on les mets dans le repère indirect de la table
                        triedre_effecteur.transform.position = new Vector3(PositionAndRotation.position.x, PositionAndRotation.position.z, PositionAndRotation.position.y);
                        triedre_effecteur.transform.rotation = new Quaternion(-PositionAndRotation.orientation.x, -PositionAndRotation.orientation.z, -PositionAndRotation.orientation.y, PositionAndRotation.orientation.w);

                        //On calcule les coordonnées indirectes dans le reepère monde
                        Matrix4x4 m = Matrix4x4.TRS(triedre_effecteur.transform.position, triedre_effecteur.transform.rotation, new Vector3(1, 1, 1));
                        Matrix4x4 triedre_monde = scriptinverse.mat_robot_monde * m;

                        //On donne les coordonnées à l'objet
                        triedre_effecteur.transform.position = new Vector3(triedre_monde[0, 3], triedre_monde[1, 3], triedre_monde[2, 3]);
                        triedre_effecteur.transform.rotation = new Quaternion(triedre_monde.rotation.x, triedre_monde.rotation.y, triedre_monde.rotation.z, triedre_monde.rotation.w);
                        
                        if(script_robot_virtuel.SetTriedre == false)
                        {
                            script_robot_virtuel.triedre_robot_virtuel.transform.position = new Vector3(triedre_monde[0, 3], triedre_monde[1, 3], triedre_monde[2, 3]);
                            script_robot_virtuel.triedre_robot_virtuel.transform.rotation = new Quaternion(triedre_monde.rotation.x, triedre_monde.rotation.y, triedre_monde.rotation.z, triedre_monde.rotation.w);
                            script_robot_virtuel.SetTriedre = true;
                        }
                    }
                }
            }
            if(count % 2 == 0) {
                if (topic == "position_robot")
                {
                    JointState Joint_State = JsonUtility.FromJson<JointState>(msg);
                    script_robot.UpdatePosition(Joint_State.position);
                    if(script_robot_virtuel.SetJoints == false)
                    {
                        script_robot_virtuel.UpdatePosition(Joint_State.position);
                        script_robot_virtuel.SetJoints = true;
                    }
                }
                if (topic == "position_robot_virtuel_casque")
                {
                    JointState Joint_State = JsonUtility.FromJson<JointState>(msg);
                    script_robot_virtuel.UpdatePosition(Joint_State.position);
                }
                else if (topic == "M2MQTT_Unity/test")
                {
                    if (autoTest)
                    {
                        autoTest = false;
                        Disconnect();
                    }
                }
            }
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            AddUiMessage("Received: " + msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            if (updateUI)
            {
                UpdateUI();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            if (autoTest)
            {
                autoConnect = true;
            }
        }
    }
}
